//////////////////////////////////////////////////////
// MK Glow Asteroid Spawner         				//
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
    public class AsteroidSpawner : MonoBehaviour
    {
        private static readonly float _spawnTime = 0.125f;
        [SerializeField]
        private GameObject _asteroidObject = null;

        [SerializeField]
        private int _maxObjects = 0;

        private int _spawnedObjects = 0;

        private float _time = 0;

        private void Update()
        {
            if(_spawnedObjects < _maxObjects)
            {
                if(_time > _spawnTime)
                {
                    Instantiate(_asteroidObject, transform.position, Quaternion.identity);
                    ++_spawnedObjects;
                    _time = 0;
                }
                _time += Time.smoothDeltaTime;
            }
        }
    }
}
