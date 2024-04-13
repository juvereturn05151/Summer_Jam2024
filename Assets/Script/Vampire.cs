using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Human")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
