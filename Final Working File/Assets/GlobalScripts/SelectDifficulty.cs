using UnityEngine;
using System.Collections;

public class SelectDifficulty : MonoBehaviour 
{
	public string m_sNameofLevel;
	
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
		Application.LoadLevel(m_sNameofLevel);
	}
}
