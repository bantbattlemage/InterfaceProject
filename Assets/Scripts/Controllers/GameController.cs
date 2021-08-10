using RestClient.Core;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public string ServerURL = "";

	public GameObject PlayerControllerPrefab;

	public PlayerController Player { get; private set; }

	private GameController _instance;
	public GameController Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
			}

			return _instance;
		}
	}

	void Start()
	{
		GameObject newPlayerObject = Instantiate(PlayerControllerPrefab);
		Player = newPlayerObject.GetComponent<PlayerController>();
		Player.Initialize();

		StartCoroutine(RestWebClient.Instance.HttpGet(ServerURL, (r) => OnRequestComplete(r)));
	}

	void OnRequestComplete(Response response)
	{
		Debug.Log($"Status Code: {response.StatusCode}");
		Debug.Log($"Data: {response.Data}");
		Debug.Log($"Error: {response.Error}");
	}

	void OnGet(string result)
	{
		Debug.Log(result);
	}
}
