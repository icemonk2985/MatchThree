using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Tile : MonoBehaviour
{
    //Basic Variable Types
    private bool b_TileHeld = false;
    private float f_DragAngle = 0.0f;

    //Advanced Variable Types
    public Vector2 vec_TilePosition;
    private Vector2 vec_FirstTileClickedPosition;
    private Vector2 vec_FinalTileClickedPosition;

    //Personal Variable Types
    public TileTypes Tile;

    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        if (b_TileHeld)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f));
            Game_Board.instance.GetBackgroundArray((int)vec_TilePosition.x / 2, (int)vec_TilePosition.y / 2).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BackgroundTileHighlightedSprite");
        }        
    }

    private void SetUp()
    {
        Tile = (TileTypes)Random.Range(0, 7);
        GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(Tile);
        vec_TilePosition = transform.position;
        return;
    }

    private void OnMouseDown()
    {
        b_TileHeld = true;
        GetComponent<BoxCollider2D>().enabled = false;
        vec_FirstTileClickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return;
    }

    public void OnMouseUp()
    {
        b_TileHeld = false;

        GetComponent<BoxCollider2D>().enabled = true;

        vec_FinalTileClickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        FindDragDirection();

        if (transform.position.x > vec_TilePosition.x + 1 ||
            transform.position.x < vec_TilePosition.x - 1 ||
            transform.position.y > vec_TilePosition.y + 1 ||
            transform.position.y < vec_TilePosition.y - 1)
        {
            Game_Board.instance.MoveTiles(this, f_DragAngle);
            
            transform.position = vec_TilePosition;
        }
        else//*/
        {
            transform.position = vec_TilePosition;
        }

        Game_Board.instance.GetBackgroundArray((int)vec_TilePosition.x / 2, (int)vec_TilePosition.y / 2).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BackgroundTileSprite");

        return;
    }

    private void FindDragDirection()
    {
        f_DragAngle = Mathf.Atan2(vec_FinalTileClickedPosition.y - vec_FirstTileClickedPosition.y, vec_FinalTileClickedPosition.x - vec_FirstTileClickedPosition.x) * 180f / Mathf.PI;
        Debug.Log(f_DragAngle);
        return;
    }

    private void OnMouseEnter()
    {
        Game_Board.instance.GetBackgroundArray((int)vec_TilePosition.x / 2, (int)vec_TilePosition.y / 2).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BackgroundTileHighlightedSprite");
        return;
    }

    private void OnMouseExit()
    {
        Game_Board.instance.GetBackgroundArray((int)vec_TilePosition.x / 2, (int)vec_TilePosition.y / 2).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BackgroundTileSprite");
        return;
    }
}