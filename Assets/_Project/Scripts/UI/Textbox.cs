using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class Textbox : MonoBehaviour
{
    private Image myImage;
    private Image altImage;
    private Image profileHolder;
    private Vector3 regularPos;
    private Vector3 withProfilePos;
    private Vector3 profileHidePos;
    private Vector3 profileShowPos;
        //experiment
    private TextMeshProUGUI[] allTexts; 

    private int currentProfile;
    private float camDefault;
    private GameMaster master;
    private CharacterMovement playerMove;
    private RectTransform myTransform;
    private RectTransform profileTransform;
    private int previousProfile = -1;
    private Action questionOutcome = null;

    [SerializeField] private Sprite[] profileImages;
    public int numOfTextBoxes;

    // Start is called before the first frame update
    void Start()
    {
        allTexts = new TextMeshProUGUI[numOfTextBoxes];
        for(int i=0;i<numOfTextBoxes;i++)
        {
            allTexts[i] = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            allTexts[i].text = "";
        }

        myImage = GetComponent<Image>();
        altImage = transform.parent.GetChild(1).GetComponent<Image>(); //The profile box
        profileHolder = altImage.transform.GetChild(0).GetComponent<Image>();
        master = GameMaster.instance.GetComponent<GameMaster>();
        playerMove = master.player.GetComponent<CharacterMovement>();
        myTransform = GetComponent<RectTransform>();
        profileTransform = altImage.GetComponent<RectTransform>();

        regularPos = new Vector3(0, -131.4f, 0);
        withProfilePos = new Vector3(71.8f, -131.4f, 0);
        profileShowPos = new Vector3(-189.8f, -131.4f, 0);
        profileHidePos = new Vector3(-47, -131.4f, 0);
        myImage.enabled = false;
        camDefault = Camera.main.transform.rotation.eulerAngles.x;

        transform.position = transform.parent.position + new Vector3(0, -275, 0);
        altImage.enabled = false;
        profileHolder.enabled = false;
    }

    public float displayText(int person, params string[] text)
    {
        
        //makes talk time be 1 second longer than 1/10th a second per character
        float talkTime = 0;
        foreach (string line in text){
            talkTime = line.Length * 0.075f +talkTime;
        }
        talkTime ++;


        if(myImage.enabled == false)
        {
            myImage.enabled = true;
            myTransform.LeanMove(regularPos, 0.8f);
            Camera.main.transform.LeanRotateX(22.9f, 0.8f);
            playerMove.canMove = false;
            talkTime += 0.8f;
        }
        if(previousProfile != person)
        {
            talkTime += 0.6f;
        }
        
        currentProfile = person;
        StartCoroutine("writeText", text);
        return talkTime;
    }

    private void displayProfile(int person)
    {
    
        //person 0 is no profile
        if (person > 0 && altImage.enabled == false)
        {
            //Move and display profile
            altImage.enabled = true;
            profileHolder.enabled = true;
            profileHolder.sprite = profileImages[person];
            myTransform.LeanMove(withProfilePos, 0.5f);
            profileTransform.LeanMove(profileShowPos, 0.5f);
        }
        //Put away profile
        else if(person == 0 && altImage.enabled == true)
        {
            myTransform.LeanMove(regularPos, 0.5f);
            profileTransform.LeanMove(profileHidePos, 0.5f).setOnComplete(() =>
            {
                altImage.enabled = false;
                profileHolder.enabled = false;
            });
        }
        //Switch to a different profile
        else if(person != previousProfile)
        {
            profileTransform.LeanMove(profileHidePos, 0.4f).setOnComplete(() =>
            {
                profileHolder.sprite = profileImages[person];
                profileTransform.LeanMove(profileShowPos, 0.4f);
            });
        }
        previousProfile = person;
    }

    public void askQuestion(Action outcome, params string[] text)
    {
        currentProfile = 0;
        questionOutcome= outcome;
        StartCoroutine("poseQuestion", text);
    }

    public float putAway()
    {
        StartCoroutine("putBoxAway");
        if(currentProfile != 0)
        {
            return 1.3f;
        }
        return 0.8f;
    }

    IEnumerator writeText(params string[] message)
    {
        //clears stuff
        for(int i =0; i< allTexts.Length;i++){allTexts[i].text ="";allTexts[i].color = Color.white;}


        //Wait for textbox to appear
        if (previousProfile == -1)
        {
            yield return new WaitForSeconds(0.8f);
        }
        //Give extra time to display profiles
        if (previousProfile != currentProfile)
        {
            displayProfile(currentProfile);
            yield return new WaitForSeconds(0.5f);
        }

        allTexts = new TextMeshProUGUI[message.Length];
        //my time to cry! ~chouch   
        for(int i=0;i<message.Length;i++)
        {
            //writing text on the elements on screen
            allTexts[i] = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            //My time to shine!
            allTexts[i].text = "";
            for (int j = 0; j < message[i].Length; j++)
            {
                allTexts[i].text += message[i][j];
                yield return new WaitForSeconds(0.05f);
            }
        }
    

    }

    IEnumerator poseQuestion(params string[] texts)
    {
        
        //clears stuff
        for(int i =0; i< allTexts.Length;i++){allTexts[i].text ="";allTexts[i].color = Color.white;}
        
        
        //Pop open textbox if needed + wait for my turn
        if (myImage.enabled == false)
        {
            myImage.enabled = true;
            myTransform.LeanMove(regularPos, 0.8f);
            Camera.main.transform.LeanRotateX(22.9f, 0.8f);
            playerMove.canMove = false;
        }

        //Remove profile if present
        if (previousProfile != 0)
        {
            displayProfile(0);
            yield return new WaitForSeconds(0.5f);
        }

        allTexts = new TextMeshProUGUI[texts.Length];
        for(int i=0;i<texts.Length;i++)
        {
            allTexts[i] = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            allTexts[i].text = texts[i];
            allTexts[i].enabled = true;
        }
        int answerSelected = 0;
        allTexts[0].color = Color.yellow;
        //Select with E
        while (Input.GetKeyDown(KeyCode.E) == false)
        {
            //Change selection
                //upward
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (answerSelected == 0)
                {
                    answerSelected = allTexts.Length -1;
                    allTexts[0].color = Color.white;
                }
                else
                {
                    allTexts[answerSelected].color = Color.white;
                    answerSelected--;
                }
            }
                //downward
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (answerSelected == allTexts.Length -1)
                {
                    allTexts[answerSelected].color = Color.white;
                    answerSelected = 0;
                }
                else
                {
                    allTexts[answerSelected].color = Color.white;
                    answerSelected++;
                }
            }
            allTexts[answerSelected].color = Color.yellow;
            yield return null;
        }

        //Little flashy sequence
        for (int i = 0; i<2; i++)
        {
            allTexts[answerSelected].color = Color.white;
            yield return new WaitForSeconds(0.15f);
            allTexts[answerSelected].color = Color.yellow;
            yield return new WaitForSeconds(0.15f);
        }
        //action repeated based on your response
        for(int i=0;i<=answerSelected;i++){questionOutcome();}
        
        
        questionOutcome = null;

    }

    IEnumerator putBoxAway()
    {
        //clears stuff
        for(int i =0; i< allTexts.Length;i++){allTexts[i].text ="";allTexts[i].color = Color.white;}

        if (altImage.enabled == true)
        {
            //Get rid of profile first
            displayProfile(0);
            yield return new WaitForSeconds(0.5f);
        }
        Camera.main.transform.LeanRotateX(camDefault, 0.8f);
        transform.LeanMove(transform.parent.position + new Vector3(0, -275, 0), 0.8f)
            .setOnComplete(() =>
            {
                //Anonymous function (I don't need to make a separate function)
                myImage.enabled = false;
                previousProfile = -1;
                playerMove.canMove = true; //Needs to be last so you don't pause or something
            });
    }
}
