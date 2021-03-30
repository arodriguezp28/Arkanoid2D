 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPaddle : Collectable
{
    protected override void ApplyEffect()
    {
        barra.Instance.StartShooting();
    }
}
