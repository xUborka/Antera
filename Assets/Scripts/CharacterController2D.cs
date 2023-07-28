using UnityEngine;
using Mirror;


public class CharacterController2D : NetworkBehaviour
{
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private bool m_FacingRight = true;
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;

    private void Awake(){
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector2 target_velocity)
    {
        if (!isOwned) { return; }

        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, target_velocity, ref m_Velocity, m_MovementSmoothing);

        // If the input is moving the player right and the player is facing left...
        if (target_velocity.x > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if (target_velocity.x < 0 && m_FacingRight)
        {
            Flip();
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
