using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfacePanel : MonoBehaviour
{
	public UnityEngine.UI.Text PanelName;

	void Start()
	{

	}

	public void Initialize(string name)
	{
		PanelName.text = name;
	}
}
