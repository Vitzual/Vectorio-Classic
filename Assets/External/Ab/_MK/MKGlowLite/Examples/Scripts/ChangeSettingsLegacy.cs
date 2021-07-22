//////////////////////////////////////////////////////
// MK Glow Change Settings Legacy        			//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2017 All rights reserved.            //
//////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Glow.Example
{
    public class ChangeSettingsLegacy : MonoBehaviour
    {
        private Legacy.MKGlowLite _mkGlow;

        private void Awake()
        {
            _mkGlow = GetComponent<Legacy.MKGlowLite>();
        }

        //Main
        public int debugView
        { 
            get { return (int)_mkGlow.debugView; }
            set { _mkGlow.debugView = (DebugView)value; }
        }
        public float anamorphicRatio
        { 
            get { return _mkGlow.anamorphicRatio; }
            set { _mkGlow.anamorphicRatio = value; }
        }

        //Bloom
        public float bloomThreshold
		{ 
			get { return _mkGlow.bloomThreshold.minValue; }
			set { _mkGlow.bloomThreshold.minValue = value; }
		}
        public float bloomClamp
		{ 
			get { return _mkGlow.bloomThreshold.maxValue; }
			set { _mkGlow.bloomThreshold.maxValue = value; }
		}
        public float bloomScattering
		{ 
			get { return _mkGlow.bloomScattering; }
			set { _mkGlow.bloomScattering = value; }
		}
        public float bloomIntensity
		{ 
			get { return _mkGlow.bloomIntensity; }
			set { _mkGlow.bloomIntensity = value; }
		}

        //Lens Surface
         public bool allowLensSurface
		{ 
			get { return _mkGlow.allowLensSurface; }
			set { _mkGlow.allowLensSurface = value; }
		}
        public float lensSurfaceDirtIntensity
		{ 
			get { return _mkGlow.lensSurfaceDirtIntensity; }
			set { _mkGlow.lensSurfaceDirtIntensity = value; }
		}
        public float lensSurfaceDiffractionIntensity
		{ 
			get { return _mkGlow.lensSurfaceDiffractionIntensity; }
			set { _mkGlow.lensSurfaceDiffractionIntensity = value; }
		}
    }
}