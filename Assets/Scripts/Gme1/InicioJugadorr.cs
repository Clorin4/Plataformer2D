using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InicioJugadorr : MonoBehaviour
{
    public Transform[] spawnPoints; // Un arreglo para almacenar los puntos de spawn

    private void Start()
    {
        int indexJugador1 = PlayerPrefs.GetInt("Jugador1Index");
        int indexJugador2 = PlayerPrefs.GetInt("Jugador2Index");

        if (indexJugador1 >= 0 && indexJugador1 < GameManager.Instance.personajes.Count &&
            indexJugador2 >= 0 && indexJugador2 < GameManager.Instance.personajes.Count &&
            spawnPoints.Length >= 2)
        {
            GameObject jugador1 = Instantiate(GameManager.Instance.personajes[indexJugador1].personajeJugable, spawnPoints[0].position, Quaternion.identity);
            GameObject jugador2 = Instantiate(GameManager.Instance.personajes[indexJugador2].personajeJugable, spawnPoints[1].position, Quaternion.identity);

            // Cambiar la orientación del personaje del jugador 2 hacia la izquierda
            if (jugador2 != null)
            {
                // Obtener el componente de escala (scale) del Transform
                Vector3 scale = jugador2.transform.localScale;
                // Voltear el personaje hacia la izquierda
                scale.x = -Mathf.Abs(scale.x);
                // Aplicar la nueva escala al Transform
                jugador2.transform.localScale = scale;
            }
        }
    }

}
