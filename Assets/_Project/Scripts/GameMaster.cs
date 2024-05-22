using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject player;
    public float mouseSensitivity = 1;
    public Textbox textbox;

    // Start is called before the first frame update
    void Awake()
    {
        //Awake goes before start iirc
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
