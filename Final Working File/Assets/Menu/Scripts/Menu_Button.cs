using UnityEngine;
using System.Collections;

public class Menu_Button: MonoBehaviour 
{
	public bool 	IsStartButton		= false;
	public bool 	IsQuitButton 		= false; // Add 'public' to allow changes from inspector
	public bool 	IsBackButton 		= false;
	
	public bool 	IsGameBackButton	= false;
	public string	GoBackWhere;
	
	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(GameObject.Find("Sound_Back"));
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnMouseUp() 
	{
		if(IsQuitButton)
		{
			//Quit the game
			Application.Quit();	
		}
		else if(IsStartButton)
		{
			//Quit the game
			Application.LoadLevel(1);
		}
		else if(IsBackButton)
		{
			//Quit the game
			//GameObject.Find("Sound_Back").audio.Play();
			DontDestroyOnLoad(GameObject.Find("Sound_Back"));
			Application.LoadLevel("Menu_Selection");
		}
		else if(IsGameBackButton)
		{
			Difficulty_Selection_Settings.sLastLoadedLevel = Application.loadedLevelName;
			Application.LoadLevel(GoBackWhere);	
			if(Application.loadedLevelName == "Game_FlySwatter_Select")
			{
				Application.LoadLevel("Game_FlySwatter_Select");
			}
		}
		
        Debug.Log("Drag ended!");
    }
}