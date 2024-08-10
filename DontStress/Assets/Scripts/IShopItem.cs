using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopItem
{
    public int price { get; }
    public void Bought();
}
