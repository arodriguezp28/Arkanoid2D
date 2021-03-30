using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Bloque : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;

    public int HitPoints = 1;
    public ParticleSystem efectoDestruccion;

    public static event Action<Bloque> OnBrickDestruction;

    private void Awake(){
        this.sr = this.GetComponent<SpriteRenderer>();
        this.boxCollider = this.GetComponent<BoxCollider2D>();
        Pelota.OnLightningBallEnable += OnLightningBallEnable;
        Pelota.OnLightningBallDisable += OnLightningBallDisable;
    }

    private void OnLightningBallEnable(Pelota obj)
    {
        if (this != null)
        {
            this.boxCollider.isTrigger = true;
        }
    }

    private void OnLightningBallDisable(Pelota obj)
    {
        if (this != null)
        {
            this.boxCollider.isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool instantKill = false;

        if (collision.collider.tag == "Pelota")
        {
            Pelota pelota = collision.gameObject.GetComponent<Pelota>();
            instantKill = pelota.isLightningBall;
        }

        if (collision.collider.tag == "Pelota" || collision.collider.tag == "Projectile")
        {
            this.TakeDamage(instantKill);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool instantKill = false;

        if (collision.tag == "Pelota")
        {
            Pelota pelota = collision.gameObject.GetComponent<Pelota>();
            instantKill = pelota.isLightningBall;
        }

        if (collision.tag == "Pelota" || collision.tag == "Projectile")
        {
            this.TakeDamage(instantKill);
        }
    }

    private void TakeDamage(bool instantKill){
        this.HitPoints--;

        if (this.HitPoints <= 0 || instantKill)
        {
            BrickManager.Instance.RemainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);
            OnBrickDestroy();
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }else{
            sr.sprite = BrickManager.Instance.sprites[this.HitPoints - 1];
        }
    }

    private void OnBrickDestroy()
    {
        float buffSpawnChance = UnityEngine.Random.Range(0,100f);
        float debuffSpawnChance = UnityEngine.Random.Range(0, 100f);

        bool alreadySpawned = false;

        if (buffSpawnChance <= CollectablesManagers.Instance.BuffChance)
        {
            alreadySpawned = true;
            Collectable newBuff = this.SpawnCollectable(true);
        }

        if (debuffSpawnChance <= CollectablesManagers.Instance.DebuffChance && !alreadySpawned)
        {
            Collectable newDebuff = this.SpawnCollectable(false);
        }
    }

    private Collectable SpawnCollectable(bool isBuff)
    {
        List<Collectable> collection;

        if (isBuff)
        {
            collection = CollectablesManagers.Instance.AvailableBuffs;
        }
        else
        {
            collection = CollectablesManagers.Instance.AvailableDebuffs;
        }

        int buffIndex = UnityEngine.Random.Range(0, collection.Count);
        Collectable prefab = collection[buffIndex];
        Collectable newCollectable = Instantiate(prefab, this.transform.position, Quaternion.identity) as Collectable;

        return newCollectable;

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

    private void OnDisable()
    {
        Pelota.OnLightningBallEnable -= OnLightningBallEnable;
        Pelota.OnLightningBallDisable -= OnLightningBallDisable;
    }
}


