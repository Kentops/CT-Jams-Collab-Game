using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : Interactable
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && canInteract == true)
        {
            canInteract = false;
            StartCoroutine("action");
        }
    }

    public override IEnumerator action()
    {
        gameMaster.mySaveData.intStates["Gold"] += 1;
        GetComponent<SpriteRenderer>().enabled = false;
        //Play a sound or something, maybe pull up a wallet
        yield return null; //Make this the sound's duration
        Destroy(gameObject);
    }


}
