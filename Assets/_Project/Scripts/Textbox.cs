using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour
{
    private Image myImage;
    private Image altImage;
    private Image profileHolder;
    private Vector3 regularPos;
    private Vector3 withProfilePos;
    private Vector3 profileHidePos;
    private Vector3 profileShowPos;
    private TextMeshProUGUI myText;
    private TextMeshProUGUI altText;
    private List<string> textToDisplay;
    private List<int> profile;
    private float camDefault;
    private GameMaster master;
    private CharacterMovement playerMove;
    private RectTransform myTransform;
    private RectTransform profileTransform;
    private int previousProfile = 0;

    [SerializeField] private Sprite[] profileImages;


    // Start is called before the first frame update
    void Start()
    {
        myText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        altText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        myImage = GetComponent<Image>();
        altImage = transform.parent.GetChild(1).GetComponent<Image>(); //The profile box
        profileHolder = altImage.transform.GetChild(0).GetComponent<Image>();
        master = GameObject.FindGameObjectWithTag("Game Master").GetComponent<GameMaster>();
        playerMove = master.player.GetComponent<CharacterMovement>();
        myTransform = GetComponent<RectTransform>();
        profileTransform = altImage.GetComponent<RectTransform>();

        textToDisplay = new List<string>();
        profile = new List<int>();
        regularPos = new Vector3(0, -131.4f, 0);
        withProfilePos = new Vector3(71.8f, -131.4f, 0);
        profileShowPos = new Vector3(-189.8f, -131.4f, 0);
        profileHidePos = new Vector3(-47, -131.4f, 0);
        myImage.enabled = false;
        camDefault = Camera.main.transform.rotation.eulerAngles.x;

        transform.position = transform.parent.position + new Vector3(0, -275, 0);
        myText.text = "";
        altText.text = "";
        altImage.enabled = false;
        profileHolder.enabled = false;

        displayText("No profile");
        displayText("Shinx profile!",1);
        displayText("Go away Shinx!");
        displayText("More Shinx!", 1);
        displayText("Swap!",2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            displayText("Howdy there!" + UnityEngine.Random.Range(0,100),UnityEngine.Random.Range(0,3));
        }
    }

    public void displayText(string text, int person = 0)
    {
        if(myImage.enabled == false)
        {
            myImage.enabled = true;
            myTransform.LeanMove(regularPos, 0.8f);
            Camera.main.transform.LeanRotateX(22.9f, 0.8f);
            playerMove.canMove = false;
        }
        textToDisplay.Add(text);
        profile.Add(person);
        StartCoroutine("writeText", text);
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

    //Broken right now, hard crashes. I assume it's because I have a while loop that can be infinite
    public bool askQuestion(string option1, string option2, Action function)
    {
        //Remove profile if present
        if (previousProfile != 0)
        {
            displayProfile(0);
        }

        altText.enabled = true;
        myText.text = option1;
        altText.text = option2;
        bool oneSelected = true;
        myText.color = Color.yellow;
        while (Input.GetKeyDown(KeyCode.E) == false)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                oneSelected = !oneSelected;
                if (oneSelected == true)
                {
                    myText.color = Color.yellow;
                    altText.color = Color.white;
                }
                else
                {
                    myText.color = Color.white;
                    altText.color = Color.yellow;
                }
            }
        }
        myText.color = Color.white;
        altText.color = Color.white;
        return oneSelected;
    }

    IEnumerator writeText(string message)
    {
        //Wait for previous messages to be done + initial wait
        yield return new WaitForSeconds(0.8f);
        while(message != textToDisplay[0])
        {
            yield return new WaitForSeconds(0.2f);
        }

        //Give extra time to display profiles
        if (previousProfile != profile[0])
        {
            displayProfile(profile[0]);
            yield return new WaitForSeconds(0.5f);
        }

        //My time to shine!
        myText.text = "";
        for (int i = 0; i < message.Length; i++)
        {
            myText.text += message[i];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1 + 0.1f * message.Length); //Give time to read
        textToDisplay.RemoveAt(0);
        profile.RemoveAt(0);
        
        //Put the box away
        if(textToDisplay.Count == 0)
        {
            playerMove.canMove = true;

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
                    myText.text = "";
                    myImage.enabled = false;
                });
        }
    }

}
