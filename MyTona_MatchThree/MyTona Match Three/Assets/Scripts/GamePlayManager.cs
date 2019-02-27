using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Personal Enum
public enum TileTypes { WhiteCircle, RedSquare, YellowKite, OrangeHexagon, GreenOctogon, BlueDiamond, PurpleTriangle }

public class GamePlayManager : MonoBehaviour
{
    //Instance Variable
    public static GamePlayManager instance;
           
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
        Cursor.lockState = CursorLockMode.Confined;	
	}
	
    public Sprite TileSpriteChange(TileTypes _tiletype)
    {
        return Resources.Load<Sprite>(_tiletype.ToString());
    }
}