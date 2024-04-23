using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FruitSelectionMenu : MonoBehaviour
{
    public TextMeshProUGUI[] fruitTexts; // Textos para las cantidades de frutas
    public Image[] fruitImages; // Imágenes de las frutas
    public GameObject menuObject; // Objeto del menú

    public void ShowMenu(Dictionary<string, int> collectedFruits)
    {
        // Activa el menú
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
                    // Aquí necesitas tener las imágenes de las frutas en el orden correcto en fruitImages
                    // Es decir, fruitImages[0] debe ser la imagen de las manzanas, fruitImages[1] la de naranjas, y así sucesivamente
                    // Si no están en este orden, deberías actualizar este código para que funcione correctamente
                    break;
                }
            }
        }
    }

    public void HideMenu()
    {
        // Oculta el menú
        menuObject.SetActive(false);
    }
}
