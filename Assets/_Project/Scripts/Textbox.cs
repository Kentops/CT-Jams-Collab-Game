using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour
{
    private Image myImage;
    private Vector3 regularPos;
    private TextMeshProUGUI myText;
    private List<string> textToDisplay;
    private float camDefault;
    private GameMaster master;
    private CharacterMovement playerMove;


    // Start is called before the first frame update
    void Start()
    {
        myText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        myImage = GetComponent<Image>();
        master = GameObject.FindGameObjectWithTag("Game Master").GetComponent<GameMaster>();
        playerMove = master.player.GetComponent<CharacterMovement>();
        textToDisplay = new List<string>();
        regularPos = transform.position;
        myImage.enabled = false;
        camDefault = Camera.main.transform.rotation.eulerAngles.x;

        transform.position = transform.parent.position + new Vector3(0, -275, 0);
        myText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            displayText("Howdy there!" + Random.Range(0,100));
        }
    }

    public void displayText(string text)
    {
        if(myImage.enabled == false)
        {
            myImage.enabled = true;
            transform.LeanMove(regularPos, 0.8f);
            Camera.main.transform.LeanRotateX(22.9f, 0.8f);
            playerMove.canMove = false;
        }
            textToDisplay.Add(text);
            StartCoroutine("writeText", text);
    }

    IEnumerator writeText(string message)
    {
        //Wait for previous messages to be done + initial wait
        yield return new WaitForSeconds(0.8f);
        while(message != textToDisplay[0])
        {
            yield return new WaitForSeconds(0.2f);
        }

        myText.text = "";
        //My time to shine!
        for (int i = 0; i < message.Length; i++)
        {
            myText.text += message[i];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1 + 0.1f * message.Length); //Give time to read
        textToDisplay.RemoveAt(0);
        
        //Put the box away
        if(textToDisplay.Count == 0)
        {
            playerMove.canMove = true;
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
