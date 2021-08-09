using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
