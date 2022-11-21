using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorEventController : MonoBehaviour
{
    [SerializeField]
    private MasterData masterData;

    public void CharactorEvent(GameObject eventTarget){
        switch(EventTags.CastStrToEventTag(eventTarget.tag)){
            case EventTags.EVENT_TAG.CHEST:
                List<OpenChestEvent.CHEST_ITEM> chestItems = eventTarget.GetComponent<OpenChestEvent>().OpenChest();
                string eventMessage = "#dispclear" + System.Environment.NewLine;
                eventMessage += eventTarget.GetComponent<OpenChestEvent>().eventMessageOpening + System.Environment.NewLine;

                if(chestItems != null){
                    foreach(OpenChestEvent.CHEST_ITEM items in chestItems){
                        switch(items.ItemType){
                            case ITEM_TYPE.MATERIAL:
                                if(masterData.MaterialMaster.Count > items.ItemId){
                                    eventMessage += masterData.MaterialMaster[items.ItemId].Name + " を手に入れた。[l]" + System.Environment.NewLine;
                                }
                            break;
                            case ITEM_TYPE.TOOL:
                                if(masterData.ToolMaster.Count > items.ItemId){
                                    eventMessage += masterData.ToolMaster[items.ItemId].Name + " を手に入れた。[l]" + System.Environment.NewLine;
                                }
                            break;
                            case ITEM_TYPE.EQUIPMENT:
                                if(masterData.EquipmentMaster.Count > items.ItemId){
                                    eventMessage += masterData.EquipmentMaster[items.ItemId].Name + " を手に入れた。[l]" + System.Environment.NewLine;
                                }
                            break;
                        }
                    }
                } else {
                    eventMessage += "宝箱はからっぽだ[l]";
                }
                CommonSys.GetSystem<MapWindow>().DispEventMessage(eventMessage);
            break;

            case EventTags.EVENT_TAG.MESSAGE:
                // イベントメッセージを表示
                CommonSys.GetSystem<MapWindow>().DispEventMessage(eventTarget.GetComponent<MessageEvent>().GetEventMessage());
            break;
        }
    }
}
