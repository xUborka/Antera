using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public GameObject player_object;
    public GameObject weapon_object;
    public CharacterController2D controller;
    public float runSpeed = 100f;
    public LayerMask enemy_layer;

    private Animator player_animator;
    private Animator weapon_animator;

    private CapsuleCollider2D weapon_collider;

    // public NetworkAnimator player_network_animator;
    public NetworkAnimator weapon_network_animator;

    float horizontalMove = 0f;
    float verticalMove = 0f;

    public void Awake(){
        player_animator = player_object.GetComponent<Animator>();
        weapon_animator = weapon_object.GetComponent<Animator>();
        weapon_collider = weapon_object.GetComponent<CapsuleCollider2D>();
        // LayerMask.NameToLayer("Enemy")
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

    public void Slash(Vector3 pos, float rad){
        // List<Collider2D> results = new List<Collider2D>();
        // ContactFilter2D filter = new ContactFilter2D();
        // filter.SetLayerMask(LayerMask.NameToLayer("Enemy"));
        // weapon_collider.OverlapCollider(filter, results);
        // Physics2D.OverlapCollider(weapon_collider, filter, results);
        // Debug.Log(results.Count);
        // foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")){
        //     obj.GetComponent<GoblinScript>().Hurt();
        // }
        Debug.Log("Slash");
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(pos, rad, enemy_layer))
        {
            Debug.Log(collider);
            collider.GetComponent<GoblinScript>().Hurt();
        }
    }

    public void FixedUpdate(){
        Vector2 speed_vector = Vector2.ClampMagnitude(new Vector2(horizontalMove, verticalMove), runSpeed * Time.deltaTime);
        controller.Move(speed_vector);
    }
}
