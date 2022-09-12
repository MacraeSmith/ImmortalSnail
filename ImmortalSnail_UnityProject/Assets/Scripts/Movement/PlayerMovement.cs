using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    
    private float speed = 10;
    public float gravity = -20f;
    public float jumpHeight = 1.5f;

    public Transform groundCheck;
    public float groundDistance = 10f;
    public LayerMask groundMask;
   


    public Animator anim;
    //Rigidbody rb;

    Vector3 velocity;
    bool isGrounded;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(this.groundCheck.position + Vector3.down, 0.2f, groundMask);
        
        
        

        
        
        if (isGrounded)
        {
            Debug.Log("Is Grounded");
            if (velocity.y <0 && velocity.y > -50) 
            {
                velocity.y = -8f;
            }
           
            
            //this.anim.SetBool("IsGrounded", true);


            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            
            

            Vector3 move = this.transform.right * x + this.transform.forward * z; //MOVES PLAYER
            this.anim.SetFloat("vertical", x);
            this.anim.SetFloat("horizontal", z);
            
            if (Input.GetKey(KeyCode.LeftShift)) //SPRINT WHEN SHIFT IS HELD
            {
                //this.anim.SetBool("IsSprinting", true);
                
                controller.Move((move * 3f) * (speed) * Time.deltaTime);

            }
            else
            {
                //this.anim.SetBool("IsSprinting", false);
                
                controller.Move(move * speed * Time.deltaTime);


            }
             

            
            //if(move != Vector3.zero) //TELLS ANIMATOR TO PLAY MOVEMENT ANIMATION
            //{
                //anim.SetBool("IsMoving", true);
            //}
            //else
            //{
                //anim.SetBool("IsMoving", false);
            //}
        }
        
       
        else //ALLOWS MOVEMENT IN AIR BUT DOES NOT PLAY MOVEMENT ANIMATION
        {
            //this.anim.SetBool("IsGrounded", false);
            //this.anim.SetBool("IsSprinting", false);
            

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");



            Vector3 move = this.transform.right * x + this.transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
        }


       
        
        
        if(Input.GetButtonDown("Jump") && (isGrounded)) //Jump
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -8f * gravity);
            //this.anim.SetTrigger("JumpTrigger");
            //this.anim.SetBool("IsGrounded", false);


        }
       
     
       


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

   
   
    
}


