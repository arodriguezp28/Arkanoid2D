using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesManagers : MonoBehaviour
{
    #region Singleton
    private static CollectablesManagers _instance;

    public static CollectablesManagers Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public List<Collectable> AvailableBuffs;
    public List<Collectable> AvailableDebuffs;

    [Range(0,100)]
    public float BuffChance;
    [Range(0, 100)]
    public float DebuffChance;
}
