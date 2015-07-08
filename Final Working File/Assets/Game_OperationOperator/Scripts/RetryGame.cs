using UnityEngine;
using System.Collections;

public class RetryGame : MonoBehaviour 
{
	public static bool m_bRetryGame = false;

	// Use this for initialization
	void Start () 
	{
		m_bRetryGame = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnMouseUp()
	{
		m_bRetryGame = true;
	}
}
