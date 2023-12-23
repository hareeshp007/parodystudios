
using Cinemachine;
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
        private float gravity;
        [SerializeField]
        private Vector3 velocity;
        [SerializeField]
        private float turnSmoothTime = 0.1f;
        [SerializeField]
        private float targetAngle;
        [SerializeField]
        private float angle;

        [Header("Jump")]
        public Transform GroundCheck;
        [SerializeField]
        private LayerMask groundLayer;
        [SerializeField]
        private float groundCheckRadius;
        [SerializeField]
        private float jumpHeight;

        [Header("Gravity Change")]
        public CinemachineFreeLook freeLook;
        [SerializeField]
        private Direction currDirection;
        [SerializeField]
        private float defaultGravity;
        [SerializeField]
        private Quaternion targetRotation;
        [SerializeField]
        private float correctionAngle;

        float turnSmoothVelocity;
        bool isGrounded;


        private void Start()
        {
            playerController = GetComponent<CharacterController>();
            horizontal = 0;
            vertical = 0;
        }
        private void Update()
        {
            handleInput();
            movement();
            manualPhysics();
            playerAnimation();
            changeGravity();
        }

        private void changeGravity()
        {
            switch (currDirection)
            {
                case Direction.Left:
                    if (isGrounded && velocity.x < 0) velocity.x = defaultGravity;
                    else velocity.x += gravity * Time.deltaTime;
                    break;
                case Direction.Right:
                    if (isGrounded && velocity.x < 0) velocity.x = defaultGravity;
                    else velocity.x += -gravity * Time.deltaTime;
                    break;
                case Direction.Up:
                    if (isGrounded && velocity.x < 0) velocity.x = defaultGravity;
                    else velocity.y += -gravity * Time.deltaTime;
                    break;
                case Direction.Down:
                    if (isGrounded && velocity.x < 0) velocity.x = defaultGravity;
                    else velocity.y += gravity * Time.deltaTime;
                    break;
            }
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
            if (Input.GetKeyDown(KeyCode.Space)) jump();
            if (Input.GetKeyDown(KeyCode.UpArrow)) ChangeDirection(Direction.Up);
            if (Input.GetKeyDown(KeyCode.DownArrow)) ChangeDirection(Direction.Down);
            if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeDirection(Direction.Left);
            if (Input.GetKeyDown(KeyCode.RightArrow)) ChangeDirection(Direction.Right);
            if (Input.GetKeyUp(KeyCode.UpArrow)) ChangeDirection(Direction.Up);
            if (Input.GetKeyUp(KeyCode.DownArrow)) ChangeDirection(Direction.Down);
            if (Input.GetKeyUp(KeyCode.LeftArrow)) ChangeDirection(Direction.Left);
            if (Input.GetKeyUp(KeyCode.RightArrow)) ChangeDirection(Direction.Right);
        }

        private void jump()
        {
            Playeranimator.SetTrigger("OnAir");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

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
                    correctionAngle = -90f;
                    break;
                case Direction.Right:
                    correctionAngle = 90f;
                    break;
                case Direction.Up:
                    correctionAngle = 180f;
                    break;
                case Direction.Down:
                    correctionAngle = 0f;
                    break;
            }
            targetRotation = Quaternion.Euler(0f, 0f, correctionAngle);
            transform.rotation = targetRotation;
            Debug.Log(Camerapos);
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