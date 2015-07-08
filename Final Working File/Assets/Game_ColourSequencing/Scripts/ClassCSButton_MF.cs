using UnityEngine;
using System.Collections;

public class ClassCSButton_MF : MonoBehaviour
{
	public	int			nIndex	{
		get { return m_nIndex; }
		set
		{
			m_nIndex = value;
			m_oText.text = m_nIndex.ToString();
		}
	}
	public	bool		bActive	{
		get { return m_bActive; }
		set {
			m_bActive = value;
			if ( m_bColorblind )
			{
				if ( m_bActive )
					m_oText.text = "?";
				else
					m_oText.text = m_nIndex.ToString("00");
					
			}
			else
			{
				if ( m_bActive )
					this.renderer.material.color = Color.black;
				else
					this.renderer.material.color = m_cColor;
			}
		}
	}
	public	Color		cColor	{
		get { return m_cColor; }
		set
		{
			m_cColor = value;
			this.gameObject.renderer.material.color = m_cColor;
		}
	}
	public	TextMesh	m_oText;
	
	public	static	ClassCSM_MF	m_oManager;
	
	// Private
	private int		m_nIndex;
	private Color	m_cColor;
	private bool	m_bActive;
	private	bool	m_bColorblind;
	
	private void Awake()
	{
		m_oManager	= GameObject.Find("Plane_Menu").GetComponent<ClassCSM_MF>();
		
		m_bColorblind = m_oManager.m_bColorblind;
	}
	
	private void OnMouseDown()
	{
		if ( m_oManager.m_bGameActive && bActive )
		{
			this.renderer.material.color = Color.grey;
		}
	}
	
	private void OnMouseUp()
	{
		if ( m_oManager.m_bGameActive && bActive )
		{
			if ( m_bColorblind )
			{
				if ( m_nIndex == m_oManager.NextIndex )
				{
					m_oText.renderer.enabled = false;
					m_oManager.Next();
					bActive = false;
					return;
				}
			}
			else
			{
				if ( m_cColor == m_oManager.NextColor )
				{
					this.renderer.enabled = false;
					m_oManager.Next();
					bActive = false;
					return;
				}
			}
			
			m_oManager.Wrong();
			bActive = false;
		}
	}
}
