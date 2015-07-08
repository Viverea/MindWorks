using UnityEngine;
using System.Collections;

public class FlySwatterFlyScript : MonoBehaviour 
{
	public bool m_bIsCharacter = false;
	
	private bool m_bCanFly = false;
	private bool m_bIsSquished = false;
	
	private float m_fFlightTimer = 0.0f;
	private float m_fFlightTimeThreshold = 0.0f;
	private Vector3 m_vFlightDirection;
	private float m_fFlightVelocity = 1.0f;
	
	private char m_cCharValue;
	private int m_nIntValue;
	
	private float m_fXMin;
	private float m_fXMax;
	private float m_fYMin;
	private float m_fYMax;
	
	public GameObject m_goWingsPrefab;
	public GameObject m_goWings;
	
	// Use this for initialization
	void Start () 
	{
		m_fXMin = GameObject.Find("SceneManager").GetComponent<FlySwatterGlobalVarScript>().m_fXMin;
		m_fXMax = GameObject.Find("SceneManager").GetComponent<FlySwatterGlobalVarScript>().m_fXMax;
		m_fYMin = GameObject.Find("SceneManager").GetComponent<FlySwatterGlobalVarScript>().m_fYMin;
		m_fYMax = GameObject.Find("SceneManager").GetComponent<FlySwatterGlobalVarScript>().m_fYMax;
		
		if(m_bIsCharacter)
		{
			m_cCharValue = this.gameObject.GetComponent<TextMesh>().text[0];
		}
		else
		{		
			m_nIntValue = int.Parse(this.gameObject.GetComponent<TextMesh>().text);
		}
		
		m_vFlightDirection = Vector3.zero;
		RandomiseDirection();
		
		m_goWings = (GameObject) Instantiate(m_goWingsPrefab);
		m_goWings.transform.parent = this.transform;
	
		SetFacingDirection();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bCanFly && !m_bIsSquished)
		{
			MoveFly ();
		}
			
		if(CheckOutOfBounds())
		{
			Redirect();
		}
		
		SetWingsScale();
	}
	
	public char GetCharValue()
	{
		return m_cCharValue;
	}
	
	public int GetIntValue()
	{
		return m_nIntValue;
	}
	
	public bool GetIsCharacter()
	{
		return m_bIsCharacter;
	}
	
	public void StartFlying()
	{
		m_bCanFly = true;
		
		RandomiseDirection();
		RandomiseFlightThreshold();
	}
	
	public void StopFlying()
	{
		m_bCanFly = false;
	}
	
	public void SetVelocity(float _fVelocity)
	{
		m_fFlightVelocity = _fVelocity;
	}

	void MoveFly()
	{	
		if(m_fFlightTimer < m_fFlightTimeThreshold)
		{
			transform.Translate(m_vFlightDirection * m_fFlightVelocity);
		}
		else
		{
			m_fFlightTimer = 0.0f;
			RandomiseFlightThreshold();
			RandomiseDirection();
			SetFacingDirection();
		}
		
		m_fFlightTimer += Time.deltaTime;
		
		//Collision set in colliderOnTrigger scripts
	}
	
	void RandomiseFlightThreshold()
	{
		float fTime = Random.Range(1, 20);
		m_fFlightTimeThreshold = fTime / 10.0f;
	}
	
	void RandomiseDirection()
	{
		float fRandX = Random.Range(-100, 100);
		float fRandY = Random.Range(-100, 100);
		
		Vector3 vDirection = new Vector3(fRandX, fRandY, 0.0f);
		vDirection.Normalize();
		
		m_vFlightDirection = vDirection;
	}
	
	void SetFacingDirection()
	{
		this.gameObject.transform.GetChild (0).GetComponent<FlySwatterWingsMovementScript>().ChangeFacingDirection(m_vFlightDirection);
	}
	
	public void Redirect()
	{	
		Vector3 vWingsPos = m_goWings.transform.position;

		Vector3 vWingsBottomLeftPos = vWingsPos - new Vector3(m_goWings.transform.localScale.x/1.25f, m_goWings.transform.localScale.y/1.25f, 0.0f);
		Vector3 vWingsTopRightPos = vWingsPos + new Vector3(m_goWings.transform.localScale.x/1.25f, m_goWings.transform.localScale.y/1.25f, 0.0f);
		
		float fWingsXMin = vWingsBottomLeftPos.x;
		float fWingsYMin = vWingsBottomLeftPos.y;
		float fWingsXMax = vWingsTopRightPos.x;
		float fWingsYMax = vWingsTopRightPos.y;
		
		float fRandX = 0.0f;
		float fRandY = 0.0f;
		
		if(fWingsXMin < m_fXMin)		//Hits left edge
		{
			//Redirect rightwards
			fRandX = Random.Range(0, 100);
			fRandY = Random.Range(-100, 100);
		}
		else if (fWingsXMax > m_fXMax  )	 //Hits right edge
		{
			//Redirect leftwards
			fRandX = Random.Range(-100, 0);
			fRandY = Random.Range(-100, 100);
		}
		else if (fWingsYMin < m_fYMin ) //Hits bottom edge
		{
			//Redirect upwards
			fRandX = Random.Range(-100, 100);
			fRandY = Random.Range(0, 100);
		}
		else if (fWingsYMax > m_fYMax)//Hits top edge
		{
			//Redirect leftwards
			fRandX = Random.Range(-100, 100);
			fRandY = Random.Range(-100, 0);
		}
		
		Vector3 vDirection = new Vector3(fRandX, fRandY, 0.0f);
		vDirection.Normalize();
		
		m_vFlightDirection = vDirection;
		m_fFlightTimer = 0.0f;
	
		RandomiseFlightThreshold();
		SetFacingDirection();
		
	}
	
	bool CheckOutOfBounds()
	{		
		//Check wing collision against X/Y Max/Min
		
		Vector3 vWingsPos = m_goWings.transform.position;
		Vector3 vWingsBottomLeftPos = vWingsPos - new Vector3(m_goWings.transform.localScale.x/1.25f, m_goWings.transform.localScale.y/1.25f, 0.0f);
		Vector3 vWingsTopRightPos = vWingsPos + new Vector3(m_goWings.transform.localScale.x/1.25f, m_goWings.transform.localScale.y/1.25f, 0.0f);
		
		float fWingsXMin = vWingsBottomLeftPos.x;
		float fWingsYMin = vWingsBottomLeftPos.y;
		float fWingsXMax = vWingsTopRightPos.x;
		float fWingsYMax = vWingsTopRightPos.y;
	
		if( fWingsXMin < m_fXMin ||
			fWingsXMax > m_fXMax ||
			fWingsYMin < m_fYMin ||
			fWingsYMax > m_fYMax)
		{
			return true;
		}
		else
		{
			return false;
		}
		
	}
	
	void OnDrawGizmos()
	{
		Vector3 vWingsPos = m_goWings.transform.position;
		Vector3 vWingsBottomLeftPos = vWingsPos - new Vector3(m_goWings.transform.localScale.x/2, m_goWings.transform.localScale.y/2, 0.0f);
		Vector3 vWingsTopRightPos = vWingsPos + new Vector3(m_goWings.transform.localScale.x/2, m_goWings.transform.localScale.y/2, 0.0f);
		
		Gizmos.color = Color.red;
		Gizmos.DrawLine(vWingsBottomLeftPos, vWingsTopRightPos);
		
	}
	
	void SetWingsScale()
	{
		m_goWings.transform.localScale = this.gameObject.transform.localScale * 17.5f;
		Vector3 vWingsInitialScale = m_goWings.transform.localScale;
		vWingsInitialScale.z = 2.0f;
		m_goWings.transform.localScale = vWingsInitialScale;
	
	}
	
	public void TriggerDeath()
	{
		this.gameObject.GetComponent<TextMesh>().text = "";
		
		m_bIsSquished = true;
		
		this.gameObject.transform.GetChild(0).GetComponent<FlySwatterWingsMovementScript>().TriggerDeath();
	}
	
	public bool GetIsSquished()
	{
		return m_bIsSquished;
	}
}
