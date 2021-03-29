using System;
using UnityEngine;

public class LightningBall : Collectable
{

    protected override void ApplyEffect()
    {
        foreach (var ball in ballsManagers.Instance.pelotas)
        {
            ball.StartLightningBall();
        }
    }

}
