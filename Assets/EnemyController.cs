using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{

    public enum EnemyState { CHASING_PET, CHASING_PLAYER };

    public enum EnemyType { GHOST_FOOD, GHOST_HEALTH, GHOST_MIND, MONSTER};

    public int hitPoints = 8;
    public float playerDetection = 10f;
    public EnemyType type = EnemyType.MONSTER;

    private EnemyState state;

    public AIDestinationSetter aIDestinationSetter;

    Animator animator;

    public float directContactDamageRate = 1.5f;
    private float directContactDamageCountdown;

    public AIPath aIPath;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        directContactDamageCountdown = directContactDamageRate;
    }

    private void Start()
    {
        aIDestinationSetter.target = GameController.instance.pet.transform;
        state = EnemyState.CHASING_PET;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            hitPoints--;
            if (hitPoints == 0)
            {
                animator.SetTrigger("die");
            }
        } else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterController2D>().GetHit();
        } else if (collision.gameObject.tag == "Pet")
        {
            if (type.Equals(EnemyType.MONSTER))
            {
                collision.gameObject.GetComponent<PetController>().GetHit();
            } else if (type.Equals(EnemyType.GHOST_FOOD) || type.Equals(EnemyType.GHOST_HEALTH) || type.Equals(EnemyType.GHOST_MIND))
            {
                collision.gameObject.GetComponent<PossessionController>().Possess(type);
                animator.SetTrigger("possess");
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        directContactDamageCountdown -= Time.deltaTime;

        if (directContactDamageCountdown < 0) {

            directContactDamageCountdown = directContactDamageRate;

            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<CharacterController2D>().GetHit();
            } else if (collision.gameObject.tag == "Pet")
            {
                collision.gameObject.GetComponent<PetController>().GetHit();
            }
        }
    }

    public void SetAITarget(Transform targetTransform)
    {
        aIDestinationSetter.target = targetTransform;
    }

    private void Update()
    {
        if (IsPlayerWithinRadius())
        {
            if (state == EnemyState.CHASING_PET)
            {
                aIDestinationSetter.target = GameController.instance.player.transform;
                state = EnemyState.CHASING_PLAYER;
            }
        }
        else
        {
            if (state == EnemyState.CHASING_PLAYER)
            {
                aIDestinationSetter.target = GameController.instance.pet.transform;
                state = EnemyState.CHASING_PET;
            }
        }

        animator.SetFloat("velocity", aIPath.desiredVelocity.x);
    }

    bool IsPlayerWithinRadius()
    {
        if (GameController.instance.player.dead)
            return false;
        return (GameController.instance.player.transform.position - this.transform.position).sqrMagnitude < playerDetection * playerDetection;
    }

}
