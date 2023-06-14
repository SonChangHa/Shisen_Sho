using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar2 : MonoBehaviour
{
    // �׸����� ũ��
    int gridSizeX;
    int gridSizeY;

    // ���� ���� ��ǥ ����� ��ġ
    public Vector2Int startNodePosition;
    public Vector2Int targetNodePosition;

    // 2���� �迭 �׸���
    private Node[,] grid;

    public GameManager gameManager;

    public void GoFind(Vector2Int startNodePosition, Vector2Int targetNodePosition)
    {
        // �׸��� �ʱ�ȭ
        InitGrid();

        List<Node> path = FindPath(startNodePosition, targetNodePosition);
        // ���� ��� ���
        if (path != null)
        {
            Debug.Log("���� ���:");
            foreach (Node node in path)
            {
                Debug.Log(node.position);
            }
        }
        else
        {
            Debug.Log("��θ� ã�� �� �����ϴ�.");
        }
    }

    // �׸��� �ʱ�ȭ �޼���
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
                // �� ����� ��ġ�� �̿� ��� ����
                grid[x, y] = new Node(new Vector2Int(x, y));
                if (_grid[x,y] == 1)
                    grid[x, y].isObstacle = true;
            }
        }
    }

    // A* �˰��� �޼���
    private List<Node> FindPath(Vector2Int start, Vector2Int target)
    {
        // ���� ���� ��ǥ ��� ��������
        Node startNode = grid[start.x, start.y];
        Node targetNode = grid[target.x, target.y];

        // ���� ����Ʈ�� Ŭ����� ����Ʈ �ʱ�ȭ
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        // ���� ��带 ���� ����Ʈ�� �߰�
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // ���� ��� ����
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

            // ���� ��带 ���� ����Ʈ���� �����ϰ� Ŭ����� ����Ʈ�� �߰�
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // ��ǥ ��忡 �����ߴ��� Ȯ��
            if (currentNode == targetNode)
            {
                // ���� ��� ��ȯ
                Debug.Log("Ž������");
                return RetracePath(startNode, targetNode);
            }

            // �̿� ��� Ž��
            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                // �̿� ��尡 Ŭ����� ����Ʈ�� �ִ� ��� �ǳʶ�
                if (closedList.Contains(neighbor))
                {
                    continue;
                }

                // �̿� ����� �̵� ��� ���
                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                // �̿� ��尡 ���� ����Ʈ�� ���ų� �� ���� ������� ������ �� �ִ� ���
                if (!openList.Contains(neighbor) || newMovementCostToNeighbor < neighbor.gCost)
                {
                    // �̿� ����� G, H, F ���� ������Ʈ�ϰ� �θ� ��带 ���� ���� ����
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    // �̿� ��尡 ���� ����Ʈ�� ���ٸ� �߰�
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        // ��θ� ã�� �� ����
        Debug.Log("Ž���Ұ�");
        return null;
    }

    // ���� ��θ� �������ϴ� �޼���
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

    // ����� �̿��� ã�� �޼���
    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        // ���� ����� �����¿� �̿��� Ȯ���ϰ� �׸��� ���� �ִ��� �˻�
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

    // �� ��� ������ �Ÿ��� ����ϴ� �޼��� (Manhattan �Ÿ� ���)
    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
        int distanceY = Mathf.Abs(nodeA.position.y - nodeB.position.y);

        return distanceX + distanceY;
    }
}

// �׸����� �� ��带 ��Ÿ���� Ŭ����
public class Node
{
    public Vector2Int position; // ����� ��ġ
    public int gCost; // ���� ���κ����� �̵� ���
    public int hCost; // ��ǥ �������� ���� �̵� ���
    public int fCost => gCost + hCost; // �� �̵� ���
    public Node parent; // �θ� ���
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