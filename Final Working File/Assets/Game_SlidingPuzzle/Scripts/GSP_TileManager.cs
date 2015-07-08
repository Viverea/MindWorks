using UnityEngine;
using System.Collections;

public class GSP_TileManager : MonoBehaviour
{
	public bool			m_bScramble;
	public bool			m_bTextureAspectRatio	= true;
	public float		m_fWidthSizeLimit		= 72.0f;
	
	public bool			m_bActive;
	public int 			m_nDifficulty;
	
	public GameObject	m_oFullImage;
	public GSP_Tile[]	m_aoSubImages;
	
	public int[]		m_nWidth;
	public int[]		m_nHeight;
	public Vector2[]	m_vClipping;
	
	public Texture2D[]	m_aTextures;
	
	public void RequestMove(GSP_Tile _oRequester)
	{
		if ( m_bActive )
		{
			if ( LegalMove(_oRequester.transform.localPosition) )
			{
				int nTemp = m_vUnusedIndex;
				m_vUnusedIndex = _oRequester.m_nIndex;
				_oRequester.m_nIndex = nTemp;
				
				Vector3 vTemp = m_vEmptySpace;
				m_vEmptySpace = _oRequester.transform.localPosition;
				_oRequester.StartCoroutine(_oRequester.MoveTo(vTemp));
				
				if ( PuzzleComplete() )
				{
					m_bActive = false;
				}
			}
		}
	}
	
	void Awake()
	{
		m_nTotalTiles = m_nWidth[m_nDifficulty] * m_nHeight[m_nDifficulty] - 1;
		if ( m_nTotalTiles == 0 ) m_nTotalTiles = 1;
		
		GameObject goTemp = m_aoSubImages[0].gameObject;
		m_aoSubImages = new GSP_Tile[m_nTotalTiles];
		m_aoSubImages[0] = goTemp.GetComponent<GSP_Tile>();
		
		for( int n = 1; n < m_nTotalTiles; n++ )
		{
			m_aoSubImages[n] = (GameObject.Instantiate(goTemp) as GameObject).GetComponent<GSP_Tile>();
			m_aoSubImages[n].transform.parent = m_aoSubImages[0].transform.parent;
			
			m_aoSubImages[n].transform.rotation = m_aoSubImages[0].transform.rotation;
		}
		
		ResetTexture();
	}
	
	void Start()
	{
		// Move our initial first tile to the top left corner of the full image, incase it isn't there already
		m_vOffset 	 = m_oFullImage.transform.localPosition;
		m_vOffset.x -= ( m_oFullImage.transform.localScale.x / 2 );
		m_vOffset.y += ( m_oFullImage.transform.localScale.y / 2 );
		m_vOffset.z  = -1;
		m_aoSubImages[0].transform.localPosition = m_vOffset;
		
		// Presetting our tile to duplicate and tessallating variables
		float fTotalClippedWidth = m_vClipping[m_nDifficulty].x * (m_nWidth[m_nDifficulty]-1);
		float fTotalClippedHeight = m_vClipping[m_nDifficulty].y * (m_nHeight[m_nDifficulty]-1);
		
		Vector3 vScale = m_oFullImage.transform.localScale;
		vScale.x -= fTotalClippedWidth;
		vScale.y -= fTotalClippedHeight;
		vScale.x /= m_nWidth[m_nDifficulty];
		vScale.y /= m_nHeight[m_nDifficulty];
		m_aoSubImages[0].transform.localScale = vScale;
		
		m_vOffset = vScale;
		m_vOffset.z = 0;
		m_aoSubImages[0].transform.position -= m_vOffset / 2;
		m_vOffset.x += m_vClipping[m_nDifficulty].x;
		m_vOffset.y += m_vClipping[m_nDifficulty].y;
		
		// UV precalculations and variable setting
		Mesh oTemp = m_aoSubImages[0].GetComponent<MeshFilter>().mesh;
        Vector2[] vUV = new Vector2[oTemp.vertices.Length];
		Vector2[] vUVPosition = new Vector2[vUV.Length];
		Vector2 vUVEnd = m_oFullImage.transform.localScale;
		Vector2 vUVStart = m_oFullImage.transform.localPosition - m_oFullImage.transform.localScale / 2;
		
		// Since our tiles are of the same size and shape, we calculate the vertex offsets only once
		for ( int nVertex = 0; nVertex < oTemp.vertices.Length; nVertex++ )
		{
			vUVPosition[nVertex].x = oTemp.vertices[nVertex].x * m_aoSubImages[0].transform.localScale.x;
			vUVPosition[nVertex].y = oTemp.vertices[nVertex].y * m_aoSubImages[0].transform.localScale.y;
		}
		
		// Create and tessellate our remaining tiles, and set our UVs
		for ( int x = 0, y = 0, n = 0; n < m_nTotalTiles; x++, n++ )
		{
			if ( x >= m_nWidth[m_nDifficulty] ) { ++y; x = 0; }
			
			Vector3 vPosition = m_aoSubImages[0].transform.localPosition;
			vPosition.x += m_vOffset.x * x;
			vPosition.y -= m_vOffset.y * y;
			vPosition.z  = -1;
			
			m_aoSubImages[n].transform.localScale = vScale;
			m_aoSubImages[n].transform.localPosition = vPosition;
			m_aoSubImages[n].m_vPosition = m_aoSubImages[n].transform.localPosition;
			m_aoSubImages[n].m_nIndex = n;
			m_aoSubImages[n].m_bActive = true;
			
			// Adjust our tile UVs
			oTemp = m_aoSubImages[n].GetComponent<MeshFilter>().mesh;
			for ( int nVertex = 0; nVertex < oTemp.vertices.Length; nVertex++ )
			{
				Vector2 vVertexUV;
				vVertexUV.x = oTemp.vertices[nVertex].x * m_aoSubImages[n].transform.localScale.x;
				vVertexUV.y = oTemp.vertices[nVertex].y * m_aoSubImages[n].transform.localScale.y;
				vVertexUV	= vUVPosition[nVertex] + (Vector2)m_aoSubImages[n].transform.localPosition;
				
				vUV[nVertex] = vVertexUV - vUVStart;
				
				vUV[nVertex].x /= vUVEnd.x;
				vUV[nVertex].y /= vUVEnd.y;
			}
			oTemp.uv = vUV;
		}
		// Set our "empty space" to move to
		m_vEmptySpace = m_aoSubImages[0].transform.localPosition;
		m_vEmptySpace.x += m_vOffset.x * (m_nWidth[m_nDifficulty] - 1);
		m_vEmptySpace.y -= m_vOffset.y * (m_nHeight[m_nDifficulty] - 1);
		m_vEmptySpace.z = -1;
		m_vUnusedIndex = m_nTotalTiles;
		
		// Scramble our existing tiles for the puzzle if scramble is activated
		if ( m_bScramble ) Scramble ();
		
		// Change offset to it's absolute value for legal move checking incase it isn't already
		m_vOffset.x = Mathf.Abs(m_vOffset.x);
		m_vOffset.y = Mathf.Abs(m_vOffset.y);
		
		// Hide the completed image
		m_oFullImage.gameObject.SetActive(false);
	}
	
