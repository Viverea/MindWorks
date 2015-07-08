using UnityEngine;
using System.Collections;

public class ClassNumber : MonoBehaviour 
{
	private Vector3 m_vSpawnPosition;
	private GameObject m_goNumberManager;
	
	private Vector3 m_vCloudPosition;
	
	public Vector3 m_vMovementDirection;
	
	public bool m_bHasBeenSelected = false;
	public float m_fSpeed = 5.0f;
	
	private bool m_bCheckAnswer = false;

	public int m_nNumber;
	
	// Use this for initialization
	void Start () 
	{
		m_nNumber = Random.Range (1, 10);
		this.GetComponent<TextMesh>().text = m_nNumber.ToString();
		
		m_vSpawnPosition = GameObject.Find ("SpawnPoint").transform.position;
		
		this.transform.localPosition = m_vSpawnPosition;
		
		m_goNumberManager = GameObject.Find ("NumberManager");
		
		m_goNumberManager.GetComponent<ClassNumberManager>().m_agoNumbers.Add(this.gameObject);
		
		m_vCloudPosition = this.transform.position;
		
		float fXDirection = Random.Range(-1.0f, 1.0f);
		float fYDirection = Random.Range(-1.0f, 1.0f);
		
		Vector3 vStartingMovementVector = new Vector3(fXDirection, fYDirection, 0.0f);
		m_vMovementDirection = vStartingMovementVector;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bCheckAnswer == false)
		{
			if(m_bHasBeenSelected == false)
			{
				m_vCloudPosition = (m_vCloudPosition + (m_vMovementDirection * m_fSpeed * (Time.deltaTime / 3)));
				
				this.transform.localPosition = m_vCloudPosition;
			}
		}	
		else
		{
			m_bCheckAnswer = false;
		}
	}
	
	void OnTriggerEnter(Collider _cOther)
	{
		if(_cOther.tag == "AnswerBox")
		{
			_cOther.GetComponent<ClassAnswerBox>().m_nNumber = _cOther.GetComponent<ClassAnswerBox>().m_nNumber - m_nNumber;
			_cOther.GetComponent<ClassAnswerBox>().m_nNumberAnswer = m_nNumber;
			
			Destroy (this.gameObject);
			
			GameObject goNumber = Instantiate(Resources.Load("PrefabNumber")) as GameObject;
			goNumber.transform.parent = GameObject.Find ("NumberArray").transform;
		}
		
		if(_cOther.name == "TopCollider")
		{
			m_vMovementDirection.y = m_vMovementDirection.y * -1;
		}
		else if(_cOther.name == "BottomCollider")
		{
			m_vMovementDirection.y = m_vMovementDirection.y * -1;
		}
		else if(_cOther.name == "LeftCollider")
		{
			m_vMovementDirection.x = m_vMovementDirection.x * -1;
		}
		else if(_cOther.name == "RightCollider")
		{
			m_vMovementDirection.x = m_vMovementDirection.x * -1;
		}
	}
	
	void OnMouseDown()
	{
		m_bHasBeenSelected = true;
		
		Vector3 vMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9.0f));

		this.transform.position = vMousePosition;
	}
	
	void OnMouseDrag()
	{
		Vector3 vMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9.0f));
		
		this.transform.position = vMousePosition;
	}
	
	void OnMouseUp()
	{
		m_bHasBeenSelected = false;
		
		Vector3 vMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9.0f));
		
		this.transform.position = vMousePosition;
		
		m_bCheckAnswer = true;
	}
}
