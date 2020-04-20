using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{

    public CharacterController2D characterController;

    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 8f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        if (!GameController.instance.player.dead)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.AddForce(characterController.GetLookDirection().normalized * bulletForce, ForceMode2D.Impulse);

        SoundManager.instance.PlaySingle(GameController.instance.shootBulletSound);
    }
}
