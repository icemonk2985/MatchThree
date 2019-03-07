using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class MainMenuHighScores : MonoBehaviour {

    [SerializeField] TextMeshProUGUI tmp_HighScoreText_1;
    [SerializeField] TextMeshProUGUI tmp_HighScoreText_2;
    [SerializeField] TextMeshProUGUI tmp_HighScoreText_3;
    [SerializeField] TextMeshProUGUI tmp_HighScoreText_4;
    [SerializeField] TextMeshProUGUI tmp_HighScoreText_5;
	
	void Update ()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CTLM.txt");
        //path = Resources.
        using (StreamReader sr = new StreamReader(path))
        {
            tmp_HighScoreText_1.text = " HIGH SCORE:" + (sr.ReadLine());
        }

        path = Path.Combine(Application.streamingAssetsPath, "CTLT.txt");
        using (StreamReader sr = new StreamReader(path))
        {
            tmp_HighScoreText_2.text = " HIGH SCORE:" + (sr.ReadLine());
        }

        path = Path.Combine(Application.streamingAssetsPath, "SALM.txt");
        using (StreamReader sr = new StreamReader(path))
        {
            tmp_HighScoreText_3.text = " HIGH SCORE:" + (sr.ReadLine());
        }

        path = Path.Combine(Application.streamingAssetsPath, "SALT.txt");
        using (StreamReader sr = new StreamReader(path))
        {
            tmp_HighScoreText_4.text = " HIGH SCORE:" + (sr.ReadLine());
        }

        path = Path.Combine(Application.streamingAssetsPath, "FP.txt");
        using (StreamReader sr = new StreamReader(path))
        {
            tmp_HighScoreText_5.text = " HIGH SCORE:" + (sr.ReadLine());
        }
    }
}
