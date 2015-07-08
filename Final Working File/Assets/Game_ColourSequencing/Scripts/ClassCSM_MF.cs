using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class eFTCGridType 
{
	public const byte 
		eRandomise	= 0,
		eFlood		= 1,
		eGridStatic	= 2,
		eListStatic	= 3,
		eBothStatic	= 4
	;
};

public class ClassCSM_MF : MonoBehaviour
{
// Public
	#region _Public_
	
	public	int				NextIndex {
		get { return m_lIndexAnswers[0]; }
	}
	public	Color			NextColor {
		get { return m_lColorAnswers[0]; }
	}
	public	int				m_nDifficulty		= 1;
	public	int				m_nScore			= 0;
	public	float			m_fTimer			= 30.0f;
	public	float			m_fDisplayTime		= 2.0f;
	public	float			m_fTickTime			= 3.0f;
	public	bool			m_bGameActive		= false;
	public	bool			m_bColorblind		= false;
	public	bool			m_bShowRemaining	= false;
	public	bool			m_bRandColorOrder	= true;
	public	GameObject		m_goWrong;
	public	GameObject		m_goCorrect;
	public	GameObject		m_goDisplay;
	public	TextMesh		m_oIndexDisplay;
	public	TextMesh		m_oScore;
	public	TextMesh		m_oCountdown;
	public	TextMesh		m_oTimer;
	public	byte			m_eType				= eFTCGridType.eRandomise;
	
	public void Next ()
	{
		if ( m_bColorblind )
		{
			m_lIndexAnswers.RemoveAt (0);
			m_nScore += 10;
			m_oScore.text = m_nScore.ToString();
		
			if ( m_lIndexAnswers.Count > 0 )
			{
				m_oIndexDisplay.text = m_lIndexAnswers[0].ToString();
				// Play sound?
				return;
			}
			
			m_nScore += 10;//m_nLength - m_lIndexAnswers.Count;
			m_oIndexDisplay.text = "";
		}
		else
		{
			m_lColorAnswers.RemoveAt (0);
			m_nScore += 10;//m_nLength - m_lColorAnswers.Count;
			m_oScore.text = m_nScore.ToString();
		
			if ( m_lColorAnswers.Count > 0 )
			{
				m_goDisplay.renderer.material.color = m_lColorAnswers[0];
				// Play sound?
				return;
			}
			
			m_goDisplay.renderer.material.color = Color.white;
		}
		
		Correct();
	}
	public void Wrong ()
	{
		if ( m_nLength > 2 )	--m_nLength;
		
		m_lIndexAnswers.Clear();
		m_lColorAnswers.Clear();
		m_oIndexDisplay.text = "";
//		m_goDisplay.renderer.material.color = Color.white;
		
		if ( m_bShowRemaining )
		{
			for ( int n = 0; n < m_oButton.Length; ++n )
			{
				m_oButton[n].bActive = false;
			}
		}
		
		StopAllCoroutines();
		StartCoroutine(Cross (1.0f));
	}
	public void Start()
	{
		if ( m_nDifficulty < 1 ) m_nDifficulty = 1;
		m_nLength = 2 * m_nDifficulty;
		if ( m_nLength > 16 ) m_nLength = 16;
		StartCoroutine(Countdown());
	}
	#endregion
	
// Private
	#region _Private_
	
	private	int					m_nLength;
	private int[]				m_anIndexPool;
	private	Color[]				m_acColorPool;
	private	List<int>			m_lIndexAnswers		= new List<int>();
	private	List<Color>			m_lColorAnswers		= new List<Color>();
	
	private	ClassCSButton_MF[]	m_oButton;
	
