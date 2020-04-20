using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    public enum Type { MEDICINE, FOOD, TOY, GEM};

    public Type type = Type.FOOD;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CircleCollider2D collider2D;


    private void Awake()
    {
        spriteRenderer = transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider2D = GetComponent<CircleCollider2D>();
    }

    public void Init(Type _type)
    {
        type = _type;
        spriteRenderer.sprite = GameController.instance.playerItemsManager.GetSpriteForType(_type);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            collider2D.enabled = false;

            animator.SetTrigger("pickup");
            GameController.instance.playerItemsManager.AddItem(type);

        }
    }

}
