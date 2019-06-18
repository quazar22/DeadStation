using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButtonSwitcher : MonoBehaviour
{
    static GameObject player = null;
    static WeaponManager wm = null;
    Button button;

    void Start()
    {
        if(!player || !wm)
        {
            player = GameObject.Find(Character.PLAYER);
            wm = player.GetComponent<WeaponManager>();
        }

        button = GetComponent<Button>();
        switch(name)
        {
            case "ShotgunButton":
                button.onClick.AddListener(() => wm.SwitchWeapon(wm.weapon_list[Weapon.SHOTGUN]));
                break;
            case "AutoRifleButton":
                button.onClick.AddListener(() => wm.SwitchWeapon(wm.weapon_list[Weapon.AUTORIFLE]));
                break;
            case "LaserCannon":
                button.onClick.AddListener(() => wm.SwitchWeapon(wm.weapon_list[Weapon.LASERCANNON]));
                break;
            case "GrenadeLauncher":
                button.onClick.AddListener(() => wm.SwitchWeapon(wm.weapon_list[Weapon.GRENADELAUNCHER]));
                break;
        }
    }

}
