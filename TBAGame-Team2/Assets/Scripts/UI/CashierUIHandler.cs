using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CashierUIHandler : MonoBehaviour
{
    public void OnNumberButtonPressed(TextMeshProUGUI numberText)
    {
        int number = int.Parse(numberText.text);
        Cashier.Instance.OnNumberPressed(number);
    }
    
    public void OnAddButtonPressed()
    {
        Cashier.Instance.OnAddPressed();
    }
    
    public void OnEqualsButtonPressed()
    {
        Cashier.Instance.OnEqualsPressed();
    }
    
    public void OnClearButtonPressed()
    {
        Cashier.Instance.OnClearPressed();
    }
    
    public void OnDenominationButtonPressed(TextMeshProUGUI denominationText) // Changed parameter type
    {
        int value = int.Parse(denominationText.text.Replace("$", "").Replace(",", ""));
        Cashier.Instance.OnDenominationSelected(value);
    }
}
