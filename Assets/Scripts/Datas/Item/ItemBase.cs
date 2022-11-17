using UnityEngine;

public class ItemBase
{
    public int Id {get; set;}
    public string Name {get; set;}
    public string Discription {get; set;}
    public string IconTextureName {get; set;}
    public ITEM_TYPE Type {get; set;}
    public ITEM_EFFECT_TYPE Effect {get; set;}
    public int UseLimit {get; set;}
    public int UseCount {get; set;} = 0;
    public float EffectValue {get; set;}
    public float SellValue {get; set;}
    public float BuyValue {get; set;}
}
