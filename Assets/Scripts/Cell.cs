using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject[] walls;


    public Cell[] neighbors; // Top = 0, Right = 1, Bottom = 2, Left = 3

    public int row;
    public int column;

    // Start is called before the first frame update
    void Awake()
    {
        neighbors = new Cell[4];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

