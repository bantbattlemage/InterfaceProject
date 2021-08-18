using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelContent : MonoBehaviour
{
	public RectTransform Rect { get { if (_rect == null) { _rect = GetComponent<RectTransform>(); } return _rect; } }
	private RectTransform _rect;

	public virtual void Initialize()
	{

	}

	public virtual void Initialize(int number)
	{
		Initialize();
	}
}
