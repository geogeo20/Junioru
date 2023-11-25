using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    [SerializeField] private List<OrderItem> orderItems;
    [SerializeField] private Image characterVisual;
    [SerializeField] private TMP_Text orderReward;
    [SerializeField] private GameObject serveButton;


    private GridManager gridManagerRef;

    public void Init(Sprite charcterSprite, int reward, List<Sprite> itemsSprite, List<int> itemsAmmount, GridManager gridRef)
    {
        foreach (var item in orderItems)
        {
            item.ToogleItem(false);
        }

        characterVisual.sprite = charcterSprite;
        orderReward.text = string.Format("+{0}", reward.ToString());

        for (int i = 0; i < itemsSprite.Count; i++)
        {
            if (itemsAmmount[i] > 0)
            {
                orderItems[i].SetItem(itemsSprite[i], itemsAmmount[i]);
            }
        }

        gridManagerRef = gridRef;
    }

    public void ToogleServeButton(bool toogle)
    {
        serveButton.SetActive(toogle);
    }

    public void ServeOrder()
    {
        gridManagerRef.ServeOrder();
    }
}

[Serializable]
public class OrderItem
{
    public Image ItemVisual;
    public TMP_Text ItemAmmount;
    public GameObject ItemHolder;

    public void SetItem(Sprite visual, int ammount)
    {
        ItemVisual.sprite = visual;
        ItemAmmount.text = string.Format("x{0}", ammount);
        ToogleItem(true);
    }

    public void ToogleItem(bool toogle)
    {
        ItemHolder.SetActive(toogle);
    }
}