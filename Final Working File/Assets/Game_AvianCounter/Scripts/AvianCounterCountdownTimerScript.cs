using UnityEngine;
using System.Collections;

public class AvianCounterCountdownTimerScript : MonoBehaviour 
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
	
	//Make use of existing countdown text to render question mark
	public void DisplayQuestionMark()
	{
		m_3dtTimerText.GetComponent<TextMesh>().text = "?";
		m_3dtTimerText.GetComponent<TextMesh>().color = Color.white;
		m_3dtTimerText.transform.localScale = new Vector3(2.0f,2.0f,2.0f);
		m_3dtTimerText.transform.localPosition = new Vector3(-20.0f,-8.0f,-20.0f);
		m_3dtTimerText.renderer.enabled = true;
	}
	
	// Used to display answer
	public void DisplayNumber(int _nNumber)
	{
		m_3dtTimerText.GetComponent<TextMesh>().text = _nNumber.ToString();
		m_3dtTimerText.GetComponent<TextMesh>().color = Color.white;
//		m_3dtTimerText.transform.localScale = new Vector3(2.0f,2.0f,2.0f);
		m_3dtTimerText.transform.localPosition = new Vector3(20.0f,-8.0f,-20.0f);
		m_3dtTimerText.renderer.enabled = true;
	}
	
	public void ReturnSizeToNormal()
	{
		m_3dtTimerText.GetComponent<TextMesh>().color = new Color(0.5882f, 0f, 0.1294f, 1f);
		m_3dtTimerText.transform.localScale = new Vector3(3.0f,3.0f,3.0f);
		m_3dtTimerText.transform.localPosition = new Vector3(28.0f,-8.0f,-20.0f);
	}
}