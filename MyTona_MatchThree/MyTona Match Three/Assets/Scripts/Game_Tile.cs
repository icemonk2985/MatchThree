using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Tile : MonoBehaviour
{
    //Basic Variable Types
    public bool b_ChangeTile = false;
    public bool b_PossibleMatch = false;
    private bool b_TileHeld = false;
    private float f_DragAngle = 0.0f;

    //Advanced Variable Types
    public Vector2 vec_TilePosition;
    private Vector2 vec_FirstTileClickedPosition;
    private Vector2 vec_FinalTileClickedPosition;

    //Personal Variable Types
    public TileTypes Tile;
    
    private void Update()
    {
        if (b_TileHeld)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f));
            Game_Board.instance.GetBackgroundArray((int)vec_TilePosition.x / 2, (int)vec_TilePosition.y / 2).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BackgroundTileHighlightedSprite");
        }        
    }

    public void SetUp()
    {
        Tile = (TileTypes)Random.Range(1, 8);
        GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(Tile);
        vec_TilePosition = transform.position;
        return;
    }

    private void OnMouseDown()
    {
        if (GamePlayManager.instance.i_MovesUsed < 25 && !(GamePlayManager.instance.b_GamePaused))
        {
            b_TileHeld = true;

            GetComponent<BoxCollider2D>().enabled = false;

            vec_FirstTileClickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        return;
    }

    public void OnMouseUp()
    {
        if (b_TileHeld)
        {
            b_TileHeld = false;

            GetComponent<BoxCollider2D>().enabled = true;

            vec_FinalTileClickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            FindDragDirection();

            Game_Board.instance.MoveTiles(this, f_DragAngle);

            if (GamePlayManager.instance.b_LimitedMoves)
            {
                GamePlayManager.instance.i_MovesUsed++;
            }

            if (!Game_Board.instance.CheckValidMove())
            {
                Debug.Log("Not Valid Move");
                Game_Board.instance.MoveTiles(this, (f_DragAngle));
            }

            transform.position = vec_TilePosition;

            Game_Board.instance.GetBackgroundArray((int)vec_TilePosition.x / 2, (int)vec_TilePosition.y / 2).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BackgroundTileSprite");

            return;
        }
    }

    private void FindDragDirection()
    {
        f_DragAngle = Mathf.Atan2(vec_FinalTileClickedPosition.y - vec_FirstTileClickedPosition.y, vec_FinalTileClickedPosition.x - vec_FirstTileClickedPosition.x) * 180f / Mathf.PI;
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