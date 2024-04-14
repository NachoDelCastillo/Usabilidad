using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class EndGame : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    Transform fishParent;

    FishMovement fish;

    [SerializeField]
    Timer timer;

    [SerializeField]
    Leaderboard leaderboard;

    bool duringAnimation = false;
    bool animationFinished = false;
    public TMP_Text finishText;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        fish = collision.GetComponent<FishMovement>();
        if (fish != null)
        {
			Tracker.Instance.TrackEvent(new GameEndEvent(true));
			Tracker.Instance.FlushEvents();

			animator.SetTrigger("StartEndGame");
            duringAnimation = true;
            //fish.transform.SetParent(fishParent);
            fish.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            fish.enabled = false;
            StartCoroutine(leaderboard.SubmitScoreRoutine((int)(timer.GetCurrentTime() * 1000f)));
            AudioManager_PK.instance.Play("WinFanfare", 1f);
            AudioManager_PK.instance.Play("WinSong", 1f);
            timer.StopTimer();
            finishText.text = GameObject.Find("LootLockerManager").GetComponent<LootLockerManager>().leaderboard.getYourPosition((int)(timer.GetCurrentTime() * 1000f)).ToString() + "th BEST FISH";

            FindObjectOfType<TIMER_FLYING>().FlyToFinishLine();
        }
    }

    private void Update()
    {
        if (duringAnimation)
        {
            if (fish.GetComponent<Rigidbody2D>() != null)
                fish.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            fish.transform.GetChild(0).position = Vector3.Lerp(fish.transform.GetChild(0).position,
                fishParent.transform.position, Time.deltaTime);

            fish.transform.GetChild(0).rotation = Quaternion.Slerp(fish.transform.GetChild(0).rotation,
                fishParent.transform.rotation, Time.deltaTime);

            //fish.transform.position = fishParent.position;

        }

        if (animationFinished)
        {
            if (Input.GetButtonDown("EndGame"))
            {
                GameManager.instance.ChangeScene("MainMenu_Scene");
            }
        }
    }


    // ANIMATION EVENTS
    public void AnimationFinished()
    {
        animationFinished = true;
    }
}
