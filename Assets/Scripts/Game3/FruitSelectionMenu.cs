using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FruitSelectionMenu : MonoBehaviour
{
    public TextMeshProUGUI[] fruitTexts; // Textos para las cantidades de frutas
    public Image[] fruitImages; // Im�genes de las frutas
    public GameObject menuObject; // Objeto del men�

    public void ShowMenu(Dictionary<string, int> collectedFruits)
    {
        // Activa el men�
        menuObject.SetActive(true);

        // Itera sobre todas las frutas conocidas
        foreach (var kvp in collectedFruits)
        {
            string fruitName = kvp.Key;
            int quantity = kvp.Value;

            // Actualiza el texto de la fruta con la cantidad correspondiente
            foreach (var fruitText in fruitTexts)
            {
                if (fruitText.gameObject.name == fruitName + "Text")
                {
                    fruitText.text = quantity.ToString();
                    break;
                }
            }

            // Actualiza la imagen de la fruta
            foreach (var fruitImage in fruitImages)
            {
                if (fruitImage.gameObject.name == fruitName + "Image")
                {
                    // Aqu� necesitas tener las im�genes de las frutas en el orden correcto en fruitImages
                    // Es decir, fruitImages[0] debe ser la imagen de las manzanas, fruitImages[1] la de naranjas, y as� sucesivamente
                    // Si no est�n en este orden, deber�as actualizar este c�digo para que funcione correctamente
                    break;
                }
            }
        }
    }

    public void HideMenu()
    {
        // Oculta el men�
        menuObject.SetActive(false);
    }
}
