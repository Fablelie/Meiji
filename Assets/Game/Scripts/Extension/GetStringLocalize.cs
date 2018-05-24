using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetStringLocalize : MonoBehaviour {

	public Text target;
	public LocalizeKey localizeKey;


	void Awake()
	{
        target = GetComponent<Text>();
		
	}

	void OnEnable()
	{
        target.text = GetLocalizeString(localizeKey);
	}

	private string GetLocalizeString(LocalizeKey e)
	{
        switch (e)
        {
            case LocalizeKey.none:
                return target.text;
            case LocalizeKey.LetStart:
				return Extension.letStart;
            case LocalizeKey.Scan:
                return Extension.scan;
			case LocalizeKey.MissionCompleted:
                return Extension.missioncompleted;
			case LocalizeKey.Next:
                return Extension.next;
			case LocalizeKey.SpecialQuestion:
                return Extension.specialQuestion;
			case LocalizeKey.Back:
                return Extension.back;

            default:
				return target.text;
        }
	}

	public enum LocalizeKey
	{
		none = 0,
		LetStart = 1,
		Scan = 2,
        MissionCompleted = 3,
		Next = 4,
        SpecialQuestion = 5,
		Back = 6,
	}
}