	private void Awake()
	{
		if ( !m_goDisplay )		m_goDisplay		= GameObject.Find("Display");
		if ( !m_oIndexDisplay )	m_oIndexDisplay = m_goDisplay.transform.Find("Colorblind").GetComponent<TextMesh>();
		if ( !m_goWrong )		m_goWrong		= GameObject.Find("Plane_Wrong");
		if ( !m_goCorrect )		m_goCorrect		= GameObject.Find("Plane_Correct");
		if ( !m_oCountdown )	m_oCountdown	= GameObject.Find("Countdown").GetComponent<TextMesh>();
		if ( !m_oTimer )		m_oTimer		= GameObject.Find("Timer").GetComponent<TextMesh>();
		if ( !m_oScore )		m_oScore		= GameObject.Find ("Score").GetComponent<TextMesh>();
		
		m_nScore = 0;
		m_goDisplay.renderer.material.color = Color.white;
		
		#region _COLORBLIND_
		if ( m_bColorblind )
		{
			m_oIndexDisplay.renderer.enabled = true;
			m_goDisplay.renderer.enabled = false;
		}
		m_anIndexPool = new int[16];
		for( int n = 0; n < m_anIndexPool.Length; ++n)
			m_anIndexPool[n] = n+1;
		#endregion
		#region _NORMAL_
		m_acColorPool		= new Color[16];
		m_acColorPool[0]	= Color.blue;						// blue
		m_acColorPool[1]	= Color.red;						// red
		m_acColorPool[2]	= Color.green; 						// green
		m_acColorPool[3]	= Color.yellow;						// yellow
		m_acColorPool[4]	= new Color(0.8f, 0.0f, 0.4f, 1f);	// purple
		m_acColorPool[5]	= new Color(1.0f, 0.4f, 1.0f, 1f);	// pink
		m_acColorPool[6]	= Color.black;						// black
		m_acColorPool[7]	= new Color(1.0f, 0.5f, 0.1f, 1f);	// something
		m_acColorPool[8]	= new Color(0.5f, 0.5f, 0.0f, 1f);	// something
		m_acColorPool[9]	= new Color(0.3f, 0.15f, 0.1f, 1f);	// Brown
		m_acColorPool[10]	= new Color(1.0f, 0.8f, 0.6f, 1f);	// something
		m_acColorPool[11]	= Color.grey;						// grey
		m_acColorPool[12]	= new Color(0.0f, 0.9f, 1.0f, 1f); 	// teal
		m_acColorPool[13]	= new Color(0.5f, 0.0f, 0.1f, 1f);	// dark red
		m_acColorPool[14]	= new Color(0.1f, 0.0f, 0.3f, 1f);	// dark blue
		m_acColorPool[15]	= new Color(0.0f, 0.3f, 0.0f, 1f);	// dark green
		#endregion
		
		if ( m_eType == eFTCGridType.eGridStatic || m_eType == eFTCGridType.eBothStatic )
		{
			int r = 0;
			#region _COLORBLIND_
			if ( m_bColorblind)
			{
				for ( int n = 0; n < m_anIndexPool.Length; ++n)
				{
					r = Random.Range(n, m_anIndexPool.Length);
					
					int nTemp = m_anIndexPool[n];
					m_anIndexPool[n] = m_anIndexPool[r];
					m_anIndexPool[r] = nTemp;
				}
			}
			#endregion
			#region _NORMAL_
			else
			{
				for ( int n = 0; n < m_acColorPool.Length; ++n)
				{
					Color cTemp;
					r = Random.Range(n, m_acColorPool.Length);
					
					cTemp = m_acColorPool[n];
					m_acColorPool[n] = m_acColorPool[r];
					m_acColorPool[r] = cTemp;
				}
			}
			#endregion
		}
		
		GameObject goTemp;
		
		m_oButton						= new ClassCSButton_MF[m_acColorPool.Length];
		m_oButton[0]					= GameObject.Find("ButtonPlane").GetComponent<ClassCSButton_MF>();
		m_oButton[0].cColor				= m_acColorPool[0];
		m_oButton[0].nIndex 			= m_anIndexPool[0];
		m_oButton[0].renderer.enabled	= false;
		
		for ( int n = 1, x = 1, y = 0; n < m_acColorPool.Length; ++n )
		{
			goTemp									= GameObject.Instantiate(m_oButton[0].gameObject) as GameObject;
			m_oButton[n]							= goTemp.GetComponent<ClassCSButton_MF>();
			
			m_oButton[n].transform.parent			= m_oButton[0].transform.parent;
			m_oButton[n].transform.localPosition	= new Vector3( -6.0f * x++, -6.0f * y, 0);
			if ( x > 3 ) { x = 0; ++y; }
			
			m_oButton[n].cColor = m_acColorPool[n];
			m_oButton[n].nIndex = m_anIndexPool[n];
		}
		
		if ( m_eType == eFTCGridType.eBothStatic )
		{
			int rPosition = Random.Range(0, m_oButton.Length);
			ClassCSButton_MF oTemp = m_oButton[rPosition];
			m_oButton[rPosition] = m_oButton[0];
			m_oButton[0] = oTemp;
		}
	}
	
