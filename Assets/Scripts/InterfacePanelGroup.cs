using System.Collections.Generic;
using UnityEngine;

public class InterfacePanelGroup : MonoBehaviour
{
    private List<InterfacePanel> _subPanels = new List<InterfacePanel>();

    public void AddPanel(InterfacePanel panel)
    {
        _subPanels.Add(panel);
    }

    public void RemoveAndDestroyPanel(InterfacePanel panel)
    {
        if(!_subPanels.Contains(panel))
        {
            throw new System.Exception("Tried to remove panel that is not in group");
        }

        _subPanels.Remove(panel);
        Object.Destroy(panel.gameObject);
    }

    public void Cleanup()
    {
        if (_subPanels.Count == 0)
        {
            Object.Destroy(gameObject);
        }

        Debug.LogWarning(_subPanels.Count);
    }
}
