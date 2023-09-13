using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactionDistance = 5f; // Distancia para activar la interacción.
    public GameObject interactionUI; // El objeto de interfaz de usuario que mostrará el icono de diálogo o el botón de acción.

    private bool isPlayerInRange = false;

    private void Start()
    {
        interactionUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            // Muestra el icono de diálogo o el botón de acción en tu UI.
            interactionUI.SetActive(true);
            
            // Aquí puedes agregar lógica para responder a la interacción del jugador, como abrir un diálogo.
        }
        else
        {
            // Oculta el icono de diálogo o el botón de acción cuando el jugador está fuera de rango.
            interactionUI.SetActive(false);
        }

        // Aquí puedes agregar lógica adicional, como manejar la interacción cuando el jugador presiona un botón.
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

