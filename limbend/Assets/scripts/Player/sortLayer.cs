using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sortLayer : MonoBehaviour
{
    private Renderer rd;
    public bool isStatic = false;
    private SpriteRenderer sp;

    private void Awake()
    {
        rd = GetComponent<Renderer>();
        sp = GetComponent<SpriteRenderer>();
        if (isStatic)
        {
            rd.sortingOrder = Mathf.RoundToInt((transform.position.y - rd.bounds.extents.y) * 100f) * -1;
            Destroy(this);
        }
    }

    private void LateUpdate()
    {
        rd.sortingOrder = Mathf.RoundToInt((transform.position.y - rd.bounds.extents.y) * 100f) * -1;
    }
}
