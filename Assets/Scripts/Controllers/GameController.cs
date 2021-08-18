using RestClient.Core;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public string ServerURL = "https://marketgame.azurewebsites.net/api/";
	public bool UseLocalHost;
	public bool LogInEnabled;
	public GameObject PlayerControllerPrefab;

	public PlayerController Player { get; private set; }
	public string SessionToken { get; set; }

	private static string LocalHost { get { return "https://localhost:5001/api/"; } }

	private static GameController _instance;
	public static GameController Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameController>();
			}

			return _instance;
		}
	}

	void Start()
	{
		if (UseLocalHost)
		{
			ServerURL = LocalHost;
		}

		GameObject newPlayerObject = Instantiate(PlayerControllerPrefab);
		Player = newPlayerObject.GetComponent<PlayerController>();
		Player.Initialize();
		ChatController.Initialize();
	}
}
