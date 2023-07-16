using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public CharacterController2D controller;
    public float runSpeed = 100f;

    float horizontalMove = 0f;
    float verticalMove = 0f;


    // Start is called before the first frame update
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
            animator.SetBool("Moving", true);
        } else {
            animator.SetBool("Moving", false);
        }

        if (Input.GetButtonDown("Attack"))
		{
			animator.SetBool("Attack", true);
		}
        else {
			animator.SetBool("Attack", false);
        }
    }

    void FixedUpdate(){
        Debug.Log(horizontalMove);
        Vector2 speed_vector = Vector2.ClampMagnitude(new Vector2(horizontalMove, verticalMove), runSpeed * Time.deltaTime);
        controller.Move(speed_vector);
    }
}
