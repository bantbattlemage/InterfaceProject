using UnityEngine;
using System.Collections.Generic;

public class InterfaceElement : MonoBehaviour
{
	public Transform Content;
	public List<PanelContent> ContentObjects;
	public RectTransform RectT { get { if (_rectT == null) { _rectT = GetComponent<RectTransform>(); } return _rectT; } }
	public ScalablePanel Scale { get { if (_scale == null) { _scale = GetComponent<ScalablePanel>(); } return _scale; } }
	public DraggablePanel Drag { get { if (_drag == null) { _drag = GetComponent<DraggablePanel>(); } return _drag; } }
	private RectTransform _rectT;
	private ScalablePanel _scale;
	private DraggablePanel _drag;

	public void AddContent(PanelContent newContent, bool replaceExistingContent = true)
	{
		if (replaceExistingContent)
		{
			for (int i = ContentObjects.Count - 1; i >= 0; i--)
			{
				ContentObjects[i].gameObject.SafeDestroy();
			}

			ContentObjects = new List<PanelContent>();
		}

		ContentObjects.Add(newContent);
		newContent.transform.SetParent(Content);
		newContent.Initialize();
	}
}