using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    public float playerSpeed = 5f;
    public float jumpforce = 5f;
    public LayerMask groundMask;
    public bool TouchGround = false;

    private Animator animator;
    private Animator animatorrun;

    public bool canMove = true;
    [SerializeField] private Vector2 velocidadRebote;

    [SerializeField] private AudioClip saltoSonido;

    public GameObject canvasFin;
    public GameObject canvasPrimero;
    public TMP_Text textMeshScore;
    int Scoree;

    public bool mirandoDerecha = true;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        int loadScoree = PlayerPrefs.GetInt("Score");
        Scoree = loadScoree;
        //Scoree = SafeData.sharedInstance.score;
        canvasFin.SetActive(false);
        //canvasPrimero.SetActive(true);
    }

    private void Update()
    {
        Scoree = SafeData.sharedInstance.score;
        //int loadScoree = PlayerPrefs.GetInt("Score");
        //Scoree = loadScoree;
        animator = GetComponent<Animator>();
        animatorrun = GetComponent<Animator>();

        if (canMove == true)
        {
            PlayerMovement();

        }
        

        if (TouchGround == true)
        {
            Jump();
            Animacionn();
            
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            TouchGround = true;
        }
        if (collision.gameObject.tag == "Meta1")
        {
            SceneManager.LoadScene(2); //NIVEL2
        }

        if (collision.gameObject.tag == "Finish")
        {
            canvasFin.SetActive(true);
            canvasPrimero.SetActive(false);
            //int loadScoree = PlayerPrefs.GetInt("Score");
            //Scoree = loadScoree;
            Time.timeScale = 0f;

            string textVar = Scoree.ToString();
            //textMeshScore = FindAnyObjectByType<TMP_Text>();
            textMeshScore.text = "TU PUNTAJE: " + textVar;
            //COSAS DEL FIN DEL JUEGO

            Debug.Log("GAME OVER");
        }

    }



    public void Rebote(Vector2 puntoGolpe)
    {
        playerRB.velocity = new Vector2(velocidadRebote.x * puntoGolpe.x, velocidadRebote.y);
        canMove = false;
        TouchGround = false;
        
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SoundManager.Instance.EjecutarSonido(saltoSonido);
            animator.SetTrigger("Saltar");
            playerRB.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            TouchGround = false;
            
        }
    }
    //SpriteRenderer.flipX = false;
    void PlayerMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0);
        playerRB.AddForce(movement * playerSpeed);

        if(moveHorizontal > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (moveHorizontal < 0 && mirandoDerecha)
        {
            Girar();
        }

    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void Animacionn()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            animatorrun.SetTrigger("Correr");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            animatorrun.SetTrigger("Correr");
        }
    }


}
