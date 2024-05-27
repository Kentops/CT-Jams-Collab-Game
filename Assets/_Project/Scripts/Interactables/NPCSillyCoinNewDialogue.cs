using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSillyCoinNewDialogue : Interactable
{
    public override IEnumerator action()
    {
        canInteract = false;
        yield return new WaitForSeconds(textbox.displayText(4, new string[] {"How do I look (',~)", "You think cool right", "please! say it D:"}));
        //Question segment
        int result = -1;
        textbox.askQuestion(() => result++ ,new string[] {"Cool", "Awesome", "uhhhh"});
        //force unity to wait for results
        while(result == -1)
        {
            yield return null;
        }
        //the results of the question
        if (result == 0)
        {
            yield return new WaitForSeconds(textbox.displayText(4, new string[] {"Glad to hear :)"}));
            if (!gameMaster.mySaveData.intStates.ContainsKey("Gold"))
            {
                gameMaster.mySaveData.intStates.Add("Gold", 0);
            }
            gameMaster.mySaveData.intStates["Gold"] += 5;
            yield return new WaitForSeconds(textbox.displayText(4, new string[] { "You are rewarded for your", "honesty", ";)" }));
        }
        else if (result == 1)
        {
            yield return new WaitForSeconds(textbox.displayText(4, new string[] {"Thanks B)"}));
            yield return new WaitForSeconds(textbox.displayText(4, new string[] {"I'm glad you feel'","that way :)))))","    O_O "}));
        }
        else 
        {
            yield return new WaitForSeconds(textbox.displayText(4, new string[] {"I'm sorry to hear that :(","because clearly ", "you're not thinking straight"}));
            yield return new WaitForSeconds(textbox.displayText(4, new string[] {"I know I look awesome"}));
        }
        //Independent of question
        yield return new WaitForSeconds(textbox.displayText(4, new string[] {"Welp, see you later.", "that is if you live that", " long. these fields are tough..."}));
        yield return new WaitForSeconds(textbox.putAway());
        canInteract = true;

    }
}
