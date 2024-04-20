using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frutas : MonoBehaviour
{
    public string fruitType; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            // Obtener el tag del jugador
            string playerTag = collision.gameObject.tag;

            // Incrementar la cantidad de frutas recolectadas según el tipo de fruta y el jugador
            int numFruits = PlayerPrefs.GetInt(fruitType + playerTag, 0);
            numFruits++;
            PlayerPrefs.SetInt(fruitType + playerTag, numFruits);

            Debug.Log(fruitType + numFruits);

            // Destruir la fruta
            Destroy(gameObject);
        }
    }

}