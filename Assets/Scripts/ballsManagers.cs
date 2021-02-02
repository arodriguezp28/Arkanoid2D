using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballsManagers : MonoBehaviour
{
    #region Singleton

    private static ballsManagers _instance;
    public static ballsManagers Instance => _instance;

    private void Awake(){
        if(_instance != null){
            Destroy(gameObject);
        }
        else{
            _instance = this;
        }
    }
   
    #endregion

    [SerializeField]
    private pelota pelota_Prefab;
    private pelota pelota_inicial;
    private Rigidbody2D pelotaInicialRB;
    public float velocidadPelotaInicial = 250;

    public List<pelota> pelotas { get; set; }

    private void Start(){
        InitBall();
    }

    private void Update(){
        if(!gameManager.Instance.IsGameStarted){
            Vector3 posicionBarra = barra.Instance.gameObject.transform.position;
            Vector3 posicionPelota = new Vector3(posicionBarra.x, posicionBarra.y + .27f, 0);
            pelota_inicial.transform.position = posicionPelota;

            if(Input.GetMouseButtonDown(0)){
                pelotaInicialRB.isKinematic = false;
                pelotaInicialRB.AddForce(new Vector2(0, velocidadPelotaInicial));
                gameManager.Instance.IsGameStarted = true;
            }
        }
    }

    private void InitBall(){
        Vector3 posicionBarra = barra.Instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(posicionBarra.x, posicionBarra.y + .27f, 0);
        pelota_inicial = Instantiate(pelota_Prefab, startingPosition, Quaternion.identity);
        pelotaInicialRB = pelota_inicial.GetComponent<Rigidbody2D>();

        this.pelotas = new List<pelota>{
            pelota_inicial
        };
    }

    

}
