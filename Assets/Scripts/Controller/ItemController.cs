using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;


public enum ItemSelected{
    Random,
    Isaac,
    Jack,
    FireFlower,
    // TODO: implements
    Amplifier
}

public class ItemController : PickUp
{
    public ItemSelected itemSelected;


    private string basePath = "Sprites/";
    private string selected;
    // private MethodInfo method;


    public new void Start()
    {
        base.Start();
        
        if(itemSelected == ItemSelected.Random)
        {
            Array values = Enum.GetValues(typeof(ItemSelected));
            itemSelected = (ItemSelected)values.GetValue(UnityEngine.Random.Range(1, values.Length));
        }

        selected = Enum.GetName(typeof(ItemSelected), itemSelected);

        SetSprite();
        // SetMethod();
    }

    public void SetItem(ItemSelected itemSelected)
    {
        this.itemSelected = itemSelected;
    }

    public override void PickUpItem()
    {
        // method.Invoke(this, null);
        PlayerManager.Instance.GetPlayer().GetComponent<PlayerController>().AddPower(selected);
        Messenger<string>.Broadcast(GameEvent.ITEM_PICKED, selected);
    }

    private void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(basePath + selected);
    }

    // private void SetMethod()
    // {
    //     method = GetType().GetMethod(selected, BindingFlags.NonPublic | BindingFlags.Instance);
    // }


    // I SEGUENTI METODI VERRANNO CHIAMATI SOLO TRAMITE REFLECTION
    // private void Isaac()
    // {
    //     Debug.Log("Isaac");
    // }

    // private void FireFlower()
    // {
    //     Debug.Log("FireFlower");
    // }
}