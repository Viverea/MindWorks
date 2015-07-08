using UnityEngine;
using System.Collections;

public class FlySwatterGlobalVarScript : MonoBehaviour 
{
	public float m_fXMin;
	public float m_fXMax;
	public float m_fYMin;
	public float m_fYMax;
	
	public float m_fRoundTime;
	public float m_fFlyFlightVelocity = 1.0f;
	
	// Use this for initialization
	void Start () 
	{
		m_fXMin = -93.5f;
		m_fXMax = 151.0f;
		m_fYMin = -94.0f;
		m_fYMax = 72.0f;
		
		m_fRoundTime = 10.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
