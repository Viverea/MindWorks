using UnityEngine;
using System.Collections;

public class GSP_Tile : MonoBehaviour
{
	public int		m_nIndex;
	
	public bool		m_bActive;
	public Vector3	m_vPosition;
	
	public IEnumerator MoveTo(Vector3 _vEmptySpace, float _fTime)
	{
		float fTime = 0f;
		Vector3 vOldPosition = m_vPosition;
		Vector3 vDeltaPosition = _vEmptySpace - vOldPosition;
		
		m_bActive = false;
		m_vPosition = _vEmptySpace;
		_vEmptySpace = transform.localPosition;
		
		while ( transform.localPosition != m_vPosition )
		{
			if ( fTime > _fTime ) fTime = _fTime;
			transform.localPosition = vOldPosition + vDeltaPosition * fTime / _fTime;
			yield return new WaitForFixedUpdate();
			fTime += Time.deltaTime;
		}
		
		m_bActive = true;
	}
	
	// 0.5s move
	public IEnumerator MoveTo(Vector3 _vEmptySpace)
	{
		float fTime = 0f;
		Vector3 vOldPosition = m_vPosition;
		Vector3 vDeltaPosition = _vEmptySpace - vOldPosition;
		
		m_bActive = false;
		m_vPosition = _vEmptySpace;
		_vEmptySpace = transform.localPosition;
		
		while ( transform.localPosition != m_vPosition )
		{
			if ( fTime > 1.0f ) fTime = 1.0f;
			transform.localPosition = vOldPosition + vDeltaPosition * fTime;
			yield return new WaitForFixedUpdate();
			fTime += Time.deltaTime * 2;
		}
		
		m_bActive = true;
	}
	
	public void OnMouseUpAsButton()
	{
		if ( m_bActive )
		{
			SendMessageUpwards("RequestMove", this);
		}
	}
}