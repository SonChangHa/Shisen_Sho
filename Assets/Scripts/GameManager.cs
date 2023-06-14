using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    List<Vector2> gridPos = new List<Vector2>();

    public int[,] grid;

    public int boardRow; //최대 8
    public int boardCol; //최대 8
    public int objectCount_half;

    public GameObject tile;

    Transform boardHolder;

    Sprite[] tilePicSet;


    void InitList(int row, int col)
    {
        gridPos.Clear();

        for(float x = 1; x <= col + 1; x++)
        {
            for(float y = 1; y <= row + 1; y++)
            {
                gridPos.Add(new Vector2(x, y));
            }
        }
    }

    public int[,] returnGrid()
    {
        return grid;
    }

    Vector2 RandomPos()
    {
        int randomIndex = Random.Range(0, gridPos.Count);

        Vector2 randomPos = gridPos[randomIndex];

        grid[(int)randomPos.y, (int)randomPos.x] = 1;

        gridPos.RemoveAt(randomIndex);

        return randomPos;
    }

    void MakeBoard()
    {
        for (int i = 0; i < objectCount_half; i++)
        {
            int tileValue = Random.Range(0, tilePicSet.Length);
            for (int j = 0; j < 2; j++)
            {
                Vector2 vec = RandomPos();
                GameObject temp = Instantiate(tile, new Vector2(vec.x, vec.y*1.46f), Quaternion.identity);
                SpriteRenderer SR = temp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                SR.sprite = tilePicSet[tileValue];
                temp.GetComponent<Tile>().tileValue = tileValue;
                temp.GetComponent<Tile>().tileX = (int)vec.x;
                temp.GetComponent<Tile>().tileY = (int)vec.y;
                temp.transform.SetParent(boardHolder);
            }
        }
    }

    private void Awake()
    {
        grid = new int[boardRow + 3, boardCol + 3];

        boardHolder = new GameObject("Board").transform;
        tilePicSet = Resources.LoadAll<Sprite>("TilePic");
        InitList(boardRow, boardCol);
        MakeBoard();


        for (int y = 0; y < grid.GetLength(0); y++)
        {
            string temp = "";
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                temp += grid[y, x].ToString();
            }
            Debug.Log(temp);
        }

    }

}
