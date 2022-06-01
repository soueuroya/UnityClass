using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.W;
    public KeyCode slidingKey = KeyCode.LeftShift;
    public float lastTime = 0;
    public Zoom cameraZoom;

    public Rigidbody rb;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        // Get the rigidbody on this.
        rb = GetComponent<Rigidbody>();
        cameraZoom = GetComponentInChildren<Zoom>();
    }

    void Update()
    {
        // Update IsRunning from input.
        if (Input.GetKeyDown(runningKey))
        {
            if(Time.time - lastTime <= 0.5f)
            {
                lastTime = Time.time;
                IsRunning = true;
                StartCoroutine(SetZoom(0.05f));
            }
            else
            {
                IsRunning = false;
                lastTime = Time.time;
                StartCoroutine(SetZoom(0));
            }
        }
        else if (!Input.GetKey(runningKey))
        {
            StartCoroutine(SetZoom(0));
        }
        //IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        

        // Apply movement.
        rb.velocity = transform.rotation * new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);
    }

    IEnumerator SetZoom(float target)
    {
        while (Mathf.Abs(cameraZoom.currentZoom - target) > 0.05f)
        {
            if (target > cameraZoom.currentZoom)
            {
                cameraZoom.currentZoom += 0.005f;
            }
            else
            {
                cameraZoom.currentZoom -= 0.005f;
            }
            yield return new WaitForSeconds(0.001f);
        }
        cameraZoom.currentZoom = target;
    }

}