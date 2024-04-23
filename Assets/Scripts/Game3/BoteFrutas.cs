using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoteFrutas : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float interactionRadius = 3f;
    private bool isPlayer1Near = false; // Indica si el jugador 1 está cerca
    private bool isPlayer2Near = false; // Indica si el jugador 2 está cerca
    private bool isInteracting = false;
    //private bool isOnClickedE = false;
    public GameObject fruitSelectionMenu;
    public GameObject globoTextE;
    public GameObject globoTextShift;

    private void Start()
    {
        //isOnClickedE = false;
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

        if (isPlayer1Near && !isInteracting && !isPlayer2Near)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //isOnClickedE = true;
                isInteracting = true;
                fruitSelectionMenu.GetComponent<FruitSelectionMenu>().ShowMenu(true);
            }
        }

        if (isPlayer2Near && !isInteracting && !isPlayer1Near)
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                //isOnClickedE = false; // Player 2 no usa la tecla E
                isInteracting = true;
                fruitSelectionMenu.GetComponent<FruitSelectionMenu>().ShowMenu(false);
            }
        }

        if (isInteracting)
        {
            if ((isPlayer1Near && Input.GetKeyDown(KeyCode.S)) || (isPlayer2Near && Input.GetKeyDown(KeyCode.DownArrow)))
            {
                fruitSelectionMenu.GetComponent<FruitSelectionMenu>().SelectFruit();
                fruitSelectionMenu.SetActive(false); // Desactiva el menú de selección de frutas
                isInteracting = false;
            }
        }

        // Gestión de los globos de texto
        globoTextE.SetActive(isPlayer1Near && !isInteracting);
        globoTextShift.SetActive(isPlayer2Near && !isInteracting);
    }

    public void FuncBuscarPlayers()
    {
        float distance1 = Vector3.Distance(transform.position, player1.position);
        float distance2 = Vector3.Distance(transform.position, player2.position);

        // Comprueba si el jugador 1 está dentro del radio de interacción.
        isPlayer1Near = distance1 <= interactionRadius;

        // Comprueba si el jugador 2 está dentro del radio de interacción.
        isPlayer2Near = distance2 <= interactionRadius;
    }
}
