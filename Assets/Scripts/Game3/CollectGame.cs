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
    private bool isInRange = false; 
    private bool isInteracting = false;
    private bool isOnClickedE = false;
    public GameObject globoTextE;
    public GameObject globoTextShift;

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
        globoTextShift.SetActive(false);


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
        if (distance1 <= interactionRadius)
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
                CheckOrderCompletion();
                Debug.Log("CHECAMO P1");
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
            isInRange = false;
            isOnClickedE = false;
            globoTextE.SetActive(false);
        }

        if (distance2 <= interactionRadius)
        {
            isInRange = true;

            if (!isOnClickedE)
            {
                globoTextShift.SetActive(true);
            }

            if (isInRange == true && Input.GetKeyDown(KeyCode.RightShift))
            {
                globoTextShift.SetActive(false);
                isOnClickedE = true;
                CheckOrderCompletion();
                Debug.Log("CHECAMO P2");
            }

        }
        else if (!isInRange == true && isInteracting == true)
        {
            isOnClickedE = false;
            globoTextShift.SetActive(false);
            isInteracting = false;
        }
        else
        {
            isOnClickedE = false;
            isInRange = false;
            globoTextShift.SetActive(false);
        }
    }


    private void CheckOrderCompletion()
    {
        if (currentOrder != null)
        {
            // Obtener la longitud mínima de las tres listas
            int minCount = Mathf.Min(currentOrder.fruitNames.Count, currentOrder.quantities.Count, currentOrder.operations.Count);

            // Verificar para cada jugador por separado
            bool orderCompletedPlayer1 = true;
            bool orderCompletedPlayer2 = true;

            // Obtener las frutas recolectadas por cada jugador
            int collectedApplesPlayer1 = PlayerPrefs.GetInt("ManzanasPlayer1", 0);
            int collectedApplesPlayer2 = PlayerPrefs.GetInt("ManzanasPlayer2", 0);
            int collectedOrangesPlayer1 = PlayerPrefs.GetInt("NaranjasPlayer1", 0);
            int collectedOrangesPlayer2 = PlayerPrefs.GetInt("NaranjasPlayer2", 0);
            int collectedBananasPlayer1 = PlayerPrefs.GetInt("PlatanosPlayer1", 0);
            int collectedBananasPlayer2 = PlayerPrefs.GetInt("PlatanosPlayer2", 0);

            // Iterar sobre la longitud mínima de las listas
            for (int i = 0; i < minCount; i++)
            {
                string fruitName = currentOrder.fruitNames[i];
                int quantityNeeded = currentOrder.quantities[i];
                string operation = currentOrder.operations[i];

                // Verificar para el Player1
                if (fruitName == "Manzanas")
                {
                    int collectedApples = PlayerPrefs.GetInt("ManzanasPlayer1", 0);
                    switch (operation)
                    {
                        case "+":
                            orderCompletedPlayer1 &= collectedApples >= quantityNeeded;
                            break;
                        case "-":
                            orderCompletedPlayer1 &= collectedApples <= quantityNeeded;
                            break;
                        case "x":
                            orderCompletedPlayer1 &= collectedApples == quantityNeeded;
                            break;
                        default:
                            Debug.LogError("Unknown operation: " + operation);
                            break;
                    }
                }
                else if (fruitName == "Naranjas")
                {
                    int collectedOranges = PlayerPrefs.GetInt("NaranjasPlayer1", 0);
                    switch (operation)
                    {
                        case "+":
                            orderCompletedPlayer1 &= collectedOranges >= quantityNeeded;
                            break;
                        case "-":
                            orderCompletedPlayer1 &= collectedOranges <= quantityNeeded;
                            break;
                        case "x":
                            orderCompletedPlayer1 &= collectedOranges == quantityNeeded;
                            break;
                        default:
                            Debug.LogError("Unknown operation: " + operation);
                            break;
                    }
                }
                else if (fruitName == "Platanos")
                {
                    int collectedBananas = PlayerPrefs.GetInt("PlatanosPlayer1", 0);
                    switch (operation)
                    {
                        case "+":
                            orderCompletedPlayer1 &= collectedBananas >= quantityNeeded;
                            break;
                        case "-":
                            orderCompletedPlayer1 &= collectedBananas <= quantityNeeded;
                            break;
                        case "x":
                            orderCompletedPlayer1 &= collectedBananas == quantityNeeded;
                            break;
                        default:
                            Debug.LogError("Unknown operation: " + operation);
                            break;
                    }
                }

                // Verificar para el Player2
                if (fruitName == "Manzanas")
                {
                    int collectedApples = PlayerPrefs.GetInt("ManzanasPlayer2", 0);
                    switch (operation)
                    {
                        case "+":
                            orderCompletedPlayer2 &= collectedApples >= quantityNeeded;
                            break;
                        case "-":
                            orderCompletedPlayer2 &= collectedApples <= quantityNeeded;
                            break;
                        case "x":
                            orderCompletedPlayer2 &= collectedApples == quantityNeeded;
                            break;
                        default:
                            Debug.LogError("Unknown operation: " + operation);
                            break;
                    }
                }
                else if (fruitName == "Naranjas")
                {
                    int collectedOranges = PlayerPrefs.GetInt("NaranjasPlayer2", 0);
                    switch (operation)
                    {
                        case "+":
                            orderCompletedPlayer2 &= collectedOranges >= quantityNeeded;
                            break;
                        case "-":
                            orderCompletedPlayer2 &= collectedOranges <= quantityNeeded;
                            break;
                        case "x":
                            orderCompletedPlayer2 &= collectedOranges == quantityNeeded;
                            break;
                        default:
                            Debug.LogError("Unknown operation: " + operation);
                            break;
                    }
                }
                else if (fruitName == "Platanos")
                {
                    int collectedBananas = PlayerPrefs.GetInt("PlatanosPlayer2", 0);
                    switch (operation)
                    {
                        case "+":
                            orderCompletedPlayer2 &= collectedBananas >= quantityNeeded;
                            break;
                        case "-":
                            orderCompletedPlayer2 &= collectedBananas <= quantityNeeded;
                            break;
                        case "x":
                            orderCompletedPlayer2 &= collectedBananas == quantityNeeded;
                            break;
                        default:
                            Debug.LogError("Unknown operation: " + operation);
                            break;
                    }
                }
            }

            // Mostrar el resultado de la verificación para cada jugador
            if (orderCompletedPlayer1 && orderCompletedPlayer2)
            {
                Debug.Log("¡Ambos jugadores completaron la orden correctamente!");
            }
            else if (orderCompletedPlayer1)
            {
                Debug.Log("¡El jugador 1 completó la orden correctamente!");
            }
            else if (orderCompletedPlayer2)
            {
                Debug.Log("¡El jugador 2 completó la orden correctamente!");
            }
            else
            {
                Debug.Log("¡Aún falta recoger más frutas!");
            }
        }
    }


}
