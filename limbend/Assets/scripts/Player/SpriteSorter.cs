using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    private Renderer rd;
    public bool isStatic = false;
    private void Awake()
    {
        rd = GetComponent<Renderer>();
        if (isStatic)
        {
            rd.sortingOrder = Mathf.RoundToInt(transform.position.y * -1);
            Destroy(this);
        }
    }
    private void LateUpdate()
    {
        rd.sortingOrder = Mathf.RoundToInt(transform.position.y * -1);
    }
}
