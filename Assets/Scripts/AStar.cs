using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    private List<Node> openList;
    private List<Node> closedList;

    int[,] grid;
    Node[,] newGrid;

    Node minNode;
    int min;



    private void Start()
    {
        // A* 알고리즘을 시작하기 전에 openList와 closedList를 초기화합니다.
        openList = new List<Node>();
        closedList = new List<Node>();
        grid = GameObject.Find("GameManager").GetComponent<GameManager>().returnGrid();
        newGrid = new Node[grid.GetLength(0), grid.GetLength(1)];

        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                if (grid[y, x] == 0)
                {
                    Node temp = new Node();
                    temp.y = y;
                    temp.x = x;
                    temp.check = true;
                    newGrid[y, x] = temp;
                }
                else
                {
                    newGrid[y, x] = new Node();
                    newGrid[y, x].check = false;
                }
            }
        }
        SetNeighbors();

    }

    public void GoToA(int curX, int curY, int targetX, int targetY)
    {
        Node curNode = newGrid[curY, curX];
        curNode.x = curX;
        curNode.y = curY;

        Node targetNode = newGrid[targetY, targetX];
        targetNode.x = targetX;
        targetNode.y = targetY;

        curNode.G = 0;
        curNode.H = GetH(curNode, targetNode);
        curNode.F = curNode.G + curNode.H;

        FindPath(curNode, targetNode);
    }

    void SetNeighbors()
    {
        for (int y = 0; y < newGrid.GetLength(0); y++)
        {
            for (int x = 0; x < newGrid.GetLength(1); x++)
            {
                if (newGrid[y, x].check == true)
                {
                    if (y != 0)
                        //if(newGrid[y - 1, x].check == true)
                            newGrid[y, x].AddNeighbor(newGrid[y - 1, x]);
                    if(y != newGrid.GetLength(0)-1)
                        //if (newGrid[y + 1, x].check == true)
                            newGrid[y, x].AddNeighbor(newGrid[y + 1, x]);
                    if (x != 0)
                        //if (newGrid[y, x - 1].check == true)
                            newGrid[y, x].AddNeighbor(newGrid[y, x - 1]);
                    if (x != newGrid.GetLength(1)-1)
                        //if (newGrid[y, x + 1].check == true)
                            newGrid[y, x].AddNeighbor(newGrid[y, x + 1]);
                }
            }
        }
    }

    private void FindPath(Node startNode, Node targetNode)
    {
        

        openList.Clear();
        closedList.Clear();


        openList.Add(startNode);

        Node currentNode = openList[0];

        foreach(Node nextNode in currentNode.neighbors)
        {
            Debug.Log("??");

            nextNode.parent = currentNode;
            nextNode.G = 10;
            nextNode.H = GetH(nextNode, targetNode);
            nextNode.F = nextNode.G + nextNode.H;

            openList.Add(nextNode);

        }


        openList.Remove(currentNode);
        closedList.Add(currentNode);


        //경로탐색 2
        while (true)
        {
            currentNode = FindMinNode(openList);

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            Debug.Log(currentNode);

            foreach (Node nextNode in currentNode.neighbors)
            {
                if (!closedList.Contains(nextNode))
                {
                    if (!openList.Contains(nextNode))
                    {
                        nextNode.parent = currentNode;
                        nextNode.G = currentNode.G + 10;
                        nextNode.H = GetH(nextNode, targetNode);
                        nextNode.F = nextNode.G + nextNode.H;
                        openList.Add(nextNode);
                    }
                    else
                    {
                        if (currentNode.G + 10 < nextNode.G)
                        {
                            nextNode.parent = currentNode;
                            nextNode.G = currentNode.G + 10;
                            nextNode.F = nextNode.G + nextNode.H;
                        }
                    }
                }
            }

            if (openList.Count == 0 || openList.Contains(targetNode))
            {
                Debug.Log("찾앗다 쉬펄");
                return;
            }
        }
    }

    private int GetH(Node nodeA, Node nodeB)
    {
        int result = 0;

        result += Mathf.Abs(nodeB.y - nodeA.y);
        result += Mathf.Abs(nodeB.x - nodeA.x);

        return result;
    }

    Node FindMinNode(List<Node> openList)
    {
        min = int.MaxValue;
        minNode = null;


        for (int i = 0; i < openList.Count; i++)
        {
            if (openList[i].F < min)
            {
                min = openList[i].F;
                minNode = openList[i];
            }
        }

        //디버깅
        if (minNode == null)
        {
            Debug.Log("minNode is null");
        }

        return minNode;
    }
}

public class Node
{
    public bool check;

    public int x;
    public int y;

    public int G;
    public int H;
    public int F;
    public Node parent;
    public List<Node> neighbors = new List<Node>();

    public void AddNeighbor(Node nNode)
    {
        if (nNode != null)
            this.neighbors.Add(nNode);
    }
}