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
 

   void Start()
    {
        menuopciones.SetActive(false);
        menuPausa.SetActive(false);
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


    public void MuteToggle(bool muted)
         {
              if (muted)
                 {
                     AudioListener.volume = 0;
                 }
              else
                 {
                    AudioListener.volume = 1;
                 } 


         }
}
