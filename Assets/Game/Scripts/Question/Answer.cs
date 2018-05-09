using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Answer : MonoBehaviour {

	public string answer;
	[SerializeField] public Text displayAnswer;
	[SerializeField] private int index;

	[SerializeField] private GameObject correctIcon;
    [SerializeField] private GameObject incorrectIcon;

	public void SetTextAnswer(string answer)
	{
		if(string.IsNullOrEmpty(answer))
		{
			gameObject.SetActive(false);
			return;
		}
		else
		{
			gameObject.SetActive(true);
		}
        DisplayStateIcon(GameEnum.StateAnswer.timeout);
		this.answer = answer;
		displayAnswer.text = index + ". " + answer;
		// print(displayAnswer.cachedTextGenerator.fontSizeUsedForBestFit);
	}

	public void DisplayStateIcon(GameEnum.StateAnswer state)
	{
		correctIcon.SetActive(state == GameEnum.StateAnswer.correct);
		incorrectIcon.SetActive(state == GameEnum.StateAnswer.incorrect);
	}

	public Button GetButton()
	{
		return gameObject.GetComponent<Button>();
	}
}
