using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance;

    public enum TileTypes {WhiteCircle, RedSquare, YellowKite, OrangeHexagon, GreenOctogon, BlueDiamond, PurpleTriangle}
    
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

    void Start () {
		
	}
	
	void Update () {
		
	}

    public Sprite TileSpriteChange(TileTypes _tiletype)
    {
        Sprite spr_Temp = null;
        return spr_Temp;
    }
}