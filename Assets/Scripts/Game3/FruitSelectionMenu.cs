using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FruitSelectionMenu : MonoBehaviour
{
    public Button[] fruitButtons; // Botones de frutas (en orden: manzanas, plátanos, naranjas)
    public Button cancelButton; // Botón de cancelar
    public TextMeshProUGUI[] fruitQuantities; // Textos para las cantidades de frutas
    private int selectedButtonIndex = 0;
    private string[] fruitNames = { "Manzanas", "Platanos", "Naranjas", "Cancelar" }; // Nombres de las frutas en orden

    private void Start()
    {
        HideMenu();
    }

    public void ShowMenu(bool isPlayer1)
    {
        if (gameObject.activeSelf) // Si el menú ya está activo
        {
            if ((isPlayer1 && Input.GetKeyDown(KeyCode.E)) || (!isPlayer1 && Input.GetKeyDown(KeyCode.RightShift)))
            {
                HideMenu(); // Oculta el menú
                return;
            }
            
        }
        else
        {
            Debug.Log("DBNHUJDBFWE");
            gameObject.SetActive(true);
            UpdateFruitQuantities(isPlayer1);
            selectedButtonIndex = 0;
            MoveSelection(0);
        }
    }

    public void HideMenu()
    {
        gameObject.SetActive(false);
    }

    public void MoveSelection(int direction)
    {
        selectedButtonIndex = (selectedButtonIndex + direction) % fruitButtons.Length;
        if (selectedButtonIndex < 0)
        {
            selectedButtonIndex += fruitButtons.Length;
        }

        for (int i = 0; i < fruitButtons.Length; i++)
        {
            fruitButtons[i].image.color = (i == selectedButtonIndex) ? Color.yellow : Color.white;
        }
    }

    public void SelectFruit()
    {
        string fruitName = fruitNames[selectedButtonIndex];
        Debug.Log("Fruit selected: " + fruitName);

        // Si se selecciona "Cancelar", oculta el menú
        if (selectedButtonIndex == fruitButtons.Length - 1)
        {
            HideMenu();
            return;
        }

        // Reducir en 1 la cantidad de frutas seleccionadas del jugador activo
        if (PlayerPrefs.GetInt("CanMovePlayer1") == 1)
        {
            PlayerPrefs.SetInt(fruitName + "Player1", Mathf.Max(0, PlayerPrefs.GetInt(fruitName + "Player1") - 1));
        }
        else
        {
            PlayerPrefs.SetInt(fruitName + "Player2", Mathf.Max(0, PlayerPrefs.GetInt(fruitName + "Player2") - 1));
        }

        // Actualizar las cantidades de frutas en el menú
        UpdateFruitQuantities(PlayerPrefs.GetInt("CanMovePlayer1") == 1);
    }

    public void UpdateFruitQuantities(bool isPlayer1)
    {
        for (int i = 0; i < fruitQuantities.Length; i++)
        {
            int fruitQuantity = 0;
            if (isPlayer1)
            {
                fruitQuantity = PlayerPrefs.GetInt(fruitNames[i] + "Player1");
            }
            else
            {
                fruitQuantity = PlayerPrefs.GetInt(fruitNames[i] + "Player2");
            }
            fruitQuantities[i].text = fruitQuantity.ToString();
        }
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveSelection(-1); // Mueve la selección a la izquierda
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveSelection(1); // Mueve la selección a la derecha
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                SelectFruit(); // Selecciona la fruta
            }
        }
    }
}
