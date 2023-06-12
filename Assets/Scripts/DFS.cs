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
        int i, xx, yy;//xx�� ������ �� ���ڸ� �ľ��ϱ� ���� ��

        //������ ���
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

                //������ ����� ���, continue
                if (xx < 1 || xx > 7 || yy < 1 || yy > 7)
                    continue;

                //������ �ϴ� ���� �� �� �ִ� ���̸�, �������� �ʾҴ� ������ ���
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
