using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject PlayerControllerPrefab;

	public PlayerController Player { get; private set; }

    private GameController _instance;
    public GameController Instance
    {
        get
        {
			if(_instance == null)
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
    }
}
