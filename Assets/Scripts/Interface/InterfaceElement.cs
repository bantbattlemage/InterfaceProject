using UnityEngine;
using System.Collections.Generic;

public class InterfaceElement : MonoBehaviour
{
	public Transform Content;
	public List<GameObject> ContentObjects;
	public RectTransform RectT { get { if (_rectT == null) { _rectT = GetComponent<RectTransform>(); } return _rectT; } }
	public ScalablePanel Scale { get { if (_scale == null) { _scale = GetComponent<ScalablePanel>(); } return _scale; } }
	public DraggablePanel Drag { get { if (_drag == null) { _drag = GetComponent<DraggablePanel>(); } return _drag; } }
	private RectTransform _rectT;
	private ScalablePanel _scale;
	private DraggablePanel _drag;
}