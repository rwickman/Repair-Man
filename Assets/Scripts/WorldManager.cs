using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    public GameObject hudUI;
    public GameObject cellPrefab;
    public Monster[] monsterPrefabs = new Monster[2];
    public GameObject heroPrefab;
    public PowerPill powerPillPrefab;
    public EnergyPack energyPackPrefab;
    public GameObject GameOverCanvas;
    public GameObject ScoreCanvas;
    protected MazeGenerator maze;
    protected GameObject hero;
    public List<Monster> monsters = new List<Monster>();
    public List<Cell> monsterSpawnPoints = new List<Cell>();
    public List<PowerPill> powerPillCells = new List<PowerPill>();
    public List<EnergyPack> energyPackCells = new List<EnergyPack>();

    protected bool[,] occupiedCells = null;

    public int mazeHeight = 10;
    public int mazeWidth = 15;
    public int monsterTotal;
    public int monsterInitialTotal = 4;
    public int monsterCurrentCount;
    public int monsterRespawnWaitTime = 5;
    public int waitTillAddAdditionalMonster = 10;

    public int powerPillInitialTotal = 4;
    public int powerPillCurrentCount;

    public int energyPackTotal = 4;
    public int energyPackCurrentCount=0;
    public int energyPackWaitTime = 5;
    public bool okToAddMonster = true;

    public GameObject SpawnSpeedBoost()
    {
        return null;
    }

    public MazeGenerator getMaze()
    {
        if(maze == null)
        {
            maze = MazeGenerator.CreateMaze(cellPrefab, mazeHeight, mazeWidth);
        }
        return maze;
    }

    private void CreateMaze()
    {
        getMaze();
    }

    public void SpawnHero()
    {
        int startH = mazeHeight / 2;
        int startW = mazeWidth / 2;
        var initCell = maze.mazeCells[startW, startH];
        hero = GameObject.Instantiate(heroPrefab,initCell.transform.position,Quaternion.identity);
    }

    protected IEnumerator PrepareForNewMonster(int aWaitTime)
    {
        monsterCurrentCount++;
        yield return new WaitForSeconds(aWaitTime);
        SpawnMonster();
    }

    protected IEnumerator AddAdditionalMonster(int aWaitTime)
    {
        okToAddMonster = false;
        yield return new WaitForSeconds(aWaitTime);
        monsterTotal++;
        okToAddMonster = true;
    }
    public void SpawnMonster()
    {
        var cell = GetRandomMonsterSpawnCell();
        int monsterIndex = Mathf.FloorToInt(Random.Range(0, 2000)/1000);
        var monster = (Monster)Instantiate(monsterPrefabs[monsterIndex], new Vector3(cell.transform.position.x, powerPillPrefab.transform.position.y, cell.transform.position.z), Quaternion.identity);
        monster.manager = this;
        monster.maze = maze;
        monsters.Add(monster);
    }

    public void RemoveMonster(Monster aMonster)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] == aMonster)
            {
                monsters.RemoveAt(i);
                monsterCurrentCount--;
                return;
            }
        }
    }

    public void SpawnPowerPills()
    {
        for(int i = 0;i< powerPillInitialTotal; i++)
        {
            var cell = GetRandomCell();
            var powerPill = (PowerPill)Instantiate(powerPillPrefab, new Vector3(cell.transform.position.x,powerPillPrefab.transform.position.y, cell.transform.position.z), Quaternion.identity);
            powerPill.cell = cell;
            powerPill.manager = this;
            powerPillCells.Add(powerPill);
         }
    }

    protected IEnumerator PrepareForPowerPills(int aWaitTime)
    {
        yield return new WaitForSeconds(aWaitTime);
        SpawnPowerPills();
    }

    protected IEnumerator PrepareForNewPowerPack(int aWaitTime)
    {
        energyPackCurrentCount++;
        yield return new WaitForSeconds(aWaitTime);
        SpawnEnergyPack();
    }

    public void SpawnEnergyPack()
    {
        var cell = GetRandomCell();
        var energyPack = (EnergyPack)Instantiate(energyPackPrefab, new Vector3(cell.transform.position.x, energyPackPrefab.transform.position.y, cell.transform.position.z), Quaternion.identity);
        energyPack.cell = cell;
        energyPack.manager = this;
        energyPackCells.Add(energyPack);
    }

    public void RemovePowerPill(PowerPill aPowerPill)
    {
        for (int i = 0; i < powerPillCells.Count; i++)
        {
            if(powerPillCells[i] == aPowerPill)
            {
                occupiedCells[aPowerPill.cell.row, aPowerPill.cell.column] = false;
                powerPillCells.RemoveAt(i);
                if (powerPillCells.Count <= 0)
                {
                    GameOver();
                }
                return;
            }
        }
    }

    public void RemoveEnergyPack(EnergyPack aEnergyPack)
    {
        for (int i = 0; i < energyPackCells.Count; i++)
        {
            if (energyPackCells[i] == aEnergyPack)
            {
                occupiedCells[aEnergyPack.cell.row, aEnergyPack.cell.column] = false;
                energyPackCells.RemoveAt(i);
                energyPackCurrentCount--;
                return;
            }
        }
    }

    public Cell GetRandomCell()
    {
        while (true)
        {
            var cellHeight = Random.Range(1, mazeHeight - 1);
            var cellWidth = Random.Range(1, mazeWidth - 1);
            if(!occupiedCells[cellWidth, cellHeight])
            {
                occupiedCells[cellWidth, cellHeight] = true;
                return maze.mazeCells[cellWidth, cellHeight];
            }
        }
    }

    public Cell GetRandomMonsterSpawnCell()
    {
        var cellindex = Random.Range(0, 4);
        return monsterSpawnPoints[cellindex];
    }

    public void InitPowerPills(int aWaitTime)
    {
        StartCoroutine(PrepareForPowerPills(aWaitTime));
        return;
    }


    public void CheckEnergyPacks(int aWaitTime)
    {
        if (energyPackCurrentCount == energyPackTotal) return;
        var t = energyPackTotal - energyPackCurrentCount;
        for (var i = 0; i < t; i++)
        {
            StartCoroutine(PrepareForNewPowerPack(aWaitTime));
        }
        return;
    }

    public void CheckMonsters(int aWaitTime)
    {
        if (okToAddMonster) StartCoroutine(AddAdditionalMonster(waitTillAddAdditionalMonster));
        if (monsterCurrentCount == monsterTotal) return;
        var t = monsterTotal - monsterCurrentCount;
        for(var i = 0; i < t; i++)
        {
            StartCoroutine(PrepareForNewMonster(aWaitTime));
        }
    }


    public void InitMonsterData()
    {
        monsterSpawnPoints.Add(maze.mazeCells[0, 0]);
        monsterSpawnPoints.Add(maze.mazeCells[0, mazeHeight-1]);
        monsterSpawnPoints.Add(maze.mazeCells[mazeWidth-1, 0]);
        monsterSpawnPoints.Add(maze.mazeCells[mazeWidth-1, mazeHeight-1]);
        monsterTotal = monsterInitialTotal;
}

    public Vector3 GetFreeLocation()
    {
        return Vector3.zero;
    }

    public void UpdateUI()
    {


    }

    public void GameOver()
    {
        ScoreCanvas.SetActive(false);
        GameOverCanvas.SetActive(true);
        Transform gameOverPanel = GameOverCanvas.transform.GetChild(0);
        Text scoreText = gameOverPanel.Find("Score").GetComponent<Text>();
        Text timeText= gameOverPanel.Find("Timer").GetComponent<Text>();
        hero.SetActive(false);
        
        scoreText.text = "Score: " + hero.GetComponent<Score>().curScore;
        timeText.text = "Time: " + ScoreCanvas.transform.GetChild(0).Find("Timer").GetComponent<TimerUI>().timer;
    }

    //public void SendNotification(string title, string description)
    //{
    //    hudUI.GetComponent<InGameNotifications>().AddMessage(title, description);
    //}

    // Start is called before the first frame update
    void Start()
    {
        if (!ScoreCanvas)
        {
            ScoreCanvas = GameObject.Find("ScoreCanvas");
        }
        if (!GameOverCanvas)
        {
            GameOverCanvas = GameObject.Find("GameOverCanvas");
        }  
        CreateMaze();
        occupiedCells = new bool[mazeWidth, mazeHeight];
        SpawnHero();
        InitPowerPills(0);
        CheckEnergyPacks(0);
        InitMonsterData();
        CheckMonsters(0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnergyPacks(energyPackWaitTime);
        CheckMonsters(monsterRespawnWaitTime);
        //GameOverCanvas.SetActive(true);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateUI();
    }

    void OnDestroy()
    {
        
    }
}
