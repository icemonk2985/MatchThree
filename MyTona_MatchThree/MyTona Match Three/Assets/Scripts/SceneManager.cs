using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    //Instance Variable
    public static SceneManager instance;

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
    /*
    void Start () {
		
	}
	
	void Update () {
		
	}//*/
}