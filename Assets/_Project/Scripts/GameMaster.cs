using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject player;
    public Textbox textbox;
    public SaveData mySaveData;
    public float mouseSensitivity = 1;

    // Start is called before the first frame update
    void Awake()
    {
        //Awake goes before start iirc
        player = GameObject.FindGameObjectWithTag("Player");
        textbox = GameObject.FindGameObjectWithTag("Textbox").GetComponent<Textbox>();
        mySaveData = GetComponent<SaveData>();
        //The game master will be present in every scene
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reload()
    {
        Awake();
    }
}
