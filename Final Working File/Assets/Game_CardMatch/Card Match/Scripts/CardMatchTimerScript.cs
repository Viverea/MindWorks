using UnityEngine;
using System.Collections;

public class CardMatchTimerScript : MonoBehaviour
{
	public GameObject m_3dtTimerText;
	
	bool m_bStartTimer = false;
	
	float m_fTime = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bStartTimer)
		{
			m_fTime -= Time.deltaTime;
			
			m_3dtTimerText.GetComponent<TextMesh>().text = ((int)m_fTime + 1).ToString("F0");
			
			if(m_fTime <= 0.0f)
			{
				m_3dtTimerText.GetComponent<TextMesh>().text = "GO!";
			}
		}
	
	}
	
	public void SetTime(float _fTime)
	{
		m_fTime = _fTime;
	}
	public void DisplayTimer()
	{
		m_3dtTimerText.renderer.enabled = true;
	}
	
	public void HideTimer()
	{
		m_3dtTimerText.renderer.enabled = false;
	}
	
	public void StartTimer()
	{
		m_bStartTimer = true;
	}
	
	public void StopTimer()
	{
		m_bStartTimer = false;
	}
	
	public bool HasEndedTime()
	{
		if(m_fTime < -1.0f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

