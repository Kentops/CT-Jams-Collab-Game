using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    private GameMaster controller;
    private CharacterMovement player;
    private Vector3 backgroundDefaultPosition = new Vector3(900, 208, 0);
    private Vector3 backgroundVisiblePosition = new Vector3(724, 208, 0);

    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Game Master").GetComponent<GameMaster>();
        player = controller.player.GetComponent<CharacterMovement>();
        menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && (player.canMove || menu.activeSelf == true))
        {
            //canMove is checked so you don't pause during a conversation
            menu.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(togglePause());
        }
        //Scroll through menu options
        else if(PauseMenuButtons.selectedButton != 1 && 
            Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            PauseMenuButtons.selectedButton--;
        }
        else if(PauseMenuButtons.selectedButton != 2 && //Change 2 to be the last button number
            Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            PauseMenuButtons.selectedButton++;
        }
    }

    IEnumerator togglePause()
    {
        RectTransform backgroundMenu = menu.GetComponent<RectTransform>();
        //When Pausing
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
            //These movements are not done using LeanTwean because the timescale affects it
            while (backgroundMenu.position != backgroundVisiblePosition)
            {
                backgroundMenu.position = Vector3.MoveTowards(backgroundMenu.position,
                    backgroundVisiblePosition, 200 * Time.unscaledDeltaTime);
                yield return null;
            }
        }
        //When resuming
        else
        {
            while(backgroundMenu.position != backgroundDefaultPosition)
            {
                backgroundMenu.position = Vector3.MoveTowards(backgroundMenu.position,
                    backgroundDefaultPosition, 200 * Time.unscaledDeltaTime);
                yield return null;
            }
            menu.SetActive(false);
            Time.timeScale = 1;
        }
    }


}
