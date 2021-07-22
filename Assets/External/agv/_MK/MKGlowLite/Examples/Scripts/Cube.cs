//////////////////////////////////////////////////////
// MK Glow Cube                      				//
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
    public class Cube : MonoBehaviour
    {
        private readonly MinMaxRange _minMaxSpeed = new MinMaxRange(0.05f, 0.5f);
        [SerializeField]
        private MinMaxRange _emissionColorIntensity = new MinMaxRange(1.0f, 1.5f);
        private readonly MinMaxRange _colorChangeTime = new MinMaxRange(2f, 6f);
        private readonly MinMaxRange _colorIntensityChangeTime = new MinMaxRange(2f, 6f);
        private MinMaxRange _directionSwitchTime = new MinMaxRange(2.0f, 6.0f);

        private float _nextColorChangeTime = 0f;
        private float _nextColorIntensityChangeTime = 0f;
        private int _nextColorIndex = 0;
        private float _nextColorIntensity;
        private float _nextDirectionChangeTime = 0f;
        private float _currentSpeed;
        
        [SerializeField]
        private Color[] _colors = new Color[1];
        private Color _currentColor = new Color();
        private float _currentColorIntensity = 1;

        private Material _baseMaterial;
        private Material _usedMaterial;

        private int _emissionColorId;
        private int _colorId;

        private Vector3 _startPos;
        [SerializeField]
        private Vector3 _endPos = Vector3.zero;

        private bool _forward = true;
        private Vector3 _forwardDirection;
        private Vector3 _targetPosition 
        {
            get
            {
                if(_forward)
                {
                    return _endPos;
                }
                else
                {
                    return _startPos;
                }
            }
        }

        private void Awake()
        {
            _emissionColorId = Shader.PropertyToID("_EmissionColor");
            _colorId = Shader.PropertyToID("_Color");

            _forward = true;
            _startPos = transform.position;
            _baseMaterial = new Material(GetComponent<Renderer>().material);
            _usedMaterial = new Material(_baseMaterial);
            GetComponent<Renderer>().material = _usedMaterial;
            _nextColorIndex = Random.Range(0, _colors.Length);
            _currentColor = _colors[_nextColorIndex];
            _currentColorIntensity = Random.Range(_emissionColorIntensity.minValue, _emissionColorIntensity.maxValue);
            _usedMaterial.SetColor(_emissionColorId, _currentColor * _currentColorIntensity);
            _usedMaterial.SetColor(_colorId, _currentColor * 0.5f);
            _forwardDirection = (_endPos - _startPos).normalized;
            _currentSpeed = Random.Range(_minMaxSpeed.minValue, _minMaxSpeed.maxValue);
        }

        private void Update()
        {
            if(Time.time > _nextDirectionChangeTime )
            {
                _forward = !_forward;
                _nextDirectionChangeTime += Random.Range(_directionSwitchTime.minValue, _directionSwitchTime.maxValue);
                _currentSpeed = Random.Range(_minMaxSpeed.minValue, _minMaxSpeed.maxValue);
            }
            else
            {
                if(Vector3.Distance(_targetPosition, transform.position) < 0.1f)
                {
                    _forward = !_forward;
                    _nextDirectionChangeTime += Random.Range(_directionSwitchTime.minValue, _directionSwitchTime.maxValue);
                    _currentSpeed = Random.Range(_minMaxSpeed.minValue, _minMaxSpeed.maxValue);
                }
            }
            if(_forward)
            {
                transform.position += _forwardDirection * Time.smoothDeltaTime;
                transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.smoothDeltaTime * _currentSpeed);
            }
            else
            {
                transform.position -= _forwardDirection * Time.smoothDeltaTime;
                transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.smoothDeltaTime * _currentSpeed);
            }
            if(Time.time > _nextColorChangeTime)
            {
                _nextColorChangeTime += Random.Range(_colorChangeTime.minValue, _colorChangeTime.maxValue);
                _nextColorIndex++;
                if(_nextColorIndex > _colors.Length - 1)
                    _nextColorIndex = 0;
            }
            if(Time.time > _nextColorIntensityChangeTime)
            {
                _nextColorIntensityChangeTime += Random.Range(_colorIntensityChangeTime.minValue, _colorIntensityChangeTime.maxValue);
                _nextColorIntensity = Random.Range(_emissionColorIntensity.minValue, _emissionColorIntensity.maxValue);
            }

            _currentColor = Color.Lerp(_currentColor, _colors[_nextColorIndex], Time.smoothDeltaTime);
            _currentColorIntensity = Mathf.Lerp(_currentColorIntensity, _nextColorIntensity, Time.smoothDeltaTime);

            _usedMaterial.SetColor(_emissionColorId, _currentColor * _currentColorIntensity);
            _usedMaterial.SetColor(_colorId, _currentColor * 0.5f);
        }
    }
}
