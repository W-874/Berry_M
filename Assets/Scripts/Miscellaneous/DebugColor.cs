using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Base;

namespace ObjectController
{
    [RequireComponent(typeof(SpriteController))]
    public class DebugColor : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<SpriteController>().SetAColor(0);
        }
    }
}
