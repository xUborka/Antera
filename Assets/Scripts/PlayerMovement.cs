using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public GameObject player_object;
    public GameObject weapon_object;
    public CharacterController2D controller;
    public float runSpeed = 100f;

    private Animator player_animator;
    private Animator weapon_animator;

    // public NetworkAnimator player_network_animator;
    public NetworkAnimator weapon_network_animator;

    float horizontalMove = 0f;
    float verticalMove = 0f;

    public void Awake(){
        player_animator = player_object.GetComponent<Animator>();
        weapon_animator = weapon_object.GetComponent<Animator>();
    }

    void Start()
    {
        runSpeed = 100f;
    }

    // Update is called once per frame
    public void Update()
    {
        if (!isOwned){ return; }

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;
        if (Mathf.Abs(horizontalMove) > 0f || Mathf.Abs(verticalMove) > 0f){
            player_animator.SetBool("Moving", true);
            weapon_animator.SetBool("Moving", true);
        } else {
            player_animator.SetBool("Moving", false);
            weapon_animator.SetBool("Moving", false);
        }

        if (Input.GetButtonDown("Attack"))
		{
			weapon_network_animator.SetTrigger("Attack");
		}
    }

    public void Slash(){
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")){
            obj.GetComponent<GoblinScript>().Health -= 1;
        }
    }

    public void FixedUpdate(){
        Vector2 speed_vector = Vector2.ClampMagnitude(new Vector2(horizontalMove, verticalMove), runSpeed * Time.deltaTime);
        controller.Move(speed_vector);
    }
}
