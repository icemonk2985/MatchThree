using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Tile : MonoBehaviour
{
    public GameObject go_BaseTile;

    GamePlayManager.TileTypes Tile;
    GameObject go_PlayTile;

    private void Start()
    {
        
    }

    private void SetUp()
    {
        Tile = (GamePlayManager.TileTypes)Random.Range(0, 7);
        go_PlayTile = Instantiate(go_BaseTile, new Vector2(0f, 0f), Quaternion.identity);
        go_PlayTile.GetComponent<SpriteRenderer>().sprite = GamePlayManager.instance.TileSpriteChange(Tile);
    }
}