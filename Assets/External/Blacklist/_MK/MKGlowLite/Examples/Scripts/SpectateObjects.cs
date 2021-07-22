//////////////////////////////////////////////////////
// MK Glow Spectate Objects         				//
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
    public class SpectateObjects : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _gameObjects = new GameObject[1];
        private int _currentObject = 0;

        public void SwitchObject()
        {
            _gameObjects[_currentObject++].SetActive(false);
            if(_currentObject > _gameObjects.Length - 1)
                _currentObject = 0;
            _gameObjects[_currentObject].SetActive(true);
        }
    }
}
