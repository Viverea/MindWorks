using UnityEngine;
using System.Collections;

public class ClassRManager : MonoBehaviour
{
	// public
	public	int			m_nWidthLimit;
	public	int			m_nHeightLimit;
	public	int			m_nStartWidth;
	public	int			m_nStartHeight;
	public	float		m_fMemoryTime;
	public	float		m_fGameTime;
	public	float		m_fRoundTime;
	public	float		m_fTileAnimationDuration;
	public	float		m_fTraceDelay;
	public	float		m_fRestartDelay;
	public	float 		m_fFeedbackTime				= 1f;
	
	public TextMesh		m_oCountdown;
	public TextMesh		m_oTimer;
	public TextMesh		m_oRoundTimer;
	public GameObject	m_goWrong;
	public GameObject	m_goCorrect;
	
	public Texture[]	m_aTileTexture;
	
	public void Awake()
	{
		m_fCurrentTime		= m_fGameTime;
		m_nCurrentWidth		= m_nStartWidth;
		m_nCurrentHeight	= m_nStartHeight;
		m_aoGrid			= new ClassRTile[m_nWidthLimit,m_nHeightLimit];
		
		m_aoGrid[0,0]		= transform.Find("GameSize").GetComponent<ClassRTile>();
		m_vGameSize			= m_aoGrid[0,0].transform.localScale;
		m_aoGrid[0,0]		.gameObject.SetActive(false);
		{
			GameObject goTemp;
			int x = 0;
			int y = 1;
			while ( x < m_nWidthLimit )
			{
				if ( y < m_nHeightLimit )
				{
					goTemp = GameObject.Instantiate(m_aoGrid[0,0].gameObject) as GameObject;
					m_aoGrid[x,y] = goTemp.GetComponent<ClassRTile>();
					m_aoGrid[x,y].transform.parent			= transform;
					m_aoGrid[x,y].transform.localRotation	= m_aoGrid[0,0].transform.localRotation;
					m_aoGrid[x,y].transform.localPosition	= m_aoGrid[0,0].transform.localPosition;
					
					++y;
				}
				else
				{
					y = 0;
					++x;
				}
			}
		}
		
		StartCoroutine(StartLevel());
	}
	
	public void AnswerSelect(Vector2 _vArrayIndex)
	{
		int y = m_nCurrentHeight - 1;
		for ( int x = 0; x < m_nCurrentWidth; ++x )
		{
			m_aoGrid[x,y].m_bClickable = false;
			m_aoGrid[x,y].gameObject.SetActive(false);
		}
		m_aoGrid[(int)_vArrayIndex.x,(int)_vArrayIndex.y].gameObject.SetActive(true);
		
		StopCoroutine("Timer");
		StopCoroutine("RoundTimer");
		StartCoroutine(TraceRoute(_vArrayIndex));
	}
	
	// private
	private int 	m_nShowTile;
	private	int 	m_nCurrentWidth;
	private	int		m_nCurrentHeight;
	private	float	m_fCurrentTime;
	
	private Vector3 m_vGameSize;
	private Vector3 m_vTileSize;
	private Vector2 m_vTrim;
	private Vector2 m_vTessalatingOffset;
	private ClassRTile[,] m_aoGrid;
	
	private IEnumerator StartLevel()
	{
		yield return new WaitForSeconds(0.1f);
		yield return StartCoroutine(Countdown());
		
		ResetTileSize();
		GenerateGrid();
	}
	
	private void LevelProgress()
	{
		++m_nCurrentWidth;
		if ( m_nCurrentWidth == m_nWidthLimit )
		{
			m_nCurrentWidth = m_nStartWidth;
			++m_nCurrentHeight;
			if ( m_nCurrentHeight == m_nWidthLimit )
			{
				// END GAME
				
				// TEMPORARY RESTART GAME
				m_nCurrentWidth		= m_nStartWidth;
				m_nCurrentHeight	= m_nStartHeight;
			}
		}
	}
	
	private void GenerateGrid()
	{
		for( int x = 0; x < m_nCurrentWidth; ++x )
		{
			for( int y = 0; y < m_nCurrentHeight; ++y )
			{
				Vector3 vPosition	= Vector3.zero;
				vPosition.x			= m_vTrim.x + m_vTileSize.x * 0.5f + m_vTileSize.x * x;
				vPosition.y			= m_vTrim.y + m_vTileSize.y * 0.5f + m_vTileSize.y * y;
				vPosition.x			= vPosition.x * -1;
				
				m_aoGrid[x,y].transform.localPosition	= vPosition;
				m_aoGrid[x,y].transform.localScale		= m_vTileSize;
				m_aoGrid[x,y].gameObject.SetActive(true);
				
				if ( y == m_nCurrentHeight-1 )
				{
					m_aoGrid[x,y].m_vDirection	= Vector2.zero;
					m_aoGrid[x,y].m_vPosition.x	= x;
					m_aoGrid[x,y].m_vPosition.y	= y;
					
					m_aoGrid[x,y].renderer.material.mainTexture = m_aTileTexture[3];
				}
				else
				{
					int nRandom = RandomTexture(x, y);
					
					switch (nRandom)
					{
					case 1:
						m_aoGrid[x,y].m_vDirection	= new Vector2( 0, 1);
						break;
					case 0:
						m_aoGrid[x,y].m_vDirection	= new Vector2(-1, 1);
						break;
					case 2:
						m_aoGrid[x,y].m_vDirection	= new Vector2( 1, 1);
						break;
					}
					
					m_aoGrid[x,y].m_vPosition.x	= x;
					m_aoGrid[x,y].m_vPosition.y	= y;
					
					m_aoGrid[x,y].renderer.material.mainTexture = m_aTileTexture[nRandom];
					
					m_aoGrid[x,y].m_bClickable	= false;
				}
			}
		}
		Invoke("HideGrid", m_fMemoryTime);
	}
	
	private void HideGrid()
	{
		m_nShowTile = Random.Range(0, m_nCurrentWidth);
		for( int x = 0; x < m_nCurrentWidth; ++x )
		{
			for( int y = 0; y < m_nCurrentHeight - 1; ++y )
			{
				if ( y == 0 && x == m_nShowTile )
					continue;
				
				m_aoGrid[x,y].gameObject.SetActive(false);
			}
		}
		
		{
			int y = m_nCurrentHeight - 1;
			for ( int x = 0; x < m_nCurrentWidth; ++x )
			{
				m_aoGrid[x,y].m_bClickable = true;
			}
		}
			
		StartCoroutine("Timer");
		StartCoroutine("RoundTimer");
	}
	
	private IEnumerator TraceRoute(Vector2 _vUserAnswer)
	{
		Vector2 vTarget = new Vector2(m_nShowTile, 0);
		
		while ( vTarget.y != m_nCurrentHeight - 1 )
		{
			vTarget += m_aoGrid[(int)vTarget.x,(int)vTarget.y].m_vDirection;
			
			if ( !m_aoGrid[(int)vTarget.x,(int)vTarget.y].gameObject.activeSelf )
			{
				m_aoGrid[(int)vTarget.x,(int)vTarget.y].gameObject.SetActive(true);
				m_aoGrid[(int)vTarget.x,(int)vTarget.y].StartCoroutine(
				m_aoGrid[(int)vTarget.x,(int)vTarget.y].Appear(m_fTileAnimationDuration)
				);
			}
			
			yield return new WaitForSeconds(m_fTraceDelay);
		}
		
		if ( vTarget == _vUserAnswer )
		{
			// Correct
			LevelProgress();
			StartCoroutine(Tick(m_fFeedbackTime));
		}
		else
		{
			// Wrong
			StartCoroutine(Cross(m_fFeedbackTime));
		}
		
		yield return new WaitForSeconds(m_fRestartDelay);
		
		vTarget = new Vector2(m_nShowTile, 0);
		while ( vTarget.y != m_nCurrentHeight )
		{
			m_aoGrid[(int)vTarget.x,(int)vTarget.y].gameObject.SetActive(false);
			
			vTarget.x += m_aoGrid[(int)vTarget.x,(int)vTarget.y].m_vDirection.x;
			++vTarget.y;
		}
		m_aoGrid[(int)_vUserAnswer.x,(int)_vUserAnswer.y].gameObject.SetActive(false);
		
		StartCoroutine(StartLevel());
	}
	
	private void ResetTileSize()
	{
		m_vTileSize = m_vGameSize;
		m_vTileSize.x /= m_nCurrentWidth;
		m_vTileSize.y /= m_nCurrentHeight;
		
		if ( m_vTileSize.y > m_vTileSize.x )
			m_vTileSize.y = m_vTileSize.x;
		else if ( m_vTileSize.x > m_vTileSize.y )
			m_vTileSize.x = m_vTileSize.y;
		
		m_vTrim.x = ( m_vGameSize.x - ( m_vTileSize.x * m_nCurrentWidth  )) * 0.5f;
		m_vTrim.y = ( m_vGameSize.y - ( m_vTileSize.y * m_nCurrentHeight )) * 0.5f;
	}
	
	private int RandomTexture(int _nGridX, int _nGridY)
	{
		int nRangeMin = 0, nRangeMax = 3;
		if ( _nGridX == 0 )
			nRangeMin = 1;
		if ( _nGridX == m_nCurrentWidth - 1 )
			nRangeMax = 2;
		
		return Random.Range(nRangeMin, nRangeMax);
	}
	
	private IEnumerator Countdown()
	{
		float fCountdown				= 3.0f;
		m_oCountdown.renderer.enabled	= true;
		while ( fCountdown > 0 )
		{
			m_oCountdown.text = Mathf.CeilToInt(fCountdown).ToString();
			yield return new WaitForFixedUpdate();
			fCountdown -= Time.deltaTime;
		}
			
		while ( fCountdown > -1 )
		{
			m_oCountdown.text = "GO!";
			//m_oCountdown.fontSize = 190 + (int)(30 * fCountdown);
			yield return new WaitForFixedUpdate();
			fCountdown -= Time.deltaTime;
		}
		m_oCountdown.renderer.enabled = false;
	}
	
	private IEnumerator Timer()
	{
		while (m_fCurrentTime > 0f)
		{
			m_fCurrentTime	-= Time.deltaTime; 
			m_oTimer.text	 = m_fCurrentTime.ToString("##");
			yield return new WaitForFixedUpdate();
		}
		
		// I give up
		Application.LoadLevel(Application.loadedLevelName);
	}
	private IEnumerator RoundTimer()
	{
		float fTime = m_fRoundTime;
		while (fTime > 0f)
		{
			fTime				-= Time.deltaTime;
			m_oRoundTimer.text	 = fTime.ToString("##");
			yield return new WaitForFixedUpdate();
		}
		
		int y = m_nCurrentHeight - 1;
		for ( int x = 0; x < m_nCurrentWidth; ++x )
		{
			m_aoGrid[x,y].m_bClickable = false;
			m_aoGrid[x,y].gameObject.SetActive(false);
		}
		
		StopCoroutine("Timer");
		StopCoroutine("RoundTimer");
		StartCoroutine(TraceRoute(new Vector2(0, 0)));
	}
	
	private IEnumerator Tick(float _fDelay)
	{
		m_goWrong.renderer.enabled		= false;
		m_goCorrect.renderer.enabled	= true;
		GameObject.Find("Sound_Correct").audio.Play();
		
		yield return new WaitForSeconds(_fDelay);
		
		m_goCorrect.renderer.enabled	= false;
	}
	private IEnumerator Cross(float _fDelay)
	{
		m_goWrong.renderer.enabled		= true;
		m_goCorrect.renderer.enabled	= false;
		GameObject.Find("Sound_Wrong").audio.Play();
		
		yield return new WaitForSeconds(_fDelay);
		
		m_goWrong.renderer.enabled	= false;
	}
}