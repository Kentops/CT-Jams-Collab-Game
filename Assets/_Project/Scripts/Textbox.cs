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
    private Action[] questionOutcomes = new Action[2];

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

        displayText("Do you prefer cats or dogs?", 2);
        askQuestion("Cats", "Dogs", testFunc, testFunc1);
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

    public void askQuestion(string text1, string text2, Action option1, Action option2)
    {
        textToDisplay.Add("+");
        profile.Add(0);
        questionOutcomes[0] = option1;
        questionOutcomes[1] = option2;
        string[] temp = { text1, text2 };
        StartCoroutine("poseQuestion", temp);
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

    IEnumerator poseQuestion(string[] texts)
    {
        //Pop open textbox if needed + wait for my turn
        if (myImage.enabled == false)
        {
            myImage.enabled = true;
            myTransform.LeanMove(regularPos, 0.8f);
            Camera.main.transform.LeanRotateX(22.9f, 0.8f);
            playerMove.canMove = false;
        }

        while (textToDisplay[0] != "+")
        {
            yield return new WaitForSeconds(0.2f);
        }
        myText.text = "";

        //Remove profile if present
        if (previousProfile != 0)
        {
            displayProfile(0);
            yield return new WaitForSeconds(0.5f);
        }

        altText.enabled = true;
        myText.text = texts[0];
        altText.text = texts[1];
        bool oneSelected = true;
        myText.color = Color.yellow;
        //Select with E
        while (Input.GetKeyDown(KeyCode.E) == false)
        {
            //Change selection
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)
                || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
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
            yield return null;
        }

        if (oneSelected)
        {
            questionOutcomes[0]();
        }
        else
        {
            questionOutcomes[1]();
        }
        //Little flashy sequence to hide the fact the next text takes 0.8 seconds to display
        for (int i = 0; i<2; i++)
        {
            if (oneSelected)
            {
                myText.color = Color.white;
                yield return new WaitForSeconds(0.15f);
                myText.color = Color.yellow;
            }
            else
            {
                altText.color = Color.white;
                yield return new WaitForSeconds(0.15f);
                altText.color = Color.yellow;
            }
            yield return new WaitForSeconds(0.15f);
        }
        myText.color = Color.white;
        altText.color = Color.white;
        altText.enabled = false;
        myText.text = "";
        textToDisplay.RemoveAt(0);
        profile.RemoveAt(0);

    }

    void testFunc()
    {
        displayText("I agree!", 2);
    }
    void testFunc1()
    {
        displayText("I respectfully disagree", 2);
    }

}
