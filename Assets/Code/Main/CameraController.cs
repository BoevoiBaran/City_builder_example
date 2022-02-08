using UnityEngine;

namespace Code.Main
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera sessionCamera;
        [SerializeField] private float dragSpeed = 0.1f;
        
        private Vector3 dragOrigin;
        
        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
                return;
            }

            if (!Input.GetMouseButton(0))
            {
                return;
            }
 
            var position = sessionCamera.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
            var move = new Vector3(position.x * dragSpeed, 0, position.y * dragSpeed);
 
            transform.Translate(move, Space.World);  
        }
    }
}
