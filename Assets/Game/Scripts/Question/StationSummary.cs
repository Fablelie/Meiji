using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class StationSummary : MonoBehaviour {

	[SerializeField] private Text stationHeader;
	// [SerializeField] private Text correctText;
	// [SerializeField] private Text correctAmountText;
	// [SerializeField] private Text incorrectText;
	// [SerializeField] private Text incorrectAmountText;
	[SerializeField] private Text stationScoreText;
	// [SerializeField] private RectTransform content;
	// [SerializeField] private GameObject stationCell;
	// [SerializeField] private Button nextBtn;
	// [SerializeField] private GameObject endText;
	[SerializeField] private GameObject scanerPanel;
	[SerializeField] private GameObject bg;
	[SerializeField] private EndGameResult endGameResult;

	[SerializeField] private float speed = 10;

	[SerializeField] private Button nextBtn;

	private Station[] stationArr;

	private bool isClearAllStations;
	private float currentDisplayScore = 0;

	public void SetupDisplayStationSummary(Station s, Station[] stationArr)
	{
        currentDisplayScore = 0;
		// print("setup summary");
		// nextBtn.gameObject.SetActive(false);
		nextBtn.onClick.RemoveAllListeners();
		this.stationArr = stationArr;
		gameObject.SetActive(true);

        isClearAllStations = true;
		if(!s.isSpecialStation)
		{
			foreach (Station station in stationArr)
			{
				if(!station.isClear)
				{
					isClearAllStations = false;
					break;
				}
			}
		}
		int index = (GameManager.Instance.language == GameEnum.Language.thai) ? 0 : 1;
		
		stationHeader.text = (s.isSpecialStation) ? Extension.specialQuestion : s.stationName + " " + s.stationTitle[index];
        // ShowStationResult(s);
        ShowScore(s);
	}

	private void ShowScore(Station s)
	{
        stationScoreText.gameObject.SetActive(true);
        stationScoreText.text = "Score : " + (int)currentDisplayScore;
        bg.SetActive(true);
		StartCoroutine(ScoreAnimate(s));
	}

	IEnumerator ScoreAnimate(Station s)
	{
		nextBtn.onClick.AddListener(() => 
		{
            SoundManager.Instance.PlaySound("click");
			currentDisplayScore = s.score.Value;
            stationScoreText.text = "Score : " + (int)currentDisplayScore;
            nextBtn.onClick.RemoveAllListeners();
		});
	
		var endFrame = new WaitForEndOfFrame();
		while (currentDisplayScore < s.score.Value)
		{
			currentDisplayScore = Mathf.Clamp(currentDisplayScore + (Time.deltaTime * speed), 0, s.score.Value);
            stationScoreText.text = "Score : " + (int)currentDisplayScore;
			yield return endFrame;
		}
        nextBtn.onClick.RemoveAllListeners();
		yield return new WaitForSeconds(0.5f);

		nextBtn.onClick.AddListener(() =>
		{
            SoundManager.Instance.PlaySound("click");
			if (s.isSpecialStation)
                ShowSplashScreen();
			else
				ShowScannerPanel();
			nextBtn.onClick.RemoveAllListeners();
		});
    }

	private void DisableThisPanel()
	{
        bg.SetActive(false);
        gameObject.SetActive(false);
	}

	private void ShowSplashScreen()
	{
        DisableThisPanel();
		GameManager.Instance.BeginDrawSplashScreen();
	}

	private void ShowScannerPanel()
	{
		
		DisableThisPanel();
		if(isClearAllStations)
		{
			endGameResult.SetupEndGameResult(GameManager.Instance.globalScore.Value);
		}
		else
		{	
            scanerPanel.SetActive(true);	
		}
	}
}
