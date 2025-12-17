using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    public Collider attackCollider;
    public Collider interactCollider;

    private float velocityZ = 0.0f;
    private float velocityX = 0.0f;

    public float acceleration = 1.0f;
    public float deceleration = 1.0f;

    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 1.0f;

    public float jumpForce = 3.0f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isJumping;
    private bool isGrounded;
    private bool isFalling;


    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = true;

        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;


        DisableAttackCollider();
        DisableInteractCollider();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        bool backPressed = Input.GetKey(KeyCode.S);
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);

        //Attack Logic
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.ResetTrigger("Attack");
        }

        //Jump Logic 
        float verticalVelocity = rb.linearVelocity.y;
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("verticalVelocity", verticalVelocity);

        //Jump Logic - Takeoff

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("JumpTakeOff");
        }

        //Jump Logic - Landing
        if (isGrounded && verticalVelocity <= 0)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("Jump_Airborne"))
            {
                //animator.Play("Jump_Landing");
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            animator.SetTrigger("Interact");
        }
        else
        {
            animator.ResetTrigger("Interact");
        }

        //Set current maxVelocity
        float currentMaxVelocity = runPressed ? maximumRunVelocity : maximumWalkVelocity;

        //Forward/Backward Movement in Z
        if (forwardPressed)
            velocityZ += Time.deltaTime * acceleration;
        else if (backPressed)
            velocityZ -= Time.deltaTime * acceleration;
        else
        {
            //Decelerate towards 0 when no input is given
            if (velocityZ > 0)
                velocityZ -= Time.deltaTime * deceleration;
            else if (velocityZ < 0)
                velocityZ += Time.deltaTime * deceleration;

            //Clamp near-zero at 0
            if (Mathf.Abs(velocityZ) < 0.01f)
                velocityZ = 0f;
        }

        //Clamp Z velocity
        velocityZ = Mathf.Clamp(velocityZ, -currentMaxVelocity, currentMaxVelocity);

        float speed = new Vector2(velocityX, velocityZ).magnitude;
        animator.SetFloat("VelocityX", velocityX > 0 ? Mathf.Clamp01(velocityX) : Mathf.Clamp(velocityX, -1.0f, 0.0f));
        animator.SetFloat("VelocityZ", velocityZ > 0 ? Mathf.Clamp01(velocityZ) : Mathf.Clamp(velocityZ, -1.0f, 0.0f));
        animator.SetFloat("Speed", speed);

        //Left/Right Movement in X
        if (leftPressed)
            velocityX -= Time.deltaTime * acceleration;
        else if (rightPressed)
            velocityX += Time.deltaTime * acceleration;
        else
        {
            //Decelerate towards 0 when no input
            if (velocityX > 0)
                velocityX -= Time.deltaTime * deceleration;
            else if (velocityX < 0)
                velocityX += Time.deltaTime * deceleration;

            //Clamp near-zero to 0
            if (Mathf.Abs(velocityX) < 0.01f)
                velocityX = 0f;
        }

        //Clamp to maximum values
        velocityX = Mathf.Clamp(velocityX, -currentMaxVelocity, currentMaxVelocity);

        //Apply velocities to Rigidbody
        //rb.linearVelocity = new Vector3(velocityX, rb.linearVelocity.y, velocityZ);

        //Prevent movement in air while jumping
        if (!isGrounded)
        {
            velocityZ = 0f;
        }

        //Apply velocities to Rigidbody
        //rb.linearVelocity = new Vector3(velocityX, rb.linearVelocity.y, velocityZ);

        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("verticalVelocity", rb.linearVelocity.y);
    }


    private void OnAnimatorMove()
    {
        rb.MovePosition(rb.position + animator.deltaPosition);
    }


    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }

    public void EnableInteractCollider() { interactCollider.enabled = true; }
    public void DisableInteractCollider() { interactCollider.enabled = false; }
}