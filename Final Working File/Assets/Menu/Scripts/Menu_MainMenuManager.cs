using UnityEngine;
using System.Collections;

public class Menu_MainMenuManager: MonoBehaviour 
{
	public bool IsQuitButton 	= false; // Add 'public' to allow changes from inspector
	public bool IsBackButton 	= false;
	public bool IsStartButton 	= false;
	public bool BrainAgeCheck 	= false;
	
	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(GameObject.Find("Sound_Select"));
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
		else if(IsBackButton)
		{
			Application.LoadLevel(Application.loadedLevel-1); //Suddenly, magically fixed
		}
		else if(IsStartButton)
		{
			//Load level
			Application.LoadLevel(1);
		}
		else if(BrainAgeCheck)
		{
			//Load level
			GameObject.Find("Sound_Select").audio.Play();
			Application.LoadLevel("Menu_Selection");
		}
		
        Debug.Log("Drag ended!");
    }
}