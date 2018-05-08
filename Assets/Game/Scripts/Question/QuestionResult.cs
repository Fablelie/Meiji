using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class QuestionResult : MonoBehaviour {

	[SerializeField] private GameObject correctIcon;
	[SerializeField] private GameObject incorrectIcon;
	[SerializeField] private GameObject timeoutIcon;
	[SerializeField] private Text correctText;
	[SerializeField] private Text incorrectText;
	[SerializeField] private Text timeoutText;

	[SerializeField] private GameObject calculateScore;
	[SerializeField] private Text answerResult;
	[SerializeField] private Text resultDetail;
	[SerializeField] private Text addScore;

	[SerializeField] private Color red;
	[SerializeField] private Color green;

	private Button btn;
	public ReactiveProperty<bool> readyForNextQuestion = new ReactiveProperty<bool>(); 

	public int GetScoreInThisQuestion(GameEnum.StateAnswer e, Countdown countdown)
	{
		btn = gameObject.GetComponent<Button>();
		// readyForNextQuestion.Value = false;
        readyForNextQuestion.Value = true;
		btn.onClick.RemoveAllListeners();
		// gameObject.SetActive(true);
		// ShowStateQuestionResult(e, countdown);
		
		return (e == GameEnum.StateAnswer.correct)? 10+(10*(int)countdown.GetCurrentTime()):0;
	}

	private void ShowStateQuestionResult(GameEnum.StateAnswer e, Countdown countdown)
	{
		SetActiveIconAndText(e);
		btn.onClick.AddListener(() => ShowCalculateScore(e, countdown));
	}

	private void ShowCalculateScore(GameEnum.StateAnswer e, Countdown countdown)
	{
		btn.onClick.RemoveAllListeners();
		DisableAllTextResult();
		calculateScore.SetActive(true);
		if(e == GameEnum.StateAnswer.correct)
		{
			answerResult.text = Extension.correct + " + 10" + Extension.score;
			resultDetail.gameObject.SetActive(true);
			resultDetail.text = Extension.time + Extension.ConvertToDigitalClockFormat((int)countdown.GetCurrentTime()) + " x 10 : + " + 10*countdown.GetCurrentTime() + Extension.score;
			addScore.color = green;
			addScore.text = "+ " + (10 + (10 * countdown.GetCurrentTime())) + Extension.score;
		}
		else 
		{
			answerResult.text = ((e == GameEnum.StateAnswer.incorrect)? Extension.incorrect:Extension.timeout) + " + 0" + Extension.score;
			resultDetail.gameObject.SetActive(false);
			addScore.color = red;
            addScore.text = "+ 0" + Extension.score;
		}
		btn.onClick.AddListener(() => 
		{
			calculateScore.SetActive(false);
			readyForNextQuestion.Value = true;
			gameObject.SetActive(false);
		});
	}


	private void DisableAllTextResult()
	{
		correctText.gameObject.SetActive(false);
        incorrectText.gameObject.SetActive(false);
        timeoutText.gameObject.SetActive(false);
	}

	private void SetActiveIconAndText(GameEnum.StateAnswer e)
	{
		correctIcon.SetActive(e == GameEnum.StateAnswer.correct);
        incorrectIcon.SetActive(e == GameEnum.StateAnswer.incorrect);
        timeoutIcon.SetActive(e == GameEnum.StateAnswer.timeout);
		correctText.text = Extension.correct;
		incorrectText.text = Extension.incorrect;
		timeoutText.text = Extension.timeout;
        correctText.gameObject.SetActive(e == GameEnum.StateAnswer.correct);
        incorrectText.gameObject.SetActive(e == GameEnum.StateAnswer.incorrect);
        timeoutText.gameObject.SetActive(e == GameEnum.StateAnswer.timeout);
	}
}
