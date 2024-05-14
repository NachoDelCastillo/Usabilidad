using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordedMenu_PK : MenuManager_PK
{
    AllMenuManager_PK allMenuManager;

    [SerializeField]
    GameObject replayGameObj;

    [SerializeField]
    Button_PK exitButton;

	private void Start() {
	    Tracker.Instance.ReplayMode = false;
	}

	protected override void ExtraAwake()
    {
        allMenuManager = GetComponentInParent<AllMenuManager_PK>();
    }

    protected override void buttonPressed(int index)
    {
        base.buttonPressed(index);

        // Sound
        //AudioManager_PK.instance.Play("ButtonPress", 1);

        if (index == exitButton.GetIndex())
        {
            allMenuManager.BackButtonRecorded();
            return;
        }
        else
        {
            Tracker.Instance.ReplayMode = true;

            Tracker.Instance.SetIndexOfTheGameToReproduce(index);

            GameManager.GetInstance().ChangeScene("Gameplay");
            replayGameObj.SetActive(true);
        }
    }

    public void PopulateButtons()
    {
        // Almacenar botones
        nButtons = buttonGroup.childCount + 1;
        buttons = new Button_PK[nButtons];
        for (int i = 0; i < nButtons - 1; i++)
        {
            buttons[i] = buttonGroup.GetChild(i).GetComponent<Button_PK>();
        }

        if (nButtons > 0)
        {
            buttons[nButtons - 1] = exitButton;
        }
        else
        {
            buttons[0] = exitButton;
        }
    }
}
