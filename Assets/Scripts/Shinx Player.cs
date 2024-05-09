using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinxPlayer : CharacterMovement
{
    public Animator myAnim;
    public SpriteRenderer mySprite;

    //Does not override update or start

    public override void goingRight(float dir)
    {
        if(dir != 0 && myAnim.GetBool("Moving Up") == true)
        {
            if(dir > 0)
            {
                mySprite.flipX = true;
            }
            else
            {
                mySprite.flipX = false;
            }
            myAnim.SetBool("Moving Right", true);
        }
        else if(dir > 0)
        {
            myAnim.Play("Shinx Walk Side");
            mySprite.flipX = true;
            myAnim.SetBool("Moving Right", true);
        }
        else if(dir < 0)
        {
            myAnim.Play("Shinx Walk Side");
            mySprite.flipX = false;
            myAnim.SetBool("Moving Right", true);
        }
        else
        {
            //Not moving right
            myAnim.SetBool("Moving Right", false);
        }
    }

    public override void goingUp(float dir)
    {
        if(dir != 0 && myAnim.GetBool("Moving Right") == true)
        {
           if(dir > 0)
           {
               myAnim.Play("Shinx Strafe");
           }
           else
           {
               myAnim.Play("Shinx Strafe Front");
           }
           myAnim.SetBool("Moving Up", true);
        }
        else if(dir > 0)
        {
            myAnim.Play("Shinx Walk Behind");
            myAnim.SetBool("Moving Up", true);
        }
        else if(dir < 0)
        {
            myAnim.Play("Shinx Walk Front");
            myAnim.SetBool("Moving Up", true);
        }
        else
        {
            //Not moving up
            myAnim.SetBool("Moving Up", false);
        }
    }
}
