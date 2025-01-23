using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{ 
    protected override void ApplyMovement()
    {
        if (IsGrounded(transform.position + new Vector3(direction.x, direction.y, 0) * hSpeed) || PlayerSpotted)
        {
            var force = direction * hSpeed;
            if (PlayerSpotted && rb.velocity.magnitude < 0.001)
            {
                rb.velocity = new Vector2(rb.velocity.x, hSpeed);
            }
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, force.x, Time.deltaTime * 100), rb.velocity.y);
            
        }
    }
}
