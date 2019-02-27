using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Board : MonoBehaviour
{
    //Instance Variable
    public static Game_Board instance;

    //Basic Variable Types
    public int i_GameBoardHeight;
    public int i_GameBoardWidth;

    //Advanced Variable Types
    [SerializeField] GameObject go_BackgroundTilePrefab;
    [SerializeField] GameObject go_TilePrefab;
    private GameObject[,] arr_BackgroundTiles;

    //Personal Variable Types
    private Game_Tile[,] arr_GameBoardTiles;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    void Start ()
    {
        arr_BackgroundTiles = new GameObject[i_GameBoardWidth, i_GameBoardHeight];
        arr_GameBoardTiles = new Game_Tile[i_GameBoardWidth, i_GameBoardHeight];

        SetUp();
    }

    private void SetUp()
    {
        //This for loop sets up the game board, creating a 5x6 board
        for (int i = 0; i < i_GameBoardWidth; ++i)
        {
            for (int j = 0; j < i_GameBoardHeight; ++j)
            {
                GameObject go_TempTile;
                Vector2 vec_TempPosition = new Vector2(i * 2, j * 2);

                //Background Tile Setup
                go_TempTile = Instantiate(go_BackgroundTilePrefab, vec_TempPosition, Quaternion.identity);
                go_TempTile.transform.parent = transform;
                go_TempTile.name = "Background Tile " + i + ", " + j;
                arr_BackgroundTiles[i, j] = go_TempTile;

                //Gameplay Tile Setup
                go_TempTile = Instantiate(go_TilePrefab, vec_TempPosition, Quaternion.identity);                
                go_TempTile.name = "Gameplay Tile " + i + ", " + j;
                arr_GameBoardTiles[i, j] = go_TempTile.GetComponent<Game_Tile>();
            }
        }
        return;
    }

    public GameObject GetBackgroundArray(int _i, int _j)
    {
        return arr_BackgroundTiles[_i, _j];
    }

    /*
       45<x<135
     -135<x<-45
      135<x<-135
      -45<x<45
         */
    public void MoveTiles(Game_Tile _Tile, float _angle)
    {
        int i = (int)_Tile.vec_TilePosition.x / 2;
        int j = (int)_Tile.vec_TilePosition.y / 2;

        TileTypes _TempTile = _Tile.Tile;

        if (135 < _angle || _angle < -135f)
        {
            Debug.Log("Move Left");

            _Tile.Tile = arr_GameBoardTiles[i - 1, j].Tile;
            arr_GameBoardTiles[i - 1, j].Tile = _TempTile;

            _Tile.GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(_Tile.Tile);
            arr_GameBoardTiles[i - 1, j].GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(arr_GameBoardTiles[i - 1, j].Tile);

            return;
        }
        else if (-45 < _angle && _angle < 45f)
        {
            Debug.Log("Move Right");

            _Tile.Tile = arr_GameBoardTiles[i + 1, j].Tile;
            arr_GameBoardTiles[i + 1, j].Tile = _TempTile;

            _Tile.GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(_Tile.Tile);
            arr_GameBoardTiles[i + 1, j].GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(arr_GameBoardTiles[i + 1, j].Tile);

            return;
        }
        else if (-135 < _angle && _angle < -45f)
        {
            Debug.Log("Move Down");

            _Tile.Tile = arr_GameBoardTiles[i, j - 1].Tile;
            arr_GameBoardTiles[i, j - 1].Tile = _TempTile;

            _Tile.GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(_Tile.Tile);
            arr_GameBoardTiles[i, j - 1].GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(arr_GameBoardTiles[i, j - 1].Tile);

            return;
        }
        else if (45 < _angle && _angle < 135f)
        {
            Debug.Log("Move Up");

            _Tile.Tile = arr_GameBoardTiles[i, j + 1].Tile;
            arr_GameBoardTiles[i, j + 1].Tile = _TempTile;

            _Tile.GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(_Tile.Tile);
            arr_GameBoardTiles[i, j + 1].GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(arr_GameBoardTiles[i, j + 1].Tile);

            return;
        }
        else // Default incase an angle fails all previous IF statements
        {
            return;
        }
    }
}