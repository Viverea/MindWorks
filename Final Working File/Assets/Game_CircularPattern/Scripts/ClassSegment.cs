using UnityEngine;
using System.Collections;

public class ClassSegment : MonoBehaviour 
{
	public bool m_bMarked = false;
	
	public bool m_bSelected = false;
	
	private Color m_cNotSelected;
	private Color m_cSelected;
	
	// Use this for initialization
	void Start () 
	{
		m_cNotSelected = this.renderer.material.color; //new Color(0.0f, 148.0f, 186.0f, 255.0f);
		m_cSelected = new Color(191.0f, 212.0f, 0.0f, 255.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(this.tag == "Guide")
		{
			if(m_bMarked == true)
			{
				this.renderer.material.color = m_cSelected;
			}
			else
			{
				this.renderer.material.color = m_cNotSelected;
			}
		}
		else
		{
			if(m_bSelected == true)
			{
				this.renderer.material.color = m_cSelected;
				
				//m_bSelected = false;
			}
			else
			{
				this.renderer.material.color = m_cNotSelected;
				
				//m_bSelected = true;
			}
		}
	}
	
	void OnMouseDown ()
	{
		if(m_bSelected == false)
		{
			m_bSelected = true;
			
			//this.renderer.material.color = m_cSelected;
		}
		
		else if(m_bSelected == true)
		{
			m_bSelected = false;
			
			//this.renderer.material.color = m_cNotSelected;
		}
	}
}
