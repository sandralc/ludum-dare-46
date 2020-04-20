using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyController : MonoBehaviour
{

    [HideInInspector]
    public EnemyController enemyController;

    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 5f;
    public float frequency = 2f;

    public float playerDetectionRanged = 6f;

    public enum ShootEnemyPattern { SINGLE, CIRCLE};
    public ShootEnemyPattern pattern = ShootEnemyPattern.SINGLE;

    public bool shootingEnabled = true;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();

    }

    private void Start()
    {
        InvokeRepeating("Shoot", 2.0f, frequency);
    }

    void Shoot()
    {
        if (shootingEnabled)
        {
            switch (pattern)
            {
                case ShootEnemyPattern.SINGLE:
                    ShootSingleBullet();
                    break;
                case ShootEnemyPattern.CIRCLE:
                    ShootCircleBullet();
                    break;
            }
        }

    }

    void ShootCircleBullet()
    {
        var initialAngle = Random.Range(-45, 45);

        if (IsPetClose())
            return;

        for (int i = 0; i < 8; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            float angle = i * 45 + initialAngle;

            Vector2 finalPos = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

            rb.AddForce(finalPos * bulletForce, ForceMode2D.Impulse);

            SoundManager.instance.PlaySingle(GameController.instance.circularBulletSound);
        }
    }

    void ShootSingleBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector2 shootDirection;

        if (IsPlayerWithinRangedRadius())
        {
            shootDirection = GameController.instance.player.transform.position - this.transform.position;
        }
        else
        {
            shootDirection = GameController.instance.pet.transform.position - this.transform.position;
        }

        rb.AddForce(shootDirection.normalized * bulletForce, ForceMode2D.Impulse);

        SoundManager.instance.PlaySingle(GameController.instance.shootBulletEnemySound);
    }

    bool IsPlayerWithinRangedRadius()
    {
        if (GameController.instance.player.dead)
            return false;
        if (playerDetectionRanged > 0)
            return (GameController.instance.player.transform.position - this.transform.position).sqrMagnitude < playerDetectionRanged * playerDetectionRanged;
        else
            return false;
    }

    bool IsPetClose()
    {
        return (GameController.instance.pet.transform.position - this.transform.position).sqrMagnitude < 3f * 3f;
    }

}
