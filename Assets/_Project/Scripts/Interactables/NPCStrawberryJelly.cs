using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStrawberryJelly : Interactable
{

    public override IEnumerator action()
    {
        canInteract = false;
        yield return new WaitForSeconds(textbox.displayText(3, new string[] {"Just a slime", "they're considered weak"}));
        yield return new WaitForSeconds(textbox.displayText(3, new string[] {"but they still always", "manage to beat me!"}));
        yield return new WaitForSeconds(textbox.displayText(2, new string[] {"Just a man", "he's all alone", "shinx could take him"}));
        yield return new WaitForSeconds(textbox.displayText(0, new string[] {"Just a thought","    ____         ____","___________________"}));
        yield return new WaitForSeconds(textbox.displayText(1, new string[] {"Just a Shinx","I love him <3"}));
        yield return new WaitForSeconds(textbox.displayText(1, new string[] {"A being that far", "surpasses myself!"}));
        yield return new WaitForSeconds(textbox.putAway());
        canInteract = true;
    }
}