	private void Correct()
	{
		if ( m_nLength < m_acColorPool.Length )	++m_nLength;
		
		StopAllCoroutines();
		StartCoroutine(Tick (m_fTickTime));
	}
	
	private void GenerateColors()
	{
		int n = 0, i;
		ClassCSButton_MF oTemp;
		
		if ( m_eType == eFTCGridType.eFlood )
			i = m_oButton.Length;
		else
			i = m_nLength;
		
		if ( m_eType == eFTCGridType.eBothStatic )
		{
			int rPosition = Random.Range(i-1, m_oButton.Length);
			oTemp = m_oButton[rPosition];
			m_oButton[rPosition] = m_oButton[i-1];
			m_oButton[i-1] = oTemp;
		}
		
		while ( n < i )
		{
			int rPosition = Random.Range(n, m_oButton.Length);
			
			if ( rPosition != n && m_eType < eFTCGridType.eBothStatic )
			{
				// Swap position of current array index to the randomised position
				oTemp = m_oButton[rPosition];
				m_oButton[rPosition] = m_oButton[n];
				m_oButton[n] = oTemp;
			}
			
			#region _COLORBLIND_
			if ( m_bColorblind )
			{
				int nTemp;
				int rColor = Random.Range(n, m_anIndexPool.Length);
				if ( rColor != n && m_bRandColorOrder && m_eType < eFTCGridType.eGridStatic )
				{
					// Swap color of current array index to the randomised color
					nTemp = m_anIndexPool[rColor];
					m_anIndexPool[rColor] = m_anIndexPool[n];
					m_anIndexPool[n] = nTemp;
					
				}
				
				m_oButton[n].m_oText.renderer.enabled = true;
				if ( m_eType == eFTCGridType.eFlood )
				{
					if ( n < m_nLength )
					{
						m_lIndexAnswers.Add(m_anIndexPool[n]);
						m_oButton[n].nIndex = m_anIndexPool[n];
					}
					else
						m_oButton[n].nIndex = m_anIndexPool[Random.Range(0, m_nLength)];
				}
				else if ( m_eType == eFTCGridType.eRandomise || m_eType == eFTCGridType.eListStatic )
				{
					m_lIndexAnswers.Add(m_anIndexPool[n]);
					m_oButton[n].nIndex = m_anIndexPool[n];
				}
				else if ( m_eType == eFTCGridType.eGridStatic || m_eType == eFTCGridType.eBothStatic )
				{
					m_lIndexAnswers.Add(m_oButton[n].nIndex);
				}
					
			}
			#endregion
			#region _NORMAL_
			else
			{
				Color cTemp;
				int rColor = Random.Range(n, m_acColorPool.Length);
				if ( rColor != n && m_bRandColorOrder && m_eType < eFTCGridType.eGridStatic )
				{
					// Swap color of current array index to the randomised color
					cTemp = m_acColorPool[rColor];
					m_acColorPool[rColor] = m_acColorPool[n];
					m_acColorPool[n] = cTemp;
					
				}
				
				m_oButton[n].renderer.enabled = true;
				if ( m_eType == eFTCGridType.eFlood )
				{
					if ( n < m_nLength )
					{
						m_lColorAnswers.Add(m_acColorPool[n]);
						m_oButton[n].cColor = m_acColorPool[n];
					}
					else
						m_oButton[n].cColor = m_acColorPool[Random.Range(0, m_nLength)];
				}
				else if ( m_eType == eFTCGridType.eRandomise || m_eType == eFTCGridType.eListStatic )
				{
					m_lColorAnswers.Add(m_acColorPool[n]);
					m_oButton[n].cColor = m_acColorPool[n];
				}
				else if ( m_eType == eFTCGridType.eGridStatic || m_eType == eFTCGridType.eBothStatic )
				{
					m_lColorAnswers.Add(m_oButton[n].cColor);
				}
			}
			#endregion
			++n;
		}
	}
	
