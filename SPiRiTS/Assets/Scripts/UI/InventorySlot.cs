using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text amount;
    [SerializeField] private TMP_Text cooldownTimerText;

    Item item;
    private Color originalIconColor;
    private Color originalBackgroundColor;
    private float currentTimer = 0f;
    private bool canUse = true;


    private void Start()
    {
        originalBackgroundColor = background.color;
    }


    private void Update()
    {
        if (!canUse)
        {
            currentTimer -= Time.deltaTime;

            if (currentTimer <= 0f)
            {
                EndCooldown();
            }
            else
            {
                cooldownTimerText.text = currentTimer.ToString("F1");
            }
        }
    }


    public void AddItem(Item newItem, int count)
    {
        if (item != null)
        {
            item = newItem;

            icon.sprite = newItem.Icon;
            icon.gameObject.SetActive(true);
            originalIconColor = icon.color;
        }    

        amount.text = count.ToString();
    }


    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.gameObject.SetActive(false);

        amount.text = "";
    }


    public void UseItem()
    {
        if (canUse)
        {
            item?.Use();
            StartCooldown();
        }
    }


    public void StartCooldown()
    {
        canUse = false;
        icon.color = Color.gray;
        background.color = Color.gray;

        currentTimer = item.CooldownTimer;

        cooldownTimerText.text = currentTimer.ToString();
    }


    public void EndCooldown()
    {
        canUse = true;
        icon.color = originalIconColor;
        background.color = originalBackgroundColor;

        currentTimer = 0f;

        cooldownTimerText.text = "";
    }
}
