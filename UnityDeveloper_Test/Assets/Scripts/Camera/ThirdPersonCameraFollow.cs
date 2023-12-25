
using UnityEngine;

namespace GravityGuy.CameraFollow
{
    public class ThirdPersonCameraFollow : MonoBehaviour
    {
        

        public Transform lookAt;
        public Transform Player;
        public float distance = 10.0f;
        public float sensivity = 4.0f;
        [SerializeField]
        private float currentX = 0.0f;
        [SerializeField]
        private float currentY = 0.0f;
        [SerializeField]
        private const float YMin = -50.0f;
        [SerializeField]
        private const float YMax = 50.0f;
        [SerializeField]
        private Quaternion playerRotation;
        
        private void LateUpdate()
        {
            currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
            currentY -= Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;

            currentY = Mathf.Clamp(currentY, YMin, YMax);

            Vector3 Direction = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, playerRotation.eulerAngles.z);
            
            transform.position = lookAt.position + rotation * Direction;
            transform.rotation =rotation;
            transform.LookAt(lookAt.position);
            
        }
        public void ChangezRotation(Quaternion TargetAngle)
        {
            playerRotation = TargetAngle;
        }
    }

}
