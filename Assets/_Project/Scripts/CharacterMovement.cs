using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour
{
    public float speed;
    public float mouseSensitivity;
    public bool canMove = true;

    private CharacterController controller;
    private Vector3 dirToMove;
    private bool isGrounded;
    private GameMaster master;

    // Start is called before the first frame update
    void Start()
    {
        dirToMove = Vector3.zero;
        controller = GetComponent<CharacterController>();
        master = GameObject.FindGameObjectWithTag("Game Master").GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //Movement. WASD and arrows to move
            dirToMove.x = Input.GetAxisRaw("Horizontal");
            goingRight(dirToMove.x);

            dirToMove.z = Input.GetAxisRaw("Vertical");
            goingUp(dirToMove.z);

            //Gravity
            //Checks if the feet of the sprite are touching something
            isGrounded = Physics.Raycast(transform.position, Vector3.down, (controller.height * 0.5f) + 0.1f);

            //If you are on the ground, add a force to keep you there
            if (isGrounded == false)
            {
                dirToMove.y = -2;
            }
            else
            {
                dirToMove.y = 0;
            }

            //Adjust dir from local space to global space because Move() uses global
            dirToMove = transform.TransformDirection(dirToMove);

            //Move the character
            controller.Move(dirToMove.normalized * speed * Time.deltaTime);


            //Camera stuff
            //Right click to move camera
            if (Input.GetMouseButton(1))
            {
                int amountToRot = 0;
                if (Input.mousePosition.x > Screen.width * 0.667f)
                {
                    amountToRot = 60;
                }
                else if (Input.mousePosition.x < Screen.width * 0.333f)
                {
                    amountToRot = -60;
                }
                transform.Rotate(0, amountToRot * master.mouseSensitivity * Time.deltaTime, 0);
            }
        }
        else
        {
            goingRight(0);
            goingUp(0);
        }
    }

    public abstract void goingRight(float dir);

    public abstract void goingUp(float dir);
}
