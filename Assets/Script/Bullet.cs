using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f; // Time in seconds before the bullet is destroyed if it doesn't hit anything
    public LayerMask obstacleLayer;
    Vector3 direction;
    private Rigidbody2D rb;

    void Start()
    {
        SoundManager.Instance.Play("SnowBulletSFX");
        direction = (Human.Instance.transform.position - transform.position).normalized;
        // Destroy the bullet after the specified lifetime
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move towards the target position
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void DestroyBullet()
    {
        // Destroy the bullet
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collided with an object other than an obstacle
        if (other.gameObject.tag == "Human")
        {
            SoundManager.Instance.Play("SFX_VillagerHurt");
            Human.Instance.IsHurt = true;
            Human.Instance.PlayerHurtFeedback.PlayFeedbacks();
            Human.Instance.DecreaseWaterAmount(10f);
            DestroyBullet();
        }

        if (other.gameObject.tag == "Wall")
            DestroyBullet();
    }
}
