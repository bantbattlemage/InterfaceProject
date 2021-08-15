using RestClient.Core;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public string ServerURL = "";
	public bool LogInEnabled;

	public GameObject PlayerControllerPrefab;

	public PlayerController Player { get; private set; }

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
		GameObject newPlayerObject = Instantiate(PlayerControllerPrefab);
		Player = newPlayerObject.GetComponent<PlayerController>();
		Player.Initialize();

		// StartCoroutine(RestWebClient.Instance.HttpGet(GameController.Instance.ServerURL + "player/", (r) =>
		// {
		// 	Debug.Log(r.Data);
		// }));
	}
}
