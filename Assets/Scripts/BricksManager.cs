using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
    #region Singleton

    private static BricksManager _instance;

    public static BricksManager Instance => _instance;

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

    public Color[] BrickColors;
    public Sprite[] Sprites;
    public int CurrentLevel;
    public Brick brickPrefab;
    public List<Brick> RemainingBricks { get; set; }
    public List<int[,]> LevelsData { get; set; }
    public int initialBricksCount { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        this.bricksContainer = new GameObject("BricksContainer");
        this.LevelsData = this.LoadLevelsData();
        this.GenerateBricks();
    }

    public void LoadLevel(int level)
    {
        this.CurrentLevel = level;
        this.ClearRemainingBricks();
        this.GenerateBricks();
    }

    // Load Next Level when all the bricks are destroyed
    public void LoadNextLevel()
    {
        // Increment current Level
        this.CurrentLevel++;

        // Check if all the levels are crossed
        if (this.CurrentLevel >= this.LevelsData.Count)
        {
            // Show Victoryscreen
            GameManager.Instance.ShowVictoryScreen();
        }
        else
        {
            this.LoadLevel(this.CurrentLevel);
        }
    }

    private void ClearRemainingBricks()
    {
        foreach (var brick in this.RemainingBricks.ToArray()) // Error ToList()
        {
            Destroy(brick.gameObject);
        }
    }

    // Load Levels data from text file
    private List<int[,]> LoadLevelsData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;

        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols];
        
        int currentRow = 0;
        for (int row = 0; row < rows.Length; row++) 
        {
            string line = rows[row];
            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for(int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }
                currentRow++;
            }
            else
            {
                // End of Current level
                // Add the matrix to the last and continue the loop
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[maxRows, maxCols];
            }
        }

        return levelsData;
    }

    //Genrate Bricks from Levels Data
    private void GenerateBricks()
    {
        // Intializing Remaining bricks collection before genrating bricks
        this.RemainingBricks = new List<Brick>();
        int[,] currentLevelData = this.LevelsData[this.CurrentLevel];
        // need to get initial position to spawn bricks (for first brick only)
        float currentSpawnX = initialBrickSpawnPositionX;
        float currentSpawnY = initialBrickSpawnPositionY;

        float zShift = 0; // variable to put bricks near to camera to avoid overlapping

        for (int row = 0; row < this.maxRows; row++)
        {
            for (int col = 0; col < this.maxCols; col++)
            {
                // 
                int brickType = currentLevelData[row, col];
                if (brickType > 0)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(bricksContainer.transform, this.Sprites[brickType - 1], this.BrickColors[brickType], brickType);

                    this.RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }
                
                //shifting after spwaing every new brick
                currentSpawnX += shiftAmount;

                if (col + 1 == this.maxCols) 
                {
                    currentSpawnX = initialBrickSpawnPositionX;
                }
            }

            currentSpawnY -= shiftAmount;
        }

        this.initialBricksCount = this.RemainingBricks.Count;
        // invoking event after bricks have generated
        OnLevelLoaded?.Invoke();
    }
}
