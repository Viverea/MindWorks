using UnityEngine;
using System.Collections;

public class DFD_Grid : MonoBehaviour
{
	// Public
	public	int			m_nIndex;
	public	static bool	m_bGridActive	= false;
	
	public IEnumerator Flip(float _fTime)
	{
		Vector3 vRotation = transform.rotation.eulerAngles;
		float fStep = 180.0f / _fTime;
		
		while ( true )
		{
			if ( vRotation.y > 181.0f )
			{
				transform.rotation = Quaternion.Euler(0, 180f, 0);
				if ( m_bActive )
				{
					m_bActive = false;
					DFD_GridManager.m_oInstance.Check();
				}
				break;
			}
			else if ( vRotation.y < -1.0f )
			{
				m_bActive = true;
				transform.rotation = Quaternion.Euler(0, 0, 0);
				break;
			}
			
			yield return new WaitForFixedUpdate();
			
			vRotation.y += fStep * Time.deltaTime;
			
			this.transform.rotation = Quaternion.Euler(vRotation);
		}
	}
	
	public IEnumerator Preview(float _fFlipTime, float _fShowTime)
	{
		yield return StartCoroutine(Flip( _fFlipTime));
		yield return new WaitForSeconds( _fShowTime );
		yield return StartCoroutine(Flip(-_fFlipTime));
		
		m_bGridActive = true;
		DFD_GameManager.m_oInstance.m_bGameActive = true;
		StopAllCoroutines();
	}
	
	public void SetGrid(Texture _texture, int _nIndex)
	{
		m_bActive = false;
		m_nIndex = _nIndex;
		m_goBack.renderer.material.mainTexture = _texture;
	}
	
	// Private
	private bool		m_bActive			= false;
	private Transform	m_goBack;
	
	private void Awake()
	{
		m_goBack = transform.Find("Back");
	}
	
	private void OnMouseUp()
	{
		if ( m_bActive && m_bGridActive )
		{
			m_bGridActive = false;
			DFD_GridManager.m_oInstance.Click(this);
			StartCoroutine( Flip (DFD_GridManager.m_oInstance.m_fFlipDuration) );
		}
	}
}