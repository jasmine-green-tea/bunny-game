using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirtPile : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D triggerCollider;


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Bunny"))
        {
            collision.GetComponent<NeedSystem>().AddDirtPile(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Bunny"))
        {
            collision.GetComponent<NeedSystem>().RemoveDirtPile(this);
        }
    }
}
