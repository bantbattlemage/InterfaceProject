using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
	//	Core Child object references
	[Header("Core Child Object References")]
	public Transform ContentRoot;
	public Transform PopUpLayer;
	public Button NewPanelButton;
	public LogInScreenController LogInScreen;

	//	Core prefabs
	[Header("Core Prefabs")]
	public GameObject DefaultInterfaceGroupPrefab;
	public GameObject HorizontalInterfaceGroupPrefab;
	public GameObject VerticalInterfaceGroupPrefab;
	public GameObject PopUpPanelPrefab;

	//	Panel content prefabs
	[Header("Panel Content Prefabs")]
	public GameObject MarketPrefab;
	public GameObject MapPrefab;
	public GameObject ChatPrefab;

	//	Pop up content prefabs
	[Header("Pop Up Content Prefabs")]
	public GameObject LogOutPopUpPrefab;

	// Runtime reference lists
	[HideInInspector]
	public List<InterfacePanel> RootLevelPanels = new List<InterfacePanel>();
	[HideInInspector]
	public List<InterfacePanelGroup> ActivePanelGroups = new List<InterfacePanelGroup>();
	[HideInInspector]
	public List<PopUpPanel> ActivePopUpPanels = new List<PopUpPanel>();

	//	Singleton
	private static InterfaceController _instance;
	public static InterfaceController Instance
	{
		get
		{
			if (_instance != null)
			{
				return _instance;
			}

			_instance = FindObjectOfType<InterfaceController>();
			return _instance;
		}
	}

	void Start()
	{
		RootLevelPanels = new List<InterfacePanel>();
		ActivePanelGroups = new List<InterfacePanelGroup>();
		ActivePopUpPanels = new List<PopUpPanel>();

		ContentRoot.GetComponent<LayoutElement>().flexibleWidth = Screen.width;
		ContentRoot.GetComponent<LayoutElement>().flexibleHeight = Screen.height;

		//	Assign button callbacks
		NewPanelButton.onClick.AddListener(OnNewPanelButtonClicked);

		//	Set up Log In Screen
		LogInScreen.OnSuccessfulLogin += LogIn;
	}

	private void OnNewPanelButtonClicked()
	{
		CreateNewPanel(ContentRoot);
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
			InterfaceController.Instance.ThrowError("stray pop up?");
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

	public InterfacePanel CreateNewPanel(Transform parent = null)
	{
		if (parent == null)
		{
			parent = ContentRoot;
		}

		GameObject newPanel = Instantiate(DefaultInterfaceGroupPrefab, parent);
		InterfacePanel panel = newPanel.GetComponent<InterfacePanel>();
		string name = string.Format("New Panel {0}", FindObjectsOfType<InterfacePanel>().ToList().Count);
		panel.Initialize(name);

		if (parent == ContentRoot)
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

	public InterfacePanel CreateNewPanel(InterfacePanelGroup parent)
	{
		InterfacePanel newPanel = CreateNewPanel(parent.transform);
		return newPanel;
	}

	public InterfacePanel CreateNewPanel(GameObject contentPrefab, Transform parent = null)
	{
		PanelContent content = Instantiate(contentPrefab).GetComponent<PanelContent>();
		InterfacePanel parentPanel = null;

		if (parent != null)
		{
			parentPanel = parent.GetComponent<InterfacePanel>();
		}

		if (parentPanel != null)
		{
			parentPanel.AddContent(content);
			return parentPanel;
		}
		else
		{
			InterfacePanel newPanel = CreateNewPanel(parent);
			newPanel.AddContent(content);
			return newPanel;
		}
	}
	public InterfacePanel CreateNewPanel(GameObject contentPrefab, InterfacePanelGroup parent)
	{
		InterfacePanel newPanel = CreateNewPanel(contentPrefab, parent.transform);
		return newPanel;
	}

	public void RegisterPanelButtonEvents(InterfacePanel panel)
	{
		panel.PanelCloseButtonClicked += OnPanelCloseButtonClicked;
		panel.PanelSplitHorizontalButtonClicked += OnPanelHorizontalSplitButtonClicked;
		panel.PanelSplitVerticalButtonClicked += OnPanelVerticalSplitButtonClicked;
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
			parent = ContentRoot;
		}

		GameObject newGroupObject = Instantiate(orientation == InterfacePanelGroup.InterfacePanelGroupOrientation.Vertical ? VerticalInterfaceGroupPrefab : HorizontalInterfaceGroupPrefab, parent);
		newGroupObject.transform.SetSiblingIndex(childIndex);
		InterfacePanelGroup newGroup = newGroupObject.GetComponent<InterfacePanelGroup>();
		ActivePanelGroups.Add(newGroup);
		newGroup.InsertPanel(sender);
		newGroup.Initialize();

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
					InterfaceController.Instance.ThrowError("!?");
				}

				ActivePanelGroups[index].RemoveAndDestroyPanel(panel);
			}
			else
			{
				InterfaceController.Instance.ThrowError("panel not being tracked?");
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

	public void ReplacePanel(InterfacePanel newPanel, Transform parent)
	{
		InterfacePanel parentPanel = parent.GetComponent<InterfacePanel>();

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

	public void LogWarning(string warningMessage)
	{
		CreateNewPopUp(popUpText: warningMessage);
		Debug.LogWarning(warningMessage);
	}

	public void ThrowError(System.Exception error, bool throwException = true)
	{
		CreateNewPopUp(popUpText: error.ToString());

		if (throwException)
		{
			throw error;
		}
	}

	public void ThrowError(string error, bool throwException = true)
	{
		CreateNewPopUp(popUpText: error);

		if (throwException)
		{
			throw new System.Exception(error);
		}
	}
}
