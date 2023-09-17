using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProximityInteraction : MonoBehaviour
{
    public float interactionRadius = 3f; // Radio de interacción.
    public LayerMask interactionLayer; // Capa de objetos con los que puedes interactuar.

    private Transform player; // Referencia al transform del jugador.
    private bool isInRange = false; // Indica si el jugador está en rango de interacción.
    private bool isInteracting = false;

    public PlayerController PC;

    public GameObject globoText;
    public GameObject text;

    private void Start()
    {
        // Busca el objeto con la etiqueta "Player" y obtiene su Transform.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        globoText.SetActive(false);
        text.SetActive(false);
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

            if (isInRange == true && Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
            
        }
        else if (!isInRange == true && isInteracting == true)
        {
            text.SetActive(false);
            isInteracting = false;
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
        
        globoText.SetActive(false);

        if (!isInteracting)
        {
            text.SetActive(true);
            isInteracting = true;
        }
        else if(isInteracting && PC.O == 1)
        {
            text.SetActive(false);
            isInteracting = false;
            Debug.Log("NOS FUIMOS");
            SceneManager.LoadScene(2);
        }
        else if (isInteracting && PC.O == 2)
        {
            text.SetActive(false);
            isInteracting = false;
            Debug.Log("NOS FUIMOS");
            //SceneManager.LoadScene(2);
        }
        else if (isInteracting && PC.O == 3)
        {
            text.SetActive(false);
            isInteracting = false;
            Debug.Log("NOS FUIMOS");
            //SceneManager.LoadScene(2);
        }

    }
}
