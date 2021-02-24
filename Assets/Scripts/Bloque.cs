using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Bloque : MonoBehaviour
{
    public static event Action<Bloque> OnBrickDestruction;
    private SpriteRenderer sr;
    public int HitPoints = 1;
    public ParticleSystem efectoDestruccion;

    private void Awake(){
        this.sr = this.GetComponent<SpriteRenderer>();
    }

   private void OnCollisionEnter2D(Collision2D col){
        Pelota pelota = col.gameObject.GetComponent<Pelota>();
        ApplyCollisionLogic(pelota);
    }

    private void ApplyCollisionLogic(Pelota pelota){
        this.HitPoints--;

        if (this.HitPoints <= 0){
            OnBrickDestruction?.Invoke(this);
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }else{
            sr.sprite = BrickManager.Instance.sprites[this.HitPoints - 1];
        }
    }

    private void SpawnDestroyEffect(){
        Vector3 posBloque = gameObject.transform.position;
        Vector3 spawnPos = new Vector3(posBloque.x, posBloque.y, posBloque.z - 0.2f);
        GameObject efecto = Instantiate(efectoDestruccion.gameObject, spawnPos, Quaternion.identity);

        MainModule mm = efecto.GetComponent<ParticleSystem>().main;
        mm.startColor = sr.color;

        Destroy(efecto,efectoDestruccion.main.startLifetime.constant);
    }

    public void Init(Transform containerTransform, Sprite sprite, Color color, int hitPoints)
    {
        this.transform.SetParent(containerTransform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.HitPoints = hitPoints;
    }
}


