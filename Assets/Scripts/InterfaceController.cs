using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
	public GameObject DefaultInterfaceGroupPrefab;
	public GameObject HorizontalInterfaceGroupPrefab;
	public GameObject VerticalInterfaceGroupPrefab;

	public Button NewPanelButton;

	public Transform Body;

	public List<InterfacePanel> RootLevelPanels = new List<InterfacePanel>();
	public List<InterfacePanelGroup> ActivePanelGroups = new List<InterfacePanelGroup>();

	public delegate void PanelDestroyedEvent();
	public PanelDestroyedEvent PanelDestroyed;

	private static InterfaceController _instance;
	public static InterfaceController Instance
	{
		get
		{
			if (_instance)
			{
				return _instance;
			}

			_instance = GameObject.FindGameObjectWithTag("InterfaceController").GetComponent<InterfaceController>();
			return _instance;
		}
	}

	void Start()
	{
		//	Assign button callbacks
		NewPanelButton.onClick.AddListener(OnNewPanelButtonClicked);

		//	Assign event callbacks
		PanelDestroyed += OnPanelDestroyed;

		CreateNewPanel();
	}

	private void OnNewPanelButtonClicked()
	{
		CreateNewPanel(Body);
	}

	private void OnPanelHorizontalSplitButtonClicked(InterfacePanel sender)
	{
		SplitPanel(sender, InterfacePanelGroup.InterfacePanelGroupOrientation.Horizontal);
	}

	private void OnPanelVerticalSplitButtonClicked(InterfacePanel sender)
	{
		SplitPanel(sender, InterfacePanelGroup.InterfacePanelGroupOrientation.Vertical);
	}

	private void OnPanelCloseButtonClicked(InterfacePanel sender)
	{
		DestroyPanel(sender);
	}

	private void OnPanelDestroyed()
	{
		for (int i = ActivePanelGroups.Count - 1; i >= 0; i--)
		{
			if (ActivePanelGroups[i].Cleanup())
			{
				ActivePanelGroups.RemoveAt(i);
			}
		}
	}

	public InterfacePanel CreateNewPanel(Transform parent = null)
	{
		if (parent == null)
		{
			parent = Body;
		}

		GameObject newPanel = Instantiate(DefaultInterfaceGroupPrefab, parent);
		InterfacePanel panel = newPanel.GetComponent<InterfacePanel>();
		panel.PanelName.text = string.Format("New Panel {0}", RootLevelPanels.Count);
		Debug.Log(string.Format("{0} created", panel.PanelName.text));

		if (parent == Body)
		{
			RootLevelPanels.Add(panel);
		}
		else
		{
			InterfacePanelGroup parentGroup = parent.GetComponent<InterfacePanelGroup>();
			parentGroup.InsertPanel(panel);
		}

		panel.PanelCloseButtonClicked += OnPanelCloseButtonClicked;
		panel.PanelSplitHorizontalButtonClicked += OnPanelHorizontalSplitButtonClicked;
		panel.PanelSplitVerticalButtonClicked += OnPanelVerticalSplitButtonClicked;

		return panel;
	}

	public InterfacePanel CreateNewPanel(InterfacePanelGroup parent)
	{
		InterfacePanel newPanel = CreateNewPanel(parent.transform);
		return newPanel;
	}

	private void SplitPanel(InterfacePanel sender, InterfacePanelGroup.InterfacePanelGroupOrientation orientation)
	{
		Transform parent = RootLevelPanels.Contains(sender) ? Body : sender.ParentPanelGroup.transform;
		GameObject newGroupObject = Instantiate(orientation == InterfacePanelGroup.InterfacePanelGroupOrientation.Vertical ? VerticalInterfaceGroupPrefab : HorizontalInterfaceGroupPrefab, parent);
		InterfacePanelGroup newGroup = newGroupObject.GetComponent<InterfacePanelGroup>();
		ActivePanelGroups.Add(newGroup);
		newGroup.InsertPanel(sender);

		Debug.Log(string.Format("{0} split", sender.PanelName.text));

		CreateNewPanel(newGroup);
	}

	public void DestroyPanel(InterfacePanel panel)
	{
		if (!RootLevelPanels.Contains(panel))
		{
			if (panel.ParentPanelGroup != null)
			{
				panel.ParentPanelGroup.RemoveAndDestroyPanel(panel);
			}
			else
			{
				throw new System.Exception("panel not being tracked?");
			}
		}
		else
		{
			RootLevelPanels.Remove(panel);
			Object.Destroy(panel.gameObject);
		}

		PanelDestroyed();
	}
}
