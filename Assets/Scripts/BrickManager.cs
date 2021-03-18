using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    #region Singleton

    private static BrickManager _instance;
    public static BrickManager Instance => _instance;

    public static event Action OnLevelLoaded;

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

    private int maxRows = 17;
    private int maxCols = 12;
    private GameObject bricksContainer;
    private float initialBrickSpawnPositionX = -1.96f;
    private float initialBrickSpawnPositionY = 3.325f;
    private float shiftAmount = 0.365f;

    public Sprite[] sprites;
    public Color[] brickColors;
    public List<int[,]> levelsData { get; set; }
    public List<Bloque> RemainingBricks { get; set; }
    public int CurrentLevel;
    public Bloque brickPrefab;
    public int InitialBrickCount { get; set; }

    private void Start()
    {
        this.bricksContainer = new GameObject("BricksContainer");
        levelsData = this.LoadLevelsData();
        this.GenerateBricks();
    }

    public void LoadNextLevel()
    {
        this.CurrentLevel++;

        if (this.CurrentLevel >= this.levelsData.Count)
        {
            gameManager.Instance.ShowVictoryScreen();
        }
        else
        {
            this.LoadLevel(this.CurrentLevel);
        }
    }

    public void LoadLevel(int level)
    {
        this.CurrentLevel = level;
        this.ClearRemainingBricks();
        this.GenerateBricks();
    }

    private void ClearRemainingBricks()
    {
        foreach (Bloque bloque in this.RemainingBricks.ToList())
        {
            Destroy(bloque.gameObject);
        }
    }

    private void GenerateBricks()
    {
        this.RemainingBricks = new List<Bloque>();
        int[,] currentLevelData = this.levelsData[this.CurrentLevel];
        float currentSpawnX = initialBrickSpawnPositionX;
        float currentSpawnY = initialBrickSpawnPositionY;
        float zShift = 0;

        for(int row = 0; row < this.maxRows; row++){
            for (int col = 0; col < this.maxCols; col++){
                int brickType = currentLevelData[row, col];
                if(brickType > 0){
                    Bloque nuevoBloque = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Bloque;
                    nuevoBloque.Init(bricksContainer.transform, this.sprites[brickType - 1], this.brickColors[brickType], brickType);

                    this.RemainingBricks.Add(nuevoBloque);
                    zShift += 0.0001f;
                }
                currentSpawnX += shiftAmount;
                if(col + 1 == this.maxCols){
                    currentSpawnX = initialBrickSpawnPositionX;
                }
            }
            currentSpawnY -= shiftAmount;
        }

        this.InitialBrickCount = this.RemainingBricks.Count;
        OnLevelLoaded?.Invoke();
    }

    private List<int[,]> LoadLevelsData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;

        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols];

        int currentRows = 0;

        for(int row = 0;row < rows.Length; row++){
            string line = rows[row];

            if(line.IndexOf("--") == -1){
                string[] bricks = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                for(int col = 0; col < bricks.Length; col++){
                    currentLevel[currentRows, col] = int.Parse(bricks[col]);
                }
                currentRows++;

            }else{
                currentRows = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[maxRows, maxCols];
            }
        }
        return levelsData;
    }

}
    

