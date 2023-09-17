using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProximityInteraction : MonoBehaviour
{
    public float interactionRadius = 3f; // Radio de interacci�n.
    public LayerMask interactionLayer; // Capa de objetos con los que puedes interactuar.

    private Transform player; // Referencia al transform del jugador.
    private bool isInRange = false; // Indica si el jugador est� en rango de interacci�n.
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

        // Comprueba si el jugador est� dentro del radio de interacci�n.
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
        // Dibuja un gizmo en el editor para visualizar el radio de interacci�n.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    private void Interact()
    {
        // Este m�todo se llama cuando el jugador interact�a con el objeto o NPC.
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
