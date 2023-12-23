
using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator Playeranimator;
    public Transform Camera;
    [SerializeField]
    private CharacterController playerController;
    [SerializeField]
    private Rigidbody playerrb;
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
    private float turnSmoothTime=0.1f;
    float turnSmoothVelocity;

    private void Start()
    {
        playerController=GetComponent<CharacterController>();
        playerrb = GetComponent<Rigidbody>();
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
        if (velocity.y < 50f)
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

        if (newpos.magnitude >= 0.1f)
        {
            Debug.Log(newpos.magnitude);
            float targetAngle = MathF.Atan2(newpos.x, newpos.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.rotation.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerController.Move(moveDir.normalized * speed * Time.deltaTime);
            Playeranimator.SetFloat("speed", newpos.magnitude);
        }
        else
        {
            Playeranimator.SetFloat("speed", 0);
        }
        
    }


}
