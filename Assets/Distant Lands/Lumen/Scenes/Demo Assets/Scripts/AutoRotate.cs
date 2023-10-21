﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistantLands.Lumen.Demo
{
    [ExecuteAlways]
    public class AutoRotate : MonoBehaviour
    {


        public Vector3 rotateAngle;



        // Update is called once per frame
        void Update()
        {


            transform.eulerAngles += rotateAngle * Time.deltaTime;


        }
    }
}