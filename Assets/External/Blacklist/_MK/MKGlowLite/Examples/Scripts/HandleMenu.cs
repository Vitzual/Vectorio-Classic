//////////////////////////////////////////////////////
// MK Glow Handle Menu               				//
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
    public class HandleMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _canvas = null;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space) && Application.isPlaying)
                _canvas.SetActive(!_canvas.activeSelf);
        }
    }
}
