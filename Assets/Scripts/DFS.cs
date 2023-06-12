using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : MonoBehaviour
{
    int[,] grid;

    bool[] visited;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("GameManager").GetComponent<GameManager>().returnGrid();
        visited = new bool[grid.GetLength(0)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void dfs(int x, int y)
    {
        int i, xx, yy;//xx는 앞으로 갈 격자를 파악하기 위한 것

        //도착한 경우
        if (x == 7 && y == 7)
        {
            cnt++;
        }
        else
        {
            for (i = 0; i < 4; i++)
            {
                xx = x + dx[i];
                yy = y + dy[i];

                //범위를 벗어나는 경우, continue
                if (xx < 1 || xx > 7 || yy < 1 || yy > 7)
                    continue;

                //가려고 하는 곳이 갈 수 있는 곳이며, 지나가지 않았던 지점인 경우
                if (map[xx][yy] == 0 && ch[xx][yy] == 00)
                {
                    ch[xx][yy] = 1;
                    DFS(xx, yy);
                    ch[xx][yy] = 0;
                }
            }
        }
    }
}
