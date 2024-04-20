using UnityEngine;

public class PlayersMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;
    public float jumpForce = 5f; // Fuerza de salto
    public int index;
    public PlayerAnimatorController playerController;
    int gameChoice;
    private bool jumpRequested = false;
    public bool mirandoDerecha = true;

    private void Start()
    {
        gameChoice = PlayerPrefs.GetInt("gameIndex");
        rb = GetComponent<Rigidbody2D>();

        if (playerController != null)
        {
            if (playerController.playerTag == "Player1")
            {
                index = 1;
                gameObject.tag = "Player1";
            }
            else if (playerController.playerTag == "Player2")
            {
                index = 2;
                gameObject.tag = "Player2";
                mirandoDerecha = false;
            }
        }
        else
        {
            Debug.LogWarning("No se encontró ningún objeto PlayerAnimatorController en la escena.");
        }

        rb.gravityScale = 2f; // Ajusta este valor según lo que funcione mejor para tu juego
        // Aumenta el Drag para suavizar el movimiento en el aire
        rb.drag = 1f;
    }

    private void Update()
    {
        if (gameChoice == 3)
        {
            Animaciones();

            // Detectar la entrada del jugador y controlar el movimiento
            float moveHorizontal = Input.GetAxis("Horizontal" + index);
            float moveVertical = Input.GetAxis("Vertical" + index);

            // Calcular la dirección del movimiento
            Vector2 movement = new Vector2(moveHorizontal, 0).normalized;
            rb.velocity = new Vector2(movement.x * speed, rb.velocity.y); // Mantener la velocidad vertical actual

            if (moveHorizontal > 0 && !mirandoDerecha)
            {
                Girar();
            }
            else if (moveHorizontal < 0 && mirandoDerecha)
            {
                Girar();
            }

            // Verificar si se presiona la tecla de salto correspondiente al jugador y si está en el suelo
            if ((Input.GetKeyDown(KeyCode.W) && index == 1 || Input.GetKeyDown(KeyCode.UpArrow) && index == 2) && IsGrounded.isGrounded)
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
            playerController.StartJumpingAnimation();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Salto suave en la velocidad vertical
            jumpRequested = false;
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }


    private void Animaciones()
    {
        if (Input.GetKey(KeyCode.D) && index == 1 || Input.GetKey(KeyCode.A) && index == 1)
        {
            playerController.StartRunningAnimation2();
        }
        else if (Input.GetKeyUp(KeyCode.D) && index == 1 || Input.GetKeyUp(KeyCode.A) && index == 1)
        {
            playerController.StopRunningAnimation();
        }
        else if (Input.GetKey(KeyCode.RightArrow) && index == 2 || Input.GetKey(KeyCode.LeftArrow) && index == 2)
        {
            playerController.StartRunningAnimation2();
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) && index == 2 || Input.GetKeyUp(KeyCode.LeftArrow) && index == 2)
        {
            playerController.StopRunningAnimation();
        }
    }

}
