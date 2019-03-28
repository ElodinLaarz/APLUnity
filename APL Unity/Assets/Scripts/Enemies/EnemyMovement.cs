using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Repurposed the player dash as crude enemy movement.
    [Header("Enemy Dash Settings")]
    public float dashSpeed = 17f;
    public float dashAnimTime = 0.03f;

    // Cooldown is distinct from the player dash
    public float dashCooldownMin = 0.1f;
    public float dashCooldownMax = 0.8f;
    public float fullSpeedDuration = 0.21f;

    private float dashCooldown;
    private bool inDash = false;

    private float dashTimer;
    private Vector2 dashVelocity;
    private Vector2 desiredDashVelocity;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!inDash)
        {
            inDash = true;
            dashTimer = 0;
            StartCoroutine(Dash());
        }
        dashTimer += Time.deltaTime;

        rb.velocity = dashVelocity;
    }
    IEnumerator Dash()
    {
        while (dashTimer <= dashAnimTime)
        {
            desiredDashVelocity = new Vector2(GameManager.instance.player.transform.position.x - transform.position.x, GameManager.instance.player.transform.position.y - transform.position.y);
            desiredDashVelocity.Normalize();
            dashVelocity = new Vector2(Mathf.Lerp(desiredDashVelocity.x, desiredDashVelocity.x * dashSpeed, dashTimer / dashAnimTime),
                                                Mathf.Lerp(desiredDashVelocity.y, desiredDashVelocity.y * dashSpeed, dashTimer / dashAnimTime));
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(fullSpeedDuration);

        dashTimer = 0;
        while (dashTimer <= 2 * dashAnimTime)
        {
            desiredDashVelocity = rb.velocity.normalized * dashSpeed;
            dashVelocity = new Vector2(Mathf.Lerp(desiredDashVelocity.x, 0, dashTimer / (2* dashAnimTime)),
                                                Mathf.Lerp(desiredDashVelocity.y, 0, dashTimer / (2* dashAnimTime)));
            yield return new WaitForFixedUpdate();
        }

        dashVelocity = Vector2.zero;
        dashCooldown = Random.Range(dashCooldownMin, dashCooldownMax);
        yield return new WaitForSeconds(dashCooldown);
        inDash = false;
        yield return null;
    }
}
