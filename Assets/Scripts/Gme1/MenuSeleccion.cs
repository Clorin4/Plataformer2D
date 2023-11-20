using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuSeleccion : MonoBehaviour
{
    public GameObject canvasPersonajes;
    public GameObject canvasDificultad;

    public RawImage[] introVideo;

    [SerializeField] private Image[] imagenes; // Arreglo de im�genes para los personajes de los jugadores.
    [SerializeField] private TextMeshProUGUI[] nombres; // Arreglo de TextMeshProUGUI para los nombres de los personajes.

    private GameManager gameManager;
    private int[] index; // Arreglo de �ndices de selecci�n para los dos jugadores.


    private void Start()
    {
        gameManager = GameManager.Instance;

        canvasPersonajes.SetActive(true);
        canvasDificultad.SetActive(false);

        index = new int[2]; // Inicializar arreglo de �ndices para los dos jugadores.
        index[0] = PlayerPrefs.GetInt("Jugador1Index"); // Obtener �ndice del jugador 1 desde PlayerPrefs.
        index[1] = PlayerPrefs.GetInt("Jugador2Index"); // Obtener �ndice del jugador 2 desde PlayerPrefs.

        // Verificar si los �ndices est�n dentro del rango de personajes disponibles.
        for (int i = 0; i < 2; i++)
        {
            if (index[i] > gameManager.personajes.Count - 1 || index[i] < 0)
            {
                index[i] = 0; // Si el �ndice est� fuera de rango, establecer el primer personaje por defecto.
            }
            CambiarPantalla(i);
        }
    }

    private void CambiarPantalla(int jugador)
    {
        PlayerPrefs.SetInt("Jugador" + (jugador + 1) + "Index", index[jugador]); // Guardar el �ndice del jugador.

        imagenes[jugador].sprite = gameManager.personajes[index[jugador]].imagen;
        nombres[jugador].text = gameManager.personajes[index[jugador]].nombre;
    }

    public void SigPersonaje(int jugador)
    {
        index[jugador] = (index[jugador] + 1) % gameManager.personajes.Count; // Cambiar al siguiente personaje circularmente.
        CambiarPantalla(jugador);
        PlayerPrefs.SetInt("Jugador" + (jugador + 1) + "Index", index[jugador]); // Guardar el �ndice del jugador.
    }

    public void AnteriorPersonaje(int jugador)
    {
        index[jugador] = (index[jugador] - 1 + gameManager.personajes.Count) % gameManager.personajes.Count; // Cambiar al anterior personaje circularmente.
        CambiarPantalla(jugador);
        PlayerPrefs.SetInt("Jugador" + (jugador + 1) + "Index", index[jugador]); // Guardar el �ndice del jugador.
    }

    public void ChangeNextCanvas()
    {
        canvasPersonajes.SetActive(false);
        canvasDificultad.SetActive(true);
    }

    public void ChangeLastCanvas()
    {
        canvasPersonajes.SetActive(true);
        canvasDificultad.SetActive(false);
    }

    public void StartGame()
    {
        StartCoroutine(PlayIntroAndStartGame());
    }

    IEnumerator PlayIntroAndStartGame()
    {

        // Activar el RawImage y reproducir el video o GIF correspondiente a la combinaci�n de personajes.
        introVideo[0].gameObject.SetActive(true);
        // Aqu� carga y reproduce el video o GIF seg�n la combinaci�n de personajes elegida por los jugadores.

        // Esperar durante el tiempo necesario para mostrar la introducci�n.
        yield return new WaitForSeconds(3f);

        // Despu�s de la presentaci�n, desactivar el RawImage y cargar la siguiente escena.
        introVideo[0].gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
