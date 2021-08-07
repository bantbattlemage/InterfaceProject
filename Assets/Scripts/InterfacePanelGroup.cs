using System.Collections.Generic;
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

		_subPanels.Remove(panel);
		Object.Destroy(panel.gameObject);
	}

	public bool Cleanup()
	{
		Debug.LogWarning("Cleanup");

		if (_subPanels.Count == 0 && _subGroups.Count == 0)
		{
			Object.Destroy(gameObject);
			return true;
		}
		else if (_subPanels.Count == 1)
		{
			if (transform.parent.GetComponentInParent<InterfacePanelGroup>())
			{
				transform.parent.GetComponentInParent<InterfacePanelGroup>().InsertPanel(_subPanels[0]);
			}
			else
			{
				_subPanels[0].SetToRoot();
			}

			Object.Destroy(gameObject);
			return true;
		}
		else
		{
			return false;
		}
	}

	// public void AddSubGroup(InterfacePanelGroup panelGroup)
	// {
	// 	_subGroups.Add(panelGroup);
	// }

	// public void RemoveAndDestroySubGroup(InterfacePanelGroup panelGroup)
	// {
	// 	if (!_subGroups.Contains(panelGroup))
	// 	{
	// 		throw new System.Exception("Tried to remove panel that is not in group");
	// 	}

	// 	_subGroups.Remove(panelGroup);
	// 	Object.Destroy(panelGroup);
	// }
}
