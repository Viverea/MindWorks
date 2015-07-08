using UnityEngine;
using System.Collections;

public class ClassNumbers : MonoBehaviour
{
	public int		m_nNumber;
	public int		m_nSolution;
	public float	m_fSpeed				= 10.0f;
	public float	m_fTravelDistance		= 50.0f;
	
	public ClassNumbers m_oNext;
	public TextMesh		m_oText;
	
	private	bool	m_bHeld					= false;
	private bool	m_bDelay				= false;
	private Vector3 m_vCurrentPosition;
	private Vector3 m_vConveyorPosition;
	private ClassNumbersManager m_oManager;
	
	public void Initialize()
	{
		transform.localPosition = Vector3.zero;
		m_vConveyorPosition = transform.position;
	}
	
	public void UpdateText()
	{
		m_oText.text = m_nNumber.ToString();
	}
	
	void Awake()
	{
		m_oManager		= GameObject.Find ("NumbersManager").GetComponent<ClassNumbersManager>();
		m_oText			= GetComponent<TextMesh>();
		Initialize();
		this.enabled = false;
	}
	
	void FixedUpdate()
	{
		m_vConveyorPosition.x += m_fSpeed * Time.deltaTime;
		
		if ( !m_bHeld )
		{
			// Wait one frame if we have just let go of the mouse
			if ( m_bDelay )
			{
				m_bDelay = false;
				return;
			}
			transform.position = m_vConveyorPosition;
		}
		
		if ( transform.localPosition.x > m_fTravelDistance )
		{
			this.enabled = false;
		}
	}
	
	void OnMouseDown()
	{
		m_bHeld = true;
		Vector3 vTemp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		vTemp.z = 100.0f;
		transform.position = vTemp;
	}
	
	void OnMouseDrag()
	{
		Vector3 vTemp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		vTemp.z = 100.0f;
		transform.position = vTemp;
	}
	
	void OnMouseUp()
	{
		m_bDelay	= true;
		m_bHeld		= false;
		
		Vector3 vTemp = transform.position;
		vTemp.z = 90.0f;
		transform.position = vTemp;
	}
	
	void OnTriggerEnter(Collider _oOther)
	{
		if ( m_nSolution == _oOther.gameObject.GetComponent<ClassBoxes>().m_nSolution )
		{
			m_oManager.Correct();
			Initialize();
			this.enabled = false;
		}
		else
		{
			m_oManager.Wrong();
		}
	}
}