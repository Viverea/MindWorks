using UnityEngine;
using System.Collections;

public class DisableStuff : MonoBehaviour
{
	public GameObject[] agoStuffToDisable;
	
	public void SetObjectsActive(bool _bActive)
	{
		foreach ( GameObject go in agoStuffToDisable )
			go.SetActive(_bActive);
	}
}

