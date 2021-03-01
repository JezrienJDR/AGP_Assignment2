using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public BoxCollider box;

    private void Start()
    {
        box = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {


        var pc = collision.gameObject.GetComponent<PlayerController>();

        if(pc != null)
        {
            Debug.Log("Interactable SET");
            pc.interactable = gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        var pc = collision.gameObject.GetComponent<PlayerController>();

        if (pc != null)
        {
            if(pc.interactable == gameObject)
            {
                Debug.Log("Interactable NULL");
                pc.interactable = null;
            }
        }
    }

}
