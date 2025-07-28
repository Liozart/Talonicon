using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum Mood
{
    Normal, Happy, Angry, Nervous, Sad, Sleeping
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

    public Mood currentMood;

    float attackCoolDown = 0f;
    float attackCoolDownMax = 4f;

    void Start()
    {
        currentMood = Mood.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackCoolDown > 0f)
        {
            attackCoolDown -= Time.deltaTime;
        }
        else
        {
            if (currentMood == Mood.Angry)
            {
                SetMoodImage(Mood.Normal);
                currentMood = Mood.Normal;
            }
        }
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
                        attackCoolDown = attackCoolDownMax;
                        break;
                    case Mood.Normal:
                        attackCoolDown = attackCoolDownMax;
                        SetMoodImage(moo);
                        currentMood = moo;
                        break;
                    //Sad : still sad
                }
            }
        }
    }

    public void ChangeMoodForTime(Mood moo, float time)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeMoodForTimeCO(moo, time));
    }

    IEnumerator ChangeMoodForTimeCO(Mood moo, float time)
    {
        Mood oldMood = currentMood;
        currentMood = moo;
        SetMoodImage(moo);
        yield return new WaitForSeconds(time);
        currentMood = oldMood;
        SetMoodImage(currentMood);

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
        }
    }
}
