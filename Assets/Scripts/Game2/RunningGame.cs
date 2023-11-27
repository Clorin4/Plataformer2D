using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        

    }
    private void Update() //ORDEN PARA MOVER GRID DE PERSONAJES
    {
        float velocidadMovimiento = 1.0f; // Modifica este valor según la velocidad deseada
        Vector3 destinoPos = new Vector3(-10.0f, 0.0f, 0.0f); // Modifica esto con la posición a la que quieres mover el Grid

        GameObject gridObject = GameObject.Find("GridP1");

        if (gridObject != null)
        {
            // Movimiento suavizado del Grid hacia la posición de destino
            gridObject.transform.position = Vector3.Lerp(gridObject.transform.position, destinoPos, Time.deltaTime * velocidadMovimiento);
        }
    }

}
