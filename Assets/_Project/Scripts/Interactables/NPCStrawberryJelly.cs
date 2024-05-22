using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStrawberryJelly : Interactable
{

    public override IEnumerator action()
    {
        canInteract = false;
        yield return new WaitForSeconds(textbox.displayText("Just a slime",3));
        yield return new WaitForSeconds(textbox.displayText("Just a man", 2));
        yield return new WaitForSeconds(textbox.displayText("Just a thought"));
        yield return new WaitForSeconds(textbox.displayText("Just a Shinx", 1));
        yield return new WaitForSeconds(textbox.putAway());
        canInteract = true;
    }
}
