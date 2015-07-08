using UnityEngine;
using System.Collections;

public class gameTimer : MonoBehaviour 
{
	
	private float m_fTime = 5.0f;
	private bool m_bStart = false;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bStart)
		{
			m_fTime -= Time.deltaTime;
			
			//If timer started, update timer at sidebar too
			//+ prevent number from going below 0
			if(Mathf.Ceil(m_fTime) >= 0)
			{
				GameObject.Find("Time Counter").GetComponent<TextMesh>().text = Mathf.Ceil(m_fTime).ToString();
			}
		}
	}
	
	public float GetTime()
	{
		return m_fTime;
	}
	
	public void SetTime(float _fTime)
	{
		m_fTime = _fTime;
	}
	
	public void StartTimer()
	{
		m_bStart = true;
	}
	
	public void StopTimer()
	{
		m_bStart = false;
	}
	
	public bool HasReachedTimeLimit()
	{
		if(m_fTime <= 0.0f)
		{
			return true;
		}
		
		return false;
	}
}
