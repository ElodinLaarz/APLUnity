using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Normal Movements Variables
    public float walkSpeed = 0;
    private float curSpeed = 0;
    private bool inDash = false;
    private float timeInDash;
    private Vector2 dashDirection;
    private Vector2 moveVelocity;
    private Vector2 dashVelocity;
    public float dashSpeed;
    public float dashBuildUp;
    public float dashSlowDown;
    public float fullSpeedDuration;



    //Allows us to add the collider of our player to the movement script.
    public Animator animator;
    private Rigidbody2D rb;

    //private float sprintSpeed;
    public float maxSpeed;


    // Future Implementations -- Players Stats
    // public GameObject player;
    // private CharacterStat plStat;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        // Future Implementations -- Increase Speed as 'speed items' are collected.
        // plStat = GetComponent<CharacterStat>();

        // walkSpeed = (float)(plStat.Speed + (plStat.Agility / 5));
        // sprintSpeed = walkSpeed + (walkSpeed / 2);

    }

    void FixedUpdate()
    {
        curSpeed = walkSpeed;
        // maxSpeed = curSpeed;

        // Move senteces
        moveVelocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * curSpeed, 0.8f),
                                         Mathf.Lerp(0, Input.GetAxis("Vertical") * curSpeed, 0.8f));

        if (Input.GetKey("z") && !inDash)
        {
            dashDirection = rb.velocity;
            dashDirection.Normalize();
            inDash = true;
            timeInDash = 0;
        }

        if (inDash)
        {
            if (timeInDash < dashBuildUp)
            {
                dashVelocity = new Vector2(dashSpeed * (timeInDash / dashBuildUp) * dashDirection.x, dashSpeed * (timeInDash / dashBuildUp) * dashDirection.y);
            }
            else if (timeInDash < fullSpeedDuration)
            {
                dashVelocity = new Vector2(dashSpeed * dashDirection.x, dashSpeed * dashDirection.y);
                Debug.Log(dashVelocity);
            }
            else if (timeInDash < fullSpeedDuration + dashSlowDown)
            {
                dashVelocity = new Vector2(dashSpeed * ((fullSpeedDuration + dashSlowDown - timeInDash) / dashSlowDown) * dashDirection.x, dashSpeed * ((fullSpeedDuration + dashSlowDown - timeInDash) / dashSlowDown) * dashDirection.y);
            }
            else
            {
                inDash = false;
                dashVelocity = new Vector2(0, 0);
                Debug.Log("turned off");
            }
            timeInDash = timeInDash + Time.deltaTime;
        }

        rb.velocity = dashVelocity + moveVelocity;
        animator.SetFloat("Xspeed", rb.velocity.x);
        animator.SetFloat("Yspeed", rb.velocity.y);

    }
}
