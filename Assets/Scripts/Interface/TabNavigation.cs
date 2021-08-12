using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TabNavigation : MonoBehaviour
{
	public List<TabNavigatableInputField> InputFields;
	int _fieldIndexer;

	void Awake()
	{
		InputFields = gameObject.GetComponentsInChildren<TabNavigatableInputField>(false).ToList();

		foreach (TabNavigatableInputField inputField in InputFields)
		{
			inputField.OnPointerClicked += (e, s) => { _fieldIndexer = InputFields.IndexOf(s); };
			inputField.OnPointerDeselected += (e, s) => { /*_fieldIndexer = 0; */};
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (!InputFields.Any(x => x.isFocused))
			{
				return;
			}

			_fieldIndexer++;
			if (_fieldIndexer >= InputFields.Count)
			{
				_fieldIndexer = 0;
			}

			InputFields[_fieldIndexer].Select();
		}
	}
}
