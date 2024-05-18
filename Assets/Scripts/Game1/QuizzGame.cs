using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class QuizzGame : MonoBehaviour
{
    #region  VARIABLEEEES

    public NewBehaviourScript questionData; // Referencia al nuevo script de dificultad

    public Transform[] spawnPoints;

    public GameObject[] P1Hearts = new GameObject[10];
    public GameObject[] P2Hearts = new GameObject[10];
    public int arrindex1 = 9;
    public int arrindex2 = 9;
    public GameObject[] P1HalfHearts = new GameObject[10];
    public GameObject[] P2HalfHearts = new GameObject[10];

    public SpriteRenderer sprite3Renderer;
    public SpriteRenderer sprite2Renderer;
    public SpriteRenderer sprite1Renderer;
    public SpriteRenderer spriteAdelanteRenderer;

    public GameObject apuntador1;
    public GameObject apuntador2;

    public GameObject Reloj;

    public GameObject panelQuestion; // El panel que contiene la pregunta y los botones
    public float panelScaleDuration = 1.0f;

    public Canvas canvasMaster;
    public Canvas canvasWinners;
    public GameObject panelP1Winner;
    public GameObject panelP2Winner;
    public GameObject panelEmpate;

    public Canvas howToPlay;

    public bool J1Responde;
    public bool J2Responde;
    public bool venganza;

    private bool J1Dañado;
    private bool J2Dañado;
    private bool dañoPaDos;
    private bool halfHeart;

    private bool player1Pressed;
    private bool player2Pressed;
    private bool countDownStarted;
    private bool secondCountDownStarted;

    public GameObject teclaD;
    public GameObject teclaK;

    //public QuestionManager questionManager;
    public Question currentQuestion;
    private List<Question> questions; // Lista de preguntas

    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    int correctButtonIndex = -1;

    public int player1Health = 100;
    public int player2Health = 100;


    #endregion
    private void Start()
    {
        howToPlay.gameObject.SetActive(true);
        TurnOffVariables();

        SaberDificultad();
    }

    public void ButtonStart()
    {
        howToPlay.gameObject.SetActive(false);
        StartCoroutine(Countdown());
    }

    public void TurnOffVariables()
    {
        Reloj.SetActive(false);
        panelQuestion.SetActive(false);
        canvasWinners.gameObject.SetActive(false);
        panelP1Winner.SetActive(false);
        panelP2Winner.SetActive(false);
        panelEmpate.SetActive(false);

        for (int i = 0; i < 10; i++)
        {
            P1Hearts[i].SetActive(true);
            P2Hearts[i].SetActive(true);
            P1HalfHearts[i].SetActive(false);
            P2HalfHearts[i].SetActive(false);
        }

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
        dañoPaDos = false;
        halfHeart = false;

        teclaD.SetActive(false);
        teclaK.SetActive(false);



    }

    public void SaberDificultad()
    {
        string selectedDifficulty = PlayerPrefs.GetString("SelectedDifficulty");
        DifficultyLeveln selectedLevel = questionData.difficultyLevels.Find(level => level.name == selectedDifficulty);

        if (selectedLevel != null)
        {
            questions = new List<Question>();
            foreach (Materia materia in selectedLevel.materias)
            {
                questions.AddRange(materia.preguntas);
            }
        }
        else
        {
            Debug.LogError("Dificultad no encontrada: " + selectedDifficulty);
        }
    }

    public void GetRandomQuestion()
    {
        if (questions != null && questions.Count > 0)
        {
            int randomIndex = Random.Range(0, questions.Count);
            currentQuestion = questions[randomIndex];
            StartCoroutine(ShowQuestionAndAnswers());
        }
        else
        {
            Debug.LogError("No hay preguntas disponibles.");
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
        yield return new WaitForSeconds(4f);

        countDownStarted = false;
        float countdownTimer = 8f;
        while (countdownTimer > 0f && !countDownStarted)
        {
            teclaD.SetActive(true);
            teclaK.SetActive(true);
            Reloj.SetActive(true);

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
        else if (player1Pressed && player2Pressed)
        {
            dañoPaDos = true;
            secondCountDownStarted = true;
            player1Health -= 5;
            player2Health -= 5;
            Daños();
            Debug.Log("Ambos?");
            Reloj.SetActive(false);
            // Acciones si ambos jugadores presionaron, se puede considerar un empate
        }
        else
        {
            Debug.Log("Ninguno");
            Reloj.SetActive(false);
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

        if(PlayerPrefs.GetString("gameStyle") == "survival")
        {
            secondCountDownStarted = false;
            float countdownTimer = 12f;

            if (countdownTimer > 0f && !secondCountDownStarted)
            {
                // Obtener una pregunta aleatoria de la lista
                int randomIndex = Random.Range(0, questions.Count);
                currentQuestion = questions[randomIndex];

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
                    answerButtons[i].onClick.RemoveAllListeners();

                    if (displayedAnswers[i] == currentQuestion.options[currentQuestion.correctAnswerIndex])
                    {
                        correctButtonIndex = i; // Almacena el índice del botón con la respuesta correcta
                        answerButtons[i].onClick.AddListener(() => OnCorrectAnswerSelected());
                    }
                    else
                    {
                        answerButtons[i].onClick.AddListener(() => OnWrongAnswerSelected());
                    }
                }

                while (countdownTimer > 0f && !secondCountDownStarted)
                {
                    countdownTimer -= Time.deltaTime;
                    yield return null;
                }

                if (countdownTimer <= 0f)
                {
                    player1Health -= 5;
                    player2Health -= 5;
                    dañoPaDos = true;
                    Reloj.SetActive(false);
                    Daños();
                }
            }

        }
        else if (PlayerPrefs.GetString("gameStyle") == "xmateria")
        {
            
        }
        
    }


    public List<string> ShuffleList(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            string temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }


    public void OnCorrectAnswerSelected()
    {
        Reloj.SetActive(false);
        secondCountDownStarted = true;
        ChangeButtonColor(true);

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
    }

    public void OnWrongAnswerSelected() //PASAR TURNOOOOOOOOOOOOOO
    {
        secondCountDownStarted = true;
        //ChangeButtonColor(false);

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
            Reloj.SetActive(false);
            dañoPaDos = true;
            Debug.Log("DAÑO PA LOS DOS");
            Daños();
        }

    }

    void ChangeButtonColor(bool correctAnswer)
    {
        Color color = correctAnswer ? Color.green : Color.red;

        // Cambia solo el color del botón que contiene la respuesta correcta
        if (correctAnswer && correctButtonIndex != -1)
        {
            Image img = answerButtons[correctButtonIndex].GetComponent<Image>();
            if (img != null)
            {
                img.color = color;
            }
        }

        // Restablecer el color del botón correcto después de un tiempo determinado
        StartCoroutine(ChangeButtonColorBack());
    }

    IEnumerator ChangeButtonColorBack()
    {
        yield return new WaitForSeconds(1f); // Cambia esto al tiempo que desees mantener los colores

        if (correctButtonIndex != -1)
        {
            Image img = answerButtons[correctButtonIndex].GetComponent<Image>();
            if (img != null)
            {
                img.color = Color.white; // Cambia esto al color original que desees para el botón correcto
            }
        }
        Daños();
    }


    public void Daños() //AQUI VAN LAS ANIMACIONES DE LOS DAÑOS
    {
        PlayerAnimatorController[] playerControllers = FindObjectsOfType<PlayerAnimatorController>();

        if (player1Health > 0 && player2Health > 0)
        {
            

            foreach (var playerController in playerControllers)
            {
                if (dañoPaDos) // Verifica si dañoPaDos es verdadero para ejecutar la animación de daño en ambos jugadores
                {
                    playerController.StartDamageAnimation();
                    Debug.Log("ANIMACION DE DAÑO A JUGADOR");
                }
                else
                {
                    if (playerController.playerTag == "Player1" && J1Dañado)
                    {
                        playerController.StartDamageAnimation();
                        Debug.Log("ANIMACION DE DAÑO A JUGADOR 1");

                        // Encuentra al jugador 2 y comienza la animación de ataque
                        PlayerAnimatorController[] controllers = FindObjectsOfType<PlayerAnimatorController>();
                        foreach (var controller in controllers)
                        {
                            if (controller.playerTag == "Player2")
                            {
                                controller.StartAttackAnimation();
                                Debug.Log("ANIMACION DE ATAQUE A JUGADOR 2");
                                break;
                            }
                        }
                    }
                    else if (playerController.playerTag == "Player2" && J2Dañado)
                    {
                        playerController.StartDamageAnimation();
                        Debug.Log("ANIMACION DE DAÑO A JUGADOR 2");

                        // Encuentra al jugador 1 y comienza la animación de ataque
                        PlayerAnimatorController[] controllers = FindObjectsOfType<PlayerAnimatorController>();
                        foreach (var controller in controllers)
                        {
                            if (controller.playerTag == "Player1")
                            {
                                controller.StartAttackAnimation();
                                Debug.Log("ANIMACION DE ATAQUE A JUGADOR 1");
                                break;
                            }
                        }
                    }
                }
            }

            HeartsHUD();
            ReiniciarJuego();
        }

        else if (player1Health <= 0 && player2Health <= 0)
        {
            apuntador2.SetActive(false);
            apuntador1.SetActive(false);
            canvasWinners.gameObject.SetActive(true);
            panelEmpate.SetActive(true);

            PlayerAnimatorController[] controllers = FindObjectsOfType<PlayerAnimatorController>();
            foreach (var controller in controllers)
            {
                if (controller.playerTag == "Player1")
                {
                    controller.StartLoseAnimation();
                }
                if (controller.playerTag == "Player2")
                {
                    controller.StartLoseAnimation();
                }
            }

        }

        else
        {
            apuntador2.SetActive(false);
            apuntador1.SetActive(false);

            int indexJugador1 = PlayerPrefs.GetInt("Jugador1Index");
            int indexJugador2 = PlayerPrefs.GetInt("Jugador2Index");

            if (indexJugador1 >= 0 && indexJugador1 < GameManager.Instance.personajes.Count &&
                indexJugador2 >= 0 && indexJugador2 < GameManager.Instance.personajes.Count &&
                spawnPoints.Length >= 2)
            {
                if (player1Health <= 0) // Gana P2
                {
                    Debug.Log("GANA JUGADOR 2");
                    canvasWinners.gameObject.SetActive(true);
                    panelP2Winner.SetActive(true);

                    PlayerAnimatorController[] controllers = FindObjectsOfType<PlayerAnimatorController>();
                    foreach (var controller in controllers)
                    {
                        if (controller.playerTag == "Player2")
                        {
                            controller.StartVictoryAnimation();
                        }
                        else if (controller.playerTag == "Player1")
                        {
                            controller.StartLoseAnimation();
                        }
                    }
                }
                else if (player2Health <= 0) // Gana P1
                {
                    Debug.Log("GANA JUGADOR 1");
                    canvasWinners.gameObject.SetActive(true);
                    panelP1Winner.SetActive(true);
                    canvasMaster.gameObject.SetActive(false);

                    PlayerAnimatorController[] controllers = FindObjectsOfType<PlayerAnimatorController>();
                    foreach (var controller in controllers)
                    {
                        if (controller.playerTag == "Player1")
                        {
                            controller.StartVictoryAnimation();
                        }
                        else if (controller.playerTag == "Player2")
                        {
                            controller.StartLoseAnimation();
                        }
                    }
                }
                
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


        apuntador1.SetActive(false);
        apuntador2.SetActive(false);
        Reloj.SetActive(false);

        panelQuestion.SetActive(false);
        player1Pressed = false;
        player2Pressed = false;

        J1Responde = false;
        J2Responde = false;
        venganza = false;

        J1Dañado = false;
        J2Dañado = false;
        dañoPaDos = false;

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

    public void HeartsHUD()
    {
        if (!halfHeart)
        {
            if (J1Dañado && !dañoPaDos)
            {
                int i = arrindex1;
                do
                {
                    J1Dañado = false;
                    P1Hearts[i].SetActive(false);
                    arrindex1--;
                }
                while (J1Dañado);
            }
            else if (J2Dañado && !dañoPaDos)
            {
                int j = arrindex2;
                do
                {
                    J2Dañado = false;
                    P2Hearts[j].SetActive(false);
                    arrindex2--;
                }
                while (J2Dañado);
            }
            else if (dañoPaDos)
            {
                int i = arrindex1;
                int j = arrindex2;
                do
                {
                    halfHeart = true;

                    P2Hearts[j].SetActive(false);
                    P2HalfHearts[j].SetActive(true);
                    arrindex2--;

                    P1Hearts[i].SetActive(false);
                    P1HalfHearts[i].SetActive(true);
                    arrindex1--;

                    dañoPaDos = false;
                }
                while (dañoPaDos);
            }
        }

        else if (halfHeart)
        {
            if (J1Dañado && !dañoPaDos)
            {
                int i = arrindex1;
                int aux = i + 1;
                do
                {
                    J1Dañado = false;
                    P1Hearts[i].SetActive(false);
                    P1HalfHearts[aux].SetActive(false);
                    P1HalfHearts[i].SetActive(true);
                    arrindex1--;
                }
                while (J1Dañado);
            }
            else if (J2Dañado && !dañoPaDos)
            {
                int j = arrindex2;
                int aux = j + 1;
                do
                {
                    J2Dañado = false;
                    P2Hearts[j].SetActive(false);
                    P2HalfHearts[aux].SetActive(false);
                    P2HalfHearts[j].SetActive(true);
                    arrindex2--;
                }
                while (J2Dañado);
            }
            else if (dañoPaDos)
            {
                int i = arrindex1;
                int aux = i + 1;
                int j = arrindex2;
                int aux2 = j + 1;
                do
                {
                    halfHeart = false;

                    P1HalfHearts[aux].SetActive(false);
                    //arrindex1--;
                    P2HalfHearts[aux2].SetActive(false);
                    //arrindex2--;
                    dañoPaDos = false;
                }
                while (dañoPaDos);
            }
        }

    }


    public void Salir()
    {
        SceneManager.LoadScene(1);
    }

}
