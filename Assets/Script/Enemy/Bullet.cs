using UnityEngine;

public enum ShootingDirection 
{
    Up,
    Down,
    Left,
    Right
}

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float lifetime = 2f;

    [SerializeField]
    private float damage = 10f;

    [SerializeField]
    private bool _isDestroyableByLight;

    [SerializeField]
    private bool _isFollowPlayer = true;

    [SerializeField]
    private ShootingDirection _shootingDirection;

    private const string _sfx_snowBulletSFXString = "SnowBulletSFX";
    private Vector3 _direction;

    private void Start()
    {
        SoundManager.Instance.Play(_sfx_snowBulletSFXString);
        if (_isFollowPlayer)
        {
            _direction = (Human.Instance.transform.position - transform.position).normalized;
        }
        else
        {
            switch (_shootingDirection) 
            {
                case ShootingDirection.Up: 
                    _direction = new Vector3(0, 1, 0); 
                    break ;

                case ShootingDirection.Down:
                    _direction = new Vector3(0, -1, 0);
                    break;

                case ShootingDirection.Left:
                    _direction = new Vector3(-1, 0, 0);
                    break;

                case ShootingDirection.Right:
                    _direction = new Vector3(1, 0, 0);
                    break;
            }
        }
        
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
        if (_isDestroyableByLight)
        {
            if (other.gameObject.GetComponent<Sunlight>())
            {
                DestroyBullet();
            }
        }

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
