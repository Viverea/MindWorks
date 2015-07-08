using UnityEngine;
using System.Collections;

public class ClassBoxes : MonoBehaviour
{
	public	int			m_nSolution;
	public	int 		nDivisor {
		get { return m_nDivisor; }
		set 
		{
			m_nDivisor = value;
			if ( m_nDivisor > 0 )
				m_oDivisor.text = "÷ " + m_nDivisor;
			else
				m_oDivisor.text = "Etc.";
		}
	}
	private	int			m_nDivisor;
	private TextMesh	m_oDivisor;
	
	void Awake()
	{
		m_oDivisor = transform.FindChild("Number").GetComponent<TextMesh>();
	}
}