using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barra : MonoBehaviour
{
    #region Singleton

    private static barra _instance;
    public static barra Instance => _instance;

    public bool PaddleIsTransforming { get; set; }

    private void Awake(){
        if(_instance != null){
            Destroy(gameObject);
        }
        else{
            _instance = this;
        }
    }

    #endregion

    private Camera mainCamera;
    private float barraInicialY;
    private float defaultBarraWidthInPixels = 200;
    private float defaultLeftClamp = 123;
    private float defaultRightClamp = 421;
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    public float extendShrinkDuration = 10;
    public float paddleWidth = 2;
    public float paddleHeight = 0.28f;

    private void Start(){
        mainCamera = FindObjectOfType<Camera>();
        barraInicialY = this.transform.position.y;
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        MovimientoBarra();
    }

    public void StartWidthAnimation(float newWidth)
    {
        StartCoroutine(AnimatePaddleWidth(newWidth));
    }

    public IEnumerator AnimatePaddleWidth(float width)
    {
        this.PaddleIsTransforming = true;
        StartCoroutine(ResetPaddleWidthAfterTime(this.extendShrinkDuration));

        if (width > this.sr.size.x)
        {
            float currentWidth = this.sr.size.x;
            while (currentWidth < width)
            {
                currentWidth += Time.deltaTime * 2;
                this.sr.size = new Vector2(currentWidth, paddleHeight);
                boxCol.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        else
        {
            float currentWidth = this.sr.size.x;
            while (currentWidth > width)
            {
                currentWidth -= Time.deltaTime * 2;
                this.sr.size = new Vector2(currentWidth, paddleHeight);
                boxCol.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        this.PaddleIsTransforming = false;

    }

    private IEnumerator ResetPaddleWidthAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.StartWidthAnimation(this.paddleWidth);
    }

    private void MovimientoBarra(){
        float barraShift = (defaultBarraWidthInPixels - ((defaultBarraWidthInPixels / 2) * this.sr.size.x)) / 2;
        float leftClamp = defaultLeftClamp - barraShift;
        float rightClamp = defaultRightClamp + barraShift;

        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        float mousePositionWorldX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0, 0)).x;       
        this.transform.position = new Vector3(mousePositionWorldX, barraInicialY, 0);
    }

    private void OnCollisionEnter2D(Collision2D coll){
        if(coll.gameObject.tag == "Pelota"){
            Rigidbody2D pelotaRB = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = coll.contacts[0].point;
            Vector3 centroBarra = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            pelotaRB.velocity = Vector2.zero;

            float difference = centroBarra.x - hitPoint.x;

            if(hitPoint.x < centroBarra.x){
                pelotaRB.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), ballsManagers.Instance.velocidadPelotaInicial));
            }else{
                pelotaRB.AddForce(new Vector2((Mathf.Abs(difference * 200)), ballsManagers.Instance.velocidadPelotaInicial));
            }
        }
    }
}
