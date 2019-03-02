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
    public int i_ScoreMultiplier;

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

    void Start()
    {
        SetUp();
    }

    private void Update()
    {
        if (CheckForMatches())
        {
            IncreaseScore();
            StartCoroutine(FallingTiles());
        }

    }

    private void SetUp()
    {
        arr_BackgroundTiles = new GameObject[i_GameBoardWidth, i_GameBoardHeight];
        arr_GameBoardTiles = new Game_Tile[i_GameBoardWidth, i_GameBoardHeight];

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
                arr_GameBoardTiles[i, j].SetUp();
            }
        }

        while (CheckValidMove())
        {
            for (int i = 0; i < i_GameBoardWidth; ++i)
            {
                for (int j = 0; j < i_GameBoardHeight; ++j)
                {
                    arr_GameBoardTiles[i, j].Tile = (TileTypes)Random.Range(1, 8);
                    arr_GameBoardTiles[i, j].GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(arr_GameBoardTiles[i, j].Tile);
                }
            }
        }
        
        return;
    }

    public GameObject GetBackgroundArray(int _i, int _j)
    {
        return arr_BackgroundTiles[_i, _j];
    }

    public void MoveTiles(Game_Tile _Tile, float _angle)
    {
        int i = (int)_Tile.vec_TilePosition.x / 2;
        int j = (int)_Tile.vec_TilePosition.y / 2;

        TileTypes _TempTile = _Tile.Tile;

        //Moving the Tile
        if ((_Tile.transform.position.x > _Tile.vec_TilePosition.x + 1 ||
             _Tile.transform.position.x < _Tile.vec_TilePosition.x - 1 ||
             _Tile.transform.position.y > _Tile.vec_TilePosition.y + 1 ||
             _Tile.transform.position.y < _Tile.vec_TilePosition.y - 1))
        {
            if (45 < _angle && _angle < 135f)
            {
                //Move Tile Up

                _Tile.Tile = arr_GameBoardTiles[i, j + 1].Tile;
                arr_GameBoardTiles[i, j + 1].Tile = _TempTile;

                //_Tile.GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(_Tile.Tile);
                //arr_GameBoardTiles[i, j + 1].GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(arr_GameBoardTiles[i, j + 1].Tile);

                return;
            }
            else if (-135 < _angle && _angle < -45f)
            {
                //Move Tile Down

                _Tile.Tile = arr_GameBoardTiles[i, j - 1].Tile;
                arr_GameBoardTiles[i, j - 1].Tile = _TempTile;

                // _Tile.GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(_Tile.Tile);
                //arr_GameBoardTiles[i, j - 1].GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(arr_GameBoardTiles[i, j - 1].Tile);

                return;
            }
            else if (135 < _angle || _angle < -135f)
            {
                //Move Tile Left

                _Tile.Tile = arr_GameBoardTiles[i - 1, j].Tile;
                arr_GameBoardTiles[i - 1, j].Tile = _TempTile;

                //_Tile.GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(_Tile.Tile);
                //arr_GameBoardTiles[i-1, j].GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(arr_GameBoardTiles[i-1, j].Tile);

                return;
            }
            else if (-45 < _angle && _angle < 45f)
            {
                //Move Tile Right

                _Tile.Tile = arr_GameBoardTiles[i + 1, j].Tile;
                arr_GameBoardTiles[i + 1, j].Tile = _TempTile;

                //_Tile.GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(_Tile.Tile);
                //arr_GameBoardTiles[i+1, j ].GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(arr_GameBoardTiles[i+1, j].Tile);
            }

            return;
        }
        else
        {
            return;
        }
    }

    public bool CheckForMatches()
    {
        bool b_IsThereAMatch = false;

        //Checking to see if it is a valid match
        for (int i = 0; i < i_GameBoardWidth; ++i)
        {
            for (int j = 0; j < i_GameBoardHeight; ++j)
            {
                //Two sets of Switch statements - one for k, one for l
                //Each checks the value and only performs the calculations of matches that will not cause an out-of-bounds error

                //Check rows
                switch (i)
                {
                    case 0: // +2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 2, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + i + ", " + l + "), (" + (i + 1) + ", " + l + "), (" + (i + 2) + ", " + l + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i + 1, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i + 2, j].b_ChangeTile = true;
                            }
                            break;
                        }
                    case 1: // +1/-1, +2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 2, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + l + "), (" + (k + 1) + ", " + l + "), (" + (k + 2) + ", " + l + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i + 1, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i + 2, j].b_ChangeTile = true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 1, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + (k - 1) + ", " + l + "), (" + k + ", " + l + "), (" + (k + 1) + ", " + l + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i + 1, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i - 1, j].b_ChangeTile = true;
                            }
                            break;
                        }
                    case 2: // +1/-1, +2, -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 2, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + l + "), (" + (k + 1) + ", " + l + "), (" + (k + 2) + ", " + l + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i + 1, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i + 2, j].b_ChangeTile = true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 1, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + (k - 1) + ", " + l + "), (" + k + ", " + l + "), (" + (k + 1) + ", " + l + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i + 1, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i - 1, j].b_ChangeTile = true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 2, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + (k - 2) + ", " + l + "), (" + (k - 1) + ", " + l + "), (" + k + ", " + l + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i - 2, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i - 1, j].b_ChangeTile = true;
                            }
                            break;
                        }
                    case 3: // +1/-1, -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 1, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + (k - 1) + ", " + l + "), (" + k + ", " + l + "), (" + (k + 1) + ", " + l + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i + 1, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i - 1, j].b_ChangeTile = true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 2, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + (k - 2) + ", " + l + "), (" + (k - 1) + ", " + l + "), (" + k + ", " + l + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i - 2, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i - 1, j].b_ChangeTile = true;
                            }
                            break;
                        }
                    case 4: // -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 2, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + (k - 2) + ", " + l + "), (" + (k - 1) + ", " + l + "), (" + k + ", " + l + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i - 2, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i - 1, j].b_ChangeTile = true;
                            }
                            break;
                        }
                    default:
                        {
                            return b_IsThereAMatch;
                        }
                }

                //Check columns
                switch (j)
                {
                    case 0: // +2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + l + "), (" + k + ", " + (l + 1) + "), (" + k + ", " + (l + 2) + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j + 2].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j + 1].b_ChangeTile = true;
                            }
                            break;
                        }
                    case 1: // +1/-1, +2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + l + "), (" + k + ", " + (l + 1) + "), (" + k + ", " + (l + 2) + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j + 2].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j + 1].b_ChangeTile = true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + (l - 1) + "), (" + k + ", " + l + "), (" + k + ", " + (l + 1) + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j + 1].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j - 1].b_ChangeTile = true;
                            }
                            break;
                        }
                    case 2: // +1/-1, +2, -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + l + "), (" + k + ", " + (l + 1) + "), (" + k + ", " + (l + 2) + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j + 2].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j + 1].b_ChangeTile = true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + (l - 1) + "), (" + k + ", " + l + "), (" + k + ", " + (l + 1) + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j + 1].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j - 1].b_ChangeTile = true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + (l - 2) + "), (" + k + ", " + (l - 1) + "), (" + k + ", " + l + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j - 2].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j - 1].b_ChangeTile = true;
                            }
                            break;
                        }
                    case 3: // +1/-1, +2, -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + l + "), (" + k + ", " + (l + 1) + "), (" + k + ", " + (l + 2) + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j + 2].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j + 1].b_ChangeTile = true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + (l - 1) + "), (" + k + ", " + l + "), (" + k + ", " + (l + 1) + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j + 1].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j - 1].b_ChangeTile = true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + (l - 2) + "), (" + k + ", " + (l - 1) + "), (" + k + ", " + l + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j - 2].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j - 1].b_ChangeTile = true;
                            }
                            break;
                        }
                    case 4: // +1/-1, -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + (l - 1) + "), (" + k + ", " + l + "), (" + k + ", " + (l + 1) + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j + 1].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j - 1].b_ChangeTile = true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + (l - 2) + "), (" + k + ", " + (l - 1) + "), (" + k + ", " + l + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j - 2].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j - 1].b_ChangeTile = true;
                            }
                            break;
                        }
                    case 5: // -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                //Debug.Log("Match found at: (" + k + ", " + (l - 2) + "), (" + k + ", " + (l - 1) + "), (" + k + ", " + l + ")");
                                arr_GameBoardTiles[i, j].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j - 2].b_ChangeTile = true;
                                arr_GameBoardTiles[i, j - 1].b_ChangeTile = true;
                            }
                            break;
                        }
                    default:
                        {
                            return b_IsThereAMatch;
                        }
                }

                if (arr_GameBoardTiles[i, j].b_ChangeTile)
                {
                    b_IsThereAMatch = true;
                    i_ScoreMultiplier++;
                    arr_GameBoardTiles[i, j].Tile = TileTypes.Null;
                    arr_GameBoardTiles[i, j].b_ChangeTile = false;
                }

                arr_GameBoardTiles[i, j].GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(arr_GameBoardTiles[i, j].Tile);
            }
        }

        return b_IsThereAMatch;
    }

    public bool CheckValidMove()
    {
        //Checking to see if it is a valid move
        for (int i = 0; i < i_GameBoardWidth; ++i)
        {
            for (int j = 0; j < i_GameBoardHeight; ++j)
            {
                //Two sets of Switch statements - one for k, one for l
                //Each checks the value and only performs the calculations of matches that will not cause an out-of-bounds error

                //Check rows
                switch (i)
                {
                    case 0: // +2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 2, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            break;
                        }
                    case 1: // +1/-1, +2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 2, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 1, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            break;
                        }
                    case 2: // +1/-1, +2, -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 2, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 1, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 2, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            break;
                        }
                    case 3: // +1/-1, -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i + 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 1, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 2, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            break;
                        }
                    case 4: // -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 1, j].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i - 2, j].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            break;
                        }
                    default:
                        {
                            return false;
                        }
                }

                //Check columns
                switch (j)
                {
                    case 0: // +2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            break;
                        }
                    case 1: // +1/-1, +2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            break;
                        }
                    case 2: // +1/-1, +2, -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            break;
                        }
                    case 3: // +1/-1, +2, -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            break;
                        }
                    case 4: // +1/-1, -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j + 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            else if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            break;
                        }
                    case 5: // -2
                        {
                            if (arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 1].Tile && arr_GameBoardTiles[i, j].Tile == arr_GameBoardTiles[i, j - 2].Tile && arr_GameBoardTiles[i, j].Tile != TileTypes.Null)
                            {
                                return true;
                            }
                            break;
                        }
                    default:
                        {
                            return false;
                        }
                }
            }
        }

        //If we reach this, default to false
        return false;
    }
    
    public void IncreaseScore()
    {
        GamePlayManager.instance.i_Score += i_ScoreMultiplier * 100;

        i_ScoreMultiplier = 0;

        return;
    }

    IEnumerator FallingTiles()
    {
        for (int j = 0; j < i_GameBoardHeight; ++j)
        {
            for (int i = 0; i < i_GameBoardWidth; ++i)
            {
                if (arr_GameBoardTiles[i, j].Tile == TileTypes.Null)
                {
                    for (int k = 0; (k + j) < i_GameBoardHeight; ++k)
                    {
                        arr_GameBoardTiles[i, j].Tile = arr_GameBoardTiles[i, j + k].Tile;
                        arr_GameBoardTiles[i, j + k].Tile = TileTypes.Null;

                        if (arr_GameBoardTiles[i, j].Tile == TileTypes.Null)
                        {
                            arr_GameBoardTiles[i, j].Tile = (TileTypes)Random.Range(1, 8);
                            arr_GameBoardTiles[i, j].GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(arr_GameBoardTiles[i, j].Tile);
                        }
                        else
                        {
                            arr_GameBoardTiles[i, j].GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(arr_GameBoardTiles[i, j].Tile);
                            break;
                        }                        
                    }
                }
            }
            yield return new WaitForSeconds(0.25f);
        }

        yield return 0;
    }

    public void FindPossibleMatches()
    {


        return;
    }
}