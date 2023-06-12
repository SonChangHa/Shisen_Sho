using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileValue;
    public int tileX;
    public int tileY;

    public Sprite clickedTile;
    public Sprite clickedOutTile;

    SpriteRenderer SR;
    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked()
    {
        SR.sprite = clickedTile;
    }
    public void ClickedOut()
    {
        SR.sprite = clickedOutTile;
    }
}
