using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MultiBall : Collectable
{
    protected override void ApplyEffect()
    {
        foreach(Pelota pelota in ballsManagers.Instance.pelotas.ToList()){
            ballsManagers.Instance.SpawnBalls(pelota.gameObject.transform.position, 2, pelota.isLightningBall);
        }
    }
}
