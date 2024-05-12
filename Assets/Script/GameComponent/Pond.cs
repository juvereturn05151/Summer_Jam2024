using Unity.Mathematics;
using UnityEngine;

public class Pond : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    #region -Sunlight-

    [Header("Sunlight Variables")]
    [SerializeField] private float iceMeltTimeBySun = 3f;
    
    public float IceMeltTimeBySun
    {
        get => iceMeltTimeBySun;
        set => iceMeltTimeBySun = value;
    }

    [SerializeField] private float pondTimeEvaporate = 5f;

    public float PondTimeEvaporate
    {
        get => pondTimeEvaporate;
        set => pondTimeEvaporate = value;
    }

    #endregion

    [Space]

    [SerializeField] private float pondFillOnDay;
    public float PondFillOnDay => pondFillOnDay;

    [SerializeField] private GameObject melt;

    [SerializeField] private GameObject waterSplash;

    // Update is called once per frame
    private void Update()
    {
        if (pondTimeEvaporate <= 0) 
        {
            var meltFX = Instantiate(melt, transform.position, quaternion.identity);
            SoundManager.Instance.PlayOneShot("SFX_EnemyHurt");
            Destroy(meltFX, 1f);
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Human>() is Human human)
        {
            print("human drink water");
            human.WaterFillImage.FillWaterSlider();
            human.IncreaseWaterAmount(PondFillOnDay);
            Destroy(gameObject);

            var waterFX = Instantiate(waterSplash, transform.position, quaternion.identity);
            Destroy(waterFX, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Human>() is Human human)
        {
            human.IncreaseWaterAmount(PondFillOnDay);
            Destroy(gameObject);
        }
    }
}
