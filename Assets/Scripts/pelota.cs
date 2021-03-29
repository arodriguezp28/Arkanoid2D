using System;
using System.Collections;
using UnityEngine;

public class Pelota : MonoBehaviour
{
    private SpriteRenderer sr;

    public bool isLightningBall;
    public float lightningBallDuration = 10;

    public ParticleSystem lightningBallEffect;

    public static event Action<Pelota> OnBallDeath;
    public static event Action<Pelota> OnLightningBallEnable;
    public static event Action<Pelota> OnLightningBallDisable;

    public void Awake()
    {
        this.sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void Die()
    {
        OnBallDeath?.Invoke(this);
        Destroy(gameObject, 1);
    }

    internal void StartLightningBall()
    {
        if (!this.isLightningBall)
        {
            this.isLightningBall = true;
            this.sr.enabled = false;
            lightningBallEffect.gameObject.SetActive(true);
            StartCoroutine(StopLightningBallAfterTime(this.lightningBallDuration));

            OnLightningBallEnable?.Invoke(this);
        }
    }

    private IEnumerator StopLightningBallAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        StopLightningBall();
    }

    private void StopLightningBall()
    {
        if (this.isLightningBall)
        {
            this.isLightningBall = false;
            this.sr.enabled = true;
            lightningBallEffect.gameObject.SetActive(false);

            OnLightningBallDisable?.Invoke(this);
        }
    }
}
