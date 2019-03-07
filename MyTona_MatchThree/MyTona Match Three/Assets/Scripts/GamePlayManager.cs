using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

//Personal Enum
public enum TileTypes { Null, WhiteCircle, RedSquare, YellowKite, OrangeHexagon, GreenOctogon, BlueDiamond, PurpleTriangle }

public class GamePlayManager : MonoBehaviour
{
    //Instance Variable
    public static GamePlayManager instance;

    //Basic Variable Types
    public bool b_GamePaused;
    public bool b_LimitedMoves;
    public bool b_LimitedTime;
    public bool b_CollectTiles;
    private bool b_MuteSound = true;
    public int i_Score;
    public int i_MovesUsed;
    public int i_TilesCollected = 0;
    private int i_MovesLeft = 25;
    public float f_TimeLeft = 60f;

    //Advanced Variable Types
    [SerializeField] TextMeshProUGUI tmp_ScoreText;
    [SerializeField] TextMeshProUGUI tmp_MovesText;
    [SerializeField] TextMeshProUGUI tmp_TimeText;
    [SerializeField] TextMeshProUGUI tmp_TileText;
    [SerializeField] TextMeshProUGUI tmp_SoundText;
    [SerializeField] GameObject go_PauseMenu;
    [SerializeField] GameObject go_GameOver;

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
        Cursor.lockState = CursorLockMode.Confined;
        i_MovesLeft = 25;
        f_TimeLeft = 60f;
    }

    private void Update()
    {
        i_MovesLeft = 25 - i_MovesUsed;
        /*
        if (i_MovesLeft == 0 || f_TimeLeft <= 0f)
        {
            //end game
            GameOver();
        }//*/

        tmp_ScoreText.text = "SCORE:" + '\n' + i_Score;

        if (b_LimitedMoves)
        {
            tmp_MovesText.text = "MOVES LEFT: " + i_MovesLeft;
        }
        if (b_LimitedTime)
        {
            tmp_TimeText.text = "TIME LEFT: " + (int)f_TimeLeft;
        }
        if (b_CollectTiles)
        {
            tmp_TileText.text = "TILES " + '\n' + "COLLECTED:" + '\n' + i_TilesCollected + " / 15";
        }
    }

    public Sprite TileSpriteChange(TileTypes _tiletype)
    {
        return Resources.Load<Sprite>(_tiletype.ToString());
    }

    public void PauseMenu()
    {
        go_PauseMenu.SetActive(!(go_PauseMenu.activeSelf));
        b_GamePaused = go_PauseMenu.activeSelf;        
        return;
    }

    public void MuteSoundToggle()
    {
        b_MuteSound = !b_MuteSound;
        if (b_MuteSound)
        {
            tmp_SoundText.text = "Sound:" + '\n' + "ON";
        }
        else
        {
            tmp_SoundText.text = "Sound:" + '\n' + "OFF";
        }
        return;
    }

    public void GameOver()
    {
        if (go_GameOver != null)
        {
            //High Score Read/Write If Statements
            if (b_CollectTiles)
            {
                //Limited Moves + Collect Tiles Score
                if (b_LimitedMoves)
                {
                    int _score;
                    string path = Path.Combine(Application.streamingAssetsPath, "CTLM.txt");
                    using (StreamReader sr = new StreamReader(path))
                    {
                        _score = int.Parse(sr.ReadLine());
                        Debug.Log(_score);
                        Debug.Log(i_Score);
                    }
                    if (i_Score > _score)
                    {
                        using (StreamWriter wr = new StreamWriter(path))
                        {
                            wr.WriteLine(i_Score);
                        }
                    }
                }
                //Limited Time + Collect Tiles Score
                else if (b_LimitedTime)
                {
                    int _score;
                    string path = Path.Combine(Application.streamingAssetsPath, "CTLT.txt");
                    using (StreamReader sr = new StreamReader(path))
                    {
                        _score = int.Parse(sr.ReadLine());
                        Debug.Log(_score);
                        Debug.Log(i_Score);
                    }
                    if (i_Score > _score)
                    {
                        using (StreamWriter wr = new StreamWriter(path))
                        {
                            wr.WriteLine(i_Score);
                        }
                    }
                }
            }
            else
            {
                //Limited Moves Score
                if (b_LimitedMoves)
                {
                    int _score;
                    string path = Path.Combine(Application.streamingAssetsPath, "SALM.txt");
                    using (StreamReader sr = new StreamReader(path))
                    {
                        _score = int.Parse(sr.ReadLine());
                        Debug.Log(_score);
                        Debug.Log(i_Score);
                    }
                    if (i_Score > _score)
                    {
                        using (StreamWriter wr = new StreamWriter(path))
                        {
                            wr.WriteLine(i_Score);
                        }
                    }
                }
                //Limited Time Score
                else if (b_LimitedTime)
                {
                    int _score;
                    string path = Path.Combine(Application.streamingAssetsPath, "SALT.txt");
                    using (StreamReader sr = new StreamReader(path))
                    {
                        _score = int.Parse(sr.ReadLine());
                        Debug.Log(_score);
                        Debug.Log(i_Score);
                    }
                    if (i_Score > _score)
                    {
                        using (StreamWriter wr = new StreamWriter(path))
                        {
                            wr.WriteLine(i_Score);
                        }
                    }
                }
                //FreePlay Score
                else
                {
                    int _score;
                    string path = Path.Combine(Application.streamingAssetsPath, "FP.txt");
                    using (StreamReader sr = new StreamReader(path))
                    {
                        _score = int.Parse(sr.ReadLine());
                        Debug.Log(_score);
                        Debug.Log(i_Score);
                    }
                    if (i_Score > _score)
                    {
                        using (StreamWriter wr = new StreamWriter(path))
                        {
                            wr.WriteLine(i_Score);
                        }
                    }
                }
            }

            go_GameOver.SetActive(true);
            if (b_CollectTiles && i_TilesCollected >= 15)
            {
                go_GameOver.transform.GetComponentInChildren<TextMeshProUGUI>().text = "YOU WIN" + '\n' + '\n' + "FINAL SCORE:" + '\n' + i_Score + '\n' + '\n' + "RETURNING TO" + " THE MAIN MENU";
            }
            else
            {
                go_GameOver.transform.GetComponentInChildren<TextMeshProUGUI>().text = "GAME OVER" + '\n' + '\n' + "FINAL SCORE:" + '\n' + i_Score + '\n' + '\n' + "RETURNING TO" + " THE MAIN MENU";
            }
        }
        GameSceneManager.instance.GameOverDelay();
    }
}