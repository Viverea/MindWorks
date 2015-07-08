using UnityEngine;
using System.Collections;

public class FlySwatterWingsMovementScript : MonoBehaviour 
{
	
	private enum eFlyWingsPositions
	{
		eNone = 0,
		eWings1,
		eWings2,
		eTotal
	};
	
	private eFlyWingsPositions m_eFlyWingsPos;
	
	public Material m_matFlyWings1;
	public Material m_matFlyWings2;
	public Material m_matFlySquished;
	
	private bool m_bChangeMat = false;
	private bool m_bIsSquished = false;
	
	private int m_nFrameCount = 0;
	
	// Use this for initialization
	void Start () 
	{
		int nRandNum = Random.Range(1,3);
		
		if(nRandNum == 1)
		{
			m_eFlyWingsPos = eFlyWingsPositions.eWings1;
			this.gameObject.renderer.material = m_matFlyWings1;
		}
		else
		{
			m_eFlyWingsPos = eFlyWingsPositions.eWings2;
			this.gameObject.renderer.material = m_matFlyWings2;
		}
		
		m_nFrameCount = Random.Range(0,5);
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.transform.localPosition = new Vector3(-0.2f, 1.92f, 2.0f);
		
		if(m_bChangeMat && !m_bIsSquished)
		{		
			switch(m_eFlyWingsPos)
			{
			case eFlyWingsPositions.eWings1:
			{
				this.gameObject.renderer.material = m_matFlyWings1;
				m_bChangeMat = false;
				break;
			}
			case eFlyWingsPositions.eWings2:
			{
				this.gameObject.renderer.material = m_matFlyWings2;
				m_bChangeMat = false;
				break;
			}
			}
			

		}
		else if (m_bIsSquished)
		{
			Debug.Log (m_nFrameCount);
			
			if(m_nFrameCount > 10)
			{
				Destroy(this.gameObject.transform.parent.gameObject);
			}
		}
		
		if(!m_bIsSquished)
		{
			Flap();
		}
		
		m_nFrameCount++;
		

	}
	
	private void Flap()
	{
		if(m_nFrameCount >= 5)
		{
			if(m_eFlyWingsPos == eFlyWingsPositions.eWings1)
			{
				m_eFlyWingsPos = eFlyWingsPositions.eWings2;
			}
			else
			{
				m_eFlyWingsPos = eFlyWingsPositions.eWings1;
			}
			
			m_bChangeMat = true;
			m_nFrameCount = 0;
		}
	}
	
	public void ChangeFacingDirection(Vector3 _vMoveDirection)
	{
		Vector3 vMoveDirection = _vMoveDirection;
		
		//Quaternion qFinalRotation = this.gameObject.transform.localRotation;
		
		Vector3 vNewRotation = this.gameObject.transform.localEulerAngles;
				
		//Top right
		if(vMoveDirection.x >= 0.0f && vMoveDirection.y >= 0.0f)
		{
			float fDegrees = Mathf.Acos(vMoveDirection.y / vMoveDirection.magnitude);
			fDegrees = fDegrees * (180.0f / Mathf.PI);
			vNewRotation.z = fDegrees;
		}
		//Bottom right
		else if (vMoveDirection.x >= 0.0f && vMoveDirection.y < 0.0f)
		{
			float fDegrees = Mathf.Acos(vMoveDirection.x / vMoveDirection.magnitude);
			fDegrees = fDegrees * (180.0f / Mathf.PI);
			vNewRotation.z = 90.0f + fDegrees;
		}
		//Bottom left
		else if (vMoveDirection.x < 0.0f && vMoveDirection.y < 0.0f)
		{
			float fDegrees = Mathf.Acos(Mathf.Abs (vMoveDirection.y) / vMoveDirection.magnitude);
			fDegrees = fDegrees * (180.0f / Mathf.PI);
			vNewRotation.z = 180.0f + fDegrees;
		}
		//Top left
		else if (vMoveDirection.x < 0.0f && vMoveDirection.y >= 0.0f)
		{
			float fDegrees = Mathf.Acos(Mathf.Abs(vMoveDirection.x) / vMoveDirection.magnitude);
			fDegrees = fDegrees * (180.0f / Mathf.PI);
			vNewRotation.z = 270.0f + fDegrees;
		}
		
		this.gameObject.transform.localEulerAngles = vNewRotation;
	}
	
	public void TriggerDeath()
	{
		m_bIsSquished = true;
		
		this.gameObject.renderer.material =  m_matFlySquished;
				
		m_nFrameCount = 0;
	}
}
