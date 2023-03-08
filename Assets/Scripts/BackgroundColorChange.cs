using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorChange : MonoBehaviour
{
    #region Fields
    public Color[] colors;
    Camera cam;
    int colorIndex;
    [SerializeField]
    float colorChangeSpeed;
    [SerializeField]
    float time;
    float currentTime;
    #endregion
    #region Functions
    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        ColorChange();
        ColorChangeTime();
    }
    private void ColorChange()
    {
        cam.backgroundColor = Color.Lerp(cam.backgroundColor, colors[colorIndex], colorChangeSpeed * Time.deltaTime);
    }
    void ColorChangeTime()
    {
        if (currentTime<=0)
        {
            colorIndex++;
            CheckColorIndex();
            currentTime = time;
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
    }
    void CheckColorIndex()
    {
        if (colorIndex >=colors.Length)
        {
            colorIndex = 0;
        }
    }
    #endregion
}
