using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 move;
    public float speed, jumpForce, gravity, verticalVelocity;
    private CharacterController charController;
    private bool wallSlide,turn, superJump;
    private Animator anim;

    private SkinnedMeshRenderer playerColor;
    public Material[] colors;
    
    private void Awake()
    {
        playerColor = GameObject.Find("PlayerColor").GetComponent<SkinnedMeshRenderer>();
        charController = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        gameObject.name = PlayerPrefs.GetString("PlayerName", "Player");

        playerColor.material = colors[PlayerPrefs.GetInt("PlayerColor", 0)];
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
        if (!GameManager.instance.start)
            return;

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
        if (superJump)
        {
            superJump = false;
            verticalVelocity = jumpForce * 1.75f;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                anim.SetTrigger("Jump");
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
                if (verticalVelocity < -.6f)
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
            if (hit.collider.tag == "Trampoline" && charController.isGrounded)
                superJump = true;

            if(transform.forward != hit.collider.transform.up && transform.forward != hit.transform.right 
                && hit.collider.tag == "Ground" && !turn)
                turn = true;
        }
    }
    
    
    
}
