using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    private Vector3 cameraDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Camera.main is the active camera
        cameraDir = Camera.main.transform.forward;
        cameraDir.y = 0;

        transform.rotation = Quaternion.LookRotation(cameraDir);
    }
}
