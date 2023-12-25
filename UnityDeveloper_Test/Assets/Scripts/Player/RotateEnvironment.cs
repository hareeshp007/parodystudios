
using UnityEngine;

public class RotateEnvironment : MonoBehaviour
{
    public Transform referenceObject;
    public float rotationSpeed = 5.0f;
    public Quaternion targetRotation;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (referenceObject != null)
            {
                targetRotation = referenceObject.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
