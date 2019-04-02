using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walk/Run Speeds")]
    public float runSpeedMod = 1.5f;
    public float dashSpeedMod = 1.7f;

    private float walkSpeed = 0;
    private float maxSpeed;
    private float curSpeed = 0;

    private float dblClickRunTime = 0.1f;
    private float lastTimeH;
    private float lastTimeV;

    [Header("Dash Settings")]
    public float dashSpeed = 17f;
    public float dashBuildUp = 0.03f;
    public float dashSlowDown = 0.07f;
    public float fullSpeedDuration = 0.18f;
    private bool inDash = false;

    private float dashTimer;
    private Vector2 moveVelocity;
    private Vector2 dashVelocity;


    private Animator animator;
    private Rigidbody2D rb;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        // Avoids making the player run at the start
        lastTimeH = -dblClickRunTime;
        lastTimeV = -dblClickRunTime;

        walkSpeed = gameManager.playerStats.baseSpeed * gameManager.playerStats.dexterity;
        curSpeed = walkSpeed;
        maxSpeed = walkSpeed * runSpeedMod;
        dashSpeed = walkSpeed * dashSpeedMod;
    }


    private void Update()
    {
        //I don't want to have a run button, but it's the best I could do atm...
        if (Input.GetKeyDown("r"))
        {
            if (curSpeed == walkSpeed)
            {
                curSpeed = maxSpeed;
            }
            else
            {
                curSpeed = walkSpeed;
            }
        }
    }

    void FixedUpdate()
    {
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
        if (rb.velocity == Vector2.zero)
        {
            inDash = false;
            yield return null;
        }

        Vector2 desiredDashVelocity;
        while (dashTimer <= dashBuildUp)
        {
            desiredDashVelocity = rb.velocity.normalized;
            dashVelocity = new Vector2(Mathf.Lerp(desiredDashVelocity.x, desiredDashVelocity.x * dashSpeed, dashTimer / dashBuildUp),
                                                Mathf.Lerp(desiredDashVelocity.y, desiredDashVelocity.y * dashSpeed, dashTimer / dashBuildUp));
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(fullSpeedDuration);

        dashTimer = 0;
        while (dashTimer <= dashSlowDown)
        {
            desiredDashVelocity = rb.velocity.normalized * dashSpeed;
            dashVelocity = new Vector2(Mathf.Lerp(desiredDashVelocity.x, 0, dashTimer / dashSlowDown),
                                                Mathf.Lerp(desiredDashVelocity.y, 0, dashTimer / dashSlowDown));
            yield return new WaitForFixedUpdate();
        }

        dashVelocity = Vector2.zero;
        inDash = false;
        yield return null;
    }
}
