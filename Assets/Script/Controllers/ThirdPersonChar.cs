using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonChar : MonoBehaviour
{
    public Transform cam;

    public float moveSpeed = 5.0f;
    public float normalJumpHeight = 1.2f;
    public float superJumpHeight = 3.6f;
    public float gravity = -15.0f; //The character uses its own gravity value. The engine default is -9.81f
    public float jumpTimeout = 0.2f; //Time required to pass before being able to jump again. Set to 0f to instantly jump again
    public float fallTimeout = 0.15f; //Time required to pass before entering the fall state. Useful for walking down stairs

    [Header("Player Grounded")]
    public bool grounded = true;
    public float groundedOffset = 1.15f; //Useful for rough ground

    [Tooltip("Should match the radius of the CharacterController")]
    public float groundedRadius = 0.4f; //The radius of the grounded check.
    public LayerMask groundLayers; //What layers the character uses as ground

    // player
    private float speed;
    private float jumpHeight = 1.2f;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

    // timeout deltatime
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    // animation IDs
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;

    private CharacterController controller;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private bool rotateOnMOve = true;
    private Vector3 SpherePosition; //Check Grouned(with offset)

    private PlayerStats playerStats;
    private SwitchSkills switchSkill;
    private Collider npcCollider;

    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
        switchSkill = GetComponent<SwitchSkills>();

        AssignAnimationIDs();

        // reset our timeouts on start
        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;
    }

    void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();

        if (playerStats.currentHealth <= 0)
            Destroy(gameObject);

        if (Input.GetButtonDown("Talk"))
        {
            TalkToNPC();
        }
    }

    private void AssignAnimationIDs()
    {
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(
            transform.position.x,
            transform.position.y - groundedOffset,
            transform.position.z
        );
        SpherePosition = spherePosition;
        grounded = Physics.CheckSphere(
            spherePosition,
            groundedRadius,
            groundLayers,
            QueryTriggerInteraction.Ignore
        );

        anim.SetBool(animIDGrounded, grounded);
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(SpherePosition, groundedRadius);
    }

    private void Move()
    {
        speed = moveSpeed;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle =
                Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref turnSmoothVelocity,
                turnSmoothTime
            );
            if (rotateOnMOve)
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    private void JumpAndGravity()
    {
        if (switchSkill.currentSkill == 1)
        {
            jumpHeight = superJumpHeight;
        }
        else
        {
            jumpHeight = normalJumpHeight;
        }
        if (grounded)
        {
            // reset the fall timeout timer
            fallTimeoutDelta = fallTimeout;

            // update animator if using character
            anim.SetBool(animIDJump, false);
            anim.SetBool(animIDFreeFall, false);

            // stop our velocity dropping infinitely when grounded
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            // Jump
            if (Input.GetButtonDown("Jump") && jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                // update animator if using character
                anim.SetBool(animIDJump, true);
            }

            // jump timeout
            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            jumpTimeoutDelta = jumpTimeout;

            // fall timeout
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // update animator if using character
                anim.SetBool(animIDFreeFall, true);
            }
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        controller.Move(new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().CompareTag("Bullet"))
        {
            playerStats.TakeDamage(other.gameObject.GetComponent<Projectile>().damage);
        }
        if (other.GetComponent<Collider>().CompareTag("NPC"))
        {
            npcCollider = other;
        }
    }

    private void TalkToNPC()
    {
        npcCollider.GetComponent<DialogueTrigger>().StartConvo();
    }

    public void SetRotateOnMove(bool newRotateOnMove)
    {
        rotateOnMOve = newRotateOnMove;
    }
}
