using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour
{
    public float speed;
    public float mouseSensitivity;

    private CharacterController controller;
    private Vector3 dirToMove;

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

        //Adjust dir from local space to global space because Move() uses global
        dirToMove = transform.TransformDirection(dirToMove);
        //Move the character
        controller.Move(dirToMove.normalized * speed * Time.deltaTime);

        //Camera stuff
        //Right click to move camera
        if (Input.GetMouseButton(1))
        {
            int amountToRot = 0;
            if(Input.mousePosition.x > Screen.width * 0.6667f)
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
