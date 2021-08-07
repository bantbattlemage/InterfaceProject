using UnityEngine;
using UnityEngine.UI;

public class InterfacePanel : MonoBehaviour
{
	public Text PanelName;
	public Button CloseButton;
	public Button HorizontalSplitButton;
	public Button VerticalSplitButton;

	public Transform Body;
	public InterfacePanelGroup ParentPanelGroup { get; private set; }

	public delegate void PanelEvent(InterfacePanel sender);
	public PanelEvent PanelCloseButtonClicked;
	public PanelEvent PanelSplitHorizontalButtonClicked;
	public PanelEvent PanelSplitVerticalButtonClicked;

	void Start()
	{
		CloseButton.onClick.AddListener(OnCloseButtonClicked);
		HorizontalSplitButton.onClick.AddListener(OnSplitHorizontalButtonClicked);
		VerticalSplitButton.onClick.AddListener(OnSplitVerticalButtonClicked);
	}

	public void Initialize(string name)
	{
		PanelName.text = name;
	}

	public void AssignToPanelGroup(InterfacePanelGroup panelGroup)
	{
		transform.SetParent(panelGroup.transform);
		ParentPanelGroup = panelGroup;
	}

	public void SetToRoot()
	{
		if (InterfaceController.Instance.RootLevelPanels.Contains(this))
		{
			throw new System.Exception("Already in root");
		}

		if (ParentPanelGroup != null)
		{
			InterfacePanelGroup parent = ParentPanelGroup;
			ParentPanelGroup = null;
			parent.RemovePanel(this);
		}

		InterfaceController.Instance.RootLevelPanels.Add(this);
		transform.SetParent(InterfaceController.Instance.Body);
	}

	public void OnCloseButtonClicked()
	{
		PanelCloseButtonClicked(this);
	}

	public void OnSplitHorizontalButtonClicked()
	{
		PanelSplitHorizontalButtonClicked(this);
	}

	public void OnSplitVerticalButtonClicked()
	{
		PanelSplitVerticalButtonClicked(this);
	}
}
