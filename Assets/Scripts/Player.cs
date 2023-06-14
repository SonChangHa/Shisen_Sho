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
                GetComponent<AStar2>().GoFind(new Vector2Int(firTile.tileX, firTile.tileY), new Vector2Int(secTile.tileX, secTile.tileY));
            }
            else
            {
                firVal = -1;
                firTile.ClickedOut();
                firTile = null;
            }
        }
    }

}
