using UnityEngine;

public class UISafeArea: MonoBehaviour
{
    void Start()
    {
        var rt = this.GetComponent<RectTransform>();
        var area = Screen.safeArea;
        var w = Screen.width;

        if (area.width >= w)
        {
            rt.offsetMin *= Vector2.up;
            rt.offsetMax *= Vector2.up;
        }
    }
}