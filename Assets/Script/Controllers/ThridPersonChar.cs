using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPersonChar : MonoBehaviour
{
    public Transform cam;
    public float speed = 6f;
    public float jumpSpeed;

    private CharacterController controller;
    private float ySpeed;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public LayerMask movementMask;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        controller.SimpleMove(direction * direction.magnitude);

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            ySpeed = jumpSpeed;
        }

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        Vector3 velocity = direction * direction.magnitude;
        velocity.y = ySpeed;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        other.GetComponent<ItemPickedUp>().PickUp();
    }
}