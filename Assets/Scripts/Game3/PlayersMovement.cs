using UnityEngine;

public class PlayersMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;
    public float jumpForce = 5f; // Fuerza de salto
    private int index;
    public PlayerAnimatorController playerController;
    public bool touchGround = false;
    int gameChoice;

    private void Start()
    {
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

            // Aplicar el movimiento al Rigidbody del jugador
            rb.velocity = movement * speed;

            // Chequear si se está presionando la tecla de salto
            if (Input.GetKeyDown(KeyCode.W) && index == 1 && touchGround || Input.GetKeyDown(KeyCode.UpArrow) && index == 2 && touchGround)
            {
                // Aplicar una fuerza de salto al Rigidbody2D
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                touchGround = false;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            touchGround = true;
        }

    }
}
