using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum Mood
{
    Normal, Happy, Angry, Nervous, Sad, Sleeping, Dead
}

public class MoodManager : MonoBehaviour
{
    public Image Image_Portait;
    public Sprite Image_Portait_Normal;
    public Sprite Image_Portait_Happy;
    public Sprite Image_Portait_Angry;
    public Sprite Image_Portait_Nervous;
    public Sprite Image_Portait_Sad;
    public Sprite Image_Portait_Sleeping;
    public Sprite Image_Portait_Dead;

    public Mood currentMood;
    public Mood timedMood;
    bool isMoodTimed = false;

    void Start()
    {
        currentMood = Mood.Normal;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeMood(Mood moo, bool force)
    {
        if (force)
        {
            StopAllCoroutines();
            SetMoodImage(moo);
            currentMood = moo;
        }
        else
        {
            if (moo == Mood.Angry)
            {
                switch (currentMood)
                {
                    case Mood.Angry:
                        timedMood = Mood.Angry;
                        isMoodTimed = true;
                        break;
                    case Mood.Normal:
                        timedMood = Mood.Angry;
                        SetMoodImage(moo);
                        isMoodTimed = true;
                        break;
                    default:
                        currentMood = moo;
                        if (!isMoodTimed)
                        {
                            SetMoodImage(moo);
                        }
                        break;
                }
            }
            SetMoodImage(moo);
            currentMood = moo;
        }
    }

    public void ChangeMoodForTime(Mood moo, float time)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeMoodForTimeCO(moo, time));
    }

    IEnumerator ChangeMoodForTimeCO(Mood moo, float time)
    {
        SetMoodImage(moo);
        isMoodTimed = true;
        timedMood = moo;
        yield return new WaitForSeconds(time);
        SetMoodImage(currentMood);
        isMoodTimed = false;

    }

    public void SetMoodImage(Mood moo)
    {
        switch (moo)
        {
            case Mood.Normal:
                Image_Portait.sprite = Image_Portait_Normal;
                break;
            case Mood.Happy:
                Image_Portait.sprite = Image_Portait_Happy;
                break;
            case Mood.Angry:
                Image_Portait.sprite = Image_Portait_Angry;
                break;
            case Mood.Nervous:
                Image_Portait.sprite = Image_Portait_Nervous;
                break;
            case Mood.Sad:
                Image_Portait.sprite = Image_Portait_Sad;
                break;
            case Mood.Sleeping:
                Image_Portait.sprite = Image_Portait_Sleeping;
                break;
            case Mood.Dead:
                Image_Portait.sprite = Image_Portait_Dead;
                break;
        }
    }
}
