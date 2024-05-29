using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenuButtons : MonoBehaviour
{
    [SerializeField] private int buttonNumber;

    public static int selectedButton = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(selectedButton == buttonNumber)
        {
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }


    
}
