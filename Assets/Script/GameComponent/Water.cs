using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField]
    private float _increaseAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Human>() is Human human)
        {
            PlayerBase.Instance.IncreaseWaterAmount(_increaseAmount);
            Destroy(this.gameObject);
        }
    }
}
