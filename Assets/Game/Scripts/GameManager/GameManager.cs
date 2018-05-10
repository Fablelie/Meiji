using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour {
	
	[SerializeField] private Countdown countdown;
	[SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject firstPanel;
	[SerializeField] private GameObject scanerPanel;
	[SerializeField] private StateQuestion stateQuestion;

	public Station specialStation;
	public Station[] stationArr;

	public GameEnum.Language language = GameEnum.Language.thai;

	public string groupName;

	[SerializeField] Text scoreText;
	public ReactiveProperty<int> globalScore = new ReactiveProperty<int>();

	[SerializeField] private StationSummary stationSummary;

	private int currentStationIndex = 0;

	private IDisposable isTimeOut;

	public static GameManager m_instance;

	public static GameManager Instance {
		get {
			return m_instance;
		}
		set {
			m_instance = value;
		}
	}

	void Awake()
	{
		if(Instance == null)
			Instance = this;	

		SetEnableVuforia(false);
	}

	void Start()
	{
        ResetGameStation(true);
	}

	public void CheckLoadingPanel()
	{
		if(loadingPanel.activeInHierarchy) loadingPanel.SetActive(false);
	}

	public void StartGame()
	{
        this.ObserveEveryValueChanged(x => x.globalScore.Value).Subscribe(x => UpdateScore(x));
		for(int i = 0; i < stationArr.Length; i++)
		{
            currentStationIndex = i;
			stationArr[i].ObserveEveryValueChanged(x => x.score.Value).Subscribe(score => UpdateStationScore());
		}
		specialStation.ObserveEveryValueChanged(x => x.score.Value).Subscribe(score => Server.Instance.UpdateSpecialScore(score));
        ResetGameStation();
	}

	private void UpdateScore(int score)
	{
		Server.Instance.UpdateScore(score, GameEnum.ScoreType.normal);
		scoreText.text = "Score : " + score.ToString();
	}

	public void UpdateStationScore()
	{
        for (int i = 0; i < stationArr.Length; i++)
		{
			Server.Instance.UpdateStationScore(i, stationArr[i].score.Value);
		}
	}

	private void ResetGameStation(bool b = false)
	{
		specialStation.score.Value = 0;
        specialStation.isClear = b;
		foreach (Station station in stationArr)
        {
            station.score.Value = 0;
			station.isClear = b;
        }
	}

	private bool CheckAllStationIsClear()
	{
		foreach (Station station in stationArr)
		{
			if(!station.isClear && !specialStation.isClear) return false;
		}
		return true;
	}

	public void SetGameStation(string stationCode)
	{
		if(stationCode == "Station 1" && CheckAllStationIsClear()) StartGame();

		if(specialStation.stationName == stationCode)
		{
			foreach (Station station in stationArr) if(!station.isClear) return;
			if (!specialStation.isClear)
			{
				print("Special Station!!");
				ChangePanelToGameState(specialStation);
				return;
			}
			else return;
		}

		foreach (Station station in stationArr)
        {
			// print("station " + station.stationName + ", " + station.isClear);
            if (station.stationName == stationCode)
            {
				if (!station.isClear)
				{
                    ChangePanelToGameState(station);
					print("subscribe");
					return;
				}
				else
				{
					return;
				}
            }
        }
	}

	private void ChangePanelToGameState(Station station)
	{
        scanerPanel.GetComponent<ScannerPanelController>().SetEnableScanner(false);
        scanerPanel.SetActive(false);
        countdown.gameObject.SetActive(true);
        countdown.CountdownStart(station.stationName);

        isTimeOut = countdown.isTimeOut.Where(isTimeOut => isTimeOut).Do(_ => StartStationLoop(station)).Subscribe();
	}

	private void StartStationLoop(Station station)
	{
		isTimeOut.Dispose();
		// print("Dispose");
		StartCoroutine(StationLoop(station));
	}

	private IEnumerator StationLoop(Station station)
	{
		// yield return new WaitUntil(() => countdown.isTimeOut.Value);
		int i = 1;
		// print("Setup station : " + station.stationName);
		// foreach (Question item in station.questions)
			// print("all station question : " + item.name);
		foreach (Question item in station.questions)
		{
			// print(item.name);
			int index = (this.language == GameEnum.Language.thai) ? 0 : 1;
			stateQuestion.SetUp(
				item, 
				station.stationName, 
				station.stationTitle[index], 
				i, 
				station.questions.Length, 
				station.isSpecialStation
			);
			// Debug.Log("SetUp question : " + i + ", station : " + station.stationName);
			i++;
			 
			yield return new WaitUntil(() => stateQuestion.readyForStartQuestion);
		}
		StationSummary(station);
	}

	public void StationSummary(Station s)
	{
		stateQuestion.gameObject.SetActive(false);
		s.isClear = true;
		stationSummary.SetupDisplayStationSummary(s, stationArr);
    }

	public void UpdateStationQuestionScore(string stationName, Question question)
	{
		if(stationName == specialStation.stationName)
			foreach (Question q in specialStation.questions)
				if(q.name == question.name)
				{
					q.score = question.score;
					specialStation.score.Value += q.score;
                    globalScore.Value += q.score;
					return;
				}

		foreach (Station station in stationArr)
			if(station.stationName == stationName)
				foreach (Question q in station.questions)
					if(q.name == question.name)
					{
						q.score = question.score;
                        currentStationIndex = station.stationIndex;
						station.score.Value += q.score;
						globalScore.Value += q.score;
						return;
					}
	}

	public void SetLanguage(int indexEnum)
	{
		this.language = (GameEnum.Language)indexEnum;
        // print(">>>>>>>>>>>>>>>>>>>>>>>> " + Extension.specialQuestion);
	}

	// public void SetGroupName(Text t)
	// {
	// 	if(string.IsNullOrEmpty(t.text))
	// 		t.text = "ชื่อกลุ่ม";

	// 	groupName = t.text;
	// 	ResetGameStation();
	// 	Server.Instance.WriteNewUser(groupName);
	// }

    public void BeginDrawSplashScreen()
	{
		StartCoroutine(GotoFirstPanel());
	}

    private IEnumerator GotoFirstPanel()
    {
        SplashScreen.Begin();
        firstPanel.SetActive(true);
        while (!SplashScreen.isFinished)
        {
            SplashScreen.Draw();
            yield return null;
        }


    }

    public void SetEnableVuforia(bool isEnable)
    {
        // Vuforia.VuforiaBehaviour.Instance.enabled = isEnable;
    }
}
