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
    private float currentTimer = 0f;
    private bool canUse = true;
    private bool needsText = true;
    private int amountInt;
    private Inventory inventory;


    private void Start()
    {
        inventory = Inventory.Instance;
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
                if (needsText)
                    cooldownTimerText.text = currentTimer.ToString("F1");
            }
        }
    }


    public void AddItem(Item newItem, int count)
    {
        amountInt = count;
        amount.text = amountInt.ToString();

        item = newItem;

        icon.gameObject.SetActive(true);
        icon.sprite = newItem.Icon;
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
            if (item != null)
            {
                item.Use();
                int remaining = amountInt - 1;

                if (remaining > 0)
                {
                    inventory.Remove(item, item.PlayerIndex);
                    StartCooldown();
                }
                else
                {
                    Item tempItem = item;
                    ClearSlot();
                    inventory.Remove(tempItem, tempItem.PlayerIndex);
                    RemovedItemCooldown();
                }
            }
        }
    }


    public void StartCooldown()
    {
        canUse = false;
        icon.color = Color.gray;
        background.color = Color.gray;

        amount.text = amountInt.ToString();
        currentTimer = item.CooldownTimer;

        cooldownTimerText.text = currentTimer.ToString();
    }


    public void EndCooldown()
    {
        canUse = true;
        needsText = true;
        icon.color = Color.white;
        background.color = Color.white;

        currentTimer = 0f;

        cooldownTimerText.text = "";
    }


    public void RemovedItemCooldown()
    {
        canUse = false;
        needsText = false;
        currentTimer = .1f;
    }
}
