using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayGroupName : MonoBehaviour {

	[SerializeField] private Text groupName;

	void OnEnable()
	{
		StartCoroutine(SetGroupName());
	}

	IEnumerator SetGroupName()
	{
		yield return new WaitUntil(() => Server.Instance != null);
		
        groupName.text = Server.Instance.GetGroupName();//GetLocalizeName(Server.Instance.GetGroupName());
	}

	private string GetLocalizeName(string name)
	{
		bool isThai = (GameManager.Instance.language == GameEnum.Language.thai);
		return ((isThai) ? "ทีม : " : "Team : ") + name;
		// switch (name)
		// {
        //     case "DD":
        //         return (isThai) ? "ทีม : พี่ดีดี" : "Team : P'DD";
		// 	case "BOW":
        //         return (isThai) ? "ทีม : พี่โบว์" : "Team : P'Bow";
		// 	case "POLY":
        //         return (isThai) ? "ทีม : พี่พลอย" : "Team : P'Poly";
		// 	case "EARTH":
        //         return (isThai) ? "ทีม : พี่เอิร์ธ" : "Team : P'Earth";
        //     default:
		// 		return name;
		// }
	}
}
