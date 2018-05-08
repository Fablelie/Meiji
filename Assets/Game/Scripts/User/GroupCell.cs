using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupCell : MonoBehaviour {

	[SerializeField] private Text groupName;
	[SerializeField] private Text score;

	public void SetupGroupCell(string groupName, string score)
	{
		gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
		this.groupName.text = groupName;
		this.score.text = score;
	}
}
