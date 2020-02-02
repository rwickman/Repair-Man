using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeGenerator : MonoBehaviour
{
    private static MazeGenerator instance = null;
    public GameObject cellPrefab;

    public int mazeHeight;
    public int mazeWidth;
    public float cellSepDist = 5;

    [HideInInspector]
    public Cell[,] mazeCells;

    private bool removedExcess = true;
    public bool buildNavMesh = true;

    protected static MazeGenerator GetInstance()
    {
        if (instance == null)
        {
            instance = new GameObject("MazeGenerator").AddComponent<MazeGenerator>();
        }
        return instance;
    }

    public void OnDestroy()
    {
        instance = null;
    }

    public static MazeGenerator CreateMaze(GameObject cellPrefab, int mazeHeight, int mazeWidth)
    {
        var mazeGenerator = GetInstance();
        mazeGenerator.cellPrefab = cellPrefab;
        mazeGenerator.MazeInit(mazeHeight, mazeWidth);
        return mazeGenerator;
    }

    public void MazeInit(int mazeHeight, int mazeWidth)
    {
        this.mazeHeight = mazeHeight;
        this.mazeWidth = mazeWidth;
        this.mazeCells = new Cell[mazeWidth, mazeHeight];
        this.CreateCellGrid();
        this.CreateMaze();
        this.removedExcess = true;
        this.buildNavMesh = true;
    }


    // Start is called before the first frame update
    //void Awake()
    //{
    //    mazeCells = new Cell[mazeHeight, mazeWidth];
    //    CreateCellGrid();
    //    CreateMaze();
    //}

    // Update is called once per frame
    void LateUpdate()
    {
        if (removedExcess)
        {
            RemoveExcessWalls();

            removedExcess = false;
        }
        else if (buildNavMesh)
        {
            BuildNavMesh();
            buildNavMesh = false;
        }
    }
    

    void CreateCellGrid()
    {
        //maze = new GameObject();
        //maze.name = "Maze";

        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                mazeCells[i, j] = Instantiate(cellPrefab, new Vector3(cellSepDist * i, 0, cellSepDist * j), Quaternion.identity, GetInstance().transform).GetComponent<Cell>();
                mazeCells[i, j].name = "Cell (" + i + ", " + j + ")";
                mazeCells[i, j].row = i;
                mazeCells[i, j].column = j;
                // Set the neighbors
                if (i > 0)
                {
                    // Set others bottom neighbor
                    mazeCells[i - 1, j].neighbors[2] = mazeCells[i, j];

                    // Set this top neighbor
                    mazeCells[i, j].neighbors[0] = mazeCells[i - 1, j];
                }
                if (j > 0)
                {
                    // Set others right neighbor
                    mazeCells[i, j - 1].neighbors[1] = mazeCells[i, j];

                    // Set this left neighbor
                    mazeCells[i, j].neighbors[3] = mazeCells[i, j - 1];
                }
            }
        }
    }

    void CreateMaze()
    {
        bool[,] visited = new bool[mazeWidth, mazeHeight];

        // Select a start position
        int startH = mazeHeight / 2;
        int startW = mazeWidth / 2;
        visited[startW, startH] = true;
        Stack<Cell> cellStack = new Stack<Cell>();
        cellStack.Push(mazeCells[startW, startH]);
        while(cellStack.Count > 0)
        {
            Cell curCell = cellStack.Pop();
            List<(int, Cell)> unvisitedNeighbors;
            do {
                unvisitedNeighbors = new List<(int, Cell)>();

                // Add all unvisisted neighbors to a list
                for (int i = 0; i < curCell.neighbors.Length; i++)
                {
                    if (curCell.neighbors[i] != null && !visited[curCell.neighbors[i].row, curCell.neighbors[i].column])
                    {
                        unvisitedNeighbors.Add((i, curCell.neighbors[i]));
                        cellStack.Push(curCell.neighbors[i]);
                    }
                }
                // Vist the neighbor
                if (unvisitedNeighbors.Count > 0)
                {
                    var toVisit = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                    RemoveWall(toVisit.Item1, curCell);
                    visited[toVisit.Item2.row, toVisit.Item2.column] = true;
                }
            } while (unvisitedNeighbors.Count > 1);
            
        }
    }

    void RemoveWall(int neighborIndex, Cell curCell)
    {
        Destroy(curCell.walls[neighborIndex]);

        if (neighborIndex == 0)
        {
            Destroy(curCell.neighbors[neighborIndex].walls[2]);
        }
        else if (neighborIndex == 1)
        {
            Destroy(curCell.neighbors[neighborIndex].walls[3]);
        }
        else if (neighborIndex == 2)
        {
            Destroy(curCell.neighbors[neighborIndex].walls[0]);
        }
        else if (neighborIndex == 3)
        {
            Destroy(curCell.neighbors[neighborIndex].walls[1]);
        }
    }

    void RemoveExcessWalls()
    {
        /*
        Top = 0
        Right = 1
        Bottom = 2
        Left = 3
        */
        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                for (int k = 0; k < mazeCells[i, j].walls.Length; k += 3)
                {
                    if (mazeCells[i, j].walls[k] != null)
                    {
                        // if top wall is present
                        if (k == 0 && i > 0)
                        {
                            // Check right wall not present
                            if (mazeCells[i, j].walls[1] == null && mazeCells[i, j].neighbors[k].walls[1] == null)
                            {
                                Destroy(mazeCells[i, j].walls[k]);
                                Destroy(mazeCells[i, j].neighbors[k].walls[2]);
                            }
                            else if (mazeCells[i, j].walls[3] == null && mazeCells[i, j].neighbors[k].walls[3] == null)
                            {;
                                Destroy(mazeCells[i, j].walls[k]);
                                Destroy(mazeCells[i, j].neighbors[k].walls[2]);
                            }
                        }
                        // if left wall is present
                        else if (k == 3 && j > 0)
                        {
                            if (mazeCells[i, j].walls[0] == null && mazeCells[i, j].neighbors[k].walls[0] == null)
                            {
                                // Remove left wall
                                Destroy(mazeCells[i, j].walls[k]);
                                // Remove its right wall
                                Destroy(mazeCells[i, j].neighbors[k].walls[1]);
                            }
                            else if (mazeCells[i, j].walls[2] == null && mazeCells[i, j].neighbors[k].walls[2] == null)
                            {
                                // Remove left wall
                                Destroy(mazeCells[i, j].walls[k]);
                                // Remove its right wall
                                Destroy(mazeCells[i, j].neighbors[k].walls[1]);
                            }
                        }
                    }
                }
            }
        }
    }

    void BuildNavMesh()
    {
        foreach (Cell cell in mazeCells)
        {
            cell.gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }
}
