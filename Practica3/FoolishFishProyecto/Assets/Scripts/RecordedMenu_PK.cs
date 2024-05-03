using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordedMenu_PK : MenuManager_PK
{
    AllMenuManager_PK allMenuManager;

    [SerializeField]
    GameObject replayGameObj;

	private void Start() {
		if (Tracker.Instance != null) {
			Tracker.Instance.generalTracker = true;
			Tracker.Instance.fishMovementTracker = true;
			Tracker.Instance.recordTracker = true;
		}

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

        if (index == 0)
        {
			if (Tracker.Instance != null) {
				Tracker.Instance.generalTracker = false;
				Tracker.Instance.fishMovementTracker = false;
				Tracker.Instance.recordTracker = false;
			}

			GameManager.GetInstance().ChangeScene("Gameplay");
            replayGameObj.SetActive(true);
        }
        if (index == 1) allMenuManager.BackButtonRecorded();
    }
}
