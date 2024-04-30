using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayMenu_PK : MenuManager_PK
{
    AllMenuManager_PK allMenuManager;

    protected override void ExtraAwake()
    {
        allMenuManager = GetComponentInParent<AllMenuManager_PK>();
    }

    protected override void buttonPressed(int index)
    {
        base.buttonPressed(index);

        // Sound
        //AudioManager_PK.instance.Play("ButtonPress", 1);

        if (index == 1) allMenuManager.BackToMainMenu();
    }
}
