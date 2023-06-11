using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {



        void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
