using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{

    public Sprite heart;
    public Sprite heartH;

    public Sprite blueHeart;
    public Sprite blueHeartH;

    public Sprite yellowHeart;
    public Sprite yellowHeartH;

    public Sprite purpleHeart;
    public Sprite purpleHeartH;

    public Sprite heartSplash;
    public Sprite emptySprite;


    public Image[] hearts;
    public Image[] blueHearts;
    public Image[] yellowHearts;
    public Image[] purpleHearts;

    public Image currentItemTypeSelected;
    public TextMeshProUGUI numberOfCurrentItemTypeSelected;

    public Image foodPanel;
    public Image healthPanel;
    public Image moodPanel;

    public void UpdatePanels()
    {
        UpdateHearts();
        UpdateFood();
        UpdateHealth();
        UpdateMood();
        UpdateCurrentItemTypeSelected();
    }

    public void UpdateHearts()
    {
        float currentHeartsValue = GameController.instance.player.hitPoints;
        UpdateHeartsPanel(heart, heartH, heartSplash, hearts,  currentHeartsValue, Mathf.RoundToInt(GameController.instance.player.maxHitPoints/2));
    }

    public void UpdateFood()
    {
        float currentFoodValue = GameController.instance.pet.food;
        UpdateHeartsPanel(yellowHeart, yellowHeartH, emptySprite, yellowHearts, currentFoodValue, 3);

        float maxFoodValue = GameController.instance.pet.maxFood;
        if (maxFoodValue < 6f)
            foodPanel.sprite = GameController.instance.halvedPanel;
        else
            foodPanel.sprite = GameController.instance.normalPanel;
    }

    public void UpdateHealth()
    {
        float currentHealthValue = GameController.instance.pet.health;
        UpdateHeartsPanel(blueHeart, blueHeartH, emptySprite, blueHearts, currentHealthValue, 3);

        float maxHealthValue = GameController.instance.pet.maxHealth;
        if (maxHealthValue < 6f)
            healthPanel.sprite = GameController.instance.halvedPanel;
        else
            healthPanel.sprite = GameController.instance.normalPanel;
    }

    public void UpdateMood()
    {
        float currentMoodValue = GameController.instance.pet.happy;
        UpdateHeartsPanel(purpleHeart, purpleHeartH, emptySprite, purpleHearts, currentMoodValue, 3);

        float maxMoodValue = GameController.instance.pet.maxHappy;
        if (maxMoodValue < 6f)
            moodPanel.sprite = GameController.instance.halvedPanel;
        else
            moodPanel.sprite = GameController.instance.normalPanel;
    }

    void UpdateHeartsPanel(Sprite fullHeart, Sprite halfHeart, Sprite emptyHeart, Image[] heartList, float currentValue, int maximum)
    {

        int howManyHalfHearts = Mathf.RoundToInt(currentValue);
        int howManyFullHearts = Mathf.FloorToInt(howManyHalfHearts / 2f);
        howManyHalfHearts = Mathf.Max(howManyHalfHearts - howManyFullHearts * 2, 0);
        int howManyEmptyHearts = maximum - howManyFullHearts - howManyHalfHearts;

        //Debug.Log("TOTAL " + currentValue + ", " + howManyFullHearts + ", " + howManyHalfHearts + ", " + howManyEmptyHearts);

        int index = 0;

        for (int i = 0; i < howManyFullHearts; i++)
        {
            heartList[i].sprite = fullHeart;
            index = i;
        }

        index = howManyFullHearts;

        for (int i = index; i < howManyHalfHearts + index; i++)
        {
            if (i == -1)
                Debug.Log("Current Value " + currentValue);
            heartList[i].sprite = halfHeart;
            index = i;
        }

        index = howManyFullHearts + howManyHalfHearts;

        for (int i = index; i < howManyEmptyHearts + index; i++)
        {
            heartList[i].sprite = emptyHeart;
        }
    }

    public void UpdateCurrentItemTypeSelected()
    {
        if (GameController.instance.playerItemsManager.currentItemTypeSelectedIndex == -1) //No item
        {
            currentItemTypeSelected.sprite = emptySprite;
            numberOfCurrentItemTypeSelected.text = "";
            return;
        }

        var currentItemType = GameController.instance.playerItemsManager.itemKeys[GameController.instance.playerItemsManager.currentItemTypeSelectedIndex];
        currentItemTypeSelected.sprite = GameController.instance.playerItemsManager.GetSpriteForType(currentItemType);

        var amountOfCurrentItemType = GameController.instance.playerItemsManager.items[currentItemType];
        numberOfCurrentItemTypeSelected.text = "x" + amountOfCurrentItemType.ToString();
    }

}
