using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
	public GameObject DefaultInterfaceGroupPrefab;
    public Button NewPanelButton;

    public Transform Body;

    private List<InterfacePanel> ActivePanels = new List<InterfacePanel>();

    void Start()
	{
        NewPanelButton.onClick.AddListener(CreateNewPanel);
        CreateNewPanel();
    }

    public void CreateNewPanel()
	{
        GameObject newPanel = Instantiate(DefaultInterfaceGroupPrefab, Body);
        InterfacePanel panel = newPanel.GetComponent<InterfacePanel>();
        panel.PanelCloseButtonClicked += OnPanelCloseButtonClicked;
        ActivePanels.Add(panel);
        Debug.Log("New Panel created");
    }

    private void OnPanelCloseButtonClicked(InterfacePanel sender)
    {
        ActivePanels.Remove(sender);
        UnityEngine.Object.Destroy(sender.gameObject);
    }
}
