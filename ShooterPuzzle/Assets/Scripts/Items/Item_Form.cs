using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Form : Item
{
    public WeaponForm form;

    protected override void Pickup(Player player)
    {
        base.Pickup(player);
        if (player.currentForm != form)
        {
            player.ChangeForm(form);
        }
    }
}
