using UnityEngine;

public enum AnchorPresets
{
	TopLeft,
	TopCenter,
	TopRight,

	MiddleLeft,
	MiddleCenter,
	MiddleRight,

	BottomLeft,
	BottomCenter,
	BottomRight,
	BottomStretch,

	VertStretchLeft,
	VertStretchRight,
	VertStretchCenter,

	HorStretchTop,
	HorStretchMiddle,
	HorStretchBottom,

	StretchAll
}

public enum PivotPresets
{
	TopLeft,
	TopCenter,
	TopRight,

	MiddleLeft,
	MiddleCenter,
	MiddleRight,

	BottomLeft,
	BottomCenter,
	BottomRight,
}

public static class RectTransformExtensions
{
	public static bool IsRectTransformInsideSreen(this RectTransform rectTransform)
	{
		bool isInside = false;
		Vector3[] corners = new Vector3[4];
		rectTransform.GetWorldCorners(corners);
		int visibleCorners = 0;
		Rect rect = new Rect(0, 0, Screen.width, Screen.height);
		foreach (Vector3 corner in corners)
		{
			if (rect.Contains(corner))
			{
				visibleCorners++;
			}
		}
		if (visibleCorners == 4)
		{
			isInside = true;
		}
		return isInside;
	}

	public static void SetAnchor(this RectTransform source, AnchorPresets allign, int offsetX = 0, int offsetY = 0)
	{
		//source.anchoredPosition = new Vector3(offsetX, offsetY, 0);

		switch (allign)
		{
			case (AnchorPresets.TopLeft):
				{
					source.anchorMin = new Vector2(0, 1);
					source.anchorMax = new Vector2(0, 1);
					break;
				}
			case (AnchorPresets.TopCenter):
				{
					source.anchorMin = new Vector2(0.5f, 1);
					source.anchorMax = new Vector2(0.5f, 1);
					break;
				}
			case (AnchorPresets.TopRight):
				{
					source.anchorMin = new Vector2(1, 1);
					source.anchorMax = new Vector2(1, 1);
					break;
				}

			case (AnchorPresets.MiddleLeft):
				{
					source.anchorMin = new Vector2(0, 0.5f);
					source.anchorMax = new Vector2(0, 0.5f);
					break;
				}
			case (AnchorPresets.MiddleCenter):
				{
					source.anchorMin = new Vector2(0.5f, 0.5f);
					source.anchorMax = new Vector2(0.5f, 0.5f);
					break;
				}
			case (AnchorPresets.MiddleRight):
				{
					source.anchorMin = new Vector2(1, 0.5f);
					source.anchorMax = new Vector2(1, 0.5f);
					break;
				}

			case (AnchorPresets.BottomLeft):
				{
					source.anchorMin = new Vector2(0, 0);
					source.anchorMax = new Vector2(0, 0);
					break;
				}
			case (AnchorPresets.BottomCenter):
				{
					source.anchorMin = new Vector2(0.5f, 0);
					source.anchorMax = new Vector2(0.5f, 0);
					break;
				}
			case (AnchorPresets.BottomRight):
				{
					source.anchorMin = new Vector2(1, 0);
					source.anchorMax = new Vector2(1, 0);
					break;
				}

			case (AnchorPresets.HorStretchTop):
				{
					source.anchorMin = new Vector2(0, 1);
					source.anchorMax = new Vector2(1, 1);
					break;
				}
			case (AnchorPresets.HorStretchMiddle):
				{
					source.anchorMin = new Vector2(0, 0.5f);
					source.anchorMax = new Vector2(1, 0.5f);
					break;
				}
			case (AnchorPresets.HorStretchBottom):
				{
					source.anchorMin = new Vector2(0, 0);
					source.anchorMax = new Vector2(1, 0);
					break;
				}

			case (AnchorPresets.VertStretchLeft):
				{
					source.anchorMin = new Vector2(0, 0);
					source.anchorMax = new Vector2(0, 1);
					break;
				}
			case (AnchorPresets.VertStretchCenter):
				{
					source.anchorMin = new Vector2(0.5f, 0);
					source.anchorMax = new Vector2(0.5f, 1);
					break;
				}
			case (AnchorPresets.VertStretchRight):
				{
					source.anchorMin = new Vector2(1, 0);
					source.anchorMax = new Vector2(1, 1);
					break;
				}

			case (AnchorPresets.StretchAll):
				{
					source.anchorMin = new Vector2(0, 0);
					source.anchorMax = new Vector2(1, 1);
					break;
				}
		}
	}

	public static void SetPivot(this RectTransform source, PivotPresets preset)
	{
		Vector2 size = source.rect.size;
		Vector2 newPivot = new Vector2();

		switch (preset)
		{
			case (PivotPresets.TopLeft):
				{
					newPivot = new Vector2(0, 1);
					break;
				}
			case (PivotPresets.TopCenter):
				{
					newPivot = new Vector2(0.5f, 1);
					break;
				}
			case (PivotPresets.TopRight):
				{
					newPivot = new Vector2(1, 1);
					break;
				}

			case (PivotPresets.MiddleLeft):
				{
					newPivot = new Vector2(0, 0.5f);
					break;
				}
			case (PivotPresets.MiddleCenter):
				{
					newPivot = new Vector2(0.5f, 0.5f);
					break;
				}
			case (PivotPresets.MiddleRight):
				{
					newPivot = new Vector2(1, 0.5f);
					break;
				}

			case (PivotPresets.BottomLeft):
				{
					newPivot = new Vector2(0, 0);
					break;
				}
			case (PivotPresets.BottomCenter):
				{
					newPivot = new Vector2(0.5f, 0);
					break;
				}
			case (PivotPresets.BottomRight):
				{
					newPivot = new Vector2(1, 0);
					break;
				}
		}

		Vector2 deltaPivot = source.pivot - newPivot;
		Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
		source.pivot = newPivot;
		source.localPosition -= deltaPosition;
	}
}