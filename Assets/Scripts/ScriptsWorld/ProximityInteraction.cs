using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityInteraction : MonoBehaviour
{
    public float interactionRadius = 3f; // Radio de interacción.
    public LayerMask interactionLayer; // Capa de objetos con los que puedes interactuar.

    private Transform player; // Referencia al transform del jugador.
    private bool isInRange = false; // Indica si el jugador está en rango de interacción.

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

        // Comprueba si el jugador está dentro del radio de interacción.
        if (distance <= interactionRadius)
        {
            globoText.SetActive(true);
            isInRange = true;

            // Aquí puedes mostrar un icono de interacción o un mensaje en la pantalla.
            // Por ejemplo: "Presiona E para interactuar".

            if (isInRange == true && Input.GetKeyDown(KeyCode.E))
            {
                // El jugador ha presionado la tecla E, realiza la interacción.
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
        // Dibuja un gizmo en el editor para visualizar el radio de interacción.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    private void Interact()
    {
        // Este método se llama cuando el jugador interactúa con el objeto o NPC.
        Debug.Log("E");
        // Aquí puedes agregar la lógica de interacción específica, como mostrar un diálogo o activar una animación.
    }
}
