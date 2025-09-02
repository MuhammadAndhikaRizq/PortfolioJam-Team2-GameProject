using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Money : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image moneyImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Button moneyButton;

    private int moneyAmount;
    private bool isRevealed = false;
    
    // Event for when money is revealed
    public System.Action<int> OnMoneyRevealed;

    void Start()
    {
        if (amountText != null)
        {
            amountText.gameObject.SetActive(false);
        }

        if (moneyButton != null)
        {
            moneyButton.onClick.AddListener(RevealAmount);
        }
        
        gameObject.SetActive(false);
    }
    
    public void Setup(int amount, Sprite moneySprite)
    {
        moneyAmount = amount;
        
        if (moneyImage != null && moneySprite != null)
        {
            moneyImage.sprite = moneySprite;
        }
    }
    
    void RevealAmount()
    {
        if (isRevealed) return;
        
        isRevealed = true;
        
        if (amountText != null)
        {
            amountText.text = $"${moneyAmount}";
            amountText.gameObject.SetActive(true);
        }
        
        OnMoneyRevealed?.Invoke(moneyAmount);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        isRevealed = false;
        
        if (amountText != null)
        {
            amountText.gameObject.SetActive(false);
        }
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        isRevealed = false;
        
        if (amountText != null)
        {
            amountText.gameObject.SetActive(false);
        }
    }
}
