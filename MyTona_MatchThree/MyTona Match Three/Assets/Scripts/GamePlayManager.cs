using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Personal Enum
public enum TileTypes { Null, WhiteCircle, RedSquare, YellowKite, OrangeHexagon, GreenOctogon, BlueDiamond, PurpleTriangle }

public class GamePlayManager : MonoBehaviour
{
    //Instance Variable
    public static GamePlayManager instance;

    //Basic Variable Types
    public int i_Score;
    public int i_MovesUsed;
    private int i_MovesLeft;
    private bool b_MuteSound;

    //Advanced Variable Types
    [SerializeField] TextMeshProUGUI tmp_ScoreText;
    [SerializeField] GameObject go_PauseMenu;

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

    private void Update()
    {
        if (i_MovesLeft == 0)
        {
            //end game
        }

        tmp_ScoreText.text = "SCORE:" + '\n' + i_Score;
    }

    public Sprite TileSpriteChange(TileTypes _tiletype)
    {
        return Resources.Load<Sprite>(_tiletype.ToString());
    }

    public void PauseMenu()
    {
        go_PauseMenu.SetActive(!(go_PauseMenu.activeSelf));
        return;
    }

    public void MuteSoundToggle()
    {
        b_MuteSound = !b_MuteSound;
        return;
    }
}