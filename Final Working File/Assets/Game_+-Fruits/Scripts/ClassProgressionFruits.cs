using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClassProgressionFruits : MonoBehaviour 
{
	public int nCorrectStreak = 0;
	public int m_nScoreMultiplier = 1;
	public int m_nScorePreviousMultiplier = 1;
	
	public int m_nScore = 0;

	public List<GameObject> m_agoProgressBarNotches = new List<GameObject>();
	
	public float m_fTimeRemaining = 10.0f;

	// Use this for initialization
	void Start () 
	{
		m_agoProgressBarNotches = GameObject.Find ("BarManager").GetComponent<ClassProgressBarManager>().m_agoProgressBar;
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_nScoreMultiplier = 1;
		//GameObject.Find ("Multiplier").GetComponent<TextMesh>().text = "x 1";
		
		GameObject.Find ("Score").GetComponent<TextMesh>().text = m_nScore.ToString();
		
		for (int i = 0; i < m_agoProgressBarNotches.Count; i++)
		{
			m_agoProgressBarNotches[i].GetComponent<MeshRenderer>().enabled = false;
		}
		
		if(nCorrectStreak > 0)
		{
			int nProgressBarArrayIndex = nCorrectStreak - 1;
			
			for (int i = 0; i < m_agoProgressBarNotches.Count; i++)
			{
				if(i <= nProgressBarArrayIndex)
				{
					m_agoProgressBarNotches[i].GetComponent<MeshRenderer>().enabled = true;
				}
			}
			
			if(nCorrectStreak > 5 && nCorrectStreak < 11)
			{
				//GameObject.Find ("Multiplier").GetComponent<TextMesh>().text = "x 2";
				
				m_nScoreMultiplier = 2;
			}
			else if(nCorrectStreak > 10 && nCorrectStreak < 16)
			{
				//GameObject.Find ("Multiplier").GetComponent<TextMesh>().text = "x 3";
				
				m_nScoreMultiplier = 3;
			}
			else if(nCorrectStreak > 15)
			{
				//GameObject.Find ("Multiplier").GetComponent<TextMesh>().text = "x 4";
				
				m_nScoreMultiplier = 4;
			}
		}
		
		if(m_nScoreMultiplier > m_nScorePreviousMultiplier)
		{
			StartCoroutine(Fireworks(1.0f));
			
			//StartCoroutine(Fireworks(1.0f));
		}
		else if(m_nScoreMultiplier < m_nScorePreviousMultiplier)
		{
			StartCoroutine(Explosion(1.0f));
		}
		
		m_nScorePreviousMultiplier = m_nScoreMultiplier;
	}
	
	IEnumerator Fireworks(float _fFireworksTime)
	{
		GameObject.Find ("MultiplierFireworks").GetComponent<ParticleEmitter>().emit = true;
		
		yield return new WaitForSeconds(_fFireworksTime);
		
		GameObject.Find ("MultiplierFireworks").GetComponent<ParticleEmitter>().emit = false;
		
		yield return null;
	}
	
	IEnumerator Explosion(float _fExplosionTime)
	{
		GameObject.Find ("MultiplierExplosion").GetComponent<ParticleEmitter>().emit = true;
		
		yield return new WaitForSeconds(_fExplosionTime);
		
		GameObject.Find ("MultiplierExplosion").GetComponent<ParticleEmitter>().emit = false;
		
		yield return null;
	}
	
}
