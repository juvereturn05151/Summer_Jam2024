using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public LayerMask obstacleLayer; // Layer mask for obstacles to prevent spawning bullets inside obstacles

    private float fireTimer;

    void Update()
    {
        if (GameManager.Instance.State == GameManager.GameState.EndGame)
            return;

        // Update the fire timer
        fireTimer -= Time.deltaTime;

        // Check if it's time to fire
        if (fireTimer <= 0f)
        {
            FireBullet();
            fireTimer = 1f / fireRate; // Reset the fire timer
        }
    }

    void FireBullet()
    {
        // Instantiate a new bullet at the fire point's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);

        
    }
}
