using UnityEngine;
using System.Collections;

public class ClassSelectionButton2 : MonoBehaviour 
{
	public string m_nstrLevelName;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnMouseDown()
	{
		Application.LoadLevel("Game_NumberTapper_Select");
		
		GameObject.Destroy(GameObject.Find ("ScoreSystem").gameObject);
	}
}
