using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InterfacePanelGroup : MonoBehaviour
{
	public InterfacePanelGroupOrientation GroupOrientation;

	private List<InterfacePanel> _subPanels = new List<InterfacePanel>();
	private List<InterfacePanelGroup> _subGroups = new List<InterfacePanelGroup>();

	public enum InterfacePanelGroupOrientation
	{
		Horizontal,
		Vertical
	}

	void Awake()
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
		_subGroups.ForEach(x => { panels.Add(x.GetComponent<ScalablePanel>()); });

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
	}

	public void InsertPanel(InterfacePanel panel)
	{
		if (_subPanels.Contains(panel))
		{
			throw new System.Exception("Panel already in this group!");
		}

		if (panel.ParentPanelGroup)
		{
			panel.ParentPanelGroup.RemovePanel(panel);
		}
		else if (InterfaceController.Instance.RootLevelPanels.Contains(panel))
		{
			InterfaceController.Instance.RootLevelPanels.Remove(panel);
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

		panel.ParentPanelGroup = null;
		_subPanels.Remove(panel);
		Destroy(panel.gameObject);
		Destroy(panel);
	}

	public bool Cleanup()
	{
		List<Transform> children = InterfaceController.GetChildList(transform);

		if (children.Count == 0)
		{
			Destroy(gameObject);
			return true;
		}
		else if (children.Count == 1)
		{
			InterfacePanel panel = children[0].GetComponent<InterfacePanel>();
			if (panel != null)
			{
				if (transform.parent == InterfaceController.Instance.Body)
				{
					panel.SetToRoot();
				}
				else
				{
					transform.parent.GetComponent<InterfacePanelGroup>().InsertPanel(panel);
				}
			}
			else
			{
				children[0].SetParent(transform.parent);
			}

			Destroy(gameObject);
			return true;
		}
		else
		{
			return false;
		}
	}
}
