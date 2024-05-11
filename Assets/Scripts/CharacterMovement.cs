using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour
{
    public float speed;
    public float mouseSensitivity;

    private CharacterController controller;
    private Vector3 dirToMove;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        dirToMove = Vector3.zero;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement. WASD and arrows to move
        dirToMove.x = Input.GetAxisRaw("Horizontal");
        goingRight(dirToMove.x);

        dirToMove.z = Input.GetAxisRaw("Vertical");
        goingUp(dirToMove.z);

        //Gravity
        //Checks if the feet of the sprite(height away from transform.position) touch the Ground layer
        isGrounded = Physics.CheckSphere(transform.position, controller.height/2, LayerMask.NameToLayer("Default"));
        Debug.DrawLine(transform.position, transform.position - new Vector3(0, controller.height/2, 0));

        //If you are on the ground, add a force to keep you there
        if (isGrounded == false)
        {
            dirToMove.y = -2;
            Debug.Log("hmm");
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
            if(Input.mousePosition.x > Screen.width * 0.667f)
            {
                amountToRot = 40;
            }
            else if(Input.mousePosition.x < Screen.width * 0.333f)
            {
                amountToRot = -40;
            }
            transform.Rotate(0,amountToRot * mouseSensitivity * Time.deltaTime,0);
        }
    }

    public abstract void goingRight(float dir);

    public abstract void goingUp(float dir);
}
