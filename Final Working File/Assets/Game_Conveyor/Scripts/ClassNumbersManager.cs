﻿using UnityEngine;
using System.Collections;

public class ClassNumbersManager : MonoBehaviour
{
	public	ClassNumbers	m_oNumber;
	public	GameObject		m_oCorrect;
	public	GameObject		m_oWrong;
	
	public	int				m_nRemaining		= 15;
	public	float			m_fTimeLimit		= 60.0f;
	public	int[]			m_anDivisors;
	
	public	int				m_nMaxNumbers		= 12;
	public	float			m_fDelay			= 1.0f;
	public	ClassBoxes[]	m_oBoxes;
	
	public	int				m_nMultiplierLimit	= 2;
	public	int				m_nBaseNumberETC	= 7;

	private bool			m_bStarted;
	[SerializeField]
	private int[]			m_nDivisors;

	private TextMesh		m_oRemaining;
	private TextMesh		m_oTimer;
	private TextMesh		m_oCountdown;
	
	public void Start()
	{
		m_bStarted 		= true;
		m_oRemaining.text = m_nRemaining.ToString();
		
		RandomiseDivisors();
		StartCoroutine(Countdown());
	}
	
	public void Wrong()
	{
		StopCoroutine("WrongAns");
		StartCoroutine(WrongAns(0.5f));
	}
	public void Correct()
	{
		if ( --m_nRemaining == 0 )
		{
			DeactivateNumbers();
			StopAllCoroutines();
			StartCoroutine(GameClear(1.0f));
		}
		else
		{
			StopCoroutine("CorrectAns");
			StartCoroutine(CorrectAns(0.5f));
		}
		m_oRemaining.text = m_nRemaining.ToString();
	}
	
	void Awake ()
	{
		m_bStarted							= false;
		if(!m_oCorrect)		m_oCorrect		= GameObject.Find("Plane_Correct");
		if(!m_oWrong) 		m_oWrong		= GameObject.Find("Plane_Wrong");
		m_oRemaining						= transform.FindChild("Remaining").GetComponent<TextMesh>();
		m_oTimer							= transform.FindChild("Time").GetComponent<TextMesh>();
		m_oCountdown 						= GameObject.Find ("Countdown").GetComponent<TextMesh>();
		
		GameObject goTemp;
		ClassNumbers oTemp = m_oNumber;
		for ( int n = 0; n < m_nMaxNumbers-1; ++n )
		{
			goTemp						= GameObject.Instantiate(oTemp.gameObject) as GameObject;
			goTemp.transform.parent		= this.transform;
			goTemp.transform.localScale = Vector3.one;
			oTemp.m_oNext				= goTemp.GetComponent<ClassNumbers>();
			oTemp.m_oNext.Initialize();
			oTemp						= oTemp.m_oNext;
		}
		
		if ( m_oBoxes.Length == 0 )
		{
			GameObject[] agoTemp	= GameObject.FindGameObjectsWithTag("Puzzle");
			m_oBoxes				= new ClassBoxes[agoTemp.Length];
			
			for(int n = 0; n < agoTemp.Length; ++n)
			{
				m_oBoxes[n] = agoTemp[n].GetComponent<ClassBoxes>();
			}
		}
		
		m_nDivisors	= new int[m_oBoxes.Length];
	}
	
	void Update	()
	{
		if ( m_bStarted )
		{
			m_fTimeLimit -= Time.deltaTime;
			if ( m_fTimeLimit <= 0.05f )
			{
				m_fTimeLimit = 0f;
				DeactivateNumbers();
				StopAllCoroutines();
				StartCoroutine(GameLost(1.0f));
			}
			
			m_oTimer.text = m_fTimeLimit.ToString("00");
		}
	}
	
	private void DeactivateNumbers ()
	{
		ClassNumbers oTemp = m_oNumber;
		while ( oTemp != null )
		{
			oTemp.enabled = false;
			oTemp = oTemp.m_oNext;
		}
	}
	
