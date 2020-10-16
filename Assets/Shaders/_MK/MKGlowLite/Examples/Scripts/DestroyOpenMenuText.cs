//////////////////////////////////////////////////////
// MK Glow Destroy With Space         				//
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
    public class DestroyOpenMenuText : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text _text = null;

        [SerializeField]
        private UnityEngine.UI.Outline _outline = null;

        private float _blinkSpeed = 2;

        private float _alpha = 1;
        private Color _textColor = Color.white;
        private Color _outlineColor = new Color(0, 0, 0, 1);
        private bool _forward = false;

        private void Update()
        {
            if(_forward)
            {
                _alpha += Time.smoothDeltaTime * _blinkSpeed;
                if(_alpha >= 1)
                    _forward = !_forward;
            }
            else
            {
                _alpha -= Time.smoothDeltaTime * _blinkSpeed;
                if(_alpha <= 0)
                    _forward = !_forward;
            }

            _textColor.a = _alpha;
            _outlineColor.a = _alpha;

            _text.color = _textColor;
            _outline.effectColor = _outlineColor;

            if(Input.GetKeyDown(KeyCode.Space) || Time.time > 10)
                Destroy(this.gameObject);
        }
    }
}
