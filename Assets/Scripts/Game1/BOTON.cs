using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOTON : MonoBehaviour
{
    public GameObject canvASPausa;
    public bool activo = false;


    private void Start()
    {
        activo = false;
        canvASPausa.SetActive(false);
    }



    public void PausaBoton()
    {
        if(activo == false)
        {
            activo = true;
            canvASPausa.SetActive(true);
            Debug.Log("OLAOLALSOLAOL");
        }
        else
            
        canvASPausa.SetActive(false);



    }


}