	private void RandomiseDivisors()
	{
		int i1 = 0, i2 = 0, nTemp;
		for ( int n = 0; n < m_oBoxes.Length; ++n )
		{
			if ( m_oBoxes[n].m_nSolution < 1 )
				m_oBoxes[n].nDivisor = 0;
			else
			{
				i2 = Random.Range(i1, m_anDivisors.Length);
				nTemp = m_anDivisors[i1];
				m_anDivisors[i1] = m_anDivisors[i2];
				m_anDivisors[i2] = nTemp;
				
				m_oBoxes[n].m_nSolution = n;
				m_oBoxes[n].nDivisor = m_anDivisors[i1];
				++i1;
				++i2;
			}
			
			m_nDivisors[n] = m_oBoxes[n].nDivisor;
		}
	}
	private void GenerateDividend(ClassNumbers _oTarget)
	{
		int nSolution	= Random.Range(0, m_nDivisors.Length);
		int nDividend	= m_nDivisors[nSolution];
		int nMultiplier = Random.Range(1, m_nMultiplierLimit+1);
		
		if ( nDividend > 0 )
		{
			while ( true )
			{
				bool bTemp = true;
				
				for( int n = 0; n < m_nDivisors.Length; ++n )
				{
					if ( m_nDivisors[n] == nDividend ) continue;
					
					if ( nMultiplier == m_nDivisors[n] )
					{
						bTemp = false;
						++nMultiplier;
					}
				}
				
				if ( bTemp )
					break;
			}
			_oTarget.m_nNumber = nDividend * nMultiplier;
		}
		else
		{
			nDividend = Random.Range( 1, m_nBaseNumberETC * nMultiplier );
			while ( true )
			{
				bool bTemp = true;
				
				for( int n = 0; n < m_nDivisors.Length; ++n )
				{
					if ( m_nDivisors[n] < 2 ) continue;
					
					if ( nDividend % m_nDivisors[n] == 0 )
					{
						bTemp = false;
						++nDividend;
					}
				}
				
				if ( bTemp )
					break;
			}
			_oTarget.m_nNumber = nDividend;
		}
		
		Debug.Log( m_nDivisors[nSolution].ToString() + ": " + _oTarget.m_nNumber.ToString() );
		_oTarget.m_nSolution = nSolution;
		_oTarget.UpdateText();
	}
	
	
	IEnumerator Countdown()
	{
		if ( m_oCountdown )
		{
			float fCountdown = 3.0f;
			m_oCountdown.renderer.enabled = true;
			while ( fCountdown > 0 )
			{
				m_oCountdown.text = Mathf.CeilToInt(fCountdown).ToString();
				m_oCountdown.fontSize = 200 + (int)(100 * (fCountdown - (int)fCountdown));
				yield return new WaitForFixedUpdate();
				fCountdown -= Time.deltaTime;
			}
			
			while ( fCountdown > -1 )
			{
				m_oCountdown.text = "GO!";
				m_oCountdown.fontSize = 180 + (int)(30 * fCountdown);
				yield return new WaitForFixedUpdate();
				fCountdown -= Time.deltaTime;
			}
			m_oCountdown.renderer.enabled = false;
		}
		
		StartCoroutine(GenerateNumbers());
	}
	IEnumerator GenerateNumbers()
	{
		while (true)
		{
			ClassNumbers oNext = m_oNumber;
			while( true )
			{
				if( !oNext.enabled )
					break;
				if ( oNext.m_oNext == null )
				{
					yield return new WaitForSeconds(m_fDelay);
					oNext = m_oNumber;
					continue;
				}
				oNext = oNext.m_oNext;
			}
			
			oNext.enabled = true;
			oNext.Initialize();
			GenerateDividend(oNext);
			yield return new WaitForSeconds(m_fDelay);
		}
	}
	
	IEnumerator WrongAns(float _fDelay)
	{
		m_oWrong.renderer.enabled	= true;
		m_oCorrect.renderer.enabled	= false;
		
//		GameObject.Find("Sound_Wrong").audio.Play();
		
		yield return new WaitForSeconds(_fDelay);
		
		m_oWrong.renderer.enabled	= false;
	}
	IEnumerator CorrectAns(float _fDelay)
	{
		m_oWrong.renderer.enabled	= false;
		m_oCorrect.renderer.enabled	= true;
		
//		GameObject.Find("Sound_Correct").audio.Play();
		
		yield return new WaitForSeconds(_fDelay);
		
		m_oCorrect.renderer.enabled	= false;
	}
	IEnumerator GameClear(float _fDelay)
	{
		GameObject m_oClear			= GameObject.Find("Plane_Clear");
		m_oWrong.renderer.enabled	= false;
		m_oCorrect.renderer.enabled	= false;
		m_oClear.renderer.enabled	= true;
		
//		GameObject.Find("Sound_Correct").audio.Play();
		
		yield return new WaitForSeconds(_fDelay);
		
		m_oClear.renderer.enabled	= false;
	}
	IEnumerator GameLost(float _fDelay)
	{
		GameObject m_oLost			= GameObject.Find("Plane_Lost");
		m_oWrong.renderer.enabled	= false;
		m_oCorrect.renderer.enabled	= false;
		m_oLost.renderer.enabled	= true;
		
//		GameObject.Find("Sound_Wrong").audio.Play();
		
		yield return new WaitForSeconds(_fDelay);
		
		m_oLost.renderer.enabled	= false;
	}
}
