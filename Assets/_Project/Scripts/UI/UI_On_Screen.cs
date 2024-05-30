using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_On_Screen : MonoBehaviour
{

    //this class could be easily modified to allow for switching the image. That would be useful for a collected an item type thing. Like a finding a pokeball and then a potion in pokemon.
    //probably overloading the method at the bottom is the easiest way to do this


    public Vector3 startingPoint;
    public Vector3 targetDestination;
    public Vector3 finalDestination; 

    public bool interpolation;
    //true is sinusoidal and false is linear

    private bool working;

    private int movingState;
    //0 is offScreen; 1 is setup for moving onscreen; 2 is moving onscreen; 3 is waiting onscreen; 4 is setup to move off; 5 is moving off

    public float timeToTarget;
    public float timeAtTarget;
    public float timeToExit;
    
    private float timer;
    private float coefficient;
    
    private Vector3 translationDirection;

    // Start is called before the first frame update
    void Start()
    {
        working = false;
        movingState = 0;
        timer = timeToTarget;

        transform.position = startingPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if(working) DoingStuffs(); //only run all the checks when this bool is on that way there's only one check the rest of the time.

    }

    void DoingStuffs(){
//set up to move to target    
        if(movingState==1)
        {
            
            translationDirection = Vector3.Normalize(targetDestination - startingPoint);
            if(!interpolation) coefficient = (targetDestination.x - startingPoint.x)/timeToTarget;
            timer = timeToTarget;
            movingState ++;
        
        }
        //the moving to the target
        if(movingState==2)
        {
            if(interpolation) coefficient = (float)(Mathf.PI/2*Mathf.Abs(Mathf.Sin(timer-timeToTarget)));
            transform.Translate(translationDirection*coefficient*Time.deltaTime);
            timer -= Time.deltaTime;

            if(timer<=0)
            {
                movingState++;
                timer = timeAtTarget;
            }
        }

        //the waiting at the target
        if(movingState==3)
        {
            timer -= Time.deltaTime;
            if(timer<=0)
            {
                movingState++;
            }        
        }

        //set up to move to final placr   
        if(movingState==4)
        {
            
            translationDirection = Vector3.Normalize(finalDestination - targetDestination);
            if(!interpolation) coefficient = (finalDestination.x - targetDestination.x)/timeToExit;
            timer = timeToExit;
            movingState ++;
        
        }
        //the moving to the final place
        if(movingState==5)
        {
            if(interpolation) coefficient = (float)(Mathf.PI/2*(Mathf.Abs(Mathf.Sin(timer-timeToExit))));
            transform.Translate(translationDirection*coefficient*Time.deltaTime);
            timer -= Time.deltaTime;

            if(timer<=0)
            {
                movingState = 0;
                transform.position = startingPoint;
                working = false;
            }
        }
        
    } //end of doing stuffs


    public void PutOnScreen(){
        movingState = 1;
        transform.position = startingPoint;
        working = true;
    }

    public void PutOnScreen(float timeOnScreen)
    {
        movingState = 1;
        timeAtTarget = timeOnScreen;
        transform.position = startingPoint;
        working = true;
    }
}
