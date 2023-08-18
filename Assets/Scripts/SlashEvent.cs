using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEvent : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private CircleCollider2D sword_collider;
    private float radius = 0.6f;

    public void Awake(){
        sword_collider = GetComponent<CircleCollider2D>();
    }

    public void Slash(){
        Vector3 offset = gameObject.transform.position;
        offset.x += sword_collider.offset.x;
        offset.y += sword_collider.offset.y;
        playerMovement.Slash(offset, radius);
    }

    private void OnDrawGizmosSelected()
    {
        sword_collider = GetComponent<CircleCollider2D>();
        Gizmos.color = Color.blue;
        Vector3 offset = gameObject.transform.position;
        offset.x += sword_collider.offset.x;
        offset.y += sword_collider.offset.y;
        Gizmos.DrawWireSphere(offset, radius);
    }
}
