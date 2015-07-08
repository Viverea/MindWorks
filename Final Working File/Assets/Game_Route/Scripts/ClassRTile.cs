using UnityEngine;
using System.Collections;

public class ClassRTile : MonoBehaviour
{
	public	bool	m_bClickable;
	
	public	Vector2	m_vPosition;
	public	Vector2	m_vDirection;
	
	public void OnMouseUpAsButton()
	{
		if ( m_bClickable )
		{
			gameObject.SendMessageUpwards("AnswerSelect", m_vPosition, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public IEnumerator Appear(float _fDuration)
	{
		float fTime = 0f;
		float fRatio;
		Vector3 vScale = transform.localScale;
		
		while ( fTime < _fDuration )
		{
			fTime += Time.deltaTime;
			
			if ( fTime >= _fDuration )
				fRatio = 1f;
			else
				fRatio = fTime / _fDuration;
			
			transform.localScale = vScale * fRatio;
			yield return new WaitForEndOfFrame();
		}
	}
}