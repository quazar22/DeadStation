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
            player = GameObject.Find(Character.char_names[1]);
            wm = player.GetComponent<WeaponManager>();
        }

        button = GetComponent<Button>();
        switch(name)
        {
            case "ShotgunButton":
                button.onClick.AddListener(() => wm.SwitchWeapon(wm.GetWeapon(Weapon.weapons[0])));
                break;
            case "AutoRifleButton":
                button.onClick.AddListener(() => wm.SwitchWeapon(wm.GetWeapon(Weapon.weapons[1])));
                break;
            case "LaserCannon":
                button.onClick.AddListener(() => wm.SwitchWeapon(wm.GetWeapon(Weapon.weapons[2])));
                break;
            case "GrenadeLauncher":
                button.onClick.AddListener(() => wm.SwitchWeapon(wm.GetWeapon(Weapon.weapons[3])));
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
