using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : MonoBehaviour
{
    // 이동 가능한 4방향
    static readonly Vector2Int[] directions =
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    // 사천성 규칙상 허용되는 최대 꺾임 횟수
    const int MaxTurns = 2;

    int[,] grid;
    int gridSizeX;
    int gridSizeY;

    public GameManager gameManager;

    // 성공 시 path에 시작점 -> 꺾이는 지점들 -> 목표점 순서의 좌표 목록이 채워진다.
    public bool GoFind(Vector2Int start, Vector2Int target, out List<Vector2Int> path)
    {
        grid = gameManager.returnGrid();
        gridSizeY = grid.GetLength(0);
        gridSizeX = grid.GetLength(1);

        List<Vector2Int> corners = new List<Vector2Int> { start };

        foreach (Vector2Int dir in directions)
        {
            if (Search(start, dir, 0, target, corners))
            {
                corners.Add(target);
                Debug.Log("탐색성공");
                path = corners;
                return true;
            }
        }

        Debug.Log("탐색불가");
        path = null;
        return false;
    }

    // pos에서 dir 방향으로 한 칸 전진하며 target을 찾는다.
    // 막히면 꺾어서(turnCount + 1) 재탐색하고, 그마저 안 되면 백트래킹한다.
    // 꺾이는 지점은 corners에 기록해두고, 실패한 시도는 되돌린다.
    bool Search(Vector2Int pos, Vector2Int dir, int turnCount, Vector2Int target, List<Vector2Int> corners)
    {
        Vector2Int next = pos + dir;

        if (next == target)
            return true;

        if (!InBounds(next) || grid[next.y, next.x] == 1)
            return false;

        // 직진: 꺾임 횟수 유지
        if (Search(next, dir, turnCount, target, corners))
            return true;

        if (turnCount >= MaxTurns)
            return false;

        // 꺾기: 오던 방향의 역방향(U턴)은 제외하고 나머지 방향으로 백트래킹 탐색
        foreach (Vector2Int turnDir in directions)
        {
            if (turnDir == dir || turnDir == -dir)
                continue;

            corners.Add(next);
            if (Search(next, turnDir, turnCount + 1, target, corners))
                return true;
            corners.RemoveAt(corners.Count - 1);
        }

        return false;
    }

    bool InBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridSizeX && pos.y >= 0 && pos.y < gridSizeY;
    }
}
