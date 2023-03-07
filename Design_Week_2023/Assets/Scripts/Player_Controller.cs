    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Player_Controller : MonoBehaviour
{
 [SerializeField] private float speed = 5f;
    
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;
        rb.velocity = movement * speed;
    }
}