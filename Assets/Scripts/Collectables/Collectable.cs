using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Barra")
        {
            this.ApplyEffect();
        }

        if (collision.tag == "Barra" || collision.tag == "DeathWall")
        {
            Destroy(this.gameObject);
        }
    }

    protected abstract void ApplyEffect();
}
