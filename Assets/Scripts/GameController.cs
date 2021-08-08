using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject PlayerControllerPrefab;

	public PlayerController Player { get; private set; }

	void Start()
	{
		GameObject newPlayerObject = Instantiate(PlayerControllerPrefab);
		Player = newPlayerObject.GetComponent<PlayerController>();
	}
}
