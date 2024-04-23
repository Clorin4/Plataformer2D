using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoteFrutas : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float interactionRadius = 3f;
    public FruitSelectionMenu fruitSelectionMenu;
    public GameObject player1Text;
    public GameObject player2Text;

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
    }

    public void FuncBuscarPlayers()
    {
        float distance1 = Vector3.Distance(transform.position, player1.position);
        float distance2 = Vector3.Distance(transform.position, player2.position);

        // Comprueba si el jugador 1 está dentro del radio de interacción.
        if (distance1 <= interactionRadius)
        {
            player1Text.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("PLAYER1 EEEEEE");
                fruitSelectionMenu.ShowMenu(GetCollectedFruits(player1));
            }
        }
        else
        {
            player1Text.SetActive(false);
        }

        // Comprueba si el jugador 2 está dentro del radio de interacción.
        if (distance2 <= interactionRadius)
        {
            player2Text.SetActive(true);
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                Debug.Log("PLAYER2 SHIFFFFFT");
                fruitSelectionMenu.ShowMenu(GetCollectedFruits(player2));
            }
        }
        else
        {
            player2Text.SetActive(false);
        }
    }

    private Dictionary<string, int> GetCollectedFruits(Transform player)
    {
        Dictionary<string, int> collectedFruits = new Dictionary<string, int>
        {
            { "Manzanas", PlayerPrefs.GetInt(player.name + "Manzanas", 0) },
            { "Naranjas", PlayerPrefs.GetInt(player.name + "Naranjas", 0) },
            { "Platanos", PlayerPrefs.GetInt(player.name + "Platanos", 0) }
        };
        return collectedFruits;
    }
}
