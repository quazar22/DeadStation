using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickLocationChanger : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    Image bgImg;
    Image stick;
    Vector3 inVector;
    Color original_bgimg_color;
    Color original_stick_color;
    private float StickSide = 1f;
    Vector3 stick_original_location;

    void Start()
    {
        if (gameObject.name == "LeftStickArea")
            StickSide = -1f;

        if (name == "LeftStickArea")
        {
            bgImg = GameObject.Find("Canvas/LeftStick").GetComponent<Image>();
            stick = GameObject.Find("Canvas/LeftStick/LeftImage").GetComponent<Image>();
        }
        else
        {
            bgImg = GameObject.Find("Canvas/RightStick").GetComponent<Image>();
            stick = GameObject.Find("Canvas/RightStick/RightImage").GetComponent<Image>();
        }

        stick_original_location = bgImg.rectTransform.position;
        Debug.Log(stick_original_location);
        original_bgimg_color = bgImg.color;
        original_stick_color = stick.color;
        bgImg.color = new Color() { a = 0f };
        //stick.color = new Color() { a = 0f };

    }
    
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        bgImg.rectTransform.position = eventData.position;
        stick.rectTransform.position = eventData.position;
        bgImg.color = new Color() { a = original_bgimg_color.a, b = original_bgimg_color.b, g = original_bgimg_color.g, r = original_bgimg_color.r };
        stick.color = new Color() { a = original_stick_color.a, b = original_stick_color.b, g = original_stick_color.g, r = original_stick_color.r };
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inVector = Vector3.zero;
        stick.rectTransform.anchoredPosition = Vector3.zero;
        bgImg.rectTransform.position = stick_original_location;
        bgImg.color = new Color() { a = 0f };
        //stick.color = new Color() { a = 0f };
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / (StickSide * bgImg.rectTransform.sizeDelta.x));
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);
            inVector = new Vector3(pos.x * 2, 0, pos.y * 2);
            inVector = (inVector.magnitude > 1.0f) ? inVector.normalized : inVector;
            stick.rectTransform.anchoredPosition =
                new Vector3(inVector.x * (StickSide * bgImg.rectTransform.sizeDelta.x / 2),
                            inVector.z * (bgImg.rectTransform.sizeDelta.y / 2));
        }
    }

    public float Horizontal()
    {
        if (inVector.x != 0)
            return inVector.x * StickSide;
        else
            return Input.GetAxis("Horizontal") * StickSide;
    }

    public float Vertical()
    {
        if (inVector.z != 0)
            return inVector.z;
        else
            return Input.GetAxis("Vertical");
    }

}
