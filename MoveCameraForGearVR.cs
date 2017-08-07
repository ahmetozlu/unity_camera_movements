/*
----------------------------------------------
--- Author         : Ahmet Özlü
--- Mail           : ahmetozlu93@gmail.com
--- Date           : 1st August 2017
----------------------------------------------
*/
using UnityEngine;
using VRStandardAssets.Utils;

[RequireComponent(typeof (Rigidbody))]
public class OVR_RB_Player : MonoBehaviour
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

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        head = Camera.main.transform;
        body = transform.Find("Body");
    }
	
    private void OnEnable()
    {
        m_VRInput.OnSwipe += HandleSwipe;
        m_VRInput.OnDoubleClick += HandleDoubleClick;
        m_VRInput.OnCancel += HandleCancel;
    }

    private void OnDisable()
    {
        m_VRInput.OnSwipe -= HandleSwipe;
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
                m_isWalking += 2;
                break;
            case VRInput.SwipeDirection.RIGHT:
                m_isWalking += -2;
                break;
        }
    }

    private void HandleDown()
    {
    }
    private void HandleUp()
    {
    }
    private void HandleClick() // Quits the Game by pressing Back button on Gear VR
    {
    }

    private void HandleDoubleClick() // Stops by swiping down or double click.
    {
        m_isWalking = 0;
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
}