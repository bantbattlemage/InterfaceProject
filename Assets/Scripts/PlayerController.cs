using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public GameObject InterfacePrefab;
	public InterfaceController PlayerInterface;

	void Start()
	{
		if (PlayerInterface == null)
		{
			GameObject playerInterface = Instantiate(InterfacePrefab, transform);
			PlayerInterface = playerInterface.GetComponent<InterfaceController>();
			PlayerInterface.LogInScreen.gameObject.SetActive(true);
		}
	}
}
