using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendOrShrink : Collectable
{

    public float NewWidth = 2.5f;
    protected override void ApplyEffect()
    {
        if (barra.Instance != null && !barra.Instance.PaddleIsTransforming)
        {
            barra.Instance.StartWidthAnimation(NewWidth);
        }
    }
}
