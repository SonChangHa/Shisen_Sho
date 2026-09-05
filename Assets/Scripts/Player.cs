using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    RaycastHit2D hit;

    Tile firTile;
    Tile secTile;

    int firVal = -1;
    int secVal = -1;

    GameManager gameManager;

    LineRenderer lineRenderer;

    const float pathDisplayDuration = 0.3f;

    void Start()
    {
        gameManager = GetComponent<GameManager>();

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.sortingOrder = 10;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        MouseClickDown();
    }

    void MouseClickDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider.tag == "Tile")
            {
                TileClicked(hit);
            }

        }
    }

    void TileClicked(RaycastHit2D hit)
    {

        if (firVal == -1)
        {
            firTile = hit.collider.GetComponent<Tile>();
            firVal = firTile.tileValue;
            firTile.Clicked();
        }
        else
        {
            secTile = hit.collider.GetComponent<Tile>();
            secVal = secTile.tileValue;

            if (firVal == secVal && firTile != secTile)
            {
                List<Vector2Int> path;
                bool found = GetComponent<DFS>().GoFind(new Vector2Int(firTile.tileX, firTile.tileY), new Vector2Int(secTile.tileX, secTile.tileY), out path);

                if (found)
                {
                    StartCoroutine(ShowPathThenRemove(path, firTile, secTile));
                }
                else
                {
                    firTile.ClickedOut();
                }

                firVal = -1;
                firTile = null;
                secTile = null;
            }
            else
            {
                firVal = -1;
                firTile.ClickedOut();
                firTile = null;
            }
        }
    }

    IEnumerator ShowPathThenRemove(List<Vector2Int> path, Tile a, Tile b)
    {
        lineRenderer.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i, gameManager.GridToWorld(path[i].x, path[i].y));
        }
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(pathDisplayDuration);

        lineRenderer.enabled = false;

        RemoveTile(a);
        RemoveTile(b);
    }

    void RemoveTile(Tile tile)
    {
        gameManager.grid[tile.tileY, tile.tileX] = 0;
        Destroy(tile.gameObject);
    }

}
