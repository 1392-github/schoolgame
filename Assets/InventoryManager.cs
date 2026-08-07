using System.Collections;
using System.Collections.Generic;
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
            b.Find("Desc").GetComponent<Text>().text = string.Format(d.desc, d.descExt?.Invoke() ?? new object[0]);
            int i2 = i;
            b.Find("UseButton").GetComponent<Button>().onClick.AddListener(() => UseItem(i2));
        }
    }
    public void UseItem(int id)
    {
        if (GameData.items[GameData.inventory[id]].use?.Invoke() ?? false)
        {
            GameData.inventory.RemoveAt(id);
        }
        updateInventory();
        itemsPreview.UpdatePreview();
    }
}
