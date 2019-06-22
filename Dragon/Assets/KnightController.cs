using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    float speed = 4;
    float rotSpeed = 80;
    float gravity = 8;
    float rot = 0f;



    Vector3 moveDir = Vector3.zero;

    public GameObject knight;
    public GameObject knightRagdoll;
    CharacterController controller;
    Animator anim;


    void Start()
    {
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //controller = knight.GetComponent<CharacterController>();
        anim = knight.GetComponent<Animator>();
    }

   void Update()
    {
        if(controller != null)
        {
            Movement();
            GetInput();
        }
  
        Die();
    }

    

    void Die()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(knightRagdoll, knight.transform.position, knight.transform.rotation);
            Destroy(knight);
        }
    }
    void Movement()
    {

        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if(anim.GetBool("attacking") == true)
                {
                    return;
                }
                else if(anim.GetBool("attacking") == false)
                {
                    anim.SetInteger("condition", 1);
                    anim.SetBool("running", true);
                    moveDir = new Vector3(0, 0, 1);
                    moveDir *= speed;
                    moveDir = knight.transform.TransformDirection(moveDir);
                }      
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("running", false);
                anim.SetInteger("condition", 0);
                moveDir = new Vector3(0, 0, 0);
            }
        }
        rot += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        knight.transform.eulerAngles = new Vector3(0, rot, 0);

        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
    }
    void GetInput()
    {
        if (controller.isGrounded)
        {
            if (Input.GetMouseButton(0))
            {
                if(anim.GetBool("running") == true)
                {
                    anim.SetBool("running", false);
                    anim.SetInteger("condition", 0);
                }
                if (anim.GetBool("running") == false)
                {
                    Attacking();
                }    
            }
        }
    }

    void Attacking()
    {
        anim.SetBool("attacking", true);
        anim.SetInteger("condition", 2);
        StartCoroutine (AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(1);
        if(anim != null)
        {
            anim.SetInteger("condition", 0);
            anim.SetBool("attacking", false);
        }

    }
}
