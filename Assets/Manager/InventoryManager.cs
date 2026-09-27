using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject itemContent;
    [SerializeField] ItemsPreview itemsPreview;
    // Start is called before the first frame update
    void Start()
    {
        updateInventory();
        StatOnUpgradeScripts.inventoryManager = this;
    }
    public void updateInventory()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < GameData.inventory.Count; i++)
        {
            Transform b = Instantiate(itemContent).transform;
            b.SetParent(transform, false);
            Item d = GameData.items[GameData.inventory[i]];
            b.Find("Name").GetComponent<Text>().text = d.name;
            if (d.descExt == -1)
            {
                b.Find("Desc").GetComponent<Text>().text = d.desc;
            }
            else
            {
                b.Find("Desc").GetComponent<Text>().text = string.Format(d.desc, ((Func<object[]>)GlobalEventManager.events[d.descExt])());
            }
            int i2 = i;
            b.Find("UseButton").GetComponent<Button>().onClick.AddListener(() => UseItem(i2));
        }
    }
    public void UseItem(int id)
    {
        if (((Func<bool>)GlobalEventManager.events[GameData.items[GameData.inventory[id]].use])())
        {
            GameData.inventory.RemoveAt(id);
        }
        updateInventory();
        itemsPreview.UpdatePreview();
    }
}