	// Private
	private int			m_nTotalTiles;
	private int			m_vUnusedIndex;
	private Vector3		m_vEmptySpace;
	private Vector3		m_vOffset;
	
	private bool PuzzleComplete()
	{
		int n = 0;
		
		while ( n < m_aoSubImages.Length )
		{
			if ( m_aoSubImages[n].m_nIndex == n++ )
				continue;
			
			return false;
		}
		return true;
	}
	
	private void ResetTexture()
	{
		// Setting our texture
		int nRandom = Random.Range(0, m_aTextures.Length);
		m_oFullImage.renderer.material.mainTexture = m_aTextures[nRandom];
		
		foreach (GSP_Tile SubImage in m_aoSubImages)
			SubImage.renderer.material.mainTexture = m_aTextures[nRandom];
		
		// If we're resizing to the texture's aspect ratio
		if ( m_bTextureAspectRatio )
		{
			float fCurrentRatio = m_oFullImage.transform.localScale.x / m_oFullImage.transform.localScale.y;
			float fAspectRatio = (float)m_aTextures[nRandom].width / (float)m_aTextures[nRandom].height;
			Vector3 vScale = m_oFullImage.transform.localScale;
			vScale.x *= (fAspectRatio / fCurrentRatio);
			
			if ( vScale.x > m_fWidthSizeLimit )
			{
				vScale.x = m_fWidthSizeLimit;
				vScale.y = m_fWidthSizeLimit / (fAspectRatio / fCurrentRatio);
			}
			
			m_oFullImage.transform.localScale = vScale;
		}
	}
	
	private void Scramble()
	{
		int nRandom;
		for( int n = 0; n < m_aoSubImages.Length; n++ )
		{
			nRandom = Random.Range(n, m_aoSubImages.Length);
			
			SwapTiles(n, nRandom);
		}
		
		// Check if the grid is solvable, otherwise change it.
		if ( Inversions () % 2 != 0 )
		{
			int nIndexA = 0, nIndexB = 0;
			bool bAFound = false, bBFound = false;
			
			for ( int n = 0; n < m_nTotalTiles; ++n )
			{
				if ( m_aoSubImages[n].m_nIndex == 0 )
				{
					nIndexA = n;
					bAFound = true;
				}
				else if ( m_aoSubImages[n].m_nIndex == 1 )
				{
					nIndexB = n;
					bBFound = true;
				}
				
				if ( bAFound && bBFound )
					break;
			}
			
			SwapTiles (nIndexA, nIndexB);
		}
	}
	
	private int Inversions()
	{
		int nInversions = 0;
		
		for ( int n = 0; n < m_nTotalTiles; ++n )
		{
			for ( int i = n+1; i < m_nTotalTiles; ++i )
			{
				if ( m_aoSubImages[n].m_nIndex > m_aoSubImages[i].m_nIndex )
					++nInversions;
			}
		}
		
		return nInversions;
	}
	
	private void SwapTiles(int _nIndexA, int _nIndexB)
	{
		int nTemp;
		Vector3 vTemp;
		
		nTemp = m_aoSubImages[_nIndexB].m_nIndex;
		m_aoSubImages[_nIndexB].m_nIndex = m_aoSubImages[_nIndexA].m_nIndex;
		m_aoSubImages[_nIndexA].m_nIndex = nTemp;
		
		vTemp = m_aoSubImages[_nIndexB].m_vPosition;
		m_aoSubImages[_nIndexB].m_vPosition = m_aoSubImages[_nIndexA].m_vPosition;
		m_aoSubImages[_nIndexA].m_vPosition = vTemp;
		
		// == For now ==
		m_aoSubImages[_nIndexA].transform.localPosition = m_aoSubImages[_nIndexA].m_vPosition;
		m_aoSubImages[_nIndexB].transform.localPosition = m_aoSubImages[_nIndexB].m_vPosition;
	}
	
	private bool LegalMove(Vector3 _vPosition)
	{
		Vector3 vTemp = _vPosition - m_vEmptySpace;
		
		if ( Mathf.Abs ((int)vTemp.x) == (int)m_vOffset.x && vTemp.y == 0 )
			return true;
		else if ( Mathf.Abs ((int)vTemp.y) == (int)m_vOffset.y && vTemp.x == 0  )
			return true;
		return false;
	}
}
