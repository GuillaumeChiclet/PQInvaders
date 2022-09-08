using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerController : MonoBehaviour
{
    public EventReference CharaWalk;
    private FMOD.Studio.EventInstance instance;

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private bool isRawInput = false;
    [SerializeField] private Animator animator;

    Rigidbody2D rb;
    Camera cam;

    Vector2 inputMovement = Vector2.zero;

    private void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();

        instance = FMODUnity.RuntimeManager.CreateInstance(CharaWalk);
        instance.start();
        instance.release();
    }

    private void Update()
    {
        inputMovement.x = isRawInput ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        inputMovement.y = isRawInput ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");

        instance.setParameterByName( "IsRunning",inputMovement.magnitude > 0.1f ? 1.0f : 0.0f); 
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + inputMovement * moveSpeed * Time.fixedDeltaTime);

        animator.SetBool("running", inputMovement.magnitude > 0.1f);
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
