using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera cam;

    Rigidbody2D rb;
    Animator animator;

    Vector2 movement;
    Vector2 mousePos;
    Vector2 lookDirection;

    public Vector2 startPosition;

    public bool dead;

    [HideInInspector] public int hitPoints;
    public int maxHitPoints = 6;

    public GameObject hitEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        hitPoints = maxHitPoints;

        dead = false;
    }

    private void Start()
    {
        startPosition = this.transform.position;
    }

    void Update()
    {

        if (!dead)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            animator.SetFloat("horizontal", movement.x);
            animator.SetFloat("speed", movement.sqrMagnitude);

            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void FixedUpdate()
    {
        if (!dead)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            lookDirection = mousePos - rb.position;
            animator.SetFloat("cursor_horizontal", lookDirection.x);
        }
    }

    public Vector2 GetLookDirection()
    {
        return lookDirection;
    }

    public Vector2 GetMousePosition()
    {
        return mousePos;
    }

    public void GetHit()
    {
        DecreaseHitPoints();

        GameController.instance.HUD.UpdateHearts();

        PlayHitEffect();
    }

    void PlayHitEffect()
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            DecreaseHitPoints();
            GameController.instance.HUD.UpdateHealth();
        }
    }

    void DecreaseHitPoints()
    {
        hitPoints--;
        SoundManager.instance.PlaySingle(GameController.instance.hurtPlayerSound);
        hitPoints = Mathf.Max(0, hitPoints);
        if (hitPoints == 0)
        {
            dead = true;
            animator.SetBool("dead", dead);
        }
    }

    public void Respawn()
    {
        dead = false;
        animator.SetBool("dead", dead);
        hitPoints = maxHitPoints;

        this.transform.position = this.startPosition;
    }



}
