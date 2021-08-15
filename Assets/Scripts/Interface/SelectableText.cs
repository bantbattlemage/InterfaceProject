using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableText : MonoBehaviour
{
	public float Padding = 30;

	public InputField TextField { get { if (_text == null) { _text = GetComponent<InputField>(); } return _text; } }
	public RectTransform Rect { get { if (_rect == null) { _rect = GetComponent<RectTransform>(); } return _rect; } }

	private InputField _text;
	private RectTransform _rect;

	public static string LoremIpsum { get { return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum aliquam turpis non suscipit rhoncus. Nulla lectus nisi, tempor ut imperdiet eu, ultricies vitae neque. Duis sollicitudin lorem eget luctus cursus."; } }

	void Awake()
	{
		SetText(LoremIpsum);
	}

	void Update()
	{
		AdjustSize();
	}

	public virtual void SetText(string text, bool setHeight = true)
	{
		TextField.text = text;
		Canvas.ForceUpdateCanvases();
		AdjustSize();
	}

	public void AdjustSize()
	{
		Text t = TextField.textComponent;
		float size = (t.cachedTextGenerator.lines.Count * CalculateLineHeight(t)) + Padding;

		if (size != Rect.rect.size.y)
		{
			Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
		}
	}

	private float CalculateLineHeight(Text text)
	{
		var extents = text.cachedTextGenerator.rectExtents.size * 0.5f;
		var lineHeight = text.cachedTextGeneratorForLayout.GetPreferredHeight("A", text.GetGenerationSettings(extents));

		return lineHeight * text.lineSpacing;

		// var extents = text.cachedTextGenerator.rectExtents.size * 0.5f;
		// var setting = text.GetGenerationSettings(extents);
		// var lineHeight = text.cachedTextGeneratorForLayout.GetPreferredHeight("A", setting);
		// return lineHeight * text.lineSpacing / setting.scaleFactor;
	}
}