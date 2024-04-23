using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FruitSelectionMenu : MonoBehaviour
{
    public Button[] fruitButtons; // Botones de frutas (en orden: manzanas, plátanos, naranjas)
    public TextMeshProUGUI[] fruitQuantities; // Textos para las cantidades de frutas
    private int selectedButtonIndex = 0;
    private string[] fruitNames = { "Manzanas", "Platanos", "Naranjas" }; // Nombres de las frutas en orden

    private void Start()
    {
        HideMenu();
    }

    public void ShowMenu(bool isPlayer1)
    {
        gameObject.SetActive(true);
        UpdateFruitQuantities(isPlayer1);
        selectedButtonIndex = 0;
        MoveSelection(0);
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
        // Aquí puedes implementar la lógica para manejar la selección de la fruta
        // Por ejemplo, incrementar la cantidad de frutas seleccionadas
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
