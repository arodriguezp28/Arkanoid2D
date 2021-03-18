using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject gameOverScreen;
    public GameObject victoryScreen;

    public int vidas { get; set; }

    public int totalVidas = 3;

    public bool IsGameStarted { get; set; }

    public static event Action<int> OnLiveLost;
    
    private void Start()
    {
        this.vidas = this.totalVidas;
        Screen.SetResolution(540, 960, false);
        Pelota.OnBallDeath += OnBallDeath;
        Bloque.OnBrickDestruction += OnBrickDestruction;
    }
    
    private void OnBrickDestruction(Bloque obj)
    {
        if (BrickManager.Instance.RemainingBricks.Count <= 0)
        {
            ballsManagers.Instance.ResetBall();
            IsGameStarted = false;
            BrickManager.Instance.LoadNextLevel();
        }
    }

    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnBallDeath(Pelota obj)
    {
        if (ballsManagers.Instance.pelotas.Count <= 0){
            this.vidas--;
            if(vidas < 1){
                gameOverScreen.SetActive(true);
            }
            else
            {
                OnLiveLost?.Invoke(this.vidas);
                ballsManagers.Instance.ResetBall();
                IsGameStarted = false;
                BrickManager.Instance.LoadLevel(BrickManager.Instance.CurrentLevel);
            }
        }

    }

    private void OnDisable()
    {
        Pelota.OnBallDeath -= OnBallDeath;
    }
}
