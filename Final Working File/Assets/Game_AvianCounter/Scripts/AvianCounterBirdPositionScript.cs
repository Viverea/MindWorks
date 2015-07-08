using UnityEngine;
using System.Collections;

public class AvianCounterBirdPositionScript : MonoBehaviour 
{
	bool m_bIsOccupied = false;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void SetOccupied(bool _bIsOccupied)
	{
		m_bIsOccupied = _bIsOccupied;
	}
	
	public bool GetOccupied()
	{
		return m_bIsOccupied;
	}
}
