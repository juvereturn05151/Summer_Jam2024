using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float lifetime = 2f;

    [SerializeField]
    private float damage = 10f;

    private const string _sfx_snowBulletSFXString = "SnowBulletSFX";
    private Vector3 _direction;

    private void Start()
    {
        SoundManager.Instance.Play(_sfx_snowBulletSFXString);
        _direction = (Human.Instance.transform.position - transform.position).normalized;
        // Destroy the bullet after the specified lifetime
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move towards the target position
        transform.Translate(_direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collided with an object other than an obstacle
        if (other.gameObject.GetComponent<Human>() is Human human)
        {
            human.OnGettingHurt(damage);
            DestroyBullet();
        }

        if (other.gameObject.CompareTag(TagCollection.WallTag))
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        // Destroy the bullet
        Destroy(gameObject);
    }

}
