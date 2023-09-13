using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactionDistance = 5f; // Distancia para activar la interacci�n.
    public GameObject interactionUI; // El objeto de interfaz de usuario que mostrar� el icono de di�logo o el bot�n de acci�n.

    private bool isPlayerInRange = false;

    private void Start()
    {
        interactionUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            // Muestra el icono de di�logo o el bot�n de acci�n en tu UI.
            interactionUI.SetActive(true);
            
            // Aqu� puedes agregar l�gica para responder a la interacci�n del jugador, como abrir un di�logo.
        }
        else
        {
            // Oculta el icono de di�logo o el bot�n de acci�n cuando el jugador est� fuera de rango.
            interactionUI.SetActive(false);
        }

        // Aqu� puedes agregar l�gica adicional, como manejar la interacci�n cuando el jugador presiona un bot�n.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("GLOBO");
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("FUERA GLOBO");
            isPlayerInRange = false;
        }
    }
}

