using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInterfacePanel : InterfacePanel
{
    public RectTransform RenderImageTransform;
    public Camera RenderTextureCamera { get; private set; }
    private RenderTextureRaycast _renderTextureRaycast;

    public void Initialize()
    {
        RenderTextureCamera = GameObject.FindGameObjectWithTag("ViewportCamera").GetComponent<Camera>();
        _renderTextureRaycast = GetComponent<RenderTextureRaycast>();
        _renderTextureRaycast.RawImageRectTrans = GetComponent<RectTransform>();
        _renderTextureRaycast.RenderToTextureCamera = RenderTextureCamera;
    }
}
