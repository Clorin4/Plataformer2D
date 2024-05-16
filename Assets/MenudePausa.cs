using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenudePausa : MonoBehaviour
{
    public GameObject botonPausa;
    public GameObject menuPausa;
    public GameObject menuopciones;

    public Slider slider;
    public float sliderValue;
 

    public Toggle toggleMute;

    void Start()
    {
        menuopciones.SetActive(false);
        menuPausa.SetActive(false);
        // Obtener el componente Toggle
        toggleMute = GetComponent<Toggle>();

        // Configurar el estado del toggle según el volumen actual
        toggleMute.isOn = AudioListener.volume == 0;

        // Configurar el volumen inicial
        PlayerPrefs.SetFloat("volumenAudio", 0.75f);
        slider.value = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        AudioListener.volume = slider.value;

        // Mostrar el icono de mute según el volumen inicial
   
    }

    public void ChangeSlider(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("volumenAudio", sliderValue);
        AudioListener.volume = slider.value;
       
    }


    public void Pausa()
    {
        Time.timeScale = 0f;
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);
        menuopciones.SetActive(false);
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
    }

    public void Opciones()
    {
        menuPausa.SetActive(false);
        menuopciones.SetActive(true);
    }

    public void RegresaralmenuPrincipal()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void MuteVolumen(bool mute)
    {
        if (mute)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = slider.value; // Restaurar el volumen al valor del slider
        }

        // Actualizar el valor del slider y la imagen de mute
        sliderValue = AudioListener.volume;
      
    }
}
