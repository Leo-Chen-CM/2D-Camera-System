using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //Movement
    private float speed = 10;
    private float jumpHeight = 2.0f;
    private Vector2 playerVelocity;
    float horizontalMove = 0f;
    public Rigidbody2D rig;
    private bool isGrounded;

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
            //rig.velocity = new Vector2(rig.velocity.x, jumpHeight);

            Vector3 jump = new Vector3(0.0f, 200.0f, 0.0f);

            rig.AddForce(jump * jumpHeight);
            isGrounded = false;
            Debug.Log("You pressed jump");
        }
        //transform.position += playerVelocity * Time.deltaTime;

        rig.velocity += playerVelocity * Time.deltaTime;
        //rig.velocity = new Vector2 (horizontalMove * speed, rig.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
}
