using UnityEngine;

public static class Helpers
{
	public static void SafeDestory(this GameObject obj)
	{
		obj.transform.SetParent(null);
		obj.name = "$disposed";
		UnityEngine.Object.Destroy(obj);
		obj.SetActive(false);
	}
}