	private void DeactivateColors()
	{
		int n = 0;
		while ( n < m_oButton.Length )
		{
			if ( m_bColorblind )
			{
				if ( !m_oButton[n].m_oText.renderer.enabled )
					break;
			}
			else
			{
				if ( !m_oButton[n].renderer.enabled )
					break;
			}
			
			m_oButton[n++].bActive = true;
		}
		if ( m_bColorblind )
		{
			m_oIndexDisplay.text = m_lIndexAnswers[0].ToString();
		}
		else
		{
			m_goDisplay.renderer.material.color = m_lColorAnswers[0];
		}
		
		StartCoroutine("Timer");
	}
	
	private void StartRound ()
	{
		StartCoroutine("Round");
	}
	
	private void EndRound()
	{		
		for ( int n = 0; n < m_oButton.Length; ++n )
		{
			if ( m_bColorblind )
				m_oButton[n].m_oText.renderer.enabled = false;
			else
				m_oButton[n].renderer.enabled = false;
			m_oButton[n].bActive = false;
		}
	}
	
	IEnumerator Timer()
	{
		float fTimeRemaining = m_fTimer;
		while ( fTimeRemaining > 0 )
		{
			m_oTimer.text = Mathf.CeilToInt(fTimeRemaining).ToString("0");
			//m_oTimer.fontSize = 40 + (int)(10 * (fTimeRemaining - (int)fTimeRemaining));
			yield return new WaitForFixedUpdate();
			fTimeRemaining -= Time.deltaTime;
		}
		
		Wrong();
	}
	
	IEnumerator Round()
	{
		m_goDisplay.SetActive(false);
		m_oTimer.text = Mathf.CeilToInt(m_fTimer).ToString("##");
		
		GenerateColors();
		yield return new WaitForSeconds(m_fDisplayTime);
		DeactivateColors();
		m_goDisplay.SetActive(true);
		m_bGameActive = true;
	}
	
	IEnumerator Countdown()
	{
		m_goDisplay.SetActive(false);
		m_oTimer.text = Mathf.CeilToInt(m_fTimer).ToString("##");
		
		if ( m_oCountdown )
		{
			float fCountdown = 3.0f;
			m_oCountdown.renderer.enabled = true;
			while ( fCountdown > 0 )
			{
				m_oCountdown.text = Mathf.CeilToInt(fCountdown).ToString();
				//m_oCountdown.fontSize = 200 + (int)(100 * (fCountdown - (int)fCountdown));
				yield return new WaitForFixedUpdate();
				fCountdown -= Time.deltaTime;
			}
			
			while ( fCountdown > -1 )
			{
				m_oCountdown.text = "GO!";
				//m_oCountdown.fontSize = 180 + (int)(30 * fCountdown);
				yield return new WaitForFixedUpdate();
				fCountdown -= Time.deltaTime;
			}
			m_oCountdown.renderer.enabled = false;
		}
		
		StartRound();
	}
	
	IEnumerator Tick(float _fDelay)
	{
		m_bGameActive					= false;
		m_goCorrect.renderer.enabled	= true;
		
		GameObject.Find("Sound_Correct").audio.Play();
		
		yield return new WaitForSeconds(_fDelay);
		
		m_goCorrect.renderer.enabled	= false;
		EndRound();
		yield return new WaitForEndOfFrame();
		StartRound();
//		StartCoroutine(RestartGame());
	}
	IEnumerator Cross(float _fDelay)
	{
		m_bGameActive 				= false;
		m_goWrong.renderer.enabled	= true;
		
		GameObject.Find("Sound_Wrong").audio.Play();
		
		yield return new WaitForSeconds(_fDelay);
		
		m_goWrong.renderer.enabled	= false;
		EndRound();
		yield return new WaitForEndOfFrame();
		StartRound();
	}
	#endregion
}