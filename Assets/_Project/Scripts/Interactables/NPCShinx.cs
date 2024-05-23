using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShinx : Interactable
{
    public override IEnumerator action()
    {
        canInteract = false;
        yield return new WaitForSeconds(textbox.displayText("How are you doing?", 1));
        //Question segment
        int result = -1;
        textbox.askQuestion("Doing fine", "Doing bad", () =>
        {
            result = 0;
        }, () =>
        {
            result = 1;
        });
        //force unity to wait for results
        while(result == -1)
        {
            yield return null;
        }
        //the results of the question
        if (result == 0)
        {
            yield return new WaitForSeconds(textbox.displayText("Glad to hear :)",1));
        }
        else
        {
            yield return new WaitForSeconds(textbox.displayText("I'm sorry to hear that :(",1));
            yield return new WaitForSeconds(textbox.displayText("I hope you feel better", 1));
        }
        //Independent of question
        yield return new WaitForSeconds(textbox.displayText("Welp, see you later.", 1));
        yield return new WaitForSeconds(textbox.putAway());
        canInteract = true;

    }
}
