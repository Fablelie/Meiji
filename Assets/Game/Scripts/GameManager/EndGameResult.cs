using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameResult : MonoBehaviour {

	[SerializeField] private GameObject congratulationsPanel;
	[SerializeField] private Text score;
	[SerializeField] private GameObject rankingPanel;
	[SerializeField] private GameObject groupCellPrefab;
	[SerializeField] private RectTransform content;

	private User[] userArr;
	private List<User> equalsUserScore = new List<User>();

	public void SetupEndGameResult(int score)
	{
		gameObject.SetActive(true);
		congratulationsPanel.SetActive(true);
		// this.score.text = score.ToString();
		// userArr = Server.Instance.GetListUser().ToArray();

		// for (int i = 0; i < userArr.Length; i++)
        // {
        //     for (int j = 0; i < userArr.Length; j++)
        //     {
        //         if (userArr[i].score == userArr[j].score && userArr[i].deviceID != userArr[j].deviceID)
        //         {
        //             AddEqualsUserScore(userArr[i]);
        //             break;
        //         }
        //     }
        // }
	}

	public void ShowRankingPanel()
	{
		congratulationsPanel.SetActive(false);
		rankingPanel.SetActive(true);

		float height = 0;
        float spacing = content.gameObject.GetComponent<VerticalLayoutGroup>().spacing;
		int index = 1;
        foreach (User user in userArr)
        {
            GameObject obj = Instantiate(groupCellPrefab);
            obj.transform.parent = content;
            obj.GetComponent<GroupCell>().SetupGroupCell(index+". "+ user.groupName, user.score.ToString());
            height += obj.GetComponent<RectTransform>().rect.height + spacing;
			index++;
        }
		content.sizeDelta = new Vector2( content.sizeDelta.x, height);
    }

	public void CheckMyAccessForExtraQuestion()
	{
		bool hasAccess = false;
		foreach (User user in equalsUserScore)
		{
			if(user.deviceID == Server.Instance.GetDeviceId())
			{
				hasAccess = true;
				break;
			}
		}

		print("HasAccess : " + hasAccess);
		// if(hasAccess)
		// {
		// 	print("HasAccess : " + hasAccess);
		// }
		// else
		// {

		// }
	}

	private void AddEqualsUserScore(User user)
	{
		equalsUserScore.Add(user);
	}
}
