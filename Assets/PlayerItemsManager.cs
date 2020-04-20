using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerItemsManager : MonoBehaviour
{

    public Dictionary<ItemController.Type, int> items = new Dictionary<ItemController.Type, int>();
    public ItemController.Type[] itemKeys;

    public int currentItemTypeSelectedIndex;

    public AudioClip pickupItemSound;
    public AudioClip useItemSound;

    private void Start()
    {
        currentItemTypeSelectedIndex = -1;
    }

    public void AddItem(ItemController.Type itemType)
    {

        SoundManager.instance.PlaySingle(pickupItemSound);

        if (items.ContainsKey(itemType))
            items[itemType]++;
        else
            items.Add(itemType, 1);

        itemKeys = items.Keys.ToArray();

        currentItemTypeSelectedIndex = GetIndexOfItemType(itemType);
        GameController.instance.HUD.UpdateCurrentItemTypeSelected();
    }

    public void SubstractItem(ItemController.Type itemType)
    {

        if (items.ContainsKey(itemType) && items[itemType] > 0)
        {
            items[itemType]--;
            if (items[itemType] == 0)
            {
                items.Remove(itemType);
                itemKeys = items.Keys.ToArray();

                if (itemKeys.Length == 0)
                    currentItemTypeSelectedIndex = -1;
                else
                    currentItemTypeSelectedIndex = 0;
            }
        }
    }

    public void UseItem(ItemController.Type itemType)
    {
        if (GameController.instance.pet.IsPlayerCloseToPet())
        {
            GameController.instance.pet.ItemGifted(itemType);
            SubstractItem(itemType);

            SoundManager.instance.PlaySingle(useItemSound);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire2"))
        {
            if (currentItemTypeSelectedIndex != -1)
                UseItem(itemKeys[currentItemTypeSelectedIndex]);
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            SelectNextItem();
            GameController.instance.HUD.UpdateCurrentItemTypeSelected();
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            SelectPreviousItem();
            GameController.instance.HUD.UpdateCurrentItemTypeSelected();
        }
    }

    int GetIndexOfItemType(ItemController.Type itemType)
    {
        for (int i = 0; i < itemKeys.Length; i++)
        {
            if (itemKeys[i] == itemType)
                return i;
        }
        return -1;
    }

    void SelectNextItem()
    {
        currentItemTypeSelectedIndex++;

        if (currentItemTypeSelectedIndex >= itemKeys.Length)
            currentItemTypeSelectedIndex = 0;
    }

    void SelectPreviousItem()
    {
        currentItemTypeSelectedIndex--;
        if (currentItemTypeSelectedIndex < 0)
            currentItemTypeSelectedIndex = itemKeys.Length - 1;
    }

    public Sprite GetSpriteForType(ItemController.Type type)
    {
        Sprite sprite = null;
        switch (type)
        {
            case ItemController.Type.MEDICINE:
                sprite = GameController.instance.medicine;
                break;
            case ItemController.Type.FOOD:
                sprite = GameController.instance.food;
                break;
            case ItemController.Type.TOY:
                sprite = GameController.instance.toy;
                break;
            case ItemController.Type.GEM:
                sprite = GameController.instance.gem;
                break;
        }
        return sprite;
    }


}
