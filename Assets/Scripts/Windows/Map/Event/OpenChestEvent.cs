using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenChestEvent : MonoBehaviour
{
    [SerializeField]
    private bool isOpen = false;
    [SerializeField]
    private int ItemId = 0;

    [SerializeField]
    private Sprite chestOpenedSprite = null;

    public int OpenChest(){
        // まだ開いて無ければ開ける
        if(!isOpen){
            this.GetComponent<SpriteRenderer>().sprite = chestOpenedSprite;
            isOpen = true;
            return ItemId;
        }

        return -1;
    }
}
