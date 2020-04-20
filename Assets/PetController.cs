using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour
{

    public float food =6f;
    public float health = 6f;
    public float happy = 6f;

    public float maxFood = 6f;
    public float maxHealth = 6f;
    public float maxHappy = 6f;

    public float foodRate = -.1f;
    public float healthRate = 0f;
    public float happyRate = -.1f;

    private float updatePanelsCountdownInit = .5f;
    private float updatePanelsCountdown;

    public float playerDetection = 1.5f;


    public GameObject hitEffect;
    public GameObject celebrationEffect;
    public GameObject itemAnimationEffect;

    private Animator animator;

    public Animator floorPetArea;


    private void Awake()
    {
        updatePanelsCountdown = updatePanelsCountdownInit;
        animator = GetComponent<Animator>();

        floorPetArea = GameObject.Find("Floor_Pet_Area").GetComponent<Animator>();
    }

    private void Update()
    {

        UpdateStats();

        updatePanelsCountdown -= Time.deltaTime; 

        if (updatePanelsCountdown <= 0f)
        {
            GameController.instance.HUD.UpdatePanels();
            updatePanelsCountdown = updatePanelsCountdownInit;
        }

        if (IsPlayerCloseToPet())
            floorPetArea.SetBool("playerClose", true);
        else
            floorPetArea.SetBool("playerClose", false);

    }

    void UpdateStats()
    {

        food += foodRate * Time.deltaTime;
        health += healthRate * Time.deltaTime;
        happy += happyRate * Time.deltaTime;

        if (food <0)
        {
            health += foodRate * Time.deltaTime;
        }

        if (happy < 0)
        {
            health += happyRate * Time.deltaTime;
        }

        food = Mathf.Max(0f, Mathf.Min(maxFood, food));
        health = Mathf.Max(0f,Mathf.Min(maxHealth, health));
        happy = Mathf.Max(0f,Mathf.Min(maxHappy, happy));

        animator.SetFloat("food", food/6f);
        animator.SetFloat("health", health/ 6f);
        animator.SetFloat("happy", happy/ 6f);

    }

    public void GetHit()
    {
        health-= 1f;
        health = Mathf.Max(0, health);

        GameController.instance.HUD.UpdateHealth();

        SoundManager.instance.PlaySingle(GameController.instance.hurtSound);

        PlayHitEffect();
    }

    void PlayHitEffect()
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
    }

    public void ItemGifted(ItemController.Type typeOfItem)
    {
        Vector2 effectPosition = new Vector2(transform.position.x, transform.position.y + 0.5f);
        GameObject effect = Instantiate(celebrationEffect, effectPosition, Quaternion.identity);

        GameObject itemEffect = Instantiate(itemAnimationEffect, transform.position, Quaternion.identity);

        itemEffect.transform.Find("Item Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = GameController.instance.playerItemsManager.GetSpriteForType(typeOfItem);

        switch(typeOfItem)
        {
            case ItemController.Type.FOOD:
                food += 2f;
                food = Mathf.Max(0f, Mathf.Min(6f, food));
                break;
            case ItemController.Type.MEDICINE:
                health += 2f;
                health = Mathf.Max(0f, Mathf.Min(6f, health));
                break;
            case ItemController.Type.TOY:
                happy += 1.5f;
                happy = Mathf.Max(0f, Mathf.Min(6f, happy));
                break;
        }

        GameController.instance.HUD.UpdatePanels();
    }

    public void Possessed(EnemyController.EnemyType type)
    {
        switch (type)
        {
            case EnemyController.EnemyType.GHOST_FOOD:
                maxFood = 3f;
                break;
            case EnemyController.EnemyType.GHOST_HEALTH:
                maxHealth = 3f;
                break;
            case EnemyController.EnemyType.GHOST_MIND:
                maxHappy = 3f;
                break;
        }

        GameController.instance.HUD.UpdatePanels();
        animator.SetBool("possessed", true);
    }

    public void DePossessed (EnemyController.EnemyType type)
    {
        switch (type)
        {
            case EnemyController.EnemyType.GHOST_FOOD:
                maxFood = 6f;
                break;
            case EnemyController.EnemyType.GHOST_HEALTH:
                maxHealth = 6f;
                break;
            case EnemyController.EnemyType.GHOST_MIND:
                maxHappy = 6f;
                break;
        }

        GameController.instance.HUD.UpdatePanels();
        animator.SetBool("possessed", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            health--;
            health = Mathf.Max(0f, Mathf.Min(6f, health));
            animator.SetFloat("health", health / 6f);
            GameController.instance.HUD.UpdateHealth();

            SoundManager.instance.PlaySingle(GameController.instance.hurtSound);
        }
    }

    public void Restart()
    {
        food = maxFood;
        health = maxHappy;
        happy = maxHappy;

        animator.SetFloat("health", health / 6f);
    }

    public bool IsPlayerCloseToPet()
    {
        return (this.transform.position - GameController.instance.player.transform.position).sqrMagnitude < playerDetection * playerDetection;
    }

}
