using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaHandler : MonoBehaviour
{
    [SerializeField] private List<RectTransform> rectTransforms;
    Vector2 anchorMax;
    Vector2 anchorMin;
    Vector2 screenResolution;
    Rect safeArea;
    ScreenOrientation currentOrientation;
    List<Vector2> canvasSizes = new List<Vector2>();
    
    void Start()
    {
        currentOrientation = Screen.orientation;
        for (int i = 0; i < rectTransforms.Count; i++)
        {
            canvasSizes.Add(new Vector2(rectTransforms[i].rect.width, rectTransforms[i].rect.height));
        }
        CalculateSafeArea(currentOrientation);
    }

    void LateUpdate()
    {
        if(Screen.orientation != currentOrientation || IsCanvasDirty())
        {
            currentOrientation = Screen.orientation;
            CalculateSafeArea(currentOrientation);
        }
    }

    //This is a workaround a Unity bug where safe area calculation is wrong when orientation changes
    //For more info check https://issuetracker.unity3d.com/issues/ios-screen-dot-safearea-values-are-incorrect-after-app-pause-and-orientation-change
    private bool IsCanvasDirty()
    {
        bool isCanvasDirty = false;
        for (int i = 0; i < rectTransforms.Count; i++)
        {
            if(rectTransforms[i].rect.width != canvasSizes[i].x || rectTransforms[i].rect.height != canvasSizes[i].y)
            {
                canvasSizes[i] =new Vector2( rectTransforms[i].rect.width, rectTransforms[i].rect.height);
                isCanvasDirty = true;
            }
        }
        return isCanvasDirty;
    }

    private void CalculateSafeArea(ScreenOrientation currentOrientation)
    {
        screenResolution.x = Screen.width;
        screenResolution.y = Screen.height;
        safeArea = Screen.safeArea;
        anchorMax = new Vector2(safeArea.xMax / screenResolution.x, safeArea.yMax / screenResolution.y);
        anchorMin = new Vector2(safeArea.xMin / screenResolution.x, safeArea.yMin / screenResolution.y);

        for (int i = 0; i < rectTransforms.Count; i++)
        {
            rectTransforms[i].anchorMax = anchorMax;
            rectTransforms[i].anchorMin = anchorMin;
        }
    }
}
