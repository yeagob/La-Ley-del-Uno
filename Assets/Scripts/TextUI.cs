using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    Canvas parentCanvas;
    TextMeshProUGUI uiText;
    Image likeImage;
    // Start is called before the first frame update
    void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        uiText = GetComponent<TextMeshProUGUI>();
        likeImage = GetComponentInChildren<Image>();
        likeImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);

        Vector3 mousePos = parentCanvas.transform.TransformPoint(movePos);

        //Move the Object/Panel
        transform.position = mousePos;
    }

    public void SetText (string label)
    {
        uiText.text = label.Replace('_',' ');
    }

    internal void SendLike()
    {
        likeImage.enabled = true;
        Invoke("HideLike", 1);
    }

    void HideLike()
    {
        likeImage.enabled = false;
    }
}
