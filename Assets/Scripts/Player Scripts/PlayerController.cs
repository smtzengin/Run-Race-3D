
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 move;
    public float speed, jumpForce, gravity, verticalVelocity;
    private CharacterController charController;
    private bool wallSlide,turn;
    private Animator anim;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.instance.finish)
        {
            move = Vector3.zero;
            if (!charController.isGrounded)
                verticalVelocity -= gravity * Time.deltaTime;
            else
                verticalVelocity = 0;
            move.y = verticalVelocity;
            charController.Move(new Vector3(0, move.y * Time.deltaTime, 0));
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Dance"))
            {
                anim.SetTrigger("Dance");
                transform.eulerAngles = Vector3.up * 180;
            }
            return;
        }

        move = Vector3.zero;
        move = transform.forward;

        if (charController.isGrounded)
        {
            wallSlide = false;
            verticalVelocity = 0;
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Jump();
            }

            if (turn)
            {
                turn = false;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
            }
        }
        if (!wallSlide)
        {
            gravity = 30;
            verticalVelocity -= gravity * Time.deltaTime;
            anim.SetBool("WallSlide", true);
        }
        else
        {
            gravity = 15;
            verticalVelocity -= gravity * Time.deltaTime;
            anim.SetBool("WallSlide", false);
        }

        anim.SetBool("WallSlide", wallSlide);
        anim.SetBool("Grounded", charController.isGrounded);

        move.Normalize();
        move *= speed;

        move.y = verticalVelocity;
        charController.Move(move * Time.deltaTime);
        
    }

    void Jump()
    {
        verticalVelocity = jumpForce;
        anim.SetTrigger("Jump");
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!charController.isGrounded)
        {
            if(hit.collider.tag == "Wall" || hit.collider.tag == "Slide")
            {
                if (verticalVelocity < -.7f)
                    wallSlide = true;
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Jump();
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
                    wallSlide = false;
                }
            }
        }
        else
        {
            if(transform.forward != hit.collider.transform.up && hit.collider.tag == "Ground" && !turn)
                turn = true;
        }
    }
    
    
    
}
