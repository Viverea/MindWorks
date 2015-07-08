using UnityEngine;
using System.Collections;

// IF THIS CLASS IS IN USE TO SET DIFFICULTY WITHIN THE SAME SCENE,
// RENAME Start() to Begin() in DFD_GameManager

public class DFD_Difficulty : MonoBehaviour
{
	public int m_nDifficulty;
	
	void OnMouseUp()
	{
		// Move camera
		// Vector3 vTemp = Camera.main.transform.position;
		// vTemp.xyz -= 100.0f;
		// Camera.main.transform.position = vTemp;
		
		DFD_GameManager.m_oInstance.m_nDifficulty = m_nDifficulty;
		
		// DFD_GameManager.m_oInstance.Begin();
	}
}
