using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    public float jumpSpeed;
    public float sensitivity;
    public float maxYAngle;

    Rigidbody rb;
    Transform thisTransform;
    Collider thisCollider;
    public PhysicMaterial slippery;
    public PhysicMaterial friction;
    bool isGrounded;

    float mousePosX;
    float mousePosY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        thisCollider = GetComponent<Collider>();
        thisTransform = transform;
        isGrounded = true;
        mousePosX = Input.GetAxis("Mouse X");
    }

    // Update is called once per frame
    void Update()
    {

        Move();
        //UpdateHealth();
    }


    public void Move()
    {

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = Vector3.up * rb.velocity.y + thisTransform.forward * rb.velocity.z + thisTransform.right * speed;
            thisCollider.material = slippery;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = Vector3.up * rb.velocity.y + thisTransform.forward * rb.velocity.z + thisTransform.right * -speed;
            thisCollider.material = slippery;
        }
        else
        {
            rb.velocity = Vector3.up * rb.velocity.y + thisTransform.forward * rb.velocity.z + thisTransform.right * rb.velocity.x;
            thisCollider.material = friction;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity = Vector3.up * rb.velocity.y + thisTransform.forward * speed + thisTransform.right * rb.velocity.x;
            thisCollider.material = slippery;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = Vector3.up * rb.velocity.y + thisTransform.forward * -speed + thisTransform.right * rb.velocity.x;
            thisCollider.material = slippery;
        }
        else
        {
            rb.velocity = Vector3.up * rb.velocity.y + thisTransform.forward * rb.velocity.z + thisTransform.right * rb.velocity.x;
            thisCollider.material = friction;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = Vector3.up * jumpSpeed + thisTransform.forward * rb.velocity.z + thisTransform.right * rb.velocity.x;
            thisCollider.material = slippery;
            isGrounded = false;
        }
        else
        {
            rb.velocity = Vector3.up * rb.velocity.y + thisTransform.forward * rb.velocity.z + thisTransform.right * rb.velocity.x;
            thisCollider.material = friction;
        }

        mousePosX += Input.GetAxis("Mouse X") * sensitivity;

        float yRot = Mathf.Repeat(mousePosX, 360);
        //float xRot = Mathf.Clamp(Camera.main.transform.rotation.x - Input.GetAxis("Mouse Y") * sensitivity, -maxYAngle, maxYAngle);
        thisTransform.rotation = Quaternion.Euler(0, yRot, 0);
        //Camera.main.transform.rotation = Quaternion.Euler(xRot, 0, 0);

        if (Input.GetMouseButtonDown(0))
            Cursor.lockState = CursorLockMode.Locked;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

}
