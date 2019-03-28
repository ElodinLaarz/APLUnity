using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walk/Run Speeds")]
    public float walkSpeed = 0;
    public float maxSpeed;
    private float curSpeed = 0;

    [Header("Dash Settings")]
    public float dashSpeed = 17f;
    public float dashBuildUp = 0.03f;
    public float dashSlowDown = 0.07f;
    public float fullSpeedDuration = 0.21f;
    private bool inDash = false;

    private float dashTimer;
    private Vector2 moveVelocity;
    private Vector2 dashVelocity;


    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        fullSpeedDuration -= dashBuildUp;

        // Future Implementations -- Increase Speed as 'speed items' are collected. E.g.
        // plStat = GetComponent<CharacterStat
        // walkSpeed = (float)(plStat.Speed + (plStat.Agility / 5));
        // sprintSpeed = walkSpeed + (walkSpeed / 2);

    }

    void FixedUpdate()
    {
        curSpeed = walkSpeed;
        moveVelocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * curSpeed, 0.8f),
                                         Mathf.Lerp(0, Input.GetAxis("Vertical") * curSpeed, 0.8f));

        if (Input.GetButtonDown("Dash") && !inDash)
        {
            // Have a GUI showing the next time the player can Dash
            inDash = true;
            dashTimer = 0;
            StartCoroutine(Dash());
        }
        dashTimer += Time.deltaTime;

        // Set total velocity and give Animator necessary parameters
        // Need to add Dash Animation
        rb.velocity = dashVelocity + moveVelocity;
        animator.SetFloat("Xspeed", rb.velocity.x);
        animator.SetFloat("Yspeed", rb.velocity.y);

    }

    IEnumerator Dash()
    {
        // Implement a default direction when the player isn't moving. E.g. in direction of mouse.
        if(rb.velocity == Vector2.zero)
        {
            inDash = false;
            yield return null;
        }

        Vector2 desiredDashVelocity;
        while (dashTimer <= dashBuildUp)
        {
            desiredDashVelocity = rb.velocity.normalized;
            dashVelocity = new Vector2(Mathf.Lerp(desiredDashVelocity.x, desiredDashVelocity.x*dashSpeed, dashTimer/dashBuildUp), 
                                                Mathf.Lerp(desiredDashVelocity.y, desiredDashVelocity.y * dashSpeed, dashTimer/dashBuildUp));
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(fullSpeedDuration);

        dashTimer = 0;
        while (dashTimer <= dashSlowDown)
        {
            desiredDashVelocity = rb.velocity.normalized * dashSpeed;
            dashVelocity = new Vector2(Mathf.Lerp(desiredDashVelocity.x, 0, dashTimer/dashSlowDown), 
                                                Mathf.Lerp(desiredDashVelocity.y, 0, dashTimer/dashSlowDown));
            yield return new WaitForFixedUpdate();
        }

        dashVelocity = Vector2.zero;
        inDash = false;
        yield return null;
    }
}
