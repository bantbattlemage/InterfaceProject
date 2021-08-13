using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InterfacePanelGroup : MonoBehaviour
{
	public InterfacePanelGroupOrientation GroupOrientation;

	private List<InterfacePanel> _subPanels = new List<InterfacePanel>();

	public enum InterfacePanelGroupOrientation
	{
		Horizontal,
		Vertical
	}

	public void Initialize()
	{
		ScalablePanel p = GetComponent<ScalablePanel>();
		if (p != null)
		{
			p.Initialize();
		}
	}

	public void NormalizePanelSizes()
	{
		List<ScalablePanel> panels = new List<ScalablePanel>();
		List<Vector2> scales = new List<Vector2>();
		float totalWidth = 0;
		float totalHeight = 0;

		_subPanels.ForEach(x => { panels.Add(x.GetComponent<ScalablePanel>()); });
		GetComponentsInChildren<InterfacePanelGroup>().ToList().ForEach(x => { if (x.transform.parent == transform) panels.Add(x.GetComponent<ScalablePanel>()); });

		foreach (ScalablePanel p in panels)
		{
			LayoutElement element = p.GetComponent<LayoutElement>();
			Vector2 size = new Vector2(element.flexibleWidth, element.flexibleHeight);
			totalWidth += size.x;
			totalHeight += size.y;
			scales.Add(size);
		}

		List<Vector2> newSizes = new List<Vector2>();

		for (int i = 0; i < scales.Count; i++)
		{
			float relativeWidth = scales[i].x / totalWidth;
			float relativeHeight = scales[i].y / totalHeight;

			Vector2 newSize = new Vector2(relativeWidth * panels[i].DefaultSize.x, relativeHeight * panels[i].DefaultSize.y);
			newSizes.Add(newSize);
		}

		for (int i = 0; i < scales.Count; i++)
		{
			LayoutElement element = panels[i].GetComponent<LayoutElement>();
			element.flexibleWidth = newSizes[i].x;
			element.flexibleHeight = newSizes[i].y;
		}

		InterfacePanelGroup parent = transform.parent?.GetComponent<InterfacePanelGroup>();

		if (parent != null)
		{
			parent.NormalizePanelSizes();
		}
	}

	public void InsertPanel(InterfacePanel panel)
	{
		if (_subPanels.Contains(panel))
		{
			throw new System.Exception("Panel already in this group!");
		}

		if (InterfaceController.Instance.RootLevelPanels.Contains(panel))
		{
			InterfaceController.Instance.RootLevelPanels.Remove(panel);
		}
		else if (panel.ParentPanelGroup)
		{
			panel.ParentPanelGroup.RemovePanel(panel);
		}

		panel.AssignToPanelGroup(this);
		_subPanels.Add(panel);
	}

	public void RemovePanel(InterfacePanel panel)
	{
		if (panel == null || panel.gameObject == null)
		{
			throw new System.Exception("null panel");
		}

		if (!_subPanels.Contains(panel))
		{
			throw new System.Exception("Tried to remove panel not in group");
		}

		_subPanels.Remove(panel);
	}

	public void RemoveAndDestroyPanel(InterfacePanel panel)
	{
		if (!_subPanels.Contains(panel))
		{
			throw new System.Exception("Tried to remove panel that is not in group");
		}

		List<InterfacePanel> p = GetComponentsInChildren<InterfacePanel>().ToList();

		_subPanels.Remove(panel);

		panel.gameObject.SafeDestory();

		List<InterfacePanel> newList = GetComponentsInChildren<InterfacePanel>().ToList();
		if (newList.Contains(panel))
		{
			Debug.Log("panel is still a child of this object");
		}

		if (Cleanup())
		{
			InterfaceController.Instance.ActivePanelGroups.Remove(this);
		}
	}

	public List<Transform> GetChildList()
	{
		List<Transform> childList = new List<Transform>();
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform c = transform.GetChild(i);
			if (c.GetComponent<InterfacePanelGroup>() != null || c.GetComponent<InterfacePanel>() != null)
			{
				childList.Add(c);
			}
		}

		return childList;
	}

	public bool Cleanup()
	{
		List<Transform> children = GetChildList();

		if (children.Count == 0)
		{
			if (_subPanels.Count != 0)
			{
				throw new System.Exception("lost track of subPanel");
			}

			gameObject.SafeDestory();
			NormalizePanelSizes();
			return true;
		}
		else if (children.Count == 1)
		{
			if (children[0].GetComponent<InterfacePanelGroup>())
			{
				if (_subPanels.Count > 0)
				{
					throw new System.Exception("lost track of subPanel");
				}

				children[0].SetParent(transform.parent);
			}
			else
			{
				InterfacePanel panel = children[0].GetComponent<InterfacePanel>(); //_subPanels[0];

				if (transform.parent == InterfaceController.Instance.Body)
				{
					panel.SetToRoot();
				}
				else
				{
					transform.parent.GetComponent<InterfacePanelGroup>().InsertPanel(panel);
				}

				if (_subPanels.Count > 0)
				{
					throw new System.Exception("should not be possible to hit this");
				}
			}

			gameObject.SafeDestory();
			NormalizePanelSizes();
			return true;
		}
		else
		{
			NormalizePanelSizes();
			return false;
		}
	}
}
