using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizzGame : MonoBehaviour
{
    public SpriteRenderer sprite3Renderer;
    public SpriteRenderer sprite2Renderer;
    public SpriteRenderer sprite1Renderer;
    public SpriteRenderer spriteAdelanteRenderer;

    public GameObject panelQuestion; // El panel que contiene la pregunta y los botones
    public float panelScaleDuration = 1.0f;

    private bool player1Pressed;
    private bool player2Pressed;
    private bool countDownStarted;
    public GameObject teclaD;
    public GameObject teclaK;

    public QuestionManager questionManager;
    public Question currentQuestion;

    public TextMeshProUGUI questionText;
    public Button[] answerButtons;

    private void Start()
    {
        panelQuestion.SetActive(false);

        sprite3Renderer.gameObject.SetActive(false);
        sprite2Renderer.gameObject.SetActive(false);
        sprite1Renderer.gameObject.SetActive(false);
        spriteAdelanteRenderer.gameObject.SetActive(false);

        teclaD.SetActive(false);
        teclaK.SetActive(false);

        //RECUERDA HACER FUNCION PARA BOTON DE INSTRUCCIONES Y YA EMPEZAR AL JUEGO

        SaberDificultad();

        StartCoroutine(Countdown());
    }

    public void SaberDificultad()
    {
        string selectedDifficulty = PlayerPrefs.GetString("SelectedDifficulty");
        switch (selectedDifficulty)
        {
            case "dif1":
                questionManager.questions = questionManager.Easyquestions.ConvertAll(q => (Question)q);
                break;

            case "dif2":
                questionManager.questions = questionManager.Normalquestions.ConvertAll(q => (Question)q);
                break;

            case "dif3":
                questionManager.questions = questionManager.Hardquestions.ConvertAll(q => (Question)q);
                break;

            case "dif4":
                questionManager.questions = questionManager.Insanequestions.ConvertAll(q => (Question)q);
                break;

            case "dif5":
                questionManager.questions = questionManager.Demonquestions.ConvertAll(q => (Question)q);
                break;

            case "dif6":
                questionManager.questions = questionManager.SuperDemonquestions.ConvertAll(q => (Question)q);
                break;

            default:
                // Manejar una dificultad inesperada
                break;
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
        StartCoroutine(ShowQuestionPanel());
        StartCoroutine(DetectKeyPress());
    }

    IEnumerator DetectKeyPress()
    {
        //yield return 

        yield return new WaitForSeconds(5f);

        countDownStarted = false;
        float countdownTimer = 10f;
        while (countdownTimer > 0f && !countDownStarted)
        {
            teclaD.SetActive(true);
            teclaK.SetActive(true);

            if (Input.GetKeyDown(KeyCode.D) && !player1Pressed)
            {
                player1Pressed = true;
                countDownStarted = true;

                // Realizar acciones para el jugador 1 cuando presiona la tecla D
            }

            if (Input.GetKeyDown(KeyCode.K) && !player2Pressed)
            {
                player2Pressed = true;
                countDownStarted = true;
                // Realizar acciones para el jugador 2 cuando presiona la tecla K
            }

            countdownTimer -= Time.deltaTime;
            Debug.Log(countdownTimer);
            yield return null;
        }

        // Lógica para determinar quién presionó más rápido y proceder en consecuencia
        DetermineWinner();

        /*if (player1Pressed || player2Pressed)
        {
            yield return new WaitForSeconds(2f); // Agregar un pequeño retraso antes de reiniciar
            ReiniciarJuego();
        }*/
    } //OJOOOOOOOOO

    void DetermineWinner() //definir banderas de jugadores
    {
        if (player1Pressed && !player2Pressed)
        {
            teclaD.SetActive(false);
            teclaK.SetActive(false);

            EnableAnswerButtons();
            Debug.Log("GANA EL 1");
            // Acciones si solo el jugador 1 presionó más rápido
        }
        else if (!player1Pressed && player2Pressed)
        {
            teclaD.SetActive(false);
            teclaK.SetActive(false);

            EnableAnswerButtons();
            Debug.Log("GANA EL 2");
            // Acciones si solo el jugador 2 presionó más rápido
        }
        else if (player1Pressed && player2Pressed) //Palomita
        {
            teclaD.SetActive(false);
            teclaK.SetActive(false);
            Debug.Log("Ambos?");
            // Acciones si ambos jugadores presionaron, se puede considerar un empate
        }
        else
        {
            teclaD.SetActive(false);
            teclaK.SetActive(false);
            Debug.Log("Ninguno");
            ReiniciarJuego();
            // Acciones si ninguno presionó
        }
    }


    IEnumerator ShowQuestionPanel()
    {
        //StartCoroutine(ShowQuestionAndAnswers());
        Debug.Log("PANEEEEEEEEEEEEEEL");

        // Mostrar el panel usando iTween (escala desde 0 a 1)
        panelQuestion.SetActive(true);
        iTween.ScaleFrom(panelQuestion, Vector3.zero, panelScaleDuration);

        // Deshabilitar los botones hasta que un jugador presione su tecla
        DisableAnswerButtons();

        // Esperar a que un jugador presione su tecla
        yield return ShowQuestionAndAnswers();

        // Una vez que un jugador presiona la tecla, habilitar los botones
        //EnableAnswerButtons();
    }

    IEnumerator ShowQuestionAndAnswers()
    {
         

        // Obtener una pregunta aleatoria de la lista
        int randomIndex = Random.Range(0, questionManager.questions.Count);
        currentQuestion = questionManager.questions[randomIndex];

        Debug.Log(randomIndex);
        // Mostrar la pregunta en el TextMeshPro
        questionText.text = currentQuestion.questionText;

        // Mostrar las respuestas en los botones
        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Verificar si el índice del botón coincide con el índice de la respuesta correcta
            if (i == currentQuestion.correctAnswerIndex)
            {
                // La respuesta correcta se asigna al botón correspondiente
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.options[i];
                answerButtons[i].onClick.AddListener(() => OnCorrectAnswerSelected());
            }
            else
            {
                // Las respuestas incorrectas se asignan a los botones restantes
                int wrongAnswerIndex = Random.Range(0, currentQuestion.options.Length);
                // Se evita que una respuesta incorrecta se muestre más de una vez
                while (wrongAnswerIndex == currentQuestion.correctAnswerIndex)
                {
                    wrongAnswerIndex = Random.Range(0, currentQuestion.options.Length);
                }
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.options[wrongAnswerIndex];
                answerButtons[i].onClick.AddListener(() => OnWrongAnswerSelected());
            }
        }

        yield return null;
    }

    public void OnCorrectAnswerSelected()
    {
        ReiniciarJuego();
        // Acciones cuando se selecciona la respuesta correcta
        Debug.Log("¡Respuesta correcta seleccionada!");
        // Aquí puedes llamar a la función que maneja la respuesta correcta
    }

    public void OnWrongAnswerSelected()
    {
        ReiniciarJuego();
        // Acciones cuando se selecciona una respuesta incorrecta
        Debug.Log("Respuesta incorrecta seleccionada");
        // Aquí puedes llamar a la función que maneja la respuesta incorrecta
    }

    void DisableAnswerButtons()
    {
        foreach (Button button in answerButtons)
        {
            button.interactable = false;
        }
    }

    void EnableAnswerButtons()
    {
        foreach (Button button in answerButtons)
        {
            button.interactable = true;
        }
    }

    void ReiniciarJuego()
    {
        // Deshabilitar todos los elementos, reiniciar variables, etc.
        // Aquí reinicias todo lo necesario para comenzar un nuevo ciclo del juego

        panelQuestion.SetActive(false);
        teclaD.SetActive(false);
        teclaK.SetActive(false);
        player1Pressed = false;
        player2Pressed = false;

        // Llamar a la función que maneja el ciclo del juego desde el principio
        //StartCoroutine(Countdown());
        StartCoroutine(ShowNextQuestion());
    }

    IEnumerator ShowNextQuestion()
    {
        // Espera 2 segundos antes de mostrar la siguiente pregunta
        yield return new WaitForSeconds(2f);

        // Limpia los eventos de los botones
        ClearButtonEvents();

        // Muestra la siguiente pregunta
        yield return StartCoroutine(Countdown());
    }

    void ClearButtonEvents()
    {
        foreach (Button button in answerButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

}
