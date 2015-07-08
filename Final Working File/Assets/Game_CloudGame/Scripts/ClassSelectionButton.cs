using UnityEngine;
using System.Collections;

public class ClassSelectionButton : MonoBehaviour 
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
		Application.LoadLevel(m_nstrLevelName);
	}
}
