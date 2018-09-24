using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Camera : MonoBehaviour
    {
        public Transform cameraSubject;
        public float trackingSpeed = 10.0F;
        
        private new UnityEngine.Camera camera;
        private Vector3 displacement;

        private void Start()
        {
            camera = GetComponent<UnityEngine.Camera>();
            camera.orthographicSize = Dimensions.orthographicSize;

            displacement = transform.position - cameraSubject.position;
        }
    
        private void Update()
        {
            Vector3 targetPosition = cameraSubject.position + displacement;
            Vector3 cameraPosition = Vector3.Lerp(transform.position, targetPosition, trackingSpeed * Time.deltaTime);

            transform.position = cameraPosition;         
        }
    }
}
