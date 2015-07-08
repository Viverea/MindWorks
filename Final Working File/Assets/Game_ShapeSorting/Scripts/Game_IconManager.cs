using UnityEngine;
using System.Collections;

public class Game_IconManager : MonoBehaviour 
{
	private Vector3 		screenPoint;
	private GameObject[] 	aIcons;
	
	private static float 	f_tempXpos;
	private static float	f_tempYpos;
	private static float	f_tempZpos;
	
	private static float 	f_LastXpos;
	private static float	f_LastYpos;
	private static float	f_LastZpos;
	
	private bool			m_bUpdate;
	private bool			m_bSingleCollision;
	private bool			m_bCorrect;
	
	public int n_numberOfCircles;
	public int n_numberOfSquares;
	public int n_numberOfTriangles;
	
	// Use this for initialization
	void Start () 
	{		
		f_tempXpos = transform.position.x;
		f_tempYpos = transform.position.y;
		f_tempZpos = transform.position.z;
		
		m_bUpdate = m_bSingleCollision = m_bCorrect = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*if ( m_bUpdate )
		{
			Vector3 vTemp = this.transform.position;
			vTemp.z = 100.0f;
			if ( m_bSingleCollision )
			{
				if ( m_bCorrect )
				{
					vTemp.z = 90.0f;
					this.transform.position = vTemp;
					m_bUpdate = false;
					m_bSingleCollision = false;
					StartCoroutine(Correct (2.0f));
				}
				else
				{
					this.transform.position = vTemp;
					m_bUpdate = false;
					m_bSingleCollision = false;
					StartCoroutine(Wrong (2.0f));	
				}
			}
			else
			{
				m_bUpdate = false;
				this.transform.position = vTemp;
			}
		}*/
	}
	
	/*IEnumerator Correct(float _timeInterval)
	{
		GameObject.Find("Plane_Correct").renderer.enabled = true;
		GameObject.Find("Sound_Correct").audio.Play();
		yield return new WaitForSeconds(_timeInterval);
		GameObject.Find("Plane_Correct").renderer.enabled = false;
		//After the tick is done displaying, then you delete
		Destroy(gameObject);
	}
	
	IEnumerator Wrong(float _timeInterval)
	{
		GameObject.Find("Plane_Wrong").renderer.enabled = true;
		GameObject.Find("Sound_Wrong").audio.Play();
		yield return new WaitForSeconds(_timeInterval);
		GameObject.Find("Plane_Wrong").renderer.enabled = false;
	}
	
	/*void OnTriggerEnter(Collider other)
	{
		//If correct, destroy game object, cube score = 1
		Vector3 vNoCollision 	= new Vector3(transform.position.x,transform.position.y,100.0f);
//		Vector3 vHiddenSpot	 	= new Vector3(transform.position.x,transform.position.y,90.0f);
		
		//If it's in empty space, leave it where it is
		if(other.gameObject.name == "Cube_Empty")
		{
			gameObject.transform.position = vNoCollision;
		}
		//If other collision is the BLUE section
		else if(other.gameObject.name == "Cube_Blue")
		{
			m_bUpdate = true;
			m_bSingleCollision = !m_bSingleCollision;
			//Check if attributes of icon matches section (Circle)	//(Square)
			if(n_numberOfCircles == GameObject.Find("Cube_Blue").GetComponent<Section_Check>().noOfCircles ||
				n_numberOfSquares == GameObject.Find("Cube_Blue").GetComponent<Section_Check>().noOfSquares)
			{
				//If so, shift to a hidden spot first. Dont delete now as deleting now will 
				//start the coroutine but won't have enough time to exit the coroutine
				m_bCorrect = true;
//				gameObject.transform.position = vHiddenSpot;
//				StartCoroutine(Correct (2.0f));
			}
			else
			{
				//If not, leave it where it is and display 'X'
				m_bCorrect = false;
//				gameObject.transform.position = vNoCollision;
//				StartCoroutine(Wrong (2.0f));
			}
		}
		
		
		
		//If other collision is the GREEN section
		else if(other.gameObject.name == "Cube_Green")
		{
			m_bUpdate = true;
			m_bSingleCollision = !m_bSingleCollision;
			//(Circle)	//(Square)
			if(n_numberOfCircles == GameObject.Find("Cube_Green").GetComponent<Section_Check>().noOfCircles || 
				n_numberOfSquares == GameObject.Find("Cube_Green").GetComponent<Section_Check>().noOfSquares)
			{
				m_bCorrect = true;
//				gameObject.transform.position = vHiddenSpot;
//				StartCoroutine(Correct (2.0f));	
			}
			else
			{
				m_bCorrect = false;
//				gameObject.transform.position = vNoCollision;
//				StartCoroutine(Wrong (2.0f));
			}
		}
		
		
		
		//If other collision is the RED section
		else if(other.gameObject.name == "Cube_Red")
		{
			m_bUpdate = true;
			m_bSingleCollision = !m_bSingleCollision;
			//(Circle)	//(Square)
			if(n_numberOfCircles == GameObject.Find("Cube_Red").GetComponent<Section_Check>().noOfCircles ||
				n_numberOfSquares == GameObject.Find("Cube_Red").GetComponent<Section_Check>().noOfSquares)
			{
				m_bCorrect = true;
//				gameObject.transform.position = vHiddenSpot;
//				StartCoroutine(Correct (2.0f));
			}
			else
			{
				m_bCorrect = false;
//				gameObject.transform.position = vNoCollision;
//				StartCoroutine(Wrong (2.0f));
			}
		}				
	}*/
}
