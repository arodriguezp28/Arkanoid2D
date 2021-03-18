using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text BloquesText;
    public Text PuntosText;
    public Text VidasText;

    public int Puntos { get; set; }

    private void Awake()
    {
        Bloque.OnBrickDestruction += OnBrickDestruction;
        BrickManager.OnLevelLoaded += OnLevelLoaded;
        gameManager.OnLiveLost += OnLiveLost;
    }

    private void Start()
    {
        OnLiveLost(gameManager.Instance.totalVidas);
    }

    private void OnLiveLost(int remainingLives)
    {
        VidasText.text = $"VIDAS: {remainingLives}";
    }

    private void OnLevelLoaded()
    {
        UpdateRemainingBricksText();
        UpdateScoreText(0);
    }

    private void OnBrickDestruction(Bloque obj)
    {
        UpdateRemainingBricksText();
        UpdateScoreText(10);
    }

    private void UpdateScoreText(int increment)
    {
        this.Puntos += increment;
        string puntosString = this.Puntos.ToString().PadLeft(5, '0');
        PuntosText.text = $"PUNTOS: {Environment.NewLine}{puntosString}";
    }

    private void UpdateRemainingBricksText()
    {
        BloquesText.text = $"BLOQUES: {Environment.NewLine}{BrickManager.Instance.RemainingBricks.Count} / {BrickManager.Instance.InitialBrickCount}";
    }

    private void OnDisable()
    {
        Bloque.OnBrickDestruction -= OnBrickDestruction;
        BrickManager.OnLevelLoaded -= OnLevelLoaded;
    }
}
