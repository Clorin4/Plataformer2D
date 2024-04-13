using UnityEngine;

public class PlayersMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;
    public float jumpForce = 5f; // Fuerza de salto
    private int index;
    public PlayerAnimatorController playerController;
    public bool touchGround;
    int gameChoice;
    private Animator animatorrun;
    private bool jumpRequested = false;
    public bool mirandoDerecha = true;

    private void Start()
    {
        animatorrun = GetComponent<Animator>();
        gameChoice = PlayerPrefs.GetInt("gameIndex");
        rb = GetComponent<Rigidbody2D>();

        if (playerController != null)
        {
            if (playerController.playerTag == "Player1")
            {
                index = 1;
            }
            else if (playerController.playerTag == "Player2")
            {
                index = 2;
                mirandoDerecha = false;
            }

            Debug.Log(index);
        }
        else
        {
            Debug.LogWarning("No se encontró ningún objeto PlayerAnimatorController en la escena.");
        }
    }



    private void Update()
    {
        if (gameChoice == 3)
        {
            // Detectar la entrada del jugador y controlar el movimiento
            float moveHorizontal = Input.GetAxis("Horizontal" + index);
            float moveVertical = Input.GetAxis("Vertical" + index);

            
            // Calcular la dirección del movimiento
            Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;
            rb.velocity = movement * speed;

            if (moveHorizontal > 0 && !mirandoDerecha)
            {
                Girar();
            }
            else if (moveHorizontal < 0 && mirandoDerecha)
            {
                Girar();
            }

            if(Input.GetKeyDown(KeyCode.D) && index == 1 || Input.GetKeyDown(KeyCode.A) && index == 1)
            {
                playerController.StartRunningAnimation2();
            }
            else if (Input.GetKeyUp(KeyCode.D) && index == 1 || Input.GetKeyUp(KeyCode.A) && index == 1)
            {
                playerController.StopRunningAnimation();
                Debug.Log("TIESO");
            }

            else if (Input.GetKeyDown(KeyCode.RightArrow) && index == 2 || Input.GetKeyDown(KeyCode.LeftArrow) && index == 2)
            {
                playerController.StartRunningAnimation2();
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow) && index == 2 || Input.GetKeyUp(KeyCode.LeftArrow) && index == 2)
            {
                playerController.StopRunningAnimation();
                Debug.Log("TIESO");
            }

            //SALTOO
            if ((Input.GetKeyDown(KeyCode.W) && index == 1 && IsGrounded.isGrounded) || (Input.GetKeyDown(KeyCode.UpArrow) && index == 2 && IsGrounded.isGrounded))
            {
                jumpRequested = true;
            }
        }
    }

    private void FixedUpdate()
    {
        // Verificar si se cumplen las condiciones para el salto y el jugador está en el suelo
        if (jumpRequested && IsGrounded.isGrounded)
        {
            // Aplicar una fuerza de salto al Rigidbody2D
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("SALTOO");
            playerController.StartAttackAnimation();
            // Reiniciar la variable de solicitud de salto
            jumpRequested = false;
        }
    }
    private void Girar()
    {

        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }
}
