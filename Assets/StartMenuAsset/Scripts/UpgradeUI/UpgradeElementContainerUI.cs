using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeElementContainerUI : MonoBehaviour
{
    [SerializeField] private UpgradeElementUI[] elements;
    [SerializeField] private Button checkBox;
    [SerializeField] private UpgradeDescriptionUI descriptionUI;

    private List<Button> buttons;
    private UpgradeElementUI chosenElement;

    private void Awake()
    {
        buttons = new List<Button>();

        foreach (UpgradeElementUI element in elements)
        {
            buttons.Add(element.GetButton());
        }
    }

    private void Start()
    {
        foreach (UpgradeElementUI element in elements)
        {
            element.GetButton().onClick.AddListener(() => chosenElement = element);
            
            //if (DataSystem.Instance.gameValueData.elementLevelDic.ContainsKey(element.GetUpgradeElementSO().name))
            //{
            //    element.Set(DataSystem.Instance.gameValueData.elementLevelDic[element.GetUpgradeElementSO().name]);
            //}
        }


        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() =>
            {
                DisableAllButtonsOutline(buttons);
                EnableOutline(button);
                descriptionUI.UpdateDescription();
            });
        }

        checkBox.onClick.AddListener(() =>
        {
            chosenElement.Upgrade();
            //DataSystem.Instance.SaveElementData(chosenElement.GetUpgradeElementSO().name, chosenElement.GetUpgradeElementSO().currentLevel);
        });
    }

    public void DisableAllButtonsOutline(List<Button> buttons)
    {
        foreach (Button button in buttons)
        {
            DisableOutline(button);
        }
    }

    public UpgradeElementSO GetElementSO()
    {
        return chosenElement.GetUpgradeElementSO();
    }

    private void EnableOutline(Button button)
    {
        button.transform.GetComponent<Outline>().enabled = true;
    }

    private void DisableOutline(Button button)
    {
        button.transform.GetComponent<Outline>().enabled = false;
    }

}
