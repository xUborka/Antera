using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player_object;
    public GameObject weapon_object;
    public GameObject shield_object;
    public CharacterController2D controller;
    public float runSpeed = 100f;

    private Animator player_animator;
    private Animator weapon_animator;
    private Animator shield_animator;

    float horizontalMove = 0f;
    float verticalMove = 0f;


    void Awake(){
        player_animator = player_object.GetComponent<Animator>();
        weapon_animator = weapon_object.GetComponent<Animator>();
        shield_animator = shield_object.GetComponent<Animator>();
    }

    void Start()
    {
        runSpeed = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;
        if (Mathf.Abs(horizontalMove) > 0f || Mathf.Abs(verticalMove) > 0f){
            player_animator.SetBool("Moving", true);
            weapon_animator.SetBool("Moving", true);
            shield_animator.SetBool("Moving", true);
        } else {
            player_animator.SetBool("Moving", false);
            weapon_animator.SetBool("Moving", false);
            shield_animator.SetBool("Moving", false);
        }

        if (Input.GetButtonDown("Attack"))
		{
			player_animator.SetBool("Attack", true);
			weapon_animator.SetBool("Attack", true);
			shield_animator.SetBool("Attack", true);
		}
        else {
			player_animator.SetBool("Attack", false);
			weapon_animator.SetBool("Attack", false);
			shield_animator.SetBool("Attack", false);
        }
    }

    void FixedUpdate(){
        Debug.Log(horizontalMove);
        Vector2 speed_vector = Vector2.ClampMagnitude(new Vector2(horizontalMove, verticalMove), runSpeed * Time.deltaTime);
        controller.Move(speed_vector);
    }
}
