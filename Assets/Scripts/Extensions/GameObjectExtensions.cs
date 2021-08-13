using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public static class GameObjectExtensions
{
	public static void SafeDestroy(this GameObject obj)
	{
		obj.transform.SetParent(null);
		obj.name = "$disposed";
		UnityEngine.Object.Destroy(obj);
		obj.SetActive(false);
	}

	public static bool IsPointerOverUIElement(this GameObject source, Vector2 mousePos, out PointerEventData eventData)
	{
		eventData = new PointerEventData(EventSystem.current);
		eventData.position = mousePos;

		Debug.LogWarning(eventData.position);

		List<Transform> children = source.transform.GetComponentsInChildren<Transform>().ToList();
		children.Remove(source.transform);
		children.ForEach(x => x.gameObject.SetActive(false));

		List<RaycastResult> raycastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, raycastResults);

		children.ForEach(x => x.gameObject.SetActive(true));

		for (int index = 0; index < raycastResults.Count; index++)
		{
			RaycastResult curRaysastResult = raycastResults[index];
			Debug.LogWarning(curRaysastResult.gameObject.name);
			curRaysastResult.gameObject.name = "HIT !!!";

			if (curRaysastResult.gameObject.transform.parent == source)
			{
				return true;
			}

		}

		return false;
	}
}
