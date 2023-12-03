using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    public int playerIndex; // Indica el índice del jugador
    public string playerTag; // Indica si es "Player1" o "Player2"

    private void Start()
    {
        animator = GetComponent<Animator>();
        // Puedes usar el playerIndex o playerTag para ajustar la animación, configuración, etc.
        Debug.Log("Player Index: " + playerIndex + ", Player Tag: " + playerTag);
    }

    // Método para configurar el índice del jugador y la etiqueta del jugador
    public void SetPlayerIndexAndTag(int index, string tag)
    {
        playerIndex = index;
        Debug.Log(index);
        playerTag = tag;
        Debug.Log(tag);
    }

    public void StartDamageAnimation()
    {
        StartCoroutine(Hurt());
    }

    public void StartAttackAnimation()
    {
        StartCoroutine(Attacking());
    }

    public void StartVictoryAnimation()
    {
        //animator.SetBool("Ganador", true);
    }

    public void StartLoseAnimation()
    {
        //animator.SetBool("Perdedor", true);
    }

    IEnumerator Hurt()
    {
        yield return new WaitForSeconds(1.3f);
        animator.SetBool("Dañado", true); // Activa la animación de daño en el Animator
        yield return new WaitForSeconds(1f);
        animator.SetBool("Dañado", false);
    }

    IEnumerator Attacking()
    {

        animator.SetBool("Atacando", true);
        yield return new WaitForSeconds(1.2f);
        animator.SetBool("Atacando", false);
    }


}
