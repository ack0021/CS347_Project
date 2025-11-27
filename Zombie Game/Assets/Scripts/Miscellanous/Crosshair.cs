using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image crosshairImage;
    public float defaultSize = 20f;
    public float expandedSize = 40f;
    public float expandSpeed = 5f;
    private float targetSize;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            targetSize = expandedSize;
        else
            targetSize = defaultSize;

        float size = Mathf.Lerp(crosshairImage.rectTransform.sizeDelta.x, targetSize, Time.deltaTime * expandSpeed);
        crosshairImage.rectTransform.sizeDelta = new Vector2(size, size);
    }
}
