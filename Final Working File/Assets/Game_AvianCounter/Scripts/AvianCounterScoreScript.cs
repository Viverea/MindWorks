using UnityEngine;
using System.Collections;

public class AvianCounterScoreScript : MonoBehaviour 
{
	private int m_nScore;
	private float m_fTimer;
	private float m_fDifficultyMultiplier;
	
	private bool m_bHasTimerStarted = false;
	
	public GameObject m_3dtScore;
		
	// Use this for initialization
	void Start () 
	{
		m_nScore = 0;
		m_fTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bHasTimerStarted)
		{
			m_fTimer += Time.deltaTime;
		}
	}
	
	public void StartTimer()
	{
		m_bHasTimerStarted = true;
	}
	
	public void StopTimer()
	{
		m_bHasTimerStarted = false;
	}
	
	public void ResetTimer()
	{
		m_fTimer = 0.0f;
	}
	
	public void CalculateAndDisplayScore()
	{
		if(m_fTimer <= 1.0f)
		{
			m_nScore += (int)( 50 * m_fDifficultyMultiplier);
		}
		else if (m_fTimer > 1.0f && m_fTimer <= 2.0f)
		{
			m_nScore += (int)( 40 * m_fDifficultyMultiplier);
		}
		else if (m_fTimer > 2.0f && m_fTimer <= 3.0f)
		{
			m_nScore += (int)( 30 * m_fDifficultyMultiplier);
		}
		else if (m_fTimer > 3.0f && m_fTimer <= 4.0f)
		{
			m_nScore += (int)( 20 * m_fDifficultyMultiplier);
		}
		else // > 4.0f
		{
			m_nScore += (int)( 10 * m_fDifficultyMultiplier);
		}
		
		m_3dtScore.GetComponent<TextMesh>().text = m_nScore.ToString ();
	}
	
	public void SetDifficulty(bool _bIsLevelOne)
	{
		if(_bIsLevelOne)
		{
			m_fDifficultyMultiplier = 1.0f;
		}
		else
		{
			m_fDifficultyMultiplier = 1.0f;
		}
	}
}
