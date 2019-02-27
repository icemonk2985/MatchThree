using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Board : MonoBehaviour
{
    public int i_GameBoardHeight;
    public int i_GameBoardWidth;

    [SerializeField] GameObject go_TilePrefab;

    private Game_Tile[,] arr_GameBoardTiles;

    void Start ()
    {
        arr_GameBoardTiles = new Game_Tile[i_GameBoardWidth, i_GameBoardHeight];

        Game_Board_SetUp();
    }

    private void Game_Board_SetUp()
    {
        //This for loop sets up the game board, creating a 5x6 board
        for (int i = 0; i < i_GameBoardHeight; ++i)
        {
            for (int j = 0; j < i_GameBoardWidth; ++j)
            {
                GameObject go_BackgroundTile;
                Vector2 vec_TempPosition = new Vector2(i*2, j*2);
                Debug.Log("Width: " + i*2 + ", Height: " + j*2);
                go_BackgroundTile = Instantiate(go_TilePrefab, vec_TempPosition, Quaternion.identity);
                go_BackgroundTile.transform.parent = this.transform;
                go_BackgroundTile.name = "Background Tile " + i + ", " + j;
            }
        }
    }
}