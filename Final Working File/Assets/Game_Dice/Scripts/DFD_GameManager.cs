using UnityEngine;
using System.Collections;

public class DFD_GameManager : MonoBehaviour
{
	// Public
	public	int			m_nDifficulty		= 0; // 0 = EASY, 1 = HARD.
	public	int			m_nScore			= 0;
	public	float		m_fTimer			= 45.0f;
	public	float		m_fTimeBonus		= 5.0f;
	public	float		m_fFlipPreviewTime	= 1.0f;
	
	// In case we do not want to pause the timer when flipping upon click
	public	bool		m_bGameActive		= false;
	
	public	Vector3[]	m_vGridPosition;
	public	int[]		m_nGridLength;
	public	int[]		m_nGridHeight;
	public	float[]		m_fGridLength;
	public	float[]		m_fGridHeight;
	
	public	GameObject	m_goWrong;
	public	GameObject	m_goCorrect;
	public	TextMesh	m_oTimer;
	public	TextMesh	m_oCountdown;
	public	TextMesh	m_oFlip;
	public	TextMesh	m_oScore;
	public	TextMesh	m_oPreview;
	
	public static DFD_GameManager m_oInstance;
	
	public void Start()
	{
		DFD_GridManager.m_oInstance.m_goGrid.transform.position = m_vGridPosition[m_nDifficulty];
		
		DFD_GridManager.m_oInstance.GenerateGrid(
												m_nGridLength[m_nDifficulty],
												m_nGridHeight[m_nDifficulty],
												m_fGridLength[m_nDifficulty],
												m_fGridHeight[m_nDifficulty]
												);
		DFD_GridManager.m_oInstance.m_nLength = 2 + m_nDifficulty;
		
		m_oFlip.text	= DFD_GridManager.m_oInstance.m_nLength.ToString();
		m_oPreview.text = m_oFlip.text;
		
		StartCoroutine(Timer ());
		
		StartCoroutine(Countdown());
	}
	
	public void Correct()
	{
		m_bGameActive 	 = false;
		m_fTimer 		+= m_fTimeBonus;
		m_nScore		+= 10;//DFD_GridManager.m_oInstance.m_nLength * DFD_GridManager.m_oInstance.m_nLength;
		m_oScore.text	 = m_nScore.ToString();
		
		if ( m_goCorrect )
		{
			StartCoroutine(Tick(1.0f));
		}
		else
		{
			ResetGame();
		}
	}
	
	// Protected
	void Awake()
	{
		if ( !m_goWrong ) 	m_goWrong	= GameObject.Find("Plane_Wrong");
		if ( !m_goCorrect )	m_goCorrect	= GameObject.Find("Plane_Correct");
		if ( !m_oPreview )	m_oPreview	= GameObject.Find("PreviewFlip").GetComponent<TextMesh>();
		if ( !m_oScore )	m_oScore	= GameObject.Find("Score").GetComponent<TextMesh>();
		
		if ( !m_oInstance ) m_oInstance = this;
		else Debug.DebugBreak(); // There should not be more than one of this in the scene!
		
		m_oPreview.gameObject.SetActive(false);
	}
	
	void OnDestroy()
	{
		m_oInstance = null;
	}
	
	void ResetGame()
	{
		++DFD_GridManager.m_oInstance.m_nLength;
		DFD_GridManager.m_oInstance.Reset();
		m_oFlip.text = DFD_GridManager.m_oInstance.m_nLength.ToString();
		m_oPreview.text = m_oFlip.text;
		
		StartCoroutine(Countdown());
	}
	
	IEnumerator Countdown()
	{
		DFD_GridManager.m_oInstance.SetGridActive(false);
		
		// Preview the number of cards to flip
		m_oPreview.gameObject.SetActive(true);
		yield return new WaitForSeconds(m_fFlipPreviewTime);
		m_oPreview.gameObject.SetActive(false);
		
		// Start the actual countdown
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
				//m_oCountdown.fontSize = 190 + (int)(30 * fCountdown);
				yield return new WaitForFixedUpdate();
				fCountdown -= Time.deltaTime;
			}
			m_oCountdown.renderer.enabled = false;
		}
		
		DFD_GridManager.m_oInstance.SetGridActive(true);
		DFD_GridManager.m_oInstance.PreviewGrid();
	//	StartCoroutine(Timer());
	}
	
	IEnumerator Timer()
	{
		while ( m_fTimer > 0.0f )
		{
			yield return new WaitForFixedUpdate();
			
			if ( DFD_Grid.m_bGridActive ) // if ( m_bGameActive )
				m_fTimer -= Time.deltaTime;
			
			m_oTimer.text = m_fTimer.ToString("0");
		}
		
		if ( m_goWrong )
			yield return StartCoroutine( Cross (1.0f) );
		else
			DFD_Grid.m_bGridActive		= false;
			
	}
	
	IEnumerator Tick(float _fDelay)
	{
		m_bGameActive					= false;
		DFD_Grid.m_bGridActive			= false;
		m_goWrong.renderer.enabled		= false;
		m_goCorrect.renderer.enabled	= true;
		GameObject.Find("Sound_Correct").audio.Play();
		
		yield return new WaitForSeconds(_fDelay);
		
		m_goCorrect.renderer.enabled	= false;
		ResetGame();
	}
	IEnumerator Cross(float _fDelay)
	{
		m_bGameActive					= false;
		DFD_Grid.m_bGridActive			= false;
		m_goWrong.renderer.enabled		= true;
		m_goCorrect.renderer.enabled	= false;
		GameObject.Find("Sound_Wrong").audio.Play();
		DFD_GridManager.m_oInstance.ShowGrid();
		
		yield return new WaitForSeconds(_fDelay);
		
		m_goWrong.renderer.enabled	= false;
		
		yield return new WaitForSeconds(5.0f);
		Application.LoadLevel(Application.loadedLevelName);
	}
}
