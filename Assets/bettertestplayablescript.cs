using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bettertestplayablescript : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    BoxCollider2D bc;
    [SerializeField]
    public Transform groundCheck;
    public int speed;
    public LayerMask groundLayer;
    public int jumpPower;
     bool grounded;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        groundCheck = transform.GetChild(0).gameObject.transform;
    }

   
    bool Grounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, .2f, groundLayer);
    }
    // Update is called once per frame
    void Update()
    {
        //handle horizontal movement
        rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0f));
        if(Grounded() && Input.GetButton("Jump"))
        {
            rb.AddForce(Vector3.up * jumpPower * Time.deltaTime);
        }
    }
}
