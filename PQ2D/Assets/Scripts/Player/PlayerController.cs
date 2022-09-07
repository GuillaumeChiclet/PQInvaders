using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private bool isRawInput = false;
    [SerializeField] private Transform aimCursor;

    Rigidbody2D rb;
    Camera cam;

    Vector2 inputMovement = Vector2.zero;

    private void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        inputMovement.x = isRawInput ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        inputMovement.y = isRawInput ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = (Vector2)aimCursor.position - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        rb.MovePosition(rb.position + inputMovement * moveSpeed * Time.fixedDeltaTime);
    }

    /*
    public void Knockback()
    {
        StartCoroutine(Knock());
    }

    IEnumerator Knock()
    {
        knocked = true;
        yield return new WaitForSeconds(0.1f);
        knocked = false;
    }
    */
}
