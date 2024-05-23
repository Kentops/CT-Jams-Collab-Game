using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //Thanks Code Monkey
    [SerializeField] private float interactRange;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            List<Interactable> interactables = new List<Interactable>();
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach(Collider col in colliderArray)
            {
                if (col.TryGetComponent(out Interactable canInteract))
                {
                    interactables.Add(canInteract);
                }
            }

            //Get closest
            if(interactables.Count != 0)
            {
                Debug.Log(interactables.Count);
                Interactable closest = null;
                foreach (Interactable obj in interactables)
                {
                    if (closest == null)
                    {
                        closest = obj;
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, obj.transform.position) <
                            Vector3.Distance(transform.position, closest.transform.position))
                        {
                            closest = obj;
                        }
                    }
                }
                //Done
                closest.playerInteraction();
            }
        }

    }
}
