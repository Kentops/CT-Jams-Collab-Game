using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool canInteract = true;
    public GameMaster gameMaster;
    public Textbox textbox;

    private void Start()
    {
        gameMaster = GameMaster.instance.GetComponent<GameMaster>();
        textbox = gameMaster.textbox;
    }
    public virtual void playerInteraction()
    {
        if(canInteract == true)
        {
            StopCoroutine("action");
            StartCoroutine("action");
        }
    }

    public abstract IEnumerator action();
}
