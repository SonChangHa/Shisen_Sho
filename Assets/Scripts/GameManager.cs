using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    List<Vector3> gridPos = new List<Vector3>();

    public int boardRow;
    public int boardCol;
    public int objectCount_half;

    public GameObject tile;

    Transform boardHolder;

    Sprite[] tilePicSet;


    void InitList(int row, int col)
    {
        gridPos.Clear();

        for(float x = -col/2; x <= col/2; x++)
        {
            for(float y = -row/2; y <= row/2; y++)
            {
                gridPos.Add(new Vector3(x * 1, y * 1.46f, x));
            }
        }
    }

    Vector3 RandomPos()
    {
        int randomIndex = Random.Range(0, gridPos.Count);

        Vector3 randomPos = gridPos[randomIndex];

        gridPos.RemoveAt(randomIndex);

        return randomPos;
    }

    void MakeBoard()
    {
        for (int i = 0; i < objectCount_half; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                GameObject temp = Instantiate(tile, RandomPos(), Quaternion.identity);
                SpriteRenderer SR = temp.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                SR.sprite = tilePicSet[Random.Range(0, tilePicSet.Length)];
                temp.transform.SetParent(boardHolder);
            }
        }
    }

    private void Start()
    {
        boardHolder = new GameObject("Board").transform;
        tilePicSet = Resources.LoadAll<Sprite>("TilePic");
        InitList(boardRow, boardCol);
        MakeBoard();

    }

}
