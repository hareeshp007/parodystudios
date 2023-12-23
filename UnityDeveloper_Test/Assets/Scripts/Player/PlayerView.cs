
using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator Playeranimator;
    public Transform Camerapos;
    [SerializeField]
    private CharacterController playerController;
    [SerializeField]
    private float horizontal;
    [SerializeField]
    private float vertical;
    [SerializeField]
    private int rotationSpeed;
    [SerializeField]
    private int speed;
    [SerializeField]
    private float Gravity;
    [SerializeField]
    private Vector3 velocity;
    [SerializeField]
    private float turnSmoothTime=0.1f;
    [SerializeField]
    private float targetAngle;
    [SerializeField]
    private float angle;

    float turnSmoothVelocity;

    private void Start()
    {
        playerController=GetComponent<CharacterController>();
        horizontal = 0;
        vertical = 0;
    }
    private void Update()
    {
        handleInput();
        movement();
        manualPhysics();
    }

    private void manualPhysics()
    {
        if (velocity.y < 10f)
        {
            velocity.y += Gravity * Time.deltaTime;
            playerController.Move(velocity * Time.deltaTime);
        }
        
    }

    private void handleInput()
    {
        vertical = 0;
        horizontal = 0;
        if (Input.GetKey(KeyCode.W))
            vertical = 1f;
        if (Input.GetKey(KeyCode.S))
            vertical = -1f;
        if (Input.GetKey(KeyCode.D))
            horizontal = 1f;
        if (Input.GetKey(KeyCode.A))
            horizontal = -1f;
    }

    private void movement()
    {
        Vector3 newpos = new Vector3(horizontal, 0f, vertical).normalized;
        float offsetangle = Camerapos.eulerAngles.y;
        if(Camerapos.eulerAngles.y <= 180)
        {
            offsetangle += 180;
        }
        else
        {
            offsetangle -= 180;
        }
        if (newpos.magnitude >= 0.1f)
        {
            targetAngle = Mathf.Atan2(newpos.x, newpos.z) * Mathf.Rad2Deg + offsetangle;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerController.Move(moveDir.normalized * -speed * Time.deltaTime);
            Playeranimator.SetFloat("speed", newpos.magnitude);
        }
        else
        {
            Playeranimator.SetFloat("speed", 0);
        }
        
    }


}
