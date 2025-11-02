using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectScaler : MonoBehaviour
{
    private const float REF_ASPECT_RATIO = 2.22f;
    
    private RectTransform _rectTransform;
    private float _aspectRatio;
    [SerializeField] private float _minScale = 1f;
    [SerializeField] private float _maxScale = 1.3f;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        AspectRatio();
    }

    private void AspectRatio()
    {
        _aspectRatio = (float)Screen.width / Screen.height;
        _rectTransform.localScale = Mathf.Clamp((REF_ASPECT_RATIO / _aspectRatio),_minScale,_maxScale) * Vector3.one;
    }
   
    
}
