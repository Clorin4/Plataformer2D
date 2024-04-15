using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    public static bool isGrounded;
    public bool guded;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "NPC1"
            || collision.gameObject.tag == "NPC2" || collision.gameObject.tag == "NPC3")
        {
            isGrounded = true;
            Debug.Log("SUELOOO");
        }
        
    }

    private void Update()
    {
        guded = isGrounded;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
        Debug.Log("FUERAAAA");
    }

}
