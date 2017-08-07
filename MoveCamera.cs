/*
----------------------------------------------
--- Author         : Ahmet Özlü
--- Mail           : ahmetozlu93@gmail.com
--- Date           : 1st August 2017
----------------------------------------------
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

enum MouseButtonDown
{
    MBD_LEFT = 0,
    MBD_RIGHT,
    MBD_MIDDLE,
};

[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject focusObj = null;
    private Vector3 oldPos;
    public int zoomfactor = 10;
    private Vector3 initFocusPosition; // focusObj
    private Quaternion initFocusRotation;
    private Vector3 initCamPosition;
    private Quaternion initCamRotation;
    private int horizontal = 0, vertical = 0;
    private int flyFactor = 0;
    private float translation = 0.0f;

    // public variables --- Camera Position Text, Camera Speed and Camera Rotation Speed
    public Text text; // Text is for main camera object position on scene
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    
    void setupFocusObject(string name)
    {
        GameObject obj = this.focusObj = new GameObject(name);
        obj.transform.position = Vector3.zero;

        return;
    }

    void Start()
    {
        if (this.focusObj == null)
            this.setupFocusObject("CameraFocusObject");

        Transform trans = this.transform;
        transform.parent = this.focusObj.transform;

        trans.LookAt(this.focusObj.transform.position);

        // store init param
        storeInitCamera();

        return;
    }

    void Update()
    {
        Vector3 playerPos = GameObject.Find("Main Camera").transform.position;
        text.text = playerPos.ToString("G4");
        GetComponent<Camera>().fieldOfView += Input.GetAxis("Mouse ScrollWheel") * zoomfactor; //ZOOMIN/OUT with Mouse scroll

        if (Input.GetKey("escape"))
            Application.Quit();        

        float translation = vertical * speed;
        float rotation = horizontal * rotationSpeed;

        horizontal = 0;
        vertical = 0;

        if (Mathf.Abs(translation - 0.0f) < 0.00001f)
        {          
            translation = Input.GetAxis("Vertical") * speed;
        }

        if (Mathf.Abs(rotation - 0.0f) < 0.00001f)
        {
            rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        }

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        float translationY = flyFactor * speed * Time.deltaTime;
        transform.Translate(0, translationY, translation);
        flyFactor = 0;
        transform.Rotate(0, rotation, 0);

        this.mouseEvent();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.restoreInitCamera();
        }

        return;
    }

    void storeInitCamera()
    {
        initFocusPosition = this.focusObj.transform.position;
        initFocusRotation = this.focusObj.transform.rotation;
        initCamPosition = transform.position;
        initCamRotation = transform.rotation;
    }

    void restoreInitCamera()
    {
        this.focusObj.transform.position = initFocusPosition;
        this.focusObj.transform.rotation = initFocusRotation;
        transform.position = initCamPosition;
        transform.rotation = initCamRotation;
    }

    void mouseEvent()
    {
        float delta = Input.GetAxis("Mouse ScrollWheel");
        if (delta != 0.0f)
            this.mouseWheelEvent(delta);

        if (Input.GetMouseButtonDown((int)MouseButtonDown.MBD_LEFT) ||
           Input.GetMouseButtonDown((int)MouseButtonDown.MBD_MIDDLE) ||
           Input.GetMouseButtonDown((int)MouseButtonDown.MBD_RIGHT))
            this.oldPos = Input.mousePosition;

        this.mouseDragEvent(Input.mousePosition);

        return;
    }

    void mouseWheelEvent(float delta)
    {

        Vector3 focusToPosition = this.transform.position - this.focusObj.transform.position;

        Vector3 post = focusToPosition * (1.0f + delta * -1.0f);

        if (post.magnitude > 0.01f)
            this.transform.position = this.focusObj.transform.position + post;

        return;
    }


    void mouseDragEvent(Vector3 mousePos)
    {
        Vector3 diff = mousePos - oldPos;

        if (diff.magnitude < Vector3.kEpsilon)
            return;

        if (Input.GetMouseButton((int)MouseButtonDown.MBD_LEFT))
        {
            this.cameraRotate(new Vector3(diff.y, diff.x, 0.0f));
        }
        else if (Input.GetMouseButton((int)MouseButtonDown.MBD_MIDDLE))
        {
            this.cameraTranslate(-diff / 65.0f);
        }
        else if (Input.GetMouseButton((int)MouseButtonDown.MBD_RIGHT))
        {
            Vector3 focusToPosition = this.transform.position - this.focusObj.transform.position;
            Vector3 post = focusToPosition * (1.0f + diff.magnitude / 100 * ((diff.x >= 0) ? -1.0f : 1.0f));
            if (post.magnitude > 0.01f)
                this.transform.position = this.focusObj.transform.position + post;
        }

        this.oldPos = mousePos;

        return;
    }

    void cameraTranslate(Vector3 vec)
    {
        Transform focusTrans = this.focusObj.transform;
        Transform trans = this.transform;

        focusTrans.Translate((trans.right * vec.x) + (trans.up * vec.y));

        return;
    }

    public void cameraRotate(Vector3 eulerAngle)
    {
        Vector3 focusPos = this.focusObj.transform.position;
        Transform trans = this.transform;

        Vector3 preUpV, preAngle, prePos;
        preUpV = trans.up;
        preAngle = trans.localEulerAngles;
        prePos = trans.position;

        trans.RotateAround(focusPos, Vector3.up, eulerAngle.y);

        trans.RotateAround(focusPos, trans.right, -eulerAngle.x);

        trans.LookAt(focusPos);

        Vector3 up = trans.up;
        if (Vector3.Angle(preUpV, up) > 90.0f)
        {
            trans.localEulerAngles = preAngle;
            trans.position = prePos;
        }

        return;

        //Turn left and right with A and D key
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }

    }
    void OnGUI()
    {
        int W = Screen.width,
        H = Screen.height;
        int w = W / 15,
        h = H / 15;
        int marginX = w / 2,
        marginY = h;
        int x, y;


        GUIStyle style = new GUIStyle("button");
        style.fontSize = 30;

        x = h + (w - h) / 2 + w + marginX;
        y = H - (2 * h + w + h);
        if (GUI.RepeatButton(new Rect(x, y, w, h), "Up", style))
            flyFactor = 1;
        x = h + (w - h) / 2 + w + marginX;
        y = H - w;
        if (GUI.RepeatButton(new Rect(x, y, w, h), "Down", style))
            flyFactor = -1;

        if (Input.GetKey(KeyCode.LeftShift))
            flyFactor = 1;
        if (Input.GetKey(KeyCode.LeftControl))
            flyFactor = -1;

#if ISTABLET
		
		if (Input.GetKey(KeyCode.LeftShift))
			flyFactor = 1;
		if (Input.GetKey(KeyCode.LeftControl))
			flyFactor = -1;
#endif

        // Left Button
        x = W - (3 * w + marginX);
        y = H - (3 * h);
        if (GUI.RepeatButton(new Rect(x, y, w, h), "<", style))
            horizontal = -1;

        //Right Button
        x = W - (w + marginX);
        y = H - (3 * h);
        if (GUI.RepeatButton(new Rect(x, y, w, h), ">", style))
            horizontal = 1;

        //Forward Button
        x = W - (h + (w - h) / 2 + w + marginX);
        y = H - (2 * h + w + h);

        GUIUtility.RotateAroundPivot(-90, new Vector2(x + (h / 2), y + (w / 2)));
        if (GUI.RepeatButton(new Rect(x, y, h, w), ">", style))
            vertical = 1;

        GUIUtility.RotateAroundPivot(90, new Vector2(x + (h / 2), y + (w / 2)));

        //Backward Button
        x = W - (h + (w - h) / 2 + w + marginX);
        y = H - (w);
        GUIUtility.RotateAroundPivot(-90, new Vector2(x + (h / 2), y + (w / 2)));
        if (GUI.RepeatButton(new Rect(x, y, h, w), "<", style))
            vertical = -1;

        // Debug.Log("V = " + vertical + " H = " + horizontal);
    }
}