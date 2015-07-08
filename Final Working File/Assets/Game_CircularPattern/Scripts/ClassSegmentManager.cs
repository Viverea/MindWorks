using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClassSegmentManager : MonoBehaviour 
{
	public List <GameObject> m_agoSegments = new List<GameObject>();
	
	public bool m_bIsRotating = false;
	public float m_fAngle = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*if(m_bIsRotating == true)
		{
			this.transform.RotateAround(this.transform.position, Vector3.forward, 30f * Time.deltaTime);
			
			if(this.transform.rotation.z >= m_fAngle)
			{
				m_bIsRotating = false;
			}
		}*/
	}
}
