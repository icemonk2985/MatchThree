using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Tile : MonoBehaviour
{
    //Basic Variable Types
    public bool b_ChangeTile = false;
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

        Game_Board.instance.MoveTiles(this, f_DragAngle);

        if (!Game_Board.instance.CheckValidMove())
        {
            Game_Board.instance.MoveTiles(this, (f_DragAngle));
        }

        transform.position = vec_TilePosition;

        Game_Board.instance.GetBackgroundArray((int)vec_TilePosition.x / 2, (int)vec_TilePosition.y / 2).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BackgroundTileSprite");

        return;
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

/*
 arr_GameBoardTiles[k, j].Tile = TileTypes.Null;

                        if ((l + j) == (i_GameBoardHeight - 1))
                        {
                            TileTypes _tile;
                            do
                            {
                                _tile = (TileTypes)Random.Range(1, 8);
                            } while (arr_GameBoardTiles[k, l + j].Tile == _tile);
                            arr_GameBoardTiles[k, l + j].Tile = _tile;
                        }
                        else
                        {
                            if (arr_GameBoardTiles[k, j + 1].Tile == TileTypes.Null || arr_GameBoardTiles[k, j + 1].b_ChangeTile)
                            {
                                arr_GameBoardTiles[k, j].Tile = (TileTypes)Random.Range(1, 8);
                                Debug.Log(arr_GameBoardTiles[k, j].name + ": " + arr_GameBoardTiles[k, j].Tile);
                            }
                            else
                            {
                                Debug.Log(arr_GameBoardTiles[k, j].name + ": " + arr_GameBoardTiles[k, j].Tile);
                                Debug.Log(arr_GameBoardTiles[k, j + 1].name + ": " + arr_GameBoardTiles[k, j + 1].Tile);
                                arr_GameBoardTiles[k, j].Tile = arr_GameBoardTiles[k, j + 1].Tile;
                                arr_GameBoardTiles[k, j + 1].Tile = TileTypes.Null;
                                Debug.Log(arr_GameBoardTiles[k, j].name + ": " + arr_GameBoardTiles[k, j].Tile);
                                Debug.Log(arr_GameBoardTiles[k, j + 1].name + ": " + arr_GameBoardTiles[k, j + 1].Tile);
                            }//
                            for (int m = 0; (l + j + m) < i_GameBoardHeight; m++)
                            {
                                if (arr_GameBoardTiles[k, l + j + m].Tile != TileTypes.Null)
                                {
                                    arr_GameBoardTiles[k, l + j].Tile = arr_GameBoardTiles[k, l + j + m].Tile;
                                    arr_GameBoardTiles[k, l + j + m].Tile = TileTypes.Null;
                                }
                            }

                            if (arr_GameBoardTiles[k, l + j].Tile == TileTypes.Null)
                            {
                                arr_GameBoardTiles[k, l + j].Tile = (TileTypes) Random.Range(1, 8);
                            }
                        }
     */