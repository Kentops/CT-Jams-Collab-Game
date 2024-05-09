using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
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
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            dirToMove.x = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            dirToMove.x = 0;
        }
        if(Input.GetAxisRaw("Vertical") != 0)
        {
            dirToMove.z = Input.GetAxisRaw("Vertical");
        }
        else
        {
            dirToMove.z = 0;
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
}
