using UnityEngine;
using System.Collections;

public class AvianCounterBirdScript : MonoBehaviour 
{
	private Vector3 m_vAssignedPosition = Vector3.zero;
	private Vector3 m_vSpawnPosition = new Vector3(-100.0f, -150.0f, 0.0f);
	
	private float 	m_fJourneyFraction = 0.0f;
	
	private bool 	m_bHasReachedPosition = true;
	
	private bool 	m_bHasBeenPositioned = false;
	
	private Color	m_cTransparentColor;
	private Color   m_cFullColor;
	
	// Use this for initialization
	void Start () 
	{ 
		m_cFullColor = this.gameObject.renderer.material.color;
		m_cTransparentColor = m_cFullColor;
		m_cTransparentColor.a = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!m_bHasReachedPosition)
		{
			this.transform.position = Vector3.Lerp(m_vSpawnPosition, m_vAssignedPosition, m_fJourneyFraction);
			
			this.gameObject.renderer.material.color = Color.Lerp (m_cTransparentColor, m_cFullColor, m_fJourneyFraction);
			
			m_fJourneyFraction += 0.025f;
			
			if(m_fJourneyFraction >= 1.0f)
			{
				m_bHasReachedPosition = true;
				m_fJourneyFraction = 0.0f;
				m_bHasBeenPositioned = true;
				
				this.gameObject.renderer.material.color = m_cFullColor;
			}
		}
	}
	
	public void SetAssignedPosition(Vector3 _vPosition)
	{
		m_vAssignedPosition = _vPosition;
	}
	
	public void GoToPosition()
	{
		this.gameObject.transform.position = m_vAssignedPosition;
		m_bHasBeenPositioned = true;
	}
	
	public void StartMoving()
	{
		m_bHasReachedPosition = false;
		
		m_vSpawnPosition.x = m_vAssignedPosition.x;
		m_vSpawnPosition.y = -90.0f;
		
		this.gameObject.transform.position = m_vSpawnPosition;
		
		int nRandomNum = Random.Range (8, 13);
		float fScaleNum = (float) nRandomNum / 10.0f ;
		
		this.gameObject.transform.localScale = new Vector3(transform.localScale.x * fScaleNum, transform.localScale.y * fScaleNum, transform.localScale.z * fScaleNum);
		
		nRandomNum = Random.Range (0,2);
		
		if(nRandomNum == 1)
		{
			this.gameObject.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		}
	}
	
	public Vector3 GetAssignedPosition()
	{
		return m_vAssignedPosition;
	}
	
	
	public bool HasBeenPositioned()
	{
		return m_bHasBeenPositioned;
	}
	
}
