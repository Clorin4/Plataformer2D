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

    public GameObject panelFrases;
    public TextMeshProUGUI textPanel; // Referencia al TextMeshPro para mostrar la frase
    public TMP_InputField inputField;
    
    public PhraseManager phraseManager;
    PhraseData selectedPhraseData;
    float phraseTime;
    string[] phrases;

    // Start is called before the first frame update
    void Start()
    {
        TurnOffVariables();
        SaberDificultad();
        StartCoroutine(Countdown());
    }

    public void TurnOffVariables()
    {
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
        iTween.ScaleFrom(panelFrases, Vector3.zero, 1f);

        inputField.gameObject.SetActive(true);

        if (selectedPhraseData != null)
        {
            while (true)
            {
                // Muestra una frase aleatoria del conjunto de frases
                string randomPhrase = phrases[Random.Range(0, phrases.Length)];
                textPanel.text = randomPhrase;

                // Espera el tiempo específico para mostrar la frase
                yield return new WaitForSeconds(phraseTime);

                // Borra el texto después de mostrarlo
                textPanel.text = "";

                // Espera un tiempo antes de mostrar la siguiente frase
                //yield return new WaitForSeconds(0.1f);

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

                string playerTypedPhrase = inputField.text;

                // Comparar la frase escrita con la frase mostrada
                if (playerTypedPhrase == randomPhrase)
                {
                    inputField.text = "";
                    inputField.gameObject.SetActive(false);
                    // Avanzar el Grid del jugador correspondiente
                    AdvancePlayerGrid();
                    Debug.Log("BIEEEN");
                    ReiniciarJuego();
                    // Limpiar el InputField y desactivarlo
                    
                    break; // Salir del bucle al completar la acción del jugador actual
                }
                else
                {
                    inputField.text = "";
                    inputField.gameObject.SetActive(false);
                    // Pasar al siguiente jugador o manejar la lógica de error
                    Debug.Log("MAAAL");
                    ReiniciarJuego();
                    //NextPlayerTurn();
                }


            }
        }
        else
        {
            Debug.LogWarning("No se encontraron datos para la dificultad seleccionada.");
        }
    }

    public void AdvancePlayerGrid()
    {
        float velocidadMovimiento = 1.0f; // Modifica este valor según la velocidad deseada
        Vector3 destinoPos = new Vector3(-10.0f, 0.0f, 0.0f); // Modifica esto con la posición a la que quieres mover el Grid

        GameObject gridObject = GameObject.Find("GridP1");

        if (gridObject != null)
        {
            // Movimiento suavizado del Grid hacia la posición de destino
            gridObject.transform.position = Vector3.Lerp(gridObject.transform.position, destinoPos, Time.deltaTime * velocidadMovimiento);
        }
    }

    private void ReiniciarJuego()
    {
        panelFrases.SetActive(false);
        StartCoroutine(Countdown());
    }

}
