using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShinx : Interactable
{
    public override IEnumerator action()
    {
        canInteract = false;
        yield return new WaitForSeconds(textbox.displayText(1 , new string[] {"How are you doing?", " " ,"I'd hope well!   "}));
        //Question segment
        int result = -1;
        textbox.askQuestion(() => result++ ,new string[] {"Doing fine", "Doing bad"} );
        //force unity to wait for results
        while(result == -1)
        {
            yield return null;
        }
        //the results of the question
        if (result == 0)
        {
            yield return new WaitForSeconds(textbox.displayText(1, new string[] {"Glad to hear :)"}));
        }
        else
        {
            yield return new WaitForSeconds(textbox.displayText(1, new string[] {"I'm sorry to hear that :(","I hope you feel better"}));
        }
        //Independent of question
        yield return new WaitForSeconds(textbox.displayText(1, new string[] {"You have to go now","Your adventure awaits","Don't miss me too much now"}));
        yield return new WaitForSeconds(textbox.putAway());
        canInteract = true;

    }
}
