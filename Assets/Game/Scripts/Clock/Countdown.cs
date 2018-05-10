using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Countdown : MonoBehaviour {

	[SerializeField] private Text textHeader;
	[SerializeField] private GameEnum.CountdownType type;
	[SerializeField] private int startAt;
	[SerializeField] private Image countdownImageDisplay;
    [SerializeField] private Sprite countdownImage3;
    [SerializeField] private Sprite countdownImage2;
    [SerializeField] private Sprite countdownImage1;
    [SerializeField] private Sprite countdownImage0;

	private WaitForSeconds wait = new WaitForSeconds(1);

	// [HideInInspector] public bool isTimeOut;
	public ReactiveProperty<bool> isTimeOut = new ReactiveProperty<bool>();
	private string stopwatchStartText;
	private string stopwatchEndText;

	private float currentTime = 0;

	[SerializeField] private Color textBlack;
	[SerializeField] private Color textRed;

	[SerializeField] private Animator animator;

    public void CountdownStart (string header = "", int time = 0)
	{
		isTimeOut.Value = false;
		if(type == GameEnum.CountdownType.number)
		{
			textHeader.text = header;
			currentTime = startAt;
			StartCoroutine("NumberCount");
		}
		else if(type == GameEnum.CountdownType.guage)
		{
            countdownImageDisplay.fillAmount = 0;
			currentTime = time;
			textHeader.text = currentTime.ToString();
			StartCoroutine("GuageCount");
		}
		else 
		{
			currentTime = time;
			stopwatchStartText = "Time : ";
			StartCoroutine("ClockCount");
		}
	}

    IEnumerator GuageCount()
    {
        var w = new WaitForEndOfFrame();
        float max = currentTime;
		bool isPlayVibrate = false;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            textHeader.text = currentTime.ToString("0");
            // print(((currentTime / max) * 100) * 0.01f);
            countdownImageDisplay.fillAmount = (((((max) - currentTime) / (max)) * 83) * 0.01f)+0.17f;
            yield return w;
			if(currentTime <= 2.5f && !isPlayVibrate)
			{
                isPlayVibrate = true;
				animator.Play("TimerVibrate");
			}
        }
		animator.Play("idle");
        isTimeOut.Value = true;
    }

	IEnumerator NumberCount()
	{
		while (currentTime > 0)
		{
			countdownImageDisplay.sprite = (currentTime == 3)? countdownImage3 : (currentTime == 2)? countdownImage2 : countdownImage1;
            currentTime--;
            yield return wait;	
		}
		isTimeOut.Value = true;
		gameObject.SetActive(false);
	}

	IEnumerator ClockCount()
	{
		while (currentTime > 0)
		{
			currentTime--;
			countdownImageDisplay.color = (currentTime < 11) ? textRed : textBlack;
            // countdownImageDisplay.text = stopwatchStartText + Extension.ConvertToDigitalClockFormat(currentTime);
            yield return wait;
        }
		//TODO
		isTimeOut.Value = true;
	}

	public void Stop()
	{
        animator.Play("idle");
		StopCoroutine("GuageCount");
	}

	public float GetCurrentTime()
	{
		return currentTime;
	}
	

}
