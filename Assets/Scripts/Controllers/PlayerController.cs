using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public GameObject InterfacePrefab;
	public InterfaceController PlayerInterface { get; private set; }

    public void Initialize()
	{
        if (PlayerInterface == null)
        {
            GameObject playerInterface = Instantiate(InterfacePrefab, transform);
            PlayerInterface = playerInterface.GetComponent<InterfaceController>();
            PlayerInterface.LogInScreen.gameObject.SetActive(true);
        }
	}
}
