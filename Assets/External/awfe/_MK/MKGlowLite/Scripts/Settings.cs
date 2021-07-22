//////////////////////////////////////////////////////
// MK Glow Settings 	    	    	       		//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Glow
{
    //The default settings for each pipeline is set in the script itself
    //this could be optimized some day...
    //Used for passing user based settings into the pipeline
    internal abstract class Settings
    {
        //Main
        private MK.Glow.DebugView _debugView;
        internal MK.Glow.DebugView debugView
        { 
            get { return _debugView; }
            set { _debugView = value; }
        }

        private MK.Glow.Workflow _workflow;
        internal MK.Glow.Workflow workflow
        { 
            get { return _workflow; }
            set { _workflow = value; }
        }

        private LayerMask _selectiveRenderLayerMask;
        internal LayerMask selectiveRenderLayerMask
        { 
            get { return _selectiveRenderLayerMask; }
            set { _selectiveRenderLayerMask = value; }
        }

        private float _anamorphicRatio;
        internal float anamorphicRatio
        { 
            get { return _anamorphicRatio; }
            set { _anamorphicRatio = Mathf.Clamp(value, -1f, 1f); }
        }

        //Bloom
		private MK.Glow.MinMaxRange _bloomThreshold;
		internal MK.Glow.MinMaxRange bloomThreshold
		{ 
			get { return _bloomThreshold; }
			set { _bloomThreshold = value; }
		}

		private float _bloomScattering;
		internal float bloomScattering
		{ 
			get { return _bloomScattering; }
			set { _bloomScattering = Mathf.Clamp(value, 0f, 10f); }
		}
        
		private float _bloomIntensity;
		internal float bloomIntensity
		{ 
			get { return _bloomIntensity; }
			set { _bloomIntensity = Mathf.Max(0, value); }
		}

        //LensSurface
		private bool _allowLensSurface;
		internal bool allowLensSurface
		{ 
			get { return _allowLensSurface; }
			set { _allowLensSurface = value; }
		}

		private Texture2D _lensSurfaceDirtTexture;
		internal Texture2D lensSurfaceDirtTexture
		{ 
			get { return _lensSurfaceDirtTexture; }
			set { _lensSurfaceDirtTexture = value; }
		}

		private float _lensSurfaceDirtIntensity;
		internal float lensSurfaceDirtIntensity
		{ 
			get { return _lensSurfaceDirtIntensity; }
			set { _lensSurfaceDirtIntensity = Mathf.Max(0f, value); }
		}

        private Texture2D _lensSurfaceDirtDistortionTexture;
		internal Texture2D lensSurfaceDirtDistortionTexture
		{ 
			get { return _lensSurfaceDirtDistortionTexture; }
			set { _lensSurfaceDirtDistortionTexture = value; }
		}

        private float _lensSurfaceDirtDistortion;
		internal float lensSurfaceDirtDistortion
		{ 
			get { return _lensSurfaceDirtDistortion; }
			set { _lensSurfaceDirtDistortion = Mathf.Max(0f, value); }
		}

		private Texture2D _lensSurfaceDiffractionTexture;
		internal Texture2D lensSurfaceDiffractionTexture
		{ 
			get { return _lensSurfaceDiffractionTexture; }
			set { _lensSurfaceDiffractionTexture = value; }
		}

		private float _lensSurfaceDiffractionIntensity;
		internal float lensSurfaceDiffractionIntensity
		{ 
			get { return _lensSurfaceDiffractionIntensity; }
			set { _lensSurfaceDiffractionIntensity = Mathf.Max(0f, value); }
		}
    }
}
