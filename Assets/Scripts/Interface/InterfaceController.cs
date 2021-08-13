using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
	public GameObject DefaultInterfaceGroupPrefab;
	public GameObject HorizontalInterfaceGroupPrefab;
	public GameObject VerticalInterfaceGroupPrefab;
	public GameObject PopUpPanelPrefab;
	public GameObject LogOutPopUpPrefab;
	public GameObject MarketPanelPrefab;
	public GameObject MapPanelPrefab;

	public Button NewPanelButton;
	public LogInScreenController LogInScreen;

	public Transform Body;
	public Transform PopUpLayer;

	public List<InterfacePanel> RootLevelPanels = new List<InterfacePanel>();
	public List<InterfacePanelGroup> ActivePanelGroups = new List<InterfacePanelGroup>();
	public List<PopUpPanel> ActivePopUpPanels = new List<PopUpPanel>();

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
		Body.GetComponent<LayoutElement>().flexibleWidth = Screen.width;
		Body.GetComponent<LayoutElement>().flexibleHeight = Screen.height;

		//	Assign button callbacks
		NewPanelButton.onClick.AddListener(OnNewPanelButtonClicked);

		//	Set up Log In Screen
		LogInScreen.InitializeLogInScreen();
		LogInScreen.OnSuccessfulLogin += LogIn;
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

	private void OnPopUpPanelDestroyed(PopUpPanel sender)
	{
		if (!ActivePopUpPanels.Contains(sender))
		{
			throw new System.Exception("stray pop up?");
		}

		ActivePopUpPanels.Remove(sender);
	}

	public void LogIn()
	{
		LogInScreen.gameObject.SetActive(false);
		GameCommands.ProcessHelpCommand("HELP", null);

		Debug.Log("Logged In");
	}

	public void LogOut()
	{
		GameObject logOutPopUpObject = Instantiate(LogOutPopUpPrefab, PopUpLayer);
		PopUpPanel logOutPopUp = logOutPopUpObject.GetComponent<PopUpPanel>();
		logOutPopUp.PopUpButtons[0].onClick.AddListener(() =>
		{
			LogInScreen.gameObject.SetActive(true);
			LogInScreen.InitializeLogInScreen();
			Debug.Log("Logged Out");
			logOutPopUpObject.SafeDestroy();
		});
		logOutPopUp.PopUpButtons[1].onClick.AddListener(() =>
		{
			logOutPopUpObject.SafeDestroy();
		});
	}

	public PopUpPanel CreateNewPopUp(string popUpName = "", string popUpText = "", PopUpButtonProperties[] buttons = null, PopUpInputFieldProperties[] inputFields = null)
	{
		GameObject newPopUpObject = Instantiate(PopUpPanelPrefab, PopUpLayer);
		PopUpPanel newPopUpPanel = newPopUpObject.GetComponent<PopUpPanel>();
		newPopUpPanel.GetComponent<ScalablePanel>().IsFreeFloating = true;
		newPopUpPanel.InitializePopUp(popUpName, popUpText, buttons, inputFields);
		newPopUpPanel.PanelDetroyed += OnPopUpPanelDestroyed;

		ActivePopUpPanels.Add(newPopUpPanel);

		return newPopUpPanel;
	}

	public MarketInterfacePanel CreateMarketPanel(Transform parent = null, string targetItem = "")
	{
		if (parent == null)
		{
			parent = Body;
		}

		GameObject newPanel = Instantiate(MarketPanelPrefab, parent);
		MarketInterfacePanel panel = newPanel.GetComponent<MarketInterfacePanel>();

		panel.Initialize();
		RegisterPanelButtonEvents(panel);

		if (parent == Body)
		{
			RootLevelPanels.Add(panel);
		}

		ReplacePanel(panel, parent);

		return panel;
	}


	public MapInterfacePanel CreateMapPanel(Transform parent = null, string targetItem = "")
	{
		if (parent == null)
		{
			parent = Body;
		}

		GameObject newPanel = Instantiate(MapPanelPrefab, parent);
		MapInterfacePanel panel = newPanel.GetComponent<MapInterfacePanel>();

		panel.Initialize();
		RegisterPanelButtonEvents(panel);

		if (parent == Body)
		{
			RootLevelPanels.Add(panel);
		}

		ReplacePanel(panel, parent);

		return panel;
	}

	public void ReplacePanel(InterfacePanel newPanel, Transform parent)
	{
		InterfacePanel parentPanel = parent.GetComponent<InterfacePanel>();
		if (parentPanel != null)
		{
			int siblingIndex = parent.transform.GetSiblingIndex();

			if (parentPanel.ParentPanelGroup != null)
			{
				parentPanel.ParentPanelGroup.InsertPanel(newPanel);
				DestroyPanel(parentPanel);
			}
			else
			{
				newPanel.SetToRoot();
				DestroyPanel(parentPanel);
			}

			newPanel.transform.SetSiblingIndex(siblingIndex);
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
		string name = string.Format("New Panel {0}", FindObjectsOfType<InterfacePanel>().ToList().Count);
		panel.Initialize(name);
		//Debug.Log(string.Format("{0} created", panel.PanelName.text));

		if (parent == Body)
		{
			RootLevelPanels.Add(panel);
		}
		else
		{
			InterfacePanelGroup parentGroup = parent.GetComponent<InterfacePanelGroup>();
			parentGroup.InsertPanel(panel);
		}

		RegisterPanelButtonEvents(panel);

		return panel;
	}

	public void RegisterPanelButtonEvents(InterfacePanel panel)
	{
		panel.PanelCloseButtonClicked += OnPanelCloseButtonClicked;
		panel.PanelSplitHorizontalButtonClicked += OnPanelHorizontalSplitButtonClicked;
		panel.PanelSplitVerticalButtonClicked += OnPanelVerticalSplitButtonClicked;
	}

	public InterfacePanel CreateNewPanel(InterfacePanelGroup parent)
	{
		InterfacePanel newPanel = CreateNewPanel(parent.transform);
		return newPanel;
	}

	private void SplitPanel(InterfacePanel sender, InterfacePanelGroup.InterfacePanelGroupOrientation orientation)
	{
		Transform parent;
		int childIndex = sender.transform.GetSiblingIndex();

		if (sender.ParentPanelGroup)
		{
			parent = sender.ParentPanelGroup.transform;
		}
		else
		{
			parent = Body;
		}

		GameObject newGroupObject = Instantiate(orientation == InterfacePanelGroup.InterfacePanelGroupOrientation.Vertical ? VerticalInterfaceGroupPrefab : HorizontalInterfaceGroupPrefab, parent);
		newGroupObject.transform.SetSiblingIndex(childIndex);
		InterfacePanelGroup newGroup = newGroupObject.GetComponent<InterfacePanelGroup>();
		ActivePanelGroups.Add(newGroup);
		newGroup.InsertPanel(sender);
		newGroup.Initialize();

		//Debug.Log(string.Format("{0} split", sender.PanelName.text));

		CreateNewPanel(newGroup);
	}

	public void DestroyPanel(InterfacePanel panel)
	{
		if (!RootLevelPanels.Contains(panel))
		{
			if (ActivePanelGroups.Contains(panel.ParentPanelGroup))
			{
				int index = ActivePanelGroups.IndexOf(panel.ParentPanelGroup);

				if (index < 0 || index >= ActivePanelGroups.Count)
				{
					throw new System.Exception("!?");
				}

				//Debug.Log($"index: {index} || panelgroups: {ActivePanelGroups.Count}");
				ActivePanelGroups[index].RemoveAndDestroyPanel(panel);
				// Debug.Log($"index: {index} || panelgroups: {ActivePanelGroups.Count}");
				// Debug.Log(ActivePanelGroups[index].gameObject.name);


				// if (ActivePanelGroups[index].Cleanup())
				// {
				// 	ActivePanelGroups.RemoveAt(index);
				// }
			}
			else
			{
				throw new System.Exception("panel not being tracked?");
			}
		}
		else
		{
			RootLevelPanels.Remove(panel);
			panel.gameObject.SafeDestroy();
		}

		for (int i = ActivePanelGroups.Count - 1; i >= 0; i--)
		{
			if (ActivePanelGroups[i].Cleanup())
			{
				ActivePanelGroups.RemoveAt(i);
			}
		}
	}

	public static List<Transform> GetChildList(Transform target)
	{
		List<Transform> childList = new List<Transform>();
		for (int i = 0; i < target.childCount; i++)
		{
			Transform c = target.GetChild(i);
			if (c.GetComponent<InterfacePanelGroup>() != null || c.GetComponent<InterfacePanel>() != null)
			{
				childList.Add(c);
			}
		}

		return childList;
	}
}
