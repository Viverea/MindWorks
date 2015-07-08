using UnityEngine;
using System.Collections;

public class ClassCloudStar : MonoBehaviour 
{
	public Transform m_tPoint1;
	public Transform m_tPoint2;
	public Transform m_tPoint3;
	public Transform m_tPoint4;

	private float m_fSmoothTime = 1.0f;
	private Vector3 m_vStarVelocity = Vector3.zero;
	
	private float m_fResizeSpeed = 1.0f;
	private float m_fLargestScale = 0.7f;
	private float m_fSmallestScale = 0.1f;
	
	private bool m_bHasReachedFirstPoint = false;
	private bool m_bHasReachedSecondPoint = false;
	
	public bool m_bIsMoving = false;
	
	// Use this for initialization
	void Start () 
	{
		/*m_fTotalDistance = Vector3.Distance(m_tPoint1.position, m_tPoint2.position);
		
		m_fStartingTime = Time.time;*/
		
		m_tPoint1 = GameObject.Find ("Waypoint1").transform;
		m_tPoint2 = GameObject.Find ("Waypoint2").transform;
		m_tPoint3 = GameObject.Find ("Waypoint3").transform;
		m_tPoint4 = GameObject.Find ("Waypoint4").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*float fDistanceCovered = (Time.time - m_fStartingTime) * m_fStarSpeed;
		
		float fPercentageCovered = fDistanceCovered / m_fTotalDistance;
		
		this.transform.position = Vector3.Lerp(m_tPoint1.position, m_tPoint2.position, fPercentageCovered);*/
		
		if(m_bIsMoving == true)
		{
			if(m_bHasReachedFirstPoint == false)
			{
				this.transform.position = Vector3.SmoothDamp(this.transform.position, m_tPoint3.position, ref m_vStarVelocity, m_fSmoothTime);
				this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(m_fLargestScale, m_fLargestScale, m_fLargestScale), Time.deltaTime * m_fResizeSpeed);
			}
			else if(m_bHasReachedSecondPoint == false)
			{
				this.transform.position = Vector3.SmoothDamp(this.transform.position, m_tPoint4.position, ref m_vStarVelocity, m_fSmoothTime);
				this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(m_fSmallestScale, m_fSmallestScale, m_fSmallestScale), Time.deltaTime * m_fResizeSpeed);
			}
			
			if(m_bHasReachedSecondPoint == true)
			{
				this.transform.position = GameObject.Find ("Waypoint1").transform.position;
				
				m_bIsMoving = false;
				
				m_bHasReachedFirstPoint = false;
				m_bHasReachedSecondPoint = false;
			}
			
			if(Vector3.Distance(this.transform.position, m_tPoint3.position) <= 1.0f)
			{
				m_bHasReachedFirstPoint = true;
			}
			
			if(Vector3.Distance(this.transform.position, m_tPoint4.position) <= 1.0f)
			{
				m_bHasReachedSecondPoint = true;
			}
		}
	}

}
