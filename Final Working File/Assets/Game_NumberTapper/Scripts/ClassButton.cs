using UnityEngine;
using System.Collections;

public class ClassButton : MonoBehaviour 
{
	public GameObject m_goNumber;
	public int nNumber = 0;
	
	public Texture m_tCurrentTexture;
	public Texture m_tStartingTexture;
	public Texture m_tSelectedTexture;
	
	public bool m_bIsSelected = false;
	
	public bool m_bMarkedForSelection = false;

	// Use this for initialization
	void Start () 
	{
		m_tCurrentTexture = Resources.Load ("ButtonUnselected") as Texture;
		m_tStartingTexture = Resources.Load ("ButtonUnselected") as Texture;
		m_tSelectedTexture = Resources.Load ("ButtonSelected") as Texture;
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.renderer.material.mainTexture = m_tCurrentTexture;
		
		int.TryParse(m_goNumber.GetComponent<TextMesh>().text, out nNumber);
		
		ShowAnswer();
	}
	
	void OnMouseDown()
	{
		if(m_bIsSelected == false)
		{
			m_tCurrentTexture = m_tSelectedTexture;
			
			m_bIsSelected = true;
		}
		else
		{
			m_tCurrentTexture = m_tStartingTexture;
			
			m_bIsSelected = false;
		}
	}
	
	public void ShowAnswer()
	{
		if(GameObject.Find ("Wrong").GetComponent<MeshRenderer>().enabled == true)
		{
			if(gameObject.GetComponent<ClassButton>().nNumber == GameObject.Find("GameManager").GetComponent<NumberTapperGameManager>().m_nCaseNumber)
			{
				m_tCurrentTexture = m_tSelectedTexture;
			}
			else
			{
				m_tCurrentTexture = m_tStartingTexture;
			}
		}
	}
}
