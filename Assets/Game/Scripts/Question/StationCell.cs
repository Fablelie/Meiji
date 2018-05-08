using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationCell : MonoBehaviour {
	[SerializeField] private Text stationHeader;
	[SerializeField] private Text score;

	public void SetUpStationCell(string stationHeader, string score, bool isClear)
	{
		gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
		gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
		this.stationHeader.text = stationHeader;
		this.score.text = (isClear)?score:" -";
	}

}
