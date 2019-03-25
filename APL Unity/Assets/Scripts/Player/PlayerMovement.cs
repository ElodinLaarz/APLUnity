using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Normal Movements Variables
    public float walkSpeed = 0;
    private float curSpeed = 0;

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
        rb.velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * curSpeed, 0.8f),
                                             Mathf.Lerp(0, Input.GetAxis("Vertical") * curSpeed, 0.8f));
        animator.SetFloat("Xspeed", rb.velocity.x);
        animator.SetFloat("Yspeed", rb.velocity.y);
    }
}
