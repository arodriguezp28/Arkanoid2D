using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    #region Singleton

    private static gameManager _instance;
    public static gameManager Instance => _instance;

    private void Awake(){
        if(_instance != null){
            Destroy(gameObject);
        }
        else{
            _instance = this;
        }
    }
   
    #endregion

    public bool IsGameStarted { get; set; }
}
