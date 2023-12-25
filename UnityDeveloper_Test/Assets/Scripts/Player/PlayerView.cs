
using Cinemachine;
using Cinemachine.Utility;
using GravityGuy.CameraFollow;
using UnityEngine;

namespace GravityGuy.Player
{

    public class PlayerView : MonoBehaviour
    {
        [Header("Movement")]
        public Animator Playeranimator;
        public Transform Camerapos;
        [SerializeField]
        private CharacterController playerController;
        [SerializeField]
        private float horizontal;
        [SerializeField]
        private float vertical;
        [SerializeField]
        private int speed;

        [Header("Rotation")]
        [SerializeField]
        private int rotationSpeed;
        [SerializeField]
        private float turnSmoothTime = 0.1f;
        [SerializeField]
        private float targetAngle;
        [SerializeField]
        private float angle;

        [Header("Jump")]
        [SerializeField]
        private float gravity;
        [SerializeField]
        private Vector3 velocity;
        public Transform GroundCheck;
        [SerializeField]
        private LayerMask groundLayer;
        [SerializeField]
        private float groundCheckRadius;
        [SerializeField]
        private float jumpHeight;
        [SerializeField]
        private bool setGroundGravity;
        [SerializeField]
        private bool isGrounded;

        [Header("Gravity Change")]

        [SerializeField]
        private Direction currDirection;
        [SerializeField]
        private float defaultGravity;
        [SerializeField]
        private Quaternion targetRotation;
        [SerializeField]
        private float correctionAngle;
        [SerializeField]
        private Vector3 gravityDirection;

        [Header("HoloGram Change")]
        public GameObject Hologram;
        public Transform HeadPoint;
        [SerializeField]
        private Quaternion headRotation;
        float turnSmoothVelocity;
        


        private void Start()
        {
            playerController = GetComponent<CharacterController>();
            horizontal = 0;
            vertical = 0;
            gravityDirection =- Vector3.up;
            //ChangeDirection(Direction.Down);
            
        }
        private void Update()
        {
            handleInput();
            movement();
            manualPhysics();
            playerAnimation();
            changeGravity();
            changeHologram();
        }

        private void changeGravity()
        {
            if (isGrounded)
            {
                if ((transform.up == Vector3.up && velocity.y < 0) || (transform.up == Vector3.down && velocity.y > 0) ||
                    (transform.up == Vector3.forward && velocity.z < 0) || (transform.up == Vector3.forward && velocity.z > 0) ||
                    (transform.up == Vector3.right && velocity.x < 0) || (transform.up == Vector3.left && velocity.x > 0))
                {
                    velocity = gravityDirection * defaultGravity;
                    Debug.Log(transform.up + "   " + Vector3.up +"   " + velocity);
                }

            }
            velocity += gravityDirection * gravity * Time.deltaTime;
            playerController.Move(velocity * Time.deltaTime);
        }

        private void playerAnimation()
        {
            Playeranimator.SetBool("OnGround", isGrounded);
            if (!isGrounded)
            {
                Playeranimator.SetTrigger("OnAir");
            }
        }

        private void manualPhysics()
        {
            isGrounded = Physics.CheckSphere(GroundCheck.position, groundCheckRadius, groundLayer);
        }

        private void handleInput()
        {
            vertical = 0;
            horizontal = 0;
            if (Input.GetKey(KeyCode.W)) vertical = 1f;
            if (Input.GetKey(KeyCode.S)) vertical = -1f;
            if (Input.GetKey(KeyCode.D)) horizontal = 1f;
            if (Input.GetKey(KeyCode.A)) horizontal = -1f;
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) jump();
        }

        private void jump()
        {
            Playeranimator.SetTrigger("OnAir");
            float jumpforcesq = jumpHeight * defaultGravity * gravity;
            float jumpforce = Mathf.Sqrt(jumpforcesq);
            Debug.Log(transform.up * jumpforce + "   " + jumpforcesq);
            velocity = transform.up * jumpforce;
            playerController.Move(velocity * Time.deltaTime);
        }

        private void movement()
        {
            Vector3 newpos = new Vector3(horizontal, 0f, vertical).normalized;
            float offsetangle = Camerapos.eulerAngles.y;
            if (Camerapos.eulerAngles.y <= 180) offsetangle += 180;
            else offsetangle -= 180;
            if (newpos.magnitude >= 0.1f)
            {
                targetAngle = Mathf.Atan2(newpos.x, newpos.z) * Mathf.Rad2Deg + offsetangle;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                if(currDirection == Direction.Left || currDirection==Direction.Right)
                {
                    transform.rotation = Quaternion.Euler( angle,0f, transform.rotation.eulerAngles.z);
                }else transform.rotation = Quaternion.Euler(0f, angle, transform.rotation.eulerAngles.z);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                playerController.Move(moveDir.normalized * -speed * Time.deltaTime);
                Playeranimator.SetFloat("speed", newpos.magnitude);
            }
            else Playeranimator.SetFloat("speed", 0);
        }
        
        public void ChangeDirection(Direction direction)
        {
            velocity = Vector3.zero;
            currDirection = direction;
            switch (direction)
            {
                case Direction.Left:
                    gravityDirection = Vector3.right;
                    break;
                case Direction.Right:
                    gravityDirection = Vector3.left;
                    break;
                case Direction.Up:
                    gravityDirection = Vector3.up;
                    break;
                case Direction.Down:
                    gravityDirection = Vector3.down;
                    break;
            }
            transform.up = -gravityDirection;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
            other.gameObject.GetComponent<ICollectable>()?.OnCollect();
        }
        public void changeHologram()
        {
            if(HeadPoint.position!=Hologram.transform.position)
            {
                Hologram.transform.position = HeadPoint.position;
                Hologram.transform.rotation = Quaternion.Euler(HeadPoint.rotation.eulerAngles);
            }
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                headRotation = Quaternion.Euler(Vector3.right * 90f);
                Hologram.transform.rotation *= headRotation;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                headRotation = Quaternion.Euler(Vector3.right * -90f);
                Hologram.transform.rotation *= headRotation;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                headRotation = Quaternion.Euler(Vector3.forward * 90f);
                Hologram.transform.rotation *= headRotation;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                headRotation = Quaternion.Euler(Vector3.forward * -90f);
                Hologram.transform.rotation *= headRotation;
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log(Hologram.transform.up);
                transform.rotation = Quaternion.Euler( Hologram.transform.eulerAngles);
                gravityDirection = -transform.up;
            }
            
        }
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}