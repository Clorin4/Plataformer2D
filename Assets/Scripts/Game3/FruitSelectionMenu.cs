using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitSelectionMenu : MonoBehaviour
{
    public Button[] fruitButtons; // Botones de frutas (en orden: manzanas, plátanos, naranjas)
    public Text[] fruitQuantities; // Textos para las cantidades de frutas
    public GameObject cancelButton; // Botón de cancelar
    private int selectedButtonIndex = 0;

    private void Start()
    {
        cancelButton.SetActive(false);
    }

    public void ShowMenu(Dictionary<string, int> collectedFruits)
    {
        // Muestra los botones de frutas y el botón de cancelar
        for (int i = 0; i < fruitButtons.Length; i++)
        {
            fruitButtons[i].gameObject.SetActive(true);
            string fruitName = fruitButtons[i].gameObject.name; // Nombre del botón (asumimos que es igual al nombre de la fruta)
            if (collectedFruits.ContainsKey(fruitName))
            {
                fruitQuantities[i].text = collectedFruits[fruitName].ToString(); // Ajusta el texto del botón a la cantidad de frutas
                fruitButtons[i].interactable = true; // Hacer el botón interactivo
            }
            else
            {
                fruitQuantities[i].text = "0"; // Si no hay ninguna fruta de este tipo, muestra 0
                fruitButtons[i].interactable = false; // Hacer el botón no interactivo
            }
        }
        cancelButton.SetActive(true);
    }

    public void HideMenu()
    {
        // Oculta todos los botones al cerrar
        foreach (var button in fruitButtons)
        {
            button.gameObject.SetActive(false);
        }
        cancelButton.SetActive(false);
    }

    public void MoveSelection(int direction)
    {
        // Mueve la selección hacia la izquierda (-1) o la derecha (1)
        selectedButtonIndex = (selectedButtonIndex + direction) % fruitButtons.Length;
        if (selectedButtonIndex < 0)
        {
            selectedButtonIndex += fruitButtons.Length;
        }

        // Resalta el botón seleccionado
        for (int i = 0; i < fruitButtons.Length; i++)
        {
            fruitButtons[i].image.color = (i == selectedButtonIndex) ? Color.yellow : Color.white;
        }
    }

    public void SelectFruit()
    {
        // Lógica para seleccionar la fruta (puedes agregarla según sea necesario)
        Debug.Log("Fruit selected: " + fruitButtons[selectedButtonIndex].name);
        Debug.Log("OLA");
    }
}
