using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTags 
{
    public enum EVENT_TAG {
        UNTAGGED,
        NONE,
        GOAL,
        CHEST,
        MESSAGE
    }

    public static EVENT_TAG CastStrToEventTag(string tag){
        return CommonUtil.ParseEnum<EVENT_TAG>(tag.ToUpper());
    }

    public static bool isNormalEventTag(string tag){
        EVENT_TAG eventTag = CastStrToEventTag(tag);
        switch(eventTag){
            case EVENT_TAG.CHEST:
            case EVENT_TAG.MESSAGE:
                return true;
            case EVENT_TAG.GOAL:
            case EVENT_TAG.UNTAGGED:
            case EVENT_TAG.NONE:
            default:
                return false;
        }
    }
}