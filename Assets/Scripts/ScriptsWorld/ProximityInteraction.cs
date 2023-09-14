using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityInteraction : MonoBehaviour
{
    public float interactionRadius = 3f; // Radio de interacci�n.
    public LayerMask interactionLayer; // Capa de objetos con los que puedes interactuar.

    private Transform player; // Referencia al transform del jugador.
    private bool isInRange = false; // Indica si el jugador est� en rango de interacci�n.

    public GameObject globoText;

    private void Start()
    {
        // Busca el objeto con la etiqueta "Player" y obtiene su Transform.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        globoText.SetActive(false);
    }

    private void Update()
    {
        // Calcula la distancia entre este objeto y el jugador.
        float distance = Vector3.Distance(transform.position, player.position);

        // Comprueba si el jugador est� dentro del radio de interacci�n.
        if (distance <= interactionRadius)
        {
            globoText.SetActive(true);
            isInRange = true;

            // Aqu� puedes mostrar un icono de interacci�n o un mensaje en la pantalla.
            // Por ejemplo: "Presiona E para interactuar".

            if (isInRange == true && Input.GetKeyDown(KeyCode.E))
            {
                // El jugador ha presionado la tecla E, realiza la interacci�n.
                Interact();
            }
        }
        else
        {
            isInRange = false;
            globoText.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja un gizmo en el editor para visualizar el radio de interacci�n.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    private void Interact()
    {
        // Este m�todo se llama cuando el jugador interact�a con el objeto o NPC.
        Debug.Log("E");
        // Aqu� puedes agregar la l�gica de interacci�n espec�fica, como mostrar un di�logo o activar una animaci�n.
    }
}
