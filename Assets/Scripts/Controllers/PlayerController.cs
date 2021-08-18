using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public GameObject InterfacePrefab;
	public InterfaceController PlayerInterface { get; private set; }
	public string SessionAccessKey { get; private set; }

	public void Initialize()
	{
		if (PlayerInterface == null)
		{
			GameObject playerInterface = Instantiate(InterfacePrefab, transform);
			PlayerInterface = playerInterface.GetComponent<InterfaceController>();
		}

		if (GameController.Instance.LogInEnabled)
		{
			PlayerInterface.LogInScreen.gameObject.SetActive(true);
			PlayerInterface.LogInScreen.InitializeLogInScreen();
		}
	}

	public void SetSessionAccessKey(string newAccessKey)
	{
		SessionAccessKey = newAccessKey;
		Debug.LogWarning(SessionAccessKey);
	}
}
