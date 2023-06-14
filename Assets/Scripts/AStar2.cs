using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar2 : MonoBehaviour
{
    // 그리드의 크기
    int gridSizeX;
    int gridSizeY;

    // 시작 노드와 목표 노드의 위치
    public Vector2Int startNodePosition;
    public Vector2Int targetNodePosition;

    // 2차원 배열 그리드
    private Node[,] grid;

    public GameManager gameManager;

    public void GoFind(Vector2Int startNodePosition, Vector2Int targetNodePosition)
    {
        // 그리드 초기화
        InitGrid();

        List<Node> path = FindPath(startNodePosition, targetNodePosition);
        // 최적 경로 출력
        if (path != null)
        {
            Debug.Log("최적 경로:");
            foreach (Node node in path)
            {
                Debug.Log(node.position);
            }
        }
        else
        {
            Debug.Log("경로를 찾을 수 없습니다.");
        }
    }

    // 그리드 초기화 메서드
    private void InitGrid()
    {
        int[,] _grid = gameManager.returnGrid();
        gridSizeY = _grid.GetLength(0);
        gridSizeX = _grid.GetLength(1);

        grid = new Node[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // 각 노드의 위치와 이웃 노드 설정
                grid[x, y] = new Node(new Vector2Int(x, y));
                if (_grid[x,y] == 1)
                    grid[x, y].isObstacle = true;
            }
        }
    }

    // A* 알고리즘 메서드
    private List<Node> FindPath(Vector2Int start, Vector2Int target)
    {
        // 시작 노드와 목표 노드 가져오기
        Node startNode = grid[start.x, start.y];
        Node targetNode = grid[target.x, target.y];

        // 오픈 리스트와 클로즈드 리스트 초기화
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        // 시작 노드를 오픈 리스트에 추가
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // 현재 노드 설정
            Node currentNode = openList[0];
            //Debug.Log(currentNode.position);
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost ||
                    openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }

            // 현재 노드를 오픈 리스트에서 제거하고 클로즈드 리스트에 추가
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // 목표 노드에 도달했는지 확인
            if (currentNode == targetNode)
            {
                // 최적 경로 반환
                Debug.Log("탐색성공");
                return RetracePath(startNode, targetNode);
            }

            // 이웃 노드 탐색
            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                // 이웃 노드가 클로즈드 리스트에 있는 경우 건너뜀
                if (closedList.Contains(neighbor))
                {
                    continue;
                }

                // 이웃 노드의 이동 비용 계산
                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                // 이웃 노드가 오픈 리스트에 없거나 더 적은 비용으로 도달할 수 있는 경우
                if (!openList.Contains(neighbor) || newMovementCostToNeighbor < neighbor.gCost)
                {
                    // 이웃 노드의 G, H, F 값을 업데이트하고 부모 노드를 현재 노드로 설정
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    // 이웃 노드가 오픈 리스트에 없다면 추가
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        // 경로를 찾을 수 없음
        Debug.Log("탐색불가");
        return null;
    }

    // 최적 경로를 역추적하는 메서드
    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        return path;
    }

    // 노드의 이웃을 찾는 메서드
    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        // 현재 노드의 상하좌우 이웃을 확인하고 그리드 내에 있는지 검사
        if (node.position.x - 1 >= 0 && !grid[node.position.x - 1, node.position.y].isObstacle)
        {
            neighbors.Add(grid[node.position.x - 1, node.position.y]);
        }
        if (node.position.x + 1 < gridSizeX && !grid[node.position.x + 1, node.position.y].isObstacle)
        {
            neighbors.Add(grid[node.position.x + 1, node.position.y]);
        }
        if (node.position.y - 1 >= 0 && !grid[node.position.x, node.position.y - 1].isObstacle)
        {
            neighbors.Add(grid[node.position.x, node.position.y - 1]);
        }
        if (node.position.y + 1 < gridSizeY && !grid[node.position.x, node.position.y + 1].isObstacle)
        {
            neighbors.Add(grid[node.position.x, node.position.y + 1]);
        }

        return neighbors;
    }

    // 두 노드 사이의 거리를 계산하는 메서드 (Manhattan 거리 사용)
    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
        int distanceY = Mathf.Abs(nodeA.position.y - nodeB.position.y);

        return distanceX + distanceY;
    }
}

// 그리드의 각 노드를 나타내는 클래스
public class Node
{
    public Vector2Int position; // 노드의 위치
    public int gCost; // 시작 노드로부터의 이동 비용
    public int hCost; // 목표 노드까지의 예상 이동 비용
    public int fCost => gCost + hCost; // 총 이동 비용
    public Node parent; // 부모 노드
    public bool isObstacle;

    public Node(Vector2Int pos)
    {
        position = pos;
        gCost = 0;
        hCost = 0;
        parent = null;
        isObstacle = false;
    }
}