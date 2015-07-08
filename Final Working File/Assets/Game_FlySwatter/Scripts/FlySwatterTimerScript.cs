using UnityEngine;
using System.Collections;

public class FlySwatterTimerScript : MonoBehaviour 
{
	
	float m_fMaxTime = 0.0f;
	float m_fCurrentTime = 0.0f;
	
	float m_fRoundTime = 0.0f;
		
	public GameObject m_3dtTimerText;
	public GameObject m_3dtTimerHeaderText;
	
	bool m_bDisplayTimer = false;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_fCurrentTime += Time.deltaTime;
		
		if(m_bDisplayTimer)
		{
			m_fRoundTime += Time.deltaTime;
			
			if(m_fCurrentTime >= m_fMaxTime)
			{
				m_fCurrentTime = m_fMaxTime;
			}
			
			if(Mathf.CeilToInt(m_fMaxTime - m_fCurrentTime) < (m_fMaxTime/4.0f))
			{
				//m_3dtTimerText.GetComponent<TextMesh>().color = new Color(227.0f / 256.0f, 0.0f, 0.0f, 1.0f);
			}
			
			m_3dtTimerText.GetComponent<TextMesh>().text = Mathf.CeilToInt(m_fMaxTime - m_fCurrentTime).ToString();
		}
	}
	
	public void SetTime(float _fTime)
	{
		m_fMaxTime = _fTime;
		
		m_fCurrentTime = 0.0f;
	}
	
	public bool HasReachedTime()
	{
		if(m_fCurrentTime >= m_fMaxTime)
		{
			return true;
		}
		
		return false;
	}
	
	public float GetMaxTime()
	{
		return m_fMaxTime;
	}
	
	public float GetRoundTime()
	{
		return m_fRoundTime;	
	}
	
	public float GetCurrentTime()
	{
		return m_fCurrentTime;
	}
	
	public void DisplayTimer()
	{
		m_bDisplayTimer = true;
		
		m_3dtTimerText.renderer.enabled = true;
		m_3dtTimerHeaderText.renderer.enabled = true;
	}
	
	public void StopTimer()
	{
		m_bDisplayTimer = false;
	}
	
	public void HideTimer()
	{
		m_bDisplayTimer = false;
		
		m_3dtTimerText.renderer.enabled = false;
		m_3dtTimerHeaderText.renderer.enabled = false;
	}
	
	public void ResetTimer()
	{
		m_fRoundTime = 0.0f;
		
		m_3dtTimerText.GetComponent<TextMesh>().color = Color.white;
	}
}
