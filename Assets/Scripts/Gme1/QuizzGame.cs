using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuizzGame : MonoBehaviour
{
    //List<int> usedIndices = new List<int>();

    public SpriteRenderer sprite3Renderer;
    public SpriteRenderer sprite2Renderer;
    public SpriteRenderer sprite1Renderer;
    public SpriteRenderer spriteAdelanteRenderer;

    public GameObject apuntador1;
    public GameObject apuntador2;

    public GameObject panelQuestion; // El panel que contiene la pregunta y los botones
    public float panelScaleDuration = 1.0f;

    public bool J1Responde;
    public bool J2Responde;
    public bool venganza;

    private bool J1Dañado;
    private bool J2Dañado;

    private bool player1Pressed;
    private bool player2Pressed;
    private bool countDownStarted;
    public GameObject teclaD;
    public GameObject teclaK;

    public QuestionManager questionManager;
    public Question currentQuestion;

    public TextMeshProUGUI questionText;
    public Button[] answerButtons;

    public int player1Health = 100;
    public int player2Health = 100;

    private void Start()
    {
        panelQuestion.SetActive(false);

        apuntador1.SetActive(false);
        apuntador2.SetActive(false);

        sprite3Renderer.gameObject.SetActive(false);
        sprite2Renderer.gameObject.SetActive(false);
        sprite1Renderer.gameObject.SetActive(false);
        spriteAdelanteRenderer.gameObject.SetActive(false);

        J1Responde = false;
        J2Responde = false;
        venganza = false;

        J1Dañado = false;
        J2Dañado = false;

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
            //HACER HUD PARA QUE SEA VISIBLE EL TIEMPO
            yield return null;
        }

        teclaD.SetActive(false);
        teclaK.SetActive(false);

        
        DetermineWinner();

    } 

    void DetermineWinner() //definir banderas de jugadores
    {
        EnableArrows();

        if (player1Pressed && !player2Pressed)
        {
            J1Responde = true;

            EnableAnswerButtons();
            Debug.Log("GANA EL 1");
            // Acciones si solo el jugador 1 presionó más rápido
        }
        else if (!player1Pressed && player2Pressed)
        {
            J2Responde = true;

            EnableAnswerButtons();
            Debug.Log("GANA EL 2");
            // Acciones si solo el jugador 2 presionó más rápido
        }
        else if (player1Pressed && player2Pressed) //Palomita
        {
            Debug.Log("Ambos?");
            // Acciones si ambos jugadores presionaron, se puede considerar un empate
        }
        else
        {
            //DAÑOOO A AMBOS
            Debug.Log("Ninguno");
            Daños();
            // Acciones si ninguno presionó
        }
    }

    void EnableArrows()
    {
        if (player1Pressed)
        {
            apuntador1.SetActive(true);
            apuntador2.SetActive(false);
        }
        else if (player2Pressed)
        {
            apuntador2.SetActive(true);
            apuntador1.SetActive(false);
        }
        
    }

    IEnumerator ShowQuestionPanel()
    {
        // Mostrar el panel usando iTween (escala desde 0 a 1)
        panelQuestion.SetActive(true);
        iTween.ScaleFrom(panelQuestion, Vector3.zero, panelScaleDuration);

        // Deshabilitar los botones hasta que un jugador presione su tecla
        DisableAnswerButtons();

        // Esperar a que un jugador presione su tecla
        yield return ShowQuestionAndAnswers();
    }

    IEnumerator ShowQuestionAndAnswers()
    {
        // Obtener una pregunta aleatoria de la lista
        int randomIndex = Random.Range(0, questionManager.questions.Count);
        currentQuestion = questionManager.questions[randomIndex];

        // Mostrar la pregunta en el TextMeshPro
        questionText.text = currentQuestion.questionText;

        List<string> answers = new List<string>(currentQuestion.options);
        List<string> displayedAnswers = new List<string>();

        // Añadir la respuesta correcta a las respuestas mostradas
        displayedAnswers.Add(answers[currentQuestion.correctAnswerIndex]);
        answers.RemoveAt(currentQuestion.correctAnswerIndex);

        // Mostrar las respuestas incorrectas en los botones restantes
        for (int i = 0; i < answerButtons.Length - 1; i++)
        {
            int randomAnswerIndex = Random.Range(0, answers.Count);
            displayedAnswers.Add(answers[randomAnswerIndex]);
            answers.RemoveAt(randomAnswerIndex);
        }

        // Mezclar las respuestas mostradas
        displayedAnswers = ShuffleList(displayedAnswers);

        // Asignar las respuestas a los botones y añadir listeners
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = displayedAnswers[i];

            if (displayedAnswers[i] == currentQuestion.options[currentQuestion.correctAnswerIndex])
            {
                answerButtons[i].onClick.AddListener(() => OnCorrectAnswerSelected());
            }
            else
            {
                answerButtons[i].onClick.AddListener(() => OnWrongAnswerSelected());
            }
        }

        //EnableAnswerButtons(); // Permitir interacción con los botones

        yield return null;
    }

    List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        return list;
    }



    public void OnCorrectAnswerSelected()
    {
        if (J1Responde)
        {
            J2Dañado = true;
            player2Health -= 10;
            //DAÑO AL 2
            Debug.Log("RESPONDE BIEN EL 1");
        }
        else if (J2Responde)
        {
            J1Dañado = true;
            player1Health -= 10;
            //DAÑO AL 1
            Debug.Log("RESPONDE BIEN EL 2");
        }
        Daños();
    }

    public void OnWrongAnswerSelected() //PASAR TURNOOOOOOOOOOOOOO
    {
        if (J1Responde && !venganza)
        {
            player1Pressed = false;
            player2Pressed = true;
            J1Responde = false;
            venganza = true;
            
            DetermineWinner();
            Debug.Log("RESPONDE MALL EL 1");
        }
        else if (J2Responde && !venganza)
        {
            player2Pressed = false;
            player1Pressed = true;
            J2Responde = false;
            venganza = true;

            DetermineWinner();
            Debug.Log("RESPONDE MAL EL 2");
        }
        else if (venganza)
        {
            player1Health -= 5;
            player2Health -= 5;
            //DAÑO A AMBOS
            Debug.Log("DAÑO PA LOS DOS");
            Daños();
        }

    }

    public void Daños() //AQUI VAN LAS ANIMACIONES DE LOS DAÑOS
    {
        if (player1Health > 0 && player2Health > 0)
        {
            if (J1Dañado)
            {
                Debug.Log("ANIMACION DE DAÑO A JUGADOR 1");
            }
            else if (J2Dañado)
            {
                Debug.Log("ANIMACION DE DAÑO A JUGADOR 2");
            }
            else
                Debug.Log("ANIMACION DAÑO MUTUO");

            ReiniciarJuego();
        }
        else
        {
            if (player1Health <= 0) //Gana P2
            {
                Debug.Log("GANA JUGADOR 2");
            }
            else if (player2Health <= 0) //Gana P1
            {
                Debug.Log("GANA JUGADOR 1");
            }   
        }  
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

        apuntador1.SetActive(false);
        apuntador2.SetActive(false);

        panelQuestion.SetActive(false);
        player1Pressed = false;
        player2Pressed = false;

        J1Responde = false;
        J2Responde = false;
        venganza = false;

        J1Dañado = false;
        J2Dañado = false;

        Debug.Log("Vida del jugador 1: " + player1Health);
        Debug.Log("Vida del jugador 2: " + player2Health);

        // Llamar a la función que maneja el ciclo del juego desde el principio
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
