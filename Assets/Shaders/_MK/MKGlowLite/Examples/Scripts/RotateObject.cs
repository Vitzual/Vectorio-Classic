//////////////////////////////////////////////////////
// MK Glow Rotate Object             				//
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
    public class RotateObject : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _rotation = Vector3.zero;
        private readonly float _heightMovement = 0.125f;
        private readonly float _heightMovementSpeed = 1f;

        private Vector3 _startPosition;

        private void Awake()
        {
            _startPosition = transform.position;
        }

        void Update()
        {
            transform.Rotate(_rotation * Time.smoothDeltaTime);
            transform.position = _startPosition + Vector3.up * _heightMovement * Mathf.Sin(Time.time * _heightMovementSpeed);
        }
    }
}
