using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableText : MonoBehaviour
{
	public float Padding = 30;

	public InputField Input { get { if (_text == null) { _text = GetComponent<InputField>(); } return _text; } }
	private InputField _text;

	public static string LoremIpsum { get { return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum aliquam turpis non suscipit rhoncus. Nulla lectus nisi, tempor ut imperdiet eu, ultricies vitae neque. Duis sollicitudin lorem eget luctus cursus."; } }
	void Awake()
	{
		SetText(LoremIpsum);
	}

	void OnGUI()
	{
		Text t = Input.textComponent;
		float size = (t.cachedTextGenerator.lines.Count * CalculateLineHeight(t) + Padding);
		GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
	}

	public virtual void SetText(string text, bool setHeight = true)
	{
		Text t = Input.textComponent;
		Input.text = text;
		Canvas.ForceUpdateCanvases();

		// float size = (t.cachedTextGenerator.lines.Count * CalculateLineHeight(t));
		// GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);

		// for (int i = 0; i < myText.cachedTextGenerator.lines.Count; i++)
		// {
		// 	int startIndex = myText.cachedTextGenerator.lines[i].startCharIdx;
		// 	int endIndex = (i == myText.cachedTextGenerator.lines.Count - 1) ? myText.text.Length
		// 		: myText.cachedTextGenerator.lines[i + 1].startCharIdx;
		// 	int length = endIndex - startIndex;
		// 	Debug.Log(myText.text.Substring(startIndex, length));
		// }
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