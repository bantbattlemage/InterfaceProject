using UnityEngine;
using UnityEngine.UI;

public class InterfacePanel : InterfaceElement
{
	public Text PanelName;
	public Button CloseButton;
	public Button HorizontalSplitButton;
	public Button VerticalSplitButton;

	public InterfacePanelGroup ParentPanelGroup;
	public delegate void PanelEvent(InterfacePanel sender);
	public PanelEvent PanelCloseButtonClicked;
	public PanelEvent PanelSplitHorizontalButtonClicked;
	public PanelEvent PanelSplitVerticalButtonClicked;

	public void Initialize()
	{
		Scale.Initialize();

		CloseButton.onClick.AddListener(OnCloseButtonClicked);
		HorizontalSplitButton.onClick.AddListener(OnSplitHorizontalButtonClicked);
		VerticalSplitButton.onClick.AddListener(OnSplitVerticalButtonClicked);
	}

	public void Initialize(string name)
	{
		Initialize();

		PanelName.text = name;
	}

	public void AssignToPanelGroup(InterfacePanelGroup panelGroup)
	{
		transform.SetParent(panelGroup.transform);
		ParentPanelGroup = panelGroup;
		panelGroup.NormalizePanelSizes();
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
		Scale.ResetScale();
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
