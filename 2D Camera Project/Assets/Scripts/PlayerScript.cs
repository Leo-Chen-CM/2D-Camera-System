using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerScript : MonoBehaviour
{
    //Movement
    private float speed = 10;
    public float jumpHeight = 2.0f;
    private Vector2 playerVelocity;
    float horizontalMove = 0f;
    public Rigidbody2D rig;
    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        playerVelocity = new Vector2(horizontalMove, 0);


        if (Input.GetKey(KeyCode.W) && isGrounded)
        {
            Vector3 jump = new Vector3(0.0f, 200.0f, 0.0f);

            rig.AddForce(jump * jumpHeight);
            isGrounded = false;
            Debug.Log("You pressed jump");
        }

        rig.velocity += playerVelocity * Time.deltaTime;


        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            Debug.Log("Player has totally died");
            CameraSystem2D.Instance.FadeInOut();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
}
