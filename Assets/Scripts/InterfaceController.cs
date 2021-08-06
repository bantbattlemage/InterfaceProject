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

    private List<InterfacePanel> ActivePanels = new List<InterfacePanel>();
    private List<InterfacePanelGroup> ActivePanelGroups = new List<InterfacePanelGroup>();

    public delegate void PanelDestroyedEvent();
    public PanelDestroyedEvent PanelDestroyed;

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

	private void OnPanelDestroyed()
	{
        for (int i = ActivePanelGroups.Count - 1; i >= 0; i--)
        {
            ActivePanelGroups[i].Cleanup();
        }
    }

    public void CreateNewPanel(Transform parent = null)
	{
		if(parent == null)
		{
            parent = Body;
        }

        GameObject newPanel = Instantiate(DefaultInterfaceGroupPrefab, parent);
        InterfacePanel panel = newPanel.GetComponent<InterfacePanel>();
        panel.PanelName.text = string.Format("New Panel {0}", ActivePanels.Count);
        Debug.Log(string.Format("{0} created", panel.PanelName.text));
        ActivePanels.Add(panel);

        InterfacePanelGroup parentGroup = parent.GetComponent<InterfacePanelGroup>();
		if(parentGroup != null)
		{
            parentGroup.AddPanel(panel);
        }

        InitializePanel(panel);
    }

	public void InitializePanel(InterfacePanel panel)
	{
        panel.PanelCloseButtonClicked += OnPanelCloseButtonClicked;
        panel.PanelSplitHorizontalButtonClicked += OnPanelHorizontalSplitButtonClicked;
        panel.PanelSplitVerticalButtonClicked += OnPanelVerticalSplitButtonClicked;
    }

	private void OnPanelHorizontalSplitButtonClicked(InterfacePanel sender)
	{
        GameObject newGroup = Instantiate(HorizontalInterfaceGroupPrefab, sender.transform.parent);
        InterfacePanelGroup newPanelGroup = newGroup.GetComponent<InterfacePanelGroup>();

        sender.transform.SetParent(newPanelGroup.transform, false);
        ActivePanelGroups.Add(newPanelGroup);
        newPanelGroup.AddPanel(sender);
        Debug.Log(string.Format("{0} split horizontally", sender.PanelName.text));

        CreateNewPanel(newPanelGroup.transform);
	}

    private void OnPanelVerticalSplitButtonClicked(InterfacePanel sender)
    {
        GameObject newGroup = Instantiate(VerticalInterfaceGroupPrefab, sender.transform.parent);
        InterfacePanelGroup newPanelGroup = newGroup.GetComponent<InterfacePanelGroup>();

        sender.transform.SetParent(newPanelGroup.transform, false);
        ActivePanelGroups.Add(newPanelGroup);
        newPanelGroup.AddPanel(sender);
        Debug.Log(string.Format("{0} split horizontally", sender.PanelName.text));

        CreateNewPanel(newPanelGroup.transform);
    }

    private void OnPanelCloseButtonClicked(InterfacePanel sender)
    {
        DestroyPanel(sender);
    }

	public void DestroyPanel(InterfacePanel panel)
	{
        if (!ActivePanels.Contains(panel))
        {
            throw new System.Exception("Tried to remove panel not in list");
        }

        InterfacePanelGroup parentGroup = panel.transform.parent.GetComponent<InterfacePanelGroup>();
        if (parentGroup != null)
        {
            ActivePanels.Remove(panel);
            parentGroup.RemoveAndDestroyPanel(panel);
        }
        else
        {
            ActivePanels.Remove(panel);
            Object.Destroy(panel.gameObject);
        }

        if(PanelDestroyed != null)
		{
            PanelDestroyed();
        }
    }
}
