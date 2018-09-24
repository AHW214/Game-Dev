using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Controller2D))]
    public class Player : MonoBehaviour
    {
        Controller2D controller;

        private void Start()
        {
            controller = GetComponent<Controller2D>();
        }

        private void Update()
        {

        }
    }
}

