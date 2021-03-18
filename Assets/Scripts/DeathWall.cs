using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pelota")
        {
            Pelota pelota = collision.GetComponent<Pelota>();

            ballsManagers.Instance.pelotas.Remove(pelota);
            pelota.Die();
        }
    }
}
