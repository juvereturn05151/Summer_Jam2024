using Unity.Mathematics;
using UnityEngine;

public class Pond : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float pondTimeEvaporate = 5f;

    [SerializeField] 
    private float pondFillOnDay;
    public float PondFillOnDay => pondFillOnDay;

    [SerializeField]
    private GameObject melt;

    [SerializeField] 
    private GameObject waterSplash;

    private const string _sfx_enemyHurt = "SFX_EnemyHurt";

    private void Update()
    {
        if (pondTimeEvaporate <= 0) 
        {
            var meltFX = Instantiate(melt, transform.position, quaternion.identity);
            SoundManager.Instance.PlayOneShot(_sfx_enemyHurt);
            Destroy(meltFX, 1f);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Human>() is Human human)
        {
            human.WaterFillImage.FillWaterSlider();
            human.IncreaseWaterAmount(PondFillOnDay);

            if (pondFillOnDay >= 20)
            {
                if (GameManager.Instance.IsTutorial && AdvancedTutorialManager.Instance.CurrentTutorial.Type == TutorialType.CreateABigPond) 
                {
                    GameManager.Instance.BigPondDrank++;
                }
            }

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

    public void Evaporating() 
    {
        pondTimeEvaporate -= Time.deltaTime;
    }
}
