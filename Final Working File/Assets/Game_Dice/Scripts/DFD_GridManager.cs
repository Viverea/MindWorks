using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DFD_GridManager : MonoBehaviour
{
	// Public
	public	int			m_nLength;
	public	float		m_fFlipDuration		= 0.5f;
	public	float		m_fPreviewDuration	= 2.0f;
	public	GameObject	m_goGrid;
	public	Texture[]	m_GridTextures;
	
	public	static	DFD_GridManager m_oInstance;
	
	void Awake()
	{
		if ( !m_oInstance )	m_oInstance = this;
		else Debug.DebugBreak(); // There should not be more than one of this in the scene!
		
		m_lCheck = new List<DFD_Grid>();
		m_lSelection = new List<DFD_Grid>();
		
		m_anTextureIndex = new int[m_GridTextures.Length];
		for ( int i = 0; i < m_anTextureIndex.Length; ++i )
			m_anTextureIndex[i] = i;
	}
	
	void OnDestroy()
	{
		DFD_Grid.m_bGridActive = false;
		
		m_oInstance = null;
	}
	
	public void SetGridActive(bool _bActive)
	{
		for ( int x = 0; x < m_oGrid.GetLength(0); ++x )
		{
			for ( int y = 0; y < m_oGrid.GetLength(1); ++y )
			{
				m_oGrid[x,y].gameObject.SetActive(_bActive);
			}
		}
	}
	
	public void GenerateGrid( int _nLength, int _nHeight, float _fLengthDelta, float _fHeightDelta )
	{
		m_oGrid = new DFD_Grid[_nLength, _nHeight];
		
		if ( m_goGrid.activeSelf )
		{
			for ( int x = 0; x < _nLength; ++x )
			{
				for ( int y = 0; y < _nHeight; ++y )
				{
					GameObject goTemp = GameObject.Instantiate(m_goGrid) as GameObject;
					
					goTemp.transform.parent = this.transform;
					goTemp.transform.position = m_goGrid.transform.position;
					goTemp.transform.position += new Vector3(x * _fLengthDelta, y * _fHeightDelta, 0);
					
					m_oGrid[x,y] = goTemp.GetComponent<DFD_Grid>();
				}
			}
			RandomizeGrid();
			m_goGrid.SetActive(false);
		}
	}
	
	public void PreviewGrid()
	{
		for ( int x = 0; x < m_oGrid.GetLength(0); ++x )
		{
			for ( int y = 0; y < m_oGrid.GetLength(1); ++y )
			{
				m_oGrid[x,y].StartCoroutine(m_oGrid[x,y].Preview (m_fFlipDuration, m_fPreviewDuration));
			}
		}
	}
	
	public void ShowGrid()
	{
		for ( int x = 0; x < m_oGrid.GetLength(0); ++x )
		{
			for ( int y = 0; y < m_oGrid.GetLength(1); ++y )
			{
				m_oGrid[x,y].StartCoroutine(m_oGrid[x,y].Flip (m_fFlipDuration));
			}
		}
	}
	
	public void Click(DFD_Grid m_oClicked)
	{
		m_lSelection.Add ( m_oClicked );
		m_lCheck.Add( m_oClicked );
		
		if ( m_lSelection.Count > m_nLength )
		{
			m_lSelection[0].StartCoroutine( m_lSelection[0].Flip(-m_fFlipDuration) );
			m_lCheck.Remove( m_lSelection[0] );
			m_lSelection.RemoveAt(0);
		}
	}
	
	public void Check()
	{
		if ( m_lSelection.Count == m_nLength )
		{
			if ( WinCondition() )
			{
				DFD_GameManager.m_oInstance.Correct();
				return;
			}
			else if ( DFD_GameManager.m_oInstance.m_nScore > 0 )
			{
				--DFD_GameManager.m_oInstance.m_nScore;
				DFD_GameManager.m_oInstance.m_oScore.text = DFD_GameManager.m_oInstance.m_nScore.ToString();
			}
		}
		
		DFD_Grid.m_bGridActive						= true;
		DFD_GameManager.m_oInstance.m_bGameActive	= true;
	}
	
	public void Reset()
	{
		if ( m_nLength > m_oGrid.Length - 1 )
			m_nLength = m_oGrid.Length - 1;
		
		Quaternion qReset = Quaternion.Euler(0,0,0);
		while ( m_lSelection.Count > 0 )
		{
			m_lSelection[0].transform.rotation = qReset;
			m_lSelection.RemoveAt(0);
		}
		m_lCheck.Clear();
		
		RandomizeGrid();
	}
	
	// Private
	private DFD_Grid[,]		m_oGrid;
	private List<DFD_Grid>	m_lCheck;
	private List<DFD_Grid>	m_lSelection;
	private int[]			m_anTextureIndex;
	
	private bool WinCondition()
	{
		m_lCheck.Sort ( (x,y) => x.m_nIndex.CompareTo(y.m_nIndex) );
		for ( int n = m_lCheck.Count - 1; n > 0; --n )
		{
			if ( m_lCheck[n].m_nIndex - m_lCheck[n-1].m_nIndex == 1 )
				continue;
			
			return false;
		}
		
		return true;
	}
	
	private void RandomizeGrid()
	{
		int i = 0, nRandom;
		int temp;
		
		for ( int x = 0; x < m_oGrid.GetLength(0); ++x )
		{
			for ( int y = 0; y < m_oGrid.GetLength(1); ++y )
			{
				nRandom = Random.Range(i, m_oGrid.Length);
				
				temp = m_anTextureIndex[nRandom];
				m_anTextureIndex[nRandom] = m_anTextureIndex[i];
				m_anTextureIndex[i] = temp;
				
				m_oGrid[x,y].SetGrid( m_GridTextures[m_anTextureIndex[i]], m_anTextureIndex[i] );
				
				if ( m_GridTextures.Length == ++i )
					i = 0;
			}
		}
	}
}
