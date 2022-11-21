using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenChestEvent : MonoBehaviour
{
    [System.Serializable]
    public struct CHEST_ITEM {
        public ITEM_TYPE ItemType;
        public int ItemId;
    }

    [SerializeField]
    private bool isOpen = false;
    [SerializeField]
    private List<CHEST_ITEM> chestItemData = new List<CHEST_ITEM>();
    [SerializeField]
    private Sprite chestOpenedSprite = null;

    [TextArea(3, 10)]
    public string eventMessageOpening = "";

    /// <summary>
    /// 宝箱を開けるイベント
    /// </summary>
    /// <returns>中身のList</returns>
    public List<CHEST_ITEM> OpenChest(){
        // まだ開いて無ければ開ける
        if(!isOpen){
            this.GetComponent<SpriteRenderer>().sprite = chestOpenedSprite;
            isOpen = true;
            return chestItemData;
        }

        return null;
    }
}
