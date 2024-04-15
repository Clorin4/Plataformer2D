using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CollectGame : MonoBehaviour
{
    // Start is called before the first frame update

    public SpriteRenderer sprite3Renderer;
    public SpriteRenderer sprite2Renderer;
    public SpriteRenderer sprite1Renderer;
    public SpriteRenderer spriteAdelanteRenderer;

    public List<Image> fruitImages; // Lista de objetos tipo Image para los sprites de las frutas
    public List<TextMeshProUGUI> quantityTexts; // Lista de objetos tipo TextMeshProUGUI para las cantidades
    private Order currentOrder;




    void Start()
    {
        TurnOffVariables();
        SaberDificultad();

        StartCoroutine(Countdown());
    }

    public void TurnOffVariables()
    {
        sprite1Renderer.gameObject.SetActive(false);
        sprite2Renderer.gameObject.SetActive(false);
        sprite3Renderer.gameObject.SetActive(false);
        spriteAdelanteRenderer.gameObject.SetActive(false);
    }

    public void SaberDificultad()
    {
        string selectedDifficulty = PlayerPrefs.GetString("SelectedDifficulty");
        ShowOrder(selectedDifficulty);
    }

    public void ShowOrder(string difficulty)
    {
        FruitOrderManager fruitOrderManager = FindObjectOfType<FruitOrderManager>();
        if (fruitOrderManager != null)
        {
            List<Order> orders = fruitOrderManager.GetCurrentOrders(difficulty);
            if (orders != null && orders.Count > 0)
            {
                int randomIndex = Random.Range(0, orders.Count);
                currentOrder = orders[randomIndex]; // Seleccionar un pedido aleatorio
                for (int i = 0; i < fruitImages.Count; i++)
                {
                    fruitImages[i].sprite = currentOrder.fruits[i];
                    quantityTexts[i].text = currentOrder.quantities[i].ToString();
                }
            }
            else
            {
                Debug.LogError("No se encontraron órdenes para la dificultad: " + difficulty);
            }
        }
        else
        {
            Debug.LogWarning("No se encontró ningún objeto FruitOrderManager en la escena.");
        }
    }



    IEnumerator Countdown()
    {
        //yield return new WaitForSeconds(.3f);

        sprite3Renderer.gameObject.SetActive(true);
        yield return ScaleSpriteTo(sprite3Renderer, Vector3.zero, Vector3.one * 1f, .9f); // Escalar de 0 a un tamaño específico
        sprite3Renderer.gameObject.SetActive(false);

        yield return new WaitForSeconds(.1f);

        sprite2Renderer.gameObject.SetActive(true);
        yield return ScaleSpriteTo(sprite2Renderer, Vector3.zero, Vector3.one * 1f, .9f); // Escalar de 0 a un tamaño específico
        sprite2Renderer.gameObject.SetActive(false);

        yield return new WaitForSeconds(.1f);

        sprite1Renderer.gameObject.SetActive(true);
        yield return ScaleSpriteTo(sprite1Renderer, Vector3.zero, Vector3.one * 1f, .9f); // Escalar de 0 a un tamaño específico
        sprite1Renderer.gameObject.SetActive(false);

        yield return new WaitForSeconds(.1f);

        spriteAdelanteRenderer.gameObject.SetActive(true);
        yield return ScaleSpriteTo(spriteAdelanteRenderer, Vector3.zero, Vector3.one * .7f, .9f); // Escalar de 0 a un tamaño específico
        spriteAdelanteRenderer.gameObject.SetActive(false);

        
        //StartGame();
    }

    IEnumerator ScaleSpriteTo(SpriteRenderer spriteRenderer, Vector3 startScale, Vector3 endScale, float duration)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;
            spriteRenderer.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        spriteRenderer.transform.localScale = endScale;
    }

}
