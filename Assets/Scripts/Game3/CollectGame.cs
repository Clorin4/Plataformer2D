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
    public List<TextMeshProUGUI> quantityTexts;
    public List<TextMeshProUGUI> operationTexts;
    private Order currentOrder;

    public GameObject canvasPedido;

    public Transform player1;
    public Transform player2;
    public float interactionRadius = 3f;
    private bool isInRange = false; // Indica si el jugador está en rango de interacción.
    private bool isInteracting = false;
    private bool isOnClickedE = false;
    public GameObject globoTextE;

    public string selectedDifficulty;


    void Start()
    {
        PlayerPrefs.SetInt("ManzanasPlayer1", 0);
        PlayerPrefs.SetInt("ManzanasPlayer2", 0);
        PlayerPrefs.SetInt("NaranjasPlayer1", 0);
        PlayerPrefs.SetInt("NaranjasPlayer2", 0);
        PlayerPrefs.SetInt("PlatanosPlayer1", 0);
        PlayerPrefs.SetInt("PlatanosPlayer2", 0);

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

        canvasPedido.SetActive(false);
        globoTextE.SetActive(false);

        
    }

    public void SaberDificultad()
    {
        selectedDifficulty = PlayerPrefs.GetString("SelectedDifficulty");
        
    }

    public void ShowOrder(string difficulty)
    {
        canvasPedido.SetActive(true);

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
                    // Mostrar el símbolo matemático en el TextMeshProUGUI correspondiente
                    if (i < currentOrder.operations.Count)
                    {
                        operationTexts[i].text = currentOrder.operations[i];
                    }
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
        yield return new WaitForSeconds(.01f);
        player1 = GameObject.FindGameObjectWithTag("Player1").transform;
        player2 = GameObject.FindGameObjectWithTag("Player2").transform;

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

        yield return new WaitForSeconds(.1f);

        ShowOrder(selectedDifficulty);
        canvasPedido.SetActive(true);
        iTween.ScaleFrom(canvasPedido, Vector3.zero, 1f); // Animar la escala del canvas desde cero a su tamaño normal en 1 segundo



        StartGame();
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


    public void StartGame()
    {
        
    }


    private void Update()
    {
        float distance1 = Vector3.Distance(transform.position, player1.position);
        float distance2 = Vector3.Distance(transform.position, player2.position);


        // Comprueba si el jugador está dentro del radio de interacción.
        if (distance1 <= interactionRadius || distance2 <= interactionRadius)
        {
            isInRange = true;

            if (!isOnClickedE)
            {
                globoTextE.SetActive(true);
            }

            if (isInRange == true && Input.GetKeyDown(KeyCode.E))
            {
                globoTextE.SetActive(false);
                isOnClickedE = true;
                //CheckOrderCompletion();
            }

        }
        else if (!isInRange == true && isInteracting == true)
        {
            isOnClickedE = false;
            globoTextE.SetActive(false);
            isInteracting = false;
        }
        else
        {
            isOnClickedE = false;
            isInRange = false;
            globoTextE.SetActive(false);
            
        }
    }


    private void CheckOrderCompletion()
    {
        if (currentOrder != null)
        {
            bool orderCompleted = true;
            int totalFruitsNeeded = 0;

            // Calcular el total de frutas necesarias según la orden actual
            for (int i = 0; i < currentOrder.fruits.Count; i++)
            {
                totalFruitsNeeded += currentOrder.quantities[i];
            }

            // Realizar la operación matemática dependiendo del símbolo de la orden
            int totalCollectedFruits = 0;
            for (int i = 0; i < currentOrder.fruits.Count; i++)
            {
                string fruitTag = currentOrder.fruits[i].name == "Apple" ? "ManzanasP1" : "ManzanasP2";
                int collectedFruit = PlayerPrefs.GetInt(fruitTag, 0);
                totalCollectedFruits += collectedFruit;
            }

            // Iterar sobre cada operación en la lista
            for (int i = 0; i < currentOrder.operations.Count; i++)
            {
                // Verificar si el total recolectado coincide con el total necesario según la operación
                if (currentOrder.operations[i] == "+")
                {
                    orderCompleted &= totalCollectedFruits == totalFruitsNeeded;
                }
                else if (currentOrder.operations[i] == "-")
                {
                    orderCompleted &= totalCollectedFruits == Mathf.Abs(totalFruitsNeeded);
                }
            }

            if (orderCompleted)
            {
                // La orden se ha completado correctamente
                Debug.Log("¡Orden completada correctamente!");
            }
            else
            {
                // La orden aún no se ha completado
                Debug.Log("¡Aún falta recoger más frutas!");
            }
        }
    }





}
