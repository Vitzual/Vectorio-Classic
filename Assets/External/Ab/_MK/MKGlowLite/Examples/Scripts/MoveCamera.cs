//////////////////////////////////////////////////////
// MK Glow Move Camera               				//
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
    public class MoveCamera : MonoBehaviour
    {
        [SerializeField]
        private float _distance = 20.0f;
        [SerializeField]
        private float _horizontalSpeed = 10;
        [SerializeField]
        private float _verticalSpeed = 5;
        [SerializeField]
        private Transform _centerTransform = null;

        private void Update()
        {
            transform.position = _centerTransform.position + new Vector3(Mathf.Sin(Time.time * Mathf.Deg2Rad * _horizontalSpeed), Mathf.Sin(Time.time * Mathf.Deg2Rad * _verticalSpeed), Mathf.Cos(Time.time * Mathf.Deg2Rad * _horizontalSpeed)) * Mathf.Abs(Mathf.Sin(Time.time * 0.125f)) * _distance;
            transform.LookAt(_centerTransform);
        }
    }
}
