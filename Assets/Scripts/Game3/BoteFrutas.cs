using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoteFrutas : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float interactionRadius = 3f;
    private bool isInRangeP1 = false; // Indica si el jugador 1 está cerca
    private bool isInRangeP2 = false; // Indica si el jugador 2 está cerca
    private bool isInteracting = false;
    private bool isOnClickedE = false;
    public GameObject fruitSelectionMenu;
    public GameObject globoTextE;
    public GameObject globoTextShift;

    private void Start()
    {
        StartCoroutine(BuscarPlayers());
    }

    IEnumerator BuscarPlayers()
    {
        yield return new WaitForSeconds(.001f);
        player1 = GameObject.FindGameObjectWithTag("Player1").transform;
        player2 = GameObject.FindGameObjectWithTag("Player2").transform;
    }

    void Update()
    {
        FuncBuscarPlayers();

        if (isInRangeP1 && !isInteracting && !isInRangeP2)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isOnClickedE = true;
                isInteracting = true;
                fruitSelectionMenu.SetActive(true); // Activa el menú de selección de frutas
            }
        }

        if (isInRangeP2 && !isInteracting && !isInRangeP1)
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                isOnClickedE = true;
                isInteracting = true;
                fruitSelectionMenu.SetActive(true); // Activa el menú de selección de frutas
            }
        }

        if (isInteracting)
        {
            if (Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.D))
            {
                if (isOnClickedE)
                {
                    fruitSelectionMenu.GetComponent<FruitSelectionMenu>().MoveSelection(-1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) && !Input.GetKeyDown(KeyCode.A))
            {
                if (isOnClickedE)
                {
                    fruitSelectionMenu.GetComponent<FruitSelectionMenu>().MoveSelection(1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!isOnClickedE)
                {
                    fruitSelectionMenu.GetComponent<FruitSelectionMenu>().MoveSelection(-1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (!isOnClickedE)
                {
                    fruitSelectionMenu.GetComponent<FruitSelectionMenu>().MoveSelection(1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (isOnClickedE)
                {
                    fruitSelectionMenu.GetComponent<FruitSelectionMenu>().SelectFruit();
                    fruitSelectionMenu.SetActive(false); // Desactiva el menú de selección de frutas
                    isInteracting = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.S))
            {
                if (!isOnClickedE)
                {
                    fruitSelectionMenu.GetComponent<FruitSelectionMenu>().SelectFruit();
                    fruitSelectionMenu.SetActive(false); // Desactiva el menú de selección de frutas
                    isInteracting = false;
                }
            }
        }

        // Gestión de los globos de texto
        if (isInRangeP1 && !isInteracting)
        {
            if (!isOnClickedE)
            {
                globoTextE.SetActive(true);
            }
        }
        else
        {
            globoTextE.SetActive(false);
        }

        if (isInRangeP2 && !isInteracting)
        {
            if (!isOnClickedE)
            {
                globoTextShift.SetActive(true);
            }
        }
        else
        {
            globoTextShift.SetActive(false);
        }
    }

    public void FuncBuscarPlayers()
    {
        float distance1 = Vector3.Distance(transform.position, player1.position);
        float distance2 = Vector3.Distance(transform.position, player2.position);

        // Comprueba si el jugador 1 está dentro del radio de interacción.
        if (distance1 <= interactionRadius)
        {
            isInRangeP1 = true;
        }
        else
        {
            isInRangeP1 = false;
        }

        // Comprueba si el jugador 2 está dentro del radio de interacción.
        if (distance2 <= interactionRadius)
        {
            isInRangeP2 = true;
        }
        else
        {
            isInRangeP2 = false;
        }
    }
}
