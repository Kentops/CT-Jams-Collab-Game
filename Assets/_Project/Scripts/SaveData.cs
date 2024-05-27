using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    private string pairSeparator = "WITH";
    private string itemSeparator = "ITEM";
    private string typeSeparator = "TYPECHANGE";
    private GameMaster gameMaster;

    private void Start()
    {
        gameMaster = GetComponent<GameMaster>();
    }

    //This is a class to store all variables that will be saved
    public Dictionary<string, int> intStates = new Dictionary<string, int>()
    {
        
    };

    public void saveGame()
    {
        //Thank you CodeMonkey

        List<string> content = new List<string>();
        foreach (KeyValuePair<string,int> amount in intStates)
        {
            content.Add(amount.Key + pairSeparator + amount.Value + itemSeparator);
        }
        content.Add(typeSeparator);
        Transform player = gameMaster.player.transform;
        content.Add("" + player.position.x + "," + player.position.y + "," + player.position.z);
        string saveString = string.Join("", content);
        File.WriteAllText(Application.dataPath + "/save.txt", "" + saveString);
    }

    public void loadGame()
    {
        string content = File.ReadAllText(Application.dataPath + "/save.txt");
        string[] types = content.Split(typeSeparator);
        
        string[] items = types[0].Split(itemSeparator);
        for(int j = 0; j < items.Length-1; j++)
        {
            string[] pair = items[j].Split(pairSeparator);
                intStates.Add(pair[0], int.Parse(pair[1]));
        }
        //Load correct scene

        //Set Player position
        gameMaster.reload(); //Get correct information in gameMaster
        string[] values = types[1].Split(",");
        gameMaster.player.transform.position = new Vector3(float.Parse(values[0]),
            float.Parse(values[1]), float.Parse(values[2]));
        Debug.Log(gameMaster.player.transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            saveGame();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            loadGame();
        }
    }



}
