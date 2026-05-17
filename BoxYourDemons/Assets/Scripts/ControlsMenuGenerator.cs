using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[System.Serializable] 
public struct ManualControlBinding
{
    public string actionName; 
    public string keyName;    
}

public class ControlsMenuGenerator : MonoBehaviour
{
    [Header("References")]
    public InputActionAsset inputAsset; 
    public Transform contentContainer;  
    public GameObject controlRowPrefab; 

    [Header("Manual Additions")]
    public ManualControlBinding[] manualBindings; 

    private void OnEnable()
    {
        GenerateTable();
    }

    private void GenerateTable()
    {
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        InputActionMap playerMap = inputAsset.FindActionMap("Player");
        if (playerMap != null)
        {
            foreach (InputAction action in playerMap.actions)
            {
                foreach (InputBinding binding in action.bindings)
                {
                    if (binding.isComposite) continue;
                    
                    string actionDisplay = binding.isPartOfComposite ? 
                        $"{action.name} ({binding.name})" : action.name;

                    CreateRow(actionDisplay, binding.ToDisplayString());
                }
            }
        }

        foreach (ManualControlBinding manual in manualBindings)
        {
            CreateRow(manual.actionName, manual.keyName);
        }
    }

    private void CreateRow(string actionText, string keyText)
    {
        GameObject newRow = Instantiate(controlRowPrefab, contentContainer);
        TMP_Text[] texts = newRow.GetComponentsInChildren<TMP_Text>();
        
        texts[0].text = actionText;
        texts[1].text = keyText;
    }
}
