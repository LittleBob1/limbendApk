using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScaling : MonoBehaviour
{
    [SerializeField] RectTransform Content;
    [SerializeField] RectTransform Text;
    void Update()
    {
        var size = Content.sizeDelta;
        size.y = Text.sizeDelta.y;
        Content.sizeDelta = size;
    }
}
