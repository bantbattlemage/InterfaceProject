using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfacePanel : MonoBehaviour
{
	public Text PanelName;
    public Button CloseButton;
    public Transform Body;

    public delegate void PanelEvent(InterfacePanel sender);
    public PanelEvent PanelCloseButtonClicked;

    void Start()
	{
        CloseButton.onClick.AddListener(ClosePanel);
    }

	public void Initialize(string name)
	{
		PanelName.text = name;
	}

	public void ClosePanel()
	{
        PanelCloseButtonClicked(this);
    }
}
