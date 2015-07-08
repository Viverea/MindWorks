using UnityEngine;
using System.Collections;

public class AvianCounterKeypadColorScript : MonoBehaviour 
{
	Color m_cInitialColor;
	Color m_cSelectedColor;
	bool  m_bHasBeenSelected = false;
	float m_fTimer = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
		/*
		//Change to floats
		m_cInitialColor.r = (float) 22/256;
		m_cInitialColor.g = (float) 160/256;
		m_cInitialColor.b = (float) 134/256;
		m_cInitialColor.a = (float) 0/256;
		
		m_cSelectedColor.r = (float) 255/256;
		m_cSelectedColor.g = (float) 255/256;
		m_cSelectedColor.b = (float) 255/256;
		*/
		
		m_cInitialColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		m_cSelectedColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bHasBeenSelected)
		{
			m_fTimer += Time.deltaTime;
			
			this.gameObject.renderer.material.color = Color.Lerp (m_cSelectedColor, m_cInitialColor, (m_fTimer / 1.0f));
			
			if(m_fTimer >= 1.0f)
			{
				m_bHasBeenSelected = false;
				m_fTimer = 0.0f;
			}
		}
	}

	public void SetSelected(bool _bIsSelected)
	{
		m_bHasBeenSelected = _bIsSelected;
		
		//Change color
		this.gameObject.renderer.material.color = m_cSelectedColor;
	}
}
