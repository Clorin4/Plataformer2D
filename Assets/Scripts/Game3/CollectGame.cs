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
        yield return new WaitForSeconds(.001f);
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
            // Obtener la longitud máxima de las tres listas
            int maxCount = Mathf.Max(currentOrder.fruitNames.Count, currentOrder.quantities.Count, currentOrder.operations.Count);
            Debug.Log(maxCount);

            // Verificar para cada jugador por separado
            bool orderCompletedPlayer1 = true;
            bool orderCompletedPlayer2 = true;

            // Obtener las frutas recolectadas por cada jugador
            Dictionary<string, int> collectedFruitsPlayer1 = new Dictionary<string, int>
        {
            { "Manzanas", PlayerPrefs.GetInt("ManzanasPlayer1", 0) },
            { "Naranjas", PlayerPrefs.GetInt("NaranjasPlayer1", 0) },
            { "Platanos", PlayerPrefs.GetInt("PlatanosPlayer1", 0) }
        };

            Dictionary<string, int> collectedFruitsPlayer2 = new Dictionary<string, int>
        {
            { "Manzanas", PlayerPrefs.GetInt("ManzanasPlayer2", 0) },
            { "Naranjas", PlayerPrefs.GetInt("NaranjasPlayer2", 0) },
            { "Platanos", PlayerPrefs.GetInt("PlatanosPlayer2", 0) }
        };

            // Variables para contar las frutas de la orden
            Dictionary<string, int> totalFruitsPlayer1 = new Dictionary<string, int>
        {
            { "Manzanas", 0 },
            { "Naranjas", 0 },
            { "Platanos", 0 }
        };

            Dictionary<string, int> totalFruitsPlayer2 = new Dictionary<string, int>
        {
            { "Manzanas", 0 },
            { "Naranjas", 0 },
            { "Platanos", 0 }
        };

            // Iterar sobre la longitud máxima de las listas
            for (int i = 0; i < maxCount; i++)
            {
                // Obtener la fruta, cantidad y operación en la posición i
                string fruitName = i < currentOrder.fruitNames.Count ? currentOrder.fruitNames[i] : "";
                int quantityNeeded = i < currentOrder.quantities.Count ? currentOrder.quantities[i] : 0;
                string operation = i < currentOrder.operations.Count ? currentOrder.operations[i] : "";

                // Realizar la operación matemática correspondiente
                if (i == 0)
                {
                    // Si es el primer elemento o no es una suma, simplemente asigna la cantidad necesaria
                    totalFruitsPlayer1[fruitName] = quantityNeeded;
                    totalFruitsPlayer2[fruitName] = quantityNeeded;
                    Debug.Log("SE OCUPAAAAN " + quantityNeeded);
                }
                else
                {
                    Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    // Si es una suma y no es el primer elemento
                    if (currentOrder.fruitNames[i - 1] == fruitName)
                    {
                        Debug.Log(operation);

                        // Realiza la operación correspondiente
                        switch (operation)
                        {

                            case "+":
                                totalFruitsPlayer1[fruitName] += quantityNeeded;
                                totalFruitsPlayer2[fruitName] += quantityNeeded;
                                Debug.Log("Jugador 1: Se sumaron " + quantityNeeded + " " + fruitName + ". Total: " + totalFruitsPlayer1[fruitName]);
                                Debug.Log("Jugador 2: Se sumaron " + quantityNeeded + " " + fruitName + ". Total: " + totalFruitsPlayer2[fruitName]);
                                break;
                            case "-":
                                totalFruitsPlayer1[fruitName] -= quantityNeeded;
                                totalFruitsPlayer2[fruitName] -= quantityNeeded;
                                Debug.Log("Jugador 1: Se restaron " + quantityNeeded + " " + fruitName + ". Total: " + totalFruitsPlayer1[fruitName]);
                                Debug.Log("Jugador 2: Se restaron " + quantityNeeded + " " + fruitName + ". Total: " + totalFruitsPlayer2[fruitName]);
                                break;
                            case "x":
                                totalFruitsPlayer1[fruitName] *= quantityNeeded;
                                totalFruitsPlayer2[fruitName] *= quantityNeeded;
                                Debug.Log("Jugador 1: Se multiplicaron " + quantityNeeded + " " + fruitName + ". Total: " + totalFruitsPlayer1[fruitName]);
                                Debug.Log("Jugador 2: Se multiplicaron " + quantityNeeded + " " + fruitName + ". Total: " + totalFruitsPlayer2[fruitName]);
                                break;
                            default:
                                Debug.LogError("Unknown operation: " + operation);
                                orderCompletedPlayer1 = false; // Marcar como no completado
                                orderCompletedPlayer2 = false; // Marcar como no completado
                                break;
                        }
                    }
                    else
                    {
                        // Si la fruta es diferente a la anterior, simplemente asigna la cantidad necesaria
                        totalFruitsPlayer1[fruitName] = quantityNeeded;
                        totalFruitsPlayer2[fruitName] = quantityNeeded;

                        Debug.Log("Jugador 1: Se necesitan " + quantityNeeded + " " + fruitName + ".");
                        Debug.Log("Jugador 2: Se necesitan " + quantityNeeded + " " + fruitName + ".");
                    }
                }
            }

            // Verificar para el Player2
            foreach (KeyValuePair<string, int> kvp in totalFruitsPlayer2)
            {
                string fruitName = kvp.Key;
                int totalQuantity = kvp.Value;

                // Verificar si el jugador tiene suficientes frutas recolectadas
                if (collectedFruitsPlayer2.ContainsKey(fruitName))
                {
                    // Verificar si el jugador tiene más frutas de las necesarias
                    if (collectedFruitsPlayer2[fruitName] > totalQuantity)
                    {
                        orderCompletedPlayer2 = false; // Marcar como no completado
                        Debug.Log("Jugador 2: Tiene " + (collectedFruitsPlayer2[fruitName] - totalQuantity) + " " + fruitName + " de más.");
                    }
                    else
                    {
                        orderCompletedPlayer2 &= collectedFruitsPlayer2[fruitName] >= totalQuantity;
                        Debug.Log("Jugador 2: Se necesitan " + Mathf.Max(0, totalQuantity - collectedFruitsPlayer2[fruitName]) + " " + fruitName + " adicionales.");
                    }

                    if (!orderCompletedPlayer2) break; // Salir del bucle si ya no se completó la orden
                }
                else
                {
                    orderCompletedPlayer2 = false; // La fruta necesaria no está en la lista de recolección del jugador
                    break; // Salir del bucle si no se encuentra la fruta
                }
            }

            // Verificar para el Player1
            foreach (KeyValuePair<string, int> kvp in totalFruitsPlayer1)
            {
                string fruitName = kvp.Key;
                int totalQuantity = kvp.Value;

                // Verificar si el jugador tiene suficientes frutas recolectadas
                if (collectedFruitsPlayer1.ContainsKey(fruitName))
                {
                    // Verificar si el jugador tiene más frutas de las necesarias
                    if (collectedFruitsPlayer1[fruitName] > totalQuantity)
                    {
                        orderCompletedPlayer1 = false; // Marcar como no completado
                        Debug.Log("Jugador 1: Tiene " + (collectedFruitsPlayer1[fruitName] - totalQuantity) + " " + fruitName + " de más.");
                    }
                    else
                    {
                        orderCompletedPlayer1 &= collectedFruitsPlayer1[fruitName] >= totalQuantity;
                        Debug.Log("Jugador 1: Se necesitan " + Mathf.Max(0, totalQuantity - collectedFruitsPlayer1[fruitName]) + " " + fruitName + " adicionales.");
                    }

                    if (!orderCompletedPlayer1) break; // Salir del bucle si ya no se completó la orden
                }
                else
                {
                    orderCompletedPlayer1 = false; // La fruta necesaria no está en la lista de recolección del jugador
                    break; // Salir del bucle si no se encuentra la fruta
                }
            }

            // Mostrar el resultado de la verificación para cada jugador
            if (orderCompletedPlayer1)
            {
                Debug.Log("¡El jugador 1 completó la orden correctamente!");
            }
            else
            {
                Debug.Log("¡El jugador 1 aún necesita recolectar más frutas!");
            }

            if (orderCompletedPlayer2)
            {
                Debug.Log("¡El jugador 2 completó la orden correctamente!");
            }
            else
            {
                Debug.Log("¡El jugador 2 aún necesita recolectar más frutas!");
            }
        }
    }




}
