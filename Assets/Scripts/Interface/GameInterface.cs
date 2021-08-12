using UnityEngine;
using UnityEngine.UI;

public static class GameInterface
{
	/// <summary>
	///	Coords should be top-left anchored
	/// </summary>
	public static Vector2 GetDistanceFromAnchor(RectTransform source, Vector2 position)
	{
		float x = position.x - source.position.x;
		float y = source.position.y - position.y;

		return new Vector2(x, y);
	}
}