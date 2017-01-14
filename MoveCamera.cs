

#define ISTABLET // Tablet,telefon ve masaüstü uygulamalarında VR uygulaması farklı tasarlandı.
                // Telefon : Google cardboard ile kullanılacak
                // Masaüstü : Haritadan konum seçilecek. Yön tuşları kullanılarak VR sahne içinde gezinme yapmayı sağlayacak.
                // Tablet : Ekrana konulan yön tuşları ile VR sahne içinde gezinme sağlayacak.

using UnityEngine;
using System.Collections;


public class MoveCamera : MonoBehaviour {

	public float speed = 10.0f;
	public float rotationSpeed = 100.0f;

    int horizontal = 0, vertical = 0;
    int flyFactor = 0;
    float translation = 0.0f;

   // public bool isTablet = true;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        if (Input.GetKey("escape"))
            Application.Quit();

//#if ISTABLET

        float translation = vertical * speed;
        float rotation = horizontal * rotationSpeed;
        // reset
        // Bu işlem yapılmazsa bir kez sağ tuşuna basılınca horizontal değeri 0 dan farklı 
        // olacağından sonsuza kadar rotation işlemi yapılır.
        horizontal = 0;
        vertical = 0;

        if (Mathf.Abs(translation - 0.0f) < 0.00001f)
        {            //#else
            translation = Input.GetAxis("Vertical") * speed;
        }

        if (Mathf.Abs(rotation - 0.0f) < 0.00001f)
        {
            rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        }
//#endif

        // model sınırları dısına gitmesin diye kamera konumu kontrolu yapılacak!!!!!!!!!


        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        float translationY = flyFactor * speed * Time.deltaTime;
        transform.Translate(0, translationY, translation);
        flyFactor = 0;
        transform.Rotate(0, rotation, 0);
/*
        if (Input.GetKey(KeyCode.LeftShift))
            flyFactor = 1;
        if (Input.GetKey(KeyCode.LeftControl))
            flyFactor = -1;

        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime; // Zamanla ilerlemesi için
        rotation *= Time.deltaTime;

        float translationY = flyFactor * speed * Time.deltaTime;
        transform.Translate(0, translationY, translation);
        flyFactor = 0;

        transform.Rotate(0, rotation, 0);
*/
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
