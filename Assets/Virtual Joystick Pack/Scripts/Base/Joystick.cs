using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    public Image bgImg;
    public Image joystickimg;
    private Vector3 inVector;
    private RectTransform canvas;
    private RectTransform MenuCtrl;
    private float StickSide = 1f;

    private void Start()
    {
        if(gameObject.name == "LeftStick")
            StickSide = -1f;
        //MenuCtrl = GameObject.Find("MenuCtrl").GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        bgImg.rectTransform.sizeDelta = new Vector2(canvas.rect.width * 0.15f, canvas.rect.width * 0.15f);
        joystickimg.rectTransform.sizeDelta = new Vector2(canvas.rect.width * 0.075f, canvas.rect.width * 0.075f);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / (StickSide * bgImg.rectTransform.sizeDelta.x));
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);
            inVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inVector = (inVector.magnitude > 1.0f) ? inVector.normalized : inVector;
            joystickimg.rectTransform.anchoredPosition =
                new Vector3(inVector.x * (StickSide * bgImg.rectTransform.sizeDelta.x / 2),
                            inVector.z * (bgImg.rectTransform.sizeDelta.y / 2));
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        inVector = Vector3.zero;
        joystickimg.rectTransform.anchoredPosition = Vector3.zero;
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
