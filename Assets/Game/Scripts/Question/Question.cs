using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Question")]
public class Question : ScriptableObject {
	public int time;
	public QuestionDetail questionThai;
	public QuestionDetail questionEnglish;

	[HideInInspector] public int score;
}

[System.Serializable]
public struct QuestionDetail
{
	public string question;
	public string answer;
	public List<string> dummys;
	// public string dummy2;
	// public string dummy3;

}