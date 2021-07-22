//////////////////////////////////////////////////////
// MK Glow Effect	    			                //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.           
 //
//////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;

namespace MK.Glow
{
    using ShaderProperties = PipelineProperties.ShaderProperties;

    internal sealed class Effect
    {
        internal Effect()
        {
            _resources = MK.Glow.Resources.LoadResourcesAsset();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        // Members
        /////////////////////////////////////////////////////////////////////////////////////////////
        //always needed parameters - static
        private static MK.Glow.Resources _resources;
        private static readonly Vector2 _selectiveWorkflowThreshold = new Vector2(0.1f, 10);

        //Selective rendering objects
        private static readonly string _selectiveReplacementTag = "RenderType";
        private static readonly string _selectiveGlowCameraObjectName = "selectiveGlowCameraObject";
        private GameObject _selectiveGlowCameraObject;
        private UnityEngine.Camera _selectiveGlowCamera;

        //Renderbuffers
        private float _renderScale = 1;
        private CommandBuffer _commandBuffer;
        private RenderTarget _selectiveRenderTarget;
		private MipBuffer _bloomDownsampleBuffer, _bloomUpsampleBuffer;

        private RenderTarget _sourceFrameBuffer, _destinationFrameBuffer;
        private RenderTarget sourceFrameBuffer
        {
            get 
            {
                return _settings.workflow == Workflow.Selective && _settings.debugView != DebugView.None ? _selectiveRenderTarget : _sourceFrameBuffer;
            }
        }

        //Runtime needed
        private Keyword[] _shaderKeywords = new Keyword[] 
        {
            new Keyword("_NORMALMAP", false),
            new Keyword("_ALPHATEST_ON", false),
            new Keyword("_ALPHABLEND_ON", false),
            new Keyword("_ALPHAPREMULTIPLY_ON", false),
            new Keyword("_EMISSION", false),
            new Keyword("_METALLICGLOSSMAP", false),
            new Keyword("_DETAIL_MULX2", false),
            new Keyword("_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A", false), //No Keyword will be set
            new Keyword("_SPECULARHIGHLIGHTS_OFF", false),
            new Keyword("_GLOSSYREFLECTIONS_OFF", false),
            new Keyword("EDITOR_VISUALIZATION", false),
            new Keyword("_COLOROVERLAY_ON", false),
            new Keyword("_COLORCOLOR_ON", false),
            new Keyword("SPOT", false),
            new Keyword("DIRECTIONAL", false),
            new Keyword("DIRECTIONAL_COOKIE", false),
            new Keyword("POINT", false),
            new Keyword("", false),
            new Keyword("POINT_COOKIE", false)
        };

        //Used features
        private bool _useLensSurface;

        //Lists
        private List<RenderTarget> _renderTargetsBundle;
        private List<MaterialKeywords> _renderKeywordsBundle;

        //Rendering dependent
        private int _bloomIterations, _minIterations, _currentRenderIndex;
        internal int currentRenderIndex { get { return _currentRenderIndex; }}
        private float bloomUpsampleSpread, _lensFlareUpsampleSpread, _glareScatteringMult;
        private RenderTextureFormat _renderTextureFormat;
        internal RenderTextureFormat renderTextureFormat { get{ return _renderTextureFormat; } }
        private RenderContext[] _sourceContext, _renderContext;
        private RenderContext _selectiveRenderContext;
        private UnityEngine.Camera _renderingCamera;
        private RenderPipeline _renderPipeline;

        //Materials
        private Material _renderMaterialNoGeometry;
        internal Material renderMaterialNoGeometry { get { return _renderMaterialNoGeometry; } }

        //Settings
        private Settings _settings;

        /////////////////////////////////////////////////////////////////////////////////////////////
        // Unity MonoBehavior Messages
        /////////////////////////////////////////////////////////////////////////////////////////////
        internal void Enable(RenderPipeline renderPipeline)
        {
            _renderTextureFormat = Compatibility.CheckSupportedRenderTextureFormat();

            _renderPipeline = renderPipeline;
            _sourceContext = new RenderContext[1]{new RenderContext()};
            _renderContext = new RenderContext[PipelineProperties.renderBufferSize];
            for(int i = 0; i < PipelineProperties.renderBufferSize; i++)
                _renderContext[i] = new RenderContext();
            _selectiveRenderContext = new RenderContext();

            _renderMaterialNoGeometry = new Material(_resources.sm40Shader) { hideFlags = HideFlags.HideAndDontSave };

            _renderTargetsBundle = new List<RenderTarget>();
            _renderKeywordsBundle = new List<MaterialKeywords>();

            //create buffers
            _bloomDownsampleBuffer = new MipBuffer(PipelineProperties.CommandBufferProperties.bloomDownsampleBuffer, _renderPipeline);
            _bloomUpsampleBuffer = new MipBuffer(PipelineProperties.CommandBufferProperties.bloomUpsampleBuffer, _renderPipeline);
        }

        internal void Disable()
        {            
            _currentRenderIndex = 0;
            _renderTargetsBundle.Clear();
            _renderKeywordsBundle.Clear();

            MonoBehaviour.DestroyImmediate(_selectiveGlowCamera);
            MonoBehaviour.DestroyImmediate(_selectiveGlowCameraObject);
            MonoBehaviour.DestroyImmediate(_renderMaterialNoGeometry);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        // RenderBuffers
        /////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Prepare Scattering parameters fora given Scattering value
        /// </summary>
        /// <param name="Scattering"></param>
        /// <param name="scale"></param>
        /// <param name="iterations"></param>
        /// <param name="spread"></param>
        private void PrepareScattering(float Scattering, float scale, ref int iterations, ref float spread)
        {
            /*
            float lit = Mathf.Log(scale, 2f) + Mathf.Min(Scattering, 10f) - 10f;
            int litF = Mathf.FloorToInt(lit); 
            iterations = Mathf.Clamp(litF, 1, 15);
            spread = 0.5f + lit - litF;
            */

            float scaledIterations = scale + Mathf.Clamp(Scattering, 1f, 10.0f) - 10.0f;
            iterations = Mathf.Max(Mathf.FloorToInt(scaledIterations), 1);
            spread = scaledIterations > 1 ? 0.5f + scaledIterations - iterations : 0.5f;
        }

        /// <summary>
        /// Create renderbuffers
        /// </summary>
        private void UpdateRenderBuffers()
        {
            RenderDimension renderDimension = new RenderDimension((int)((float)_renderingCamera.pixelWidth * _renderScale), (int)((float)_renderingCamera.pixelHeight * _renderScale));
            _sourceContext[0].UpdateRenderContext(_renderingCamera, _renderTextureFormat, 0, renderDimension);
            _sourceContext[0].SinglePassStereoAdjustWidth(_renderingCamera.stereoEnabled);
            Vector2 anamorphic = new Vector2(_settings.anamorphicRatio < 0 ? -_settings.anamorphicRatio : 0f, _settings.anamorphicRatio > 0 ?  _settings.anamorphicRatio : 0f);
            renderDimension = new RenderDimension(Mathf.CeilToInt(_sourceContext[0].width / (2f - anamorphic.x)), Mathf.CeilToInt(_sourceContext[0].height / (2f - anamorphic.y)));

            float sizeScale = Mathf.Log(Mathf.FloorToInt(Mathf.Max(renderDimension.width, renderDimension.height)), 2.0f);
            //float sizeScale = Mathf.FloorToInt(Mathf.Max(renderDimension.width, renderDimension.height));

            PrepareScattering(_settings.bloomScattering, sizeScale, ref _bloomIterations, ref bloomUpsampleSpread);
            _minIterations = _bloomIterations;

            _renderingCamera.UpdateMipRenderContext(_renderContext, renderDimension, _minIterations + 1, _renderTextureFormat, 0);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        // Selective glow setup
        /////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// selective replacement shader rendering camera for the glow
        /// </summary>
        private GameObject selectiveGlowCameraObject
        {
            get
            {
                if(!_selectiveGlowCameraObject)
                {
                    _selectiveGlowCameraObject = new GameObject(_selectiveGlowCameraObjectName);
                    _selectiveGlowCameraObject.AddComponent<UnityEngine.Camera>();
                    _selectiveGlowCameraObject.hideFlags = HideFlags.HideAndDontSave;
                }
                return _selectiveGlowCameraObject;
            }
        }

        /// <summary>
        /// selective replacement shader rendering camera forthe glow
        /// </summary>
        private UnityEngine.Camera selectiveGlowCamera
        {
            get
            {
                if(_selectiveGlowCamera == null)
                {
                    _selectiveGlowCamera = selectiveGlowCameraObject.GetComponent<UnityEngine.Camera>();
                    _selectiveGlowCamera.hideFlags = HideFlags.HideAndDontSave;
                    _selectiveGlowCamera.enabled = false;
                }
                return _selectiveGlowCamera;
            }
        }

        /// <summary>
        /// Prepare replacement rendering camera forthe selective glow
        /// </summary>
        private void SetupSelectiveGlowCamera()
        {
            selectiveGlowCamera.CopyFrom(_renderingCamera);
            selectiveGlowCamera.targetTexture = _selectiveRenderTarget.renderTexture;
            selectiveGlowCamera.clearFlags = CameraClearFlags.SolidColor;
            selectiveGlowCamera.rect = new Rect(0,0, 1,1);
            selectiveGlowCamera.backgroundColor = new Color(0, 0, 0, 1);
            selectiveGlowCamera.cullingMask = _settings.selectiveRenderLayerMask;
            selectiveGlowCamera.renderingPath = RenderingPath.VertexLit;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        // CommandBuffer creation
        /////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Enable or disable all supported / unsupported shaders based on the platform
        /// </summary>
        private void CheckFeatureSupport()
        {
            //Check iflens surface is set
            if(_settings.allowLensSurface)
                _useLensSurface = true;
            else
                _useLensSurface = false;

            //If any debug view without depending feature is enabled fallback to default rendering
            /*
            if(_settings.debugView != DebugView.None)
            {
                if(!_useLensFlare && (_settings.debugView == DebugView.LensFlare || _settings.debugView == DebugView.RawLensFlare) ||
                   !_useGlare &&(_settings.debugView == DebugView.Glare || _settings.debugView == DebugView.RawGlare))
                    _settings.debugView = DebugView.None;
            }
            */
        }

        private void BeginProfileSample(string text)
        {
            if(_renderPipeline == RenderPipeline.SRP)
                _commandBuffer.BeginSample(text);
            else
                UnityEngine.Profiling.Profiler.BeginSample(text);
        }
        private void EndProfileSample(string text)
        {
            if(_renderPipeline == RenderPipeline.SRP)
                _commandBuffer.EndSample(text);
            else
                UnityEngine.Profiling.Profiler.EndSample();
        }
    
        /// <summary>
        /// Renders the effect from source into destination buffer
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        internal void Build(RenderTarget source, RenderTarget destination, Settings settings, CommandBuffer cmd, UnityEngine.Camera renderingCamera, float renderScale = 1)
        {
            _commandBuffer = cmd;
            _settings = settings;
            _renderingCamera = renderingCamera;
            _renderScale = renderScale;

            BeginProfileSample(PipelineProperties.CommandBufferProperties.samplePrepare);
            
            CheckFeatureSupport();

            _sourceFrameBuffer = source;
            _destinationFrameBuffer = destination;

            UpdateRenderBuffers();
            EndProfileSample(PipelineProperties.CommandBufferProperties.samplePrepare);

            //Prepare for selective glow
            if(_settings.workflow == Workflow.Selective)
            {
                BeginProfileSample(PipelineProperties.CommandBufferProperties.sampleReplacement);
                _selectiveRenderContext.UpdateRenderContext(_renderingCamera, _renderTextureFormat, 16, _sourceContext[0].renderDimension);
                //The allowVerticallyFlip flag seems to break sometimes orientation of the rendered glow map, therefore force the old way.
                _selectiveRenderTarget.renderTexture = RenderTexture.GetTemporary((int)((float)_renderingCamera.pixelWidth * _renderScale) / 2, (int)((float)_renderingCamera.pixelHeight * _renderScale) / 2, 16, _renderTextureFormat, RenderTextureReadWrite.Default, 1);//PipelineExtensions.GetTemporary(_selectiveRenderContext, _renderTextureFormat);
                SetupSelectiveGlowCamera();
                selectiveGlowCamera.RenderWithShader(_resources.selectiveRenderShader, _selectiveReplacementTag);
                EndProfileSample(PipelineProperties.CommandBufferProperties.sampleReplacement);
            }

            BeginProfileSample(PipelineProperties.CommandBufferProperties.sampleSetup);
            UpdateConstantBuffers();
            EndProfileSample(PipelineProperties.CommandBufferProperties.sampleSetup);

            PreSample();
            Downsample();
            Upsample();
            Composite();
        }

        /// <summary>
        /// Update the profile based on the user input
        /// </summary>
        private void UpdateConstantBuffers()
        {      
            //Common
            SetFloat(PipelineProperties.ShaderProperties.singlePassStereoScale, PipelineProperties.singlePassStereoDoubleWideEnabled ? 2 : 1);
            Matrix4x4 viewMatrix = _renderingCamera.worldToCameraMatrix;
            Shader.SetGlobalMatrix(ShaderProperties.viewMatrix.id, viewMatrix);

            //Bloom
            SetFloat(PipelineProperties.ShaderProperties.bloomIntensity, ConvertGammaValue(_settings.bloomIntensity), true);
            SetFloat(PipelineProperties.ShaderProperties.bloomSpread, bloomUpsampleSpread);
            SetFloat(PipelineProperties.ShaderProperties.bloomSpread, bloomUpsampleSpread, true);

            SetVector(PipelineProperties.ShaderProperties.bloomThreshold, _settings.workflow == Workflow.Selective ? _selectiveWorkflowThreshold : new Vector2(ConvertGammaValue(_settings.bloomThreshold.minValue), ConvertGammaValue(_settings.bloomThreshold.maxValue)), _settings.debugView == DebugView.RawBloom ? true : false);

            //LensSurface
            if(_useLensSurface)
            {
                SetFloat(PipelineProperties.ShaderProperties.lensSurfaceDirtIntensity, ConvertGammaValue(_settings.lensSurfaceDirtIntensity), true);
                SetFloat(PipelineProperties.ShaderProperties.lensSurfaceDiffractionIntensity, ConvertGammaValue(_settings.lensSurfaceDiffractionIntensity), true);
                float dirtRatio = (float)(_settings.lensSurfaceDirtTexture ? _settings.lensSurfaceDirtTexture.width : _resources.lensSurfaceDirtTextureDefault.width) / 
                (float)(_settings.lensSurfaceDirtTexture ? _settings.lensSurfaceDirtTexture.height : _resources.lensSurfaceDirtTextureDefault.height);
                float dsRatio = _renderingCamera.aspect / dirtRatio;
                float sdRatio = dirtRatio / _renderingCamera.aspect;

                SetVector(PipelineProperties.ShaderProperties.lensSurfaceDirtTexST, dirtRatio > _renderingCamera.aspect ? 
                          new Vector4(dsRatio, 1, (1f - dsRatio) * 0.5f, 0) :
                          new Vector4(1, sdRatio, 0, (1f - sdRatio) * 0.5f), true);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        // Commandbuffer helpers
        /////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set a specific keyword for the pixelshader
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="enable"></param>
        private void SetKeyword(MaterialKeywords keyword, bool enable)
        {
            //For now disable check if a keyword is already set
            //to make sure the cmd is always correctly setuped
            //if(_shaderKeywords[(int)keyword].enabled != enable)
            {
                if(_renderPipeline == RenderPipeline.SRP)
                    _commandBuffer.SetKeyword(_shaderKeywords[(int)keyword].name, enable);
                else
                    PipelineExtensions.SetKeyword(_shaderKeywords[(int)keyword].name, enable);
                _shaderKeywords[(int)keyword].enabled = enable;
            }
        }

        /// <summary>
        /// Convert an angle (degree) to a Vector2 direction
        /// </summary>
        /// <returns></returns>
        private Vector2 AngleToDirection(float angleDegree)
        {
            return new Vector2(Mathf.Sin(angleDegree * Mathf.Deg2Rad), Mathf.Cos(angleDegree * Mathf.Deg2Rad));
        }

        /// <summary>
        /// get a threshold value based on current color space
        /// </summary>
        private float ConvertGammaValue(float gammaSpacedValue)
        {
            if(QualitySettings.activeColorSpace == ColorSpace.Linear)
            {
                return Mathf.GammaToLinearSpace(gammaSpacedValue);
            }
            else
                return gammaSpacedValue;
        }

        /// <summary>
        /// get a threshold value based on current color space
        /// </summary>
        private Vector4 ConvertGammaValue(Vector4 gammaSpacedVector)
        {
            if(QualitySettings.activeColorSpace == ColorSpace.Linear)
            {
                gammaSpacedVector.x = ConvertGammaValue(gammaSpacedVector.x);
                gammaSpacedVector.y = ConvertGammaValue(gammaSpacedVector.y);
                gammaSpacedVector.z = ConvertGammaValue(gammaSpacedVector.z);
                gammaSpacedVector.w = ConvertGammaValue(gammaSpacedVector.w);
                return gammaSpacedVector;
            }
            else
                return gammaSpacedVector;
        }
        
        /// <summary>
        /// Update the renderindex (pass) forthe next Draw
        /// </summary>
        /// <param name="v"></param>
        private void UpdateRenderIndex(int v)
        {
            _currentRenderIndex = v;
        }

        /// <summary>
        /// Auto set a float value on the renderpipeline
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        private void SetFloat(ShaderProperties.DefaultProperty property, float value, bool forcePixelShader = false)
        {
            if(_renderPipeline == RenderPipeline.SRP)
                _commandBuffer.SetGlobalFloat(property.id, value);
            else
                Shader.SetGlobalFloat(property.id, value);
        }

        /// <summary>
        /// Auto set a vector value on the renderpipeline
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        private void SetVector(ShaderProperties.DefaultProperty property, Vector4 value, bool forcePixelShader = false)
        {
            if(_renderPipeline == RenderPipeline.SRP)
                _commandBuffer.SetGlobalVector(property.id, value);
            else
                Shader.SetGlobalVector(property.id, value);
        }

        /// <summary>
        /// Auto set a vector value on the renderpipeline
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        private void SetVector(ShaderProperties.DefaultProperty property, Vector3 value, bool forcePixelShader = false)
        {
            if(_renderPipeline == RenderPipeline.SRP)
                _commandBuffer.SetGlobalVector(property.id, value);
            else
                Shader.SetGlobalVector(property.id, value);
        }

        /// <summary>
        /// Auto set a vector value on the renderpipeline
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        private void SetVector(ShaderProperties.DefaultProperty property, Vector2 value, bool forcePixelShader = false)
        {
            if(_renderPipeline == RenderPipeline.SRP)
                _commandBuffer.SetGlobalVector(property.id, value);
            else
                Shader.SetGlobalVector(property.id, value);
        }

        /// <summary>
        /// Auto set a texture on the renderpipeline, 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="rt"></param>
        /// <param name="forcePixelShader"></param>
        private void SetTexture(ShaderProperties.DefaultProperty property, RenderTarget rt, bool forcePixelShader = false)
        {
            if(_renderPipeline == RenderPipeline.SRP)
                _commandBuffer.SetGlobalTexture(property.id, rt.renderTargetIdentifier);
            else
                Shader.SetGlobalTexture(property.id, rt.renderTexture);
        }
        private void SetTexture(ShaderProperties.DefaultProperty property, Texture tex, bool forcePixelShader = false)
        {
            if(_renderPipeline == RenderPipeline.SRP)
                _commandBuffer.SetGlobalTexture(property.id, tex);
            else
                Shader.SetGlobalTexture(property.id, tex);
        }
        
        /// <summary>
        /// Setup for the next draw command
        /// </summary>
        /// <param name="variant"></param>
        /// <param name="renderDimension"></param>
        /// <param name="forcePixelShader"></param>
        private void PrepareDraw(int variant, RenderDimension renderDimension)
        {
            UpdateRenderIndex(variant);
            DisableRenderKeywords();
            foreach(MaterialKeywords kw in _renderKeywordsBundle)
                SetKeyword(kw, true);
            _renderKeywordsBundle.Clear();
        }

        /// <summary>
        /// Draw into a destination framebuffer based on shadertype
        /// Always prepare for drawing using the PrepareDraw command
        /// </summary>
        /// <param name="forcePixelShader"></param>
        private void Draw()
        {
            if(_renderPipeline == RenderPipeline.SRP)
            {
                _commandBuffer.Draw(_renderTargetsBundle, _renderMaterialNoGeometry, _currentRenderIndex);
            }
            else
            {
                PipelineExtensions.Draw(_renderTargetsBundle, _renderMaterialNoGeometry, _currentRenderIndex);
            }
            _renderTargetsBundle.Clear();
        } 

        /////////////////////////////////////////////////////////////////////////////////////////////
        // Sampling
        /////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Disable render Keywords
        /// </summary>
        private void DisableRenderKeywords()
        {
            SetKeyword(MaterialKeywords.Bloom, false);
            SetKeyword(MaterialKeywords.LensSurface, false);
        }

        /// <summary>
        /// Disable debug Keywords
        /// </summary>
        private void DisableDebugKeywords()
        {
            SetKeyword(MaterialKeywords.DebugRawBloom, false);
            SetKeyword(MaterialKeywords.DebugBloom, false);
            SetKeyword(MaterialKeywords.DebugComposite, false);
        }
        
        /// <summary>
        /// Pre sample the glow map
        /// </summary>
        private void PreSample()
        {
            BeginProfileSample(PipelineProperties.CommandBufferProperties.samplePreSample);

            _bloomDownsampleBuffer.CreateTemporary(_renderContext, 0, _commandBuffer, _renderTextureFormat, _renderPipeline);
            _renderKeywordsBundle.Add(MaterialKeywords.Bloom);
            _renderTargetsBundle.Add(_bloomDownsampleBuffer.renderTargets[0]);
            
            PrepareDraw
            (
                (int)ShaderRenderPass.Presample,
                _renderContext[0].renderDimension
            );

            if(_settings.workflow == Workflow.Selective)
                SetTexture(PipelineProperties.ShaderProperties.sourceTex, _selectiveRenderTarget.renderTexture);
            else
                SetTexture(PipelineProperties.ShaderProperties.sourceTex, sourceFrameBuffer);
            
            Draw();

            if(_settings.workflow == Workflow.Selective)
                RenderTexture.ReleaseTemporary(_selectiveRenderTarget.renderTexture);

            EndProfileSample(PipelineProperties.CommandBufferProperties.samplePreSample);
        }

        /// <summary>
        /// Downsample the glow map
        /// </summary>
        private void Downsample()
        {
            BeginProfileSample(PipelineProperties.CommandBufferProperties.sampleDownsample);

            bool enableBloom;
            for(int i = 0; i < _minIterations; i++)
            {
                enableBloom = i < _bloomIterations;

                if(enableBloom)
                {
                    _bloomDownsampleBuffer.CreateTemporary(_renderContext, i + 1, _commandBuffer, _renderTextureFormat, _renderPipeline);
                    _renderKeywordsBundle.Add(MaterialKeywords.Bloom);
                    _renderTargetsBundle.Add(_bloomDownsampleBuffer.renderTargets[i + 1]);
                }

                PrepareDraw
                (   
                    (int)ShaderRenderPass.Downsample,
                    _renderContext[i + 1].renderDimension
                );
                    
                if(enableBloom)
                {
                    SetTexture(PipelineProperties.ShaderProperties.bloomTex, _bloomDownsampleBuffer.renderTargets[i]);
                }

                Draw();
            }
            EndProfileSample(PipelineProperties.CommandBufferProperties.sampleDownsample);
        }


        /// <summary>
        /// Upsample the glow map
        /// </summary>
        private void Upsample()
        {
            BeginProfileSample(PipelineProperties.CommandBufferProperties.sampleUpsample);

            bool enableBloom;
            for(int i = _minIterations; i > 0; i--)
            {   
                enableBloom = i <= _bloomIterations;

                if(enableBloom)
                {
                    _bloomUpsampleBuffer.CreateTemporary(_renderContext, i - 1, _commandBuffer, _renderTextureFormat, _renderPipeline);
                    _renderKeywordsBundle.Add(MaterialKeywords.Bloom);
                    _renderTargetsBundle.Add(_bloomUpsampleBuffer.renderTargets[i - 1]);
                }
                
                PrepareDraw
                (   
                    (int)ShaderRenderPass.Upsample, 
                    _renderContext[i - 1].renderDimension
                );

                if(enableBloom)
                {
                    SetTexture(PipelineProperties.ShaderProperties.higherMipBloomTex, _bloomDownsampleBuffer.renderTargets[i - 1]);
                    SetTexture(PipelineProperties.ShaderProperties.bloomTex, (i >= _bloomIterations) ? _bloomDownsampleBuffer.renderTargets[i] : _bloomUpsampleBuffer.renderTargets[i]);
                }

                Draw();

                if(enableBloom)
                {
                    if(i >= _bloomIterations)
                        _bloomDownsampleBuffer.ClearTemporary(_commandBuffer, i, _renderPipeline);
                    else
                    {
                        _bloomDownsampleBuffer.ClearTemporary(_commandBuffer, i, _renderPipeline);
                        _bloomUpsampleBuffer.ClearTemporary(_commandBuffer, i, _renderPipeline);
                    }
                }
            }

            _bloomDownsampleBuffer.ClearTemporary(_commandBuffer, 0, _renderPipeline);

            EndProfileSample(PipelineProperties.CommandBufferProperties.sampleUpsample);
        }

        /// <summary>
        /// Precomposite of the glow map
        /// </summary>
        private void Composite()
        {
            BeginProfileSample(PipelineProperties.CommandBufferProperties.sampleComposite);

            int renderpass;
            
            switch(_settings.debugView)
            {
                case DebugView.RawBloom:
                    _renderKeywordsBundle.Add(MaterialKeywords.DebugRawBloom);
                    renderpass = (int)ShaderRenderPass.Debug;
                break;
                case DebugView.Bloom:
                    _renderKeywordsBundle.Add(MaterialKeywords.DebugBloom);
                    renderpass = (int)ShaderRenderPass.Debug;
                break;
                case DebugView.Composite:
                    if(_useLensSurface)
                    {
                        _renderKeywordsBundle.Add(MaterialKeywords.LensSurface);
                    }
                    _renderKeywordsBundle.Add(MaterialKeywords.DebugComposite);
                    renderpass = (int)ShaderRenderPass.Debug;
                break;
                default:
                    if(_useLensSurface)
                    {
                        _renderKeywordsBundle.Add(MaterialKeywords.LensSurface);
                    }
                    renderpass = (int)ShaderRenderPass.Composite;
                break;
            }

            PrepareDraw
            (   
                renderpass,
                _sourceContext[0].renderDimension
            );

            if(_settings.workflow == Workflow.Selective && (_settings.debugView == DebugView.RawBloom))
                SetTexture(PipelineProperties.ShaderProperties.sourceTex, sourceFrameBuffer.renderTexture, true);
            else
            {
                SetTexture(PipelineProperties.ShaderProperties.sourceTex, sourceFrameBuffer, true);
                SetTexture(PipelineProperties.ShaderProperties.bloomTex, _bloomUpsampleBuffer.renderTargets[0], true);
            }

            if(_useLensSurface)
            {
                SetTexture(PipelineProperties.ShaderProperties.lensSurfaceDirtTex, _settings.lensSurfaceDirtTexture ? _settings.lensSurfaceDirtTexture : _resources.lensSurfaceDirtTextureDefault, true);
                SetTexture(PipelineProperties.ShaderProperties.lensSurfaceDiffractionTex, _settings.lensSurfaceDiffractionTexture ? _settings.lensSurfaceDiffractionTexture : _resources.lensSurfaceDiffractionTextureDefault, true);
            }

            //Dont draw when using legacy render pipeline
            if(_renderPipeline == RenderPipeline.SRP)
            {
                _renderTargetsBundle.Add(_destinationFrameBuffer);
                Draw();
                AfterCompositeCleanup();
            }
            else
            {
                PipelineExtensions.SetKeyword(_shaderKeywords[(int)MaterialKeywords.LegacyBlit].name, true);
                _renderTargetsBundle.Clear();
            }

            EndProfileSample(PipelineProperties.CommandBufferProperties.sampleComposite);
        }

        /// <summary>
        /// This cleans up the final render step
        /// </summary>
        internal void AfterCompositeCleanup()
        {
            _bloomUpsampleBuffer.ClearTemporary(_commandBuffer, 0, _renderPipeline);

            DisableDebugKeywords();
            DisableRenderKeywords();

            if(_renderPipeline == RenderPipeline.Legacy)
                PipelineExtensions.SetKeyword(_shaderKeywords[(int)MaterialKeywords.LegacyBlit].name, false);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        // Enum / structs used for rendering
        /////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// Rendering passes for shaders
        /// </summary>
        internal enum ShaderRenderPass
        {
            Copy = 0,
            Presample = 1,
            Downsample = 2,
            Upsample = 3,
            Composite = 4,
            Debug = 5
        }

        /// <summary>
        /// Material keywords represented in the keyword holder
        /// </summary>
        internal enum MaterialKeywords
        {
            Bloom = 0,
            LensSurface = 1,
            DebugRawBloom = 4,
            DebugBloom = 7,
            DebugComposite = 10,
            LegacyBlit = 11
        }
        
        /// <summary>
        /// Keyword represented as with state
        /// </summary>
        internal struct Keyword
        {
            internal string name;
            internal bool enabled;

            internal Keyword(string name, bool enabled)
            {
                this.name = name;
                this.enabled = enabled;
            }
        }
    }
}