using UnityEngine;

public class Background : MonoBehaviour { 
    Vector3 mousePosition;
    public float moveSpeed = 0.01f;
    Rigidbody2D rb;
    Vector2 position = new Vector2(0f, 0f);


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(position);
    }
}
