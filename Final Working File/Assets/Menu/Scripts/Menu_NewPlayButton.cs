using UnityEngine;
using System.Collections;

public class Menu_NewPlayButton : MonoBehaviour 
{
	//sSelectedLevel will be in the form of "(underscore)(gameName)"
	public static string sSelectedLevel;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnMouseUp()
	{
		Application.LoadLevel("Game"+sSelectedLevel+"_Select");
	}
}
