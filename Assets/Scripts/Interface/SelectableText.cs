using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableText : MonoBehaviour
{
	public InputField Text { get { if (_text == null) { _text = GetComponent<InputField>(); } return _text; } }
	private InputField _text;

	void Awake()
	{
		Text.text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum aliquam turpis non suscipit rhoncus. Nulla lectus nisi, tempor ut imperdiet eu, ultricies vitae neque. Duis sollicitudin lorem eget luctus cursus. Donec id est nisl. Proin mollis maximus enim a pretium. Nunc urna tortor, condimentum sed lacinia vitae, interdum quis orci. Quisque in porta nibh. Vivamus consequat iaculis odio ac malesuada. Praesent ac purus eu sapien dignissim fringilla ut id libero. Donec nec arcu lorem. Praesent fermentum urna leo, et fringilla sem molestie eu. Nulla vitae mollis eros. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Ut rutrum quam purus, nec elementum tellus sodales eu. Quisque posuere et nibh ut finibus. Aenean posuere, sapien at ornare convallis, sapien elit maximus quam, in pulvinar nisl elit semper nulla. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum aliquam turpis non suscipit rhoncus. Nulla lectus nisi, tempor ut imperdiet eu, ultricies vitae neque. Duis sollicitudin lorem eget luctus cursus. Donec id est nisl. Proin mollis maximus enim a pretium. Nunc urna tortor, condimentum sed lacinia vitae, interdum quis orci. Quisque in porta nibh. Vivamus consequat iaculis odio ac malesuada. Praesent ac purus eu sapien dignissim fringilla ut id libero. Donec nec arcu lorem. Praesent fermentum urna leo, et fringilla sem molestie eu. Nulla vitae mollis eros. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Ut rutrum quam purus, nec elementum tellus sodales eu. Quisque posuere et nibh ut finibus. Aenean posuere, sapien at ornare convallis, sapien elit maximus quam, in pulvinar nisl elit semper nulla.";
	}
}