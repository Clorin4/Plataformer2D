using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class RunningGame : MonoBehaviour
{
    public SpriteRenderer sprite3Renderer;
    public SpriteRenderer sprite2Renderer;
    public SpriteRenderer sprite1Renderer;
    public SpriteRenderer spriteAdelanteRenderer;

    private Coroutine countdownCoroutine; 
    private Coroutine showPhraseCoroutine; 
    private Coroutine playerTurnTimerCoroutine;

    public GameObject panelFrases;
    public TextMeshProUGUI textPanel; // frase
    public TMP_InputField inputField;
    public Button submitButton;
    private string currentRandomPhrase;

    public PhraseManager phraseManager;
    PhraseData selectedPhraseData;
    float phraseTime;
    string[] phrases;

    //private string currentRandomPhrase;
    private bool isTimerRunning = false;
    private float elapsedTime = 0f;

    private bool avanzamo = false;
    private bool isPlayer1Turn = true; // Variable para controlar los turnos
    private int currentPlayer = 1; // Variable para identificar el jugador actual

    int i = -10;

    // Start is called before the first frame update
    void Start()
    {
        submitButton.onClick.AddListener(SubmitAnswerWithoutParameter); // Cambia esta línea
        TurnOffVariables();
        SaberDificultad();
        StartCoroutine(Countdown());
    }
    private void SubmitAnswerWithoutParameter()
    {
        SubmitAnswer(currentRandomPhrase); // Llama a SubmitAnswer con el parámetro almacenado
    }


    public void TurnOffVariables()
    {
        isPlayer1Turn = true;
        panelFrases.SetActive(false);
        sprite3Renderer.gameObject.SetActive(false);
        sprite2Renderer.gameObject.SetActive(false);
        sprite1Renderer.gameObject.SetActive(false);
        spriteAdelanteRenderer.gameObject.SetActive(false);
    }

    public void SaberDificultad()
    {
        DifficultyLevel selectedDifficulty;

        string selectedDifficultyy = PlayerPrefs.GetString("SelectedDifficulty");
        switch (selectedDifficultyy)
        {
            case "dif1":
                selectedDifficulty = DifficultyLevel.Easy;
                selectedPhraseData = phraseManager.GetPhraseDataByDifficulty(selectedDifficulty);
                break;

            case "dif2":
                selectedDifficulty = DifficultyLevel.Normal;
                selectedPhraseData = phraseManager.GetPhraseDataByDifficulty(selectedDifficulty);
                break;

            case "dif3":
                selectedDifficulty = DifficultyLevel.Hard;
                selectedPhraseData = phraseManager.GetPhraseDataByDifficulty(selectedDifficulty);
                break;

            case "dif4":
                selectedDifficulty = DifficultyLevel.Insane;
                selectedPhraseData = phraseManager.GetPhraseDataByDifficulty(selectedDifficulty);
                break;

            case "dif5":
                selectedDifficulty = DifficultyLevel.Demon;
                selectedPhraseData = phraseManager.GetPhraseDataByDifficulty(selectedDifficulty);
                break;

            case "dif6":
                selectedDifficulty = DifficultyLevel.SuperDemon;
                selectedPhraseData = phraseManager.GetPhraseDataByDifficulty(selectedDifficulty);
                break;

            default:
                // Manejar una dificultad inesperada
                break;
        }

        // Obtener datos de frases y tiempos para un nivel de dificultad específico (por ejemplo, "Easy")



        if (selectedPhraseData != null)
        {
            phraseTime = selectedPhraseData.phraseTime;
            phrases = selectedPhraseData.phrases;
        }
        else
        {
            Debug.LogWarning("No se encontraron datos para la dificultad seleccionada.");
        }
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1f); 
        

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
        yield return ScaleSpriteTo(spriteAdelanteRenderer, Vector3.zero, Vector3.one * .5f, .9f); // Escalar de 0 a un tamaño específico
        spriteAdelanteRenderer.gameObject.SetActive(false);

        // Lógica para iniciar el juego después de la cuenta regresiva
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
        StartCoroutine(ShowRandomPhrase());
    }

    IEnumerator ShowRandomPhrase()
    {
        panelFrases.SetActive(true);
        panelFrases.transform.localScale = Vector3.one; // Establecer el tamaño inicial

        iTween.ScaleFrom(panelFrases, Vector3.zero, 1f);

        inputField.gameObject.SetActive(true);

        if (selectedPhraseData != null)
        {
            currentRandomPhrase = phrases[Random.Range(0, phrases.Length)];
            textPanel.text = currentRandomPhrase;

            isTimerRunning = true;
            elapsedTime = 0f;

            while (elapsedTime < phraseTime)
            {
                if (!isTimerRunning) break;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (isTimerRunning)
            {
                textPanel.text = "";
                inputField.onEndEdit.AddListener(delegate { SubmitAnswer(currentRandomPhrase); });
                //EndCurrentTurn();
            }
            else
            {
                PlayerTurnTimer();
            }
        }
        else
        {
            Debug.LogWarning("No se encontraron datos para la dificultad seleccionada.");
        }


    }

    IEnumerator PlayerTurnTimer()
    {
        yield return new WaitForSeconds(.1f);

        if (isTimerRunning)
        {
            isTimerRunning = false;
            Debug.Log("Tiempo agotado para el Jugador " + currentPlayer);
            EndCurrentTurn();
        }
    }

    void EndCurrentTurn()
    {
        if (isPlayer1Turn)
        {
            isPlayer1Turn = false;
            currentPlayer = 2;
        }
        else
        {
            isPlayer1Turn = true;
            currentPlayer = 1;
        }

        Debug.Log("Turno del Jugador " + currentPlayer);
        ReiniciarJuego();
    }

    private void SubmitAnswer(string randomPhrase)
    {
        randomPhrase = textPanel.text;

        string playerTypedPhrase = inputField.text;

        if (playerTypedPhrase == randomPhrase)
        {
            inputField.text = "";
            inputField.DeactivateInputField();
            avanzamo = true;
            //AdvancePlayerGrid();
            Debug.Log("¡Correcto! Jugador " + currentPlayer);
            
            //ReiniciarJuego();
        }
        else
        {
            inputField.text = "";
            inputField.DeactivateInputField();
            Debug.Log("¡Incorrecto! Jugador " + currentPlayer);
            EndCurrentTurn();
            //ReiniciarJuego();
        }
    }
    private void Update()
    {
        if (avanzamo)
        {
           StartCoroutine(AdvancePlayerGrid());
            
        }
        
    }

    IEnumerator AdvancePlayerGrid()
    {

        float velocidadMovimiento = 1.0f; // Modifica este valor según la velocidad deseada
        Vector3 destinoPos = new Vector3(i, 0.0f, 0.0f); // Modifica esto con la posición a la que quieres mover el Grid

        GameObject gridObject = GameObject.Find("GridP1");

        if (gridObject != null)
        {
            // Movimiento suavizado del Grid hacia la posición de destino
            gridObject.transform.position = Vector3.Lerp(gridObject.transform.position, destinoPos, Time.deltaTime * velocidadMovimiento);
        }
        avanzamo = false;
        i = i - 10;
        
        
        
        yield return new WaitForSeconds(1f);
        EndCurrentTurn();
        Debug.Log(i);
    }

    private void ReiniciarJuego()
    {
        // Detener las corrutinas activas si es que están ejecutándose
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        if (showPhraseCoroutine != null)
        {
            StopCoroutine(showPhraseCoroutine);
        }

        if (playerTurnTimerCoroutine != null)
        {
            StopCoroutine(playerTurnTimerCoroutine);
        }

        //avanzamo = false;
        panelFrases.SetActive(false);
        showPhraseCoroutine = StartCoroutine(ShowRandomPhrase()); // Iniciar la siguiente frase directamente
    }


}
