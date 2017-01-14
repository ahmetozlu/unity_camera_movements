using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.UI;

/// <summary>
/// Attach this script to an Empty with two children: "MainCamera prefab from VRSampleScenes/Prefabs/Utils" and also an Empty you should call "Body".
/// This script is using the VR Input script from the VR Samples Unity Asset Store (Just drag the MainCamera onto the VR Input property on this script in the Inspector.
/// 
/// Makes your VrFps-player move in the direction/antidirection (??? what :-)) of your gaze by swiping left or right on your Gear VR's touchpad. (Still waiting for my controller from Ebay :-()
/// Stops by swiping down or double click.  Quits the Game by pressing Back button on Gear VR.  
/// 
/// Written by harald.heide.gundersen@hotmail.com on 12/13/2015.
/// Unity Verion 5.3.0f4 personal 
/// Using VR Samples free from Unity Technologies
/// Tested on a Samsung Galaxy S6 in GearVR S321 and Consumer GearVR S322.
/// </summary>

[RequireComponent(typeof (Rigidbody))]
public class MoveCameraForGearVR : MonoBehaviour
{
	[SerializeField]
	private float m_speed = 0.8f;
	[SerializeField]
	private VRInput m_VRInput;

	private Rigidbody m_Rigidbody;
	private Transform head;
	private Transform body;

	private int m_isWalking = 0;
	private Vector3 m_direction;
	public Image panel;
	//public GameObject panelMap;

	[SerializeField] private VRInteractiveItem[] m_InteractiveItemBlock;
	public GameObject panel1;
	public GameObject panel2;
	public GameObject panel3;
	public GameObject panel4;

	GameObject obj1;
	GameObject obj2;
	GameObject obj3;

	// Use this for initialization
	void Start () {
		GameObject panel = GameObject.Find("Panel");
	}
		
	void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		head = Camera.main.transform;
		body = transform.Find("Body");
	}
	private void OnEnable()
	{
		m_VRInput.OnSwipe += HandleSwipe;
		//m_VRInput.OnDown += HandleDown;
		//m_VRInput.OnUp += HandleUp;
		//m_VRInput.OnClick += HandleClick;
		m_VRInput.OnDoubleClick += HandleDoubleClick;
		m_VRInput.OnCancel += HandleCancel;
	}

	private void OnDisable()
	{
		m_VRInput.OnSwipe -= HandleSwipe;
		//m_VRInput.OnDown -= HandleDown;
		//m_VRInput.OnUp -= HandleUp;
		//m_VRInput.OnClick -= HandleClick;
		m_VRInput.OnDoubleClick -= HandleDoubleClick;
		m_VRInput.OnCancel -= HandleCancel;
	}

	private void HandleSwipe(VRInput.SwipeDirection swipeDirection)
	{
		switch (swipeDirection)
		{
		case VRInput.SwipeDirection.NONE:
			break;
		case VRInput.SwipeDirection.UP:
			transform.position = new Vector3(transform.position.x, 75, transform.position.z);
			break;
		case VRInput.SwipeDirection.DOWN:
			transform.position = new Vector3(transform.position.x, 1, transform.position.z);
			break;
		case VRInput.SwipeDirection.LEFT:
			m_isWalking += 1;
			break;
		case VRInput.SwipeDirection.RIGHT:
			m_isWalking += -1;
			break;
		}
	}

	private void HandleDown()
	{
	}
	private void HandleUp()
	{
		
	}
	private void HandleClick()
	{
		
	}

	private void HandleDoubleClick()
	{
		m_isWalking = 0;
		panel.gameObject.SetActive (false);
		//panelMap.SetActive (false);
		m_InteractiveItemBlock[0].gameObject.SetActive (false);
		m_InteractiveItemBlock[1].gameObject.SetActive (false);
		m_InteractiveItemBlock[2].gameObject.SetActive (false);
		m_InteractiveItemBlock[3].gameObject.SetActive (false);
		//m_InteractiveItemBlock[4].gameObject.SetActive (false);
		//m_InteractiveItemBlock[5].gameObject.SetActive (false);
		//m_InteractiveItemBlock[5].gameObject.SetActive (true);

		panel1.SetActive (false);
		panel2.SetActive (false);
		panel3.SetActive (false);
		panel4.SetActive (false);
	}

	private void HandleCancel()
	{
		Application.Quit();

	}

	public void FixedUpdate()
	{
		if (m_isWalking != 0)
		{
			m_direction = head.forward;
			m_direction.y = 0f;
			m_Rigidbody.transform.Translate(m_direction * m_speed * m_isWalking * Time.fixedDeltaTime);
			body.transform.rotation = Quaternion.Euler(new Vector3(0.0f, head.eulerAngles.y, 0.0f));
		}
	}

	public void setm_isWalking() {
		m_isWalking = 0;
	}
}