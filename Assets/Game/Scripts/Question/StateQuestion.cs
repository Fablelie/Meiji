using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class StateQuestion : MonoBehaviour {

	[SerializeField] private Text stationTitle;
    [SerializeField] private Image stationImage;
	[SerializeField] private Image spacialStationImage;
	[SerializeField] private Sprite specialImage;
    [SerializeField] private Sprite[] stationImages;
	[SerializeField] private Text currentQuestionText;
	// [SerializeField] private Text numberOfQuestion;
	[SerializeField] private Text questionText;
	[SerializeField] private Countdown countdown;
    [SerializeField] private Animator guageAni;
	[SerializeField] private Animator charaterAnim;
	[SerializeField] private Answer[] answers;
	[SerializeField] private Answer[] arr;

	[SerializeField] private QuestionResult questionResult;

	[SerializeField] private GameObject answerSheet;

	private Question questionDetail;
	public bool readyForStartQuestion = true;
	private AudioSource timerAudioSource;
	private string stationName;
	private bool isSpecial;
	System.IDisposable subscribeReadyForNextQuestion;
	System.IDisposable subscribeTimeOut;

    public void SetUp(Question question, string stationName, string stationTitle, int numberOfQuestion, int amountOfQuestions, bool isSpecial)
	{
		this.isSpecial = isSpecial;
		readyForStartQuestion = false;
		this.stationName = stationName;
		this.stationTitle.text = stationTitle;

		stationImage.enabled = !this.isSpecial;
		spacialStationImage.enabled = this.isSpecial;

        stationImage.sprite = GetStationSpriteByStationName(this.stationName);
		currentQuestionText.text = numberOfQuestion + "/" + amountOfQuestions;

		questionDetail = question;
		this.questionText.text = numberOfQuestion+". "+Localize(GameManager.Instance.language, questionDetail).question;

		
		arr = (isSpecial) ? RemoveLastIndexOfAnswers(answers) : RandomizeOrder(answers);
		int i = 0;
		fontSize = int.MaxValue;
		foreach (Answer item in arr)
		{
            print("Setup new Answer");
			int answerFontSize = (GameManager.Instance.language == GameEnum.Language.thai)? questionDetail.thaiFontSize: questionDetail.engFontSize;
            
			if(answerFontSize > 0)
			{
                baseFit = item.displayAnswer.resizeTextForBestFit = false;
				item.displayAnswer.fontSize = answerFontSize;
			}
			else
			{
                baseFit = item.displayAnswer.resizeTextForBestFit = true;
                item.displayAnswer.resizeTextMaxSize = 100;
                item.displayAnswer.resizeTextMinSize = 50;	
			}

			item.SetTextAnswer(GetAnswerByIndex(i, isSpecial));
			item.GetButton().onClick.AddListener(() => SendAnswer(item)); 
			i++;
			
		}

		// foreach (Answer item in arr)
		// {
		// 	item.displayAnswer.resizeTextForBestFit = false;
		// 	item.displayAnswer.fontSize = fontSize;
		// }
		gameObject.SetActive(true);
		charaterAnim.enabled = true;
        charaterAnim.SetTrigger("meijiKung");
		answerSheet.SetActive(true);
		// guageAni.enabled = true;
		string typeOfQuestionStr = (!this.isSpecial) ? "15sec" : "8sec";
		// guageAni.SetTrigger(typeOfQuestionStr);
		SoundManager.Instance.PlaySound(typeOfQuestionStr, out timerAudioSource);
		
		countdown.CountdownStart(time: question.time);
		subscribeTimeOut = countdown.isTimeOut
		.Where(isTimeOut => isTimeOut)
		.Do(_ => {
			Debug.LogError("TimeOut");
			AnswerStateChange(GameEnum.StateAnswer.timeout);
			// ani.enabled = false;
			
		})
		.Subscribe();
	}

	bool baseFit = false;
    int fontSize = int.MaxValue;
	void Update()
	{
		if(arr != null && baseFit)
		{
			foreach (Answer item in arr)
			{
                baseFit = item.displayAnswer.resizeTextForBestFit;
                item.displayAnswer.resizeTextMinSize = 50;
				item.displayAnswer.resizeTextMaxSize = 100;
                int fontSizeBaseFit = item.displayAnswer.cachedTextGenerator.fontSizeUsedForBestFit;
				print(item.name + " : " + fontSizeBaseFit);
				if(fontSizeBaseFit > 0 && item.displayAnswer.resizeTextForBestFit)
				{
					fontSize = (fontSizeBaseFit < fontSize) ? fontSizeBaseFit : fontSize;
					item.displayAnswer.resizeTextForBestFit = false;
				}
                item.displayAnswer.fontSize = (fontSize > 100) ? 100 : fontSize;
				
				if(GameManager.Instance.language == GameEnum.Language.thai)
					questionDetail.thaiFontSize = (fontSize > 100) ? 100 : fontSize;
				else
                    questionDetail.engFontSize = (fontSize > 100) ? 100 : fontSize;
				
			}
		}
	}

	private Answer[] RemoveLastIndexOfAnswers(Answer[] arr)
	{
        List<Answer> cloneArr = new List<Answer>(arr);
        cloneArr[cloneArr.Count-1].gameObject.SetActive(false);
        cloneArr.RemoveAt(cloneArr.Count-1);
		return cloneArr.ToArray();
	}

	private Sprite GetStationSpriteByStationName(string stationName)
	{
		string[] s = stationName.Split(' ');
		return (s.Length <= 1) ? specialImage : stationImages[int.Parse(s[1])-1];
	}

	private string GetAnswerByIndex(int i, bool isSpecial)
	{
		if(isSpecial)
			if(i == 0) return (GameManager.Instance.language == GameEnum.Language.thai) ? "ใช่" : "Yes";
			else return (GameManager.Instance.language == GameEnum.Language.thai) ? "ไม่" : "No";

		if(i == 0)
		{
			return Localize(GameManager.Instance.language, questionDetail).answer;
		}
		else
		{
			List<string> list = Localize(GameManager.Instance.language, questionDetail).dummys;
			// print((list.Count < i - 1) + ", i : "+ i );
			return (list.Count <= i-1) ? "" : list[i-1];
		}
	}

	private QuestionDetail Localize(GameEnum.Language e, Question q)
	{
		return (e == GameEnum.Language.thai)?q.questionThai:q.questionEnglish;
	}

	private Answer[] RandomizeOrder(Answer[] array)
    {
        var rand = new System.Random();
        return array.Select(x => new { x, r = rand.Next() })
                                       .OrderBy(x => x.r)
                                       .Select(x => x.x)
                                       .ToArray();
    }

	private void ClearAllEventInBtnAnswer()
	{
		foreach (Answer answer in answers)
		{
			answer.GetButton().onClick.RemoveAllListeners();
		}
	}

	private void SendAnswer(Answer answer)
	{
        timerAudioSource.Stop();
		SoundManager.Instance.PlaySound("click");
		// guageAni.enabled = false;
		GameEnum.StateAnswer state = 
		(answer.answer == Localize(GameManager.Instance.language, questionDetail).answer)?
				GameEnum.StateAnswer.correct:
				GameEnum.StateAnswer.incorrect;
		
		SoundManager.Instance.PlaySound((state == GameEnum.StateAnswer.correct) ? "correct" : "wrong");
		answer.DisplayStateIcon(state);
		AnswerStateChange(state);
	}

	public void AnswerStateChange(GameEnum.StateAnswer state)
	{
        subscribeTimeOut.Dispose();
		countdown.Stop();
		ClearAllEventInBtnAnswer();
		// answerSheet.SetActive(false);
		questionDetail.score = questionResult.GetScoreInThisQuestion(state, countdown);
		print("Score : " + questionDetail.score);
		GameManager.Instance.UpdateStationQuestionScore(stationName, questionDetail);
		
		StartCoroutine(DelayForNextQuestion());
	}

	private IEnumerator DelayForNextQuestion()
	{
		yield return new WaitForSeconds(2);
        charaterAnim.enabled = false;
        // end this question receive next question
		readyForStartQuestion = true;
        // subscribeReadyForNextQuestion = questionResult.readyForNextQuestion
        // .Where(b => b == true)
        // .Do(b =>
        // {
        //     readyForStartQuestion = b;
        //     subscribeReadyForNextQuestion.Dispose();
        // })
        // .Subscribe();
	}

}
