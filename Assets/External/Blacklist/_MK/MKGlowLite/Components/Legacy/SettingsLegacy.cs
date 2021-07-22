//////////////////////////////////////////////////////
// MK Glow Lite Settings Legacy	    	    	    //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Glow.Legacy
{
    internal sealed class SettingsLegacy : MK.Glow.Settings
    {
        public static implicit operator SettingsLegacy(MK.Glow.Legacy.MKGlowLite input)
        {
            SettingsLegacy settings = new SettingsLegacy();
            
            //Main
            settings.debugView = input.debugView;
            settings.workflow = input.workflow;
            settings.selectiveRenderLayerMask = input.selectiveRenderLayerMask;
            settings.anamorphicRatio = input.anamorphicRatio;

            //Bloom
            settings.bloomThreshold = input.bloomThreshold;
            settings.bloomScattering = input.bloomScattering;
            settings.bloomIntensity = input.bloomIntensity;

            //LensSurface
            settings.allowLensSurface = input.allowLensSurface;
            settings.lensSurfaceDirtTexture = input.lensSurfaceDirtTexture;
            settings.lensSurfaceDirtIntensity = input.lensSurfaceDirtIntensity;
            settings.lensSurfaceDiffractionTexture = input.lensSurfaceDiffractionTexture;
            settings.lensSurfaceDiffractionIntensity = input.lensSurfaceDiffractionIntensity;

            return settings;
        }
    }
}
