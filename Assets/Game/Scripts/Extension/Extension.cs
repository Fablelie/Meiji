using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extension {
    
	public static string ConvertToDigitalClockFormat(int time)
    {
        int mm = 0;
        int ss = 0;
        if (time >= 60)
        {
            mm = time / 60;
            ss = time % 60;
        }
        else
        {
            ss = time;
        }
        return string.Format("{0}:{1}", FillZero(mm), FillZero(ss));
    }

    private static string FillZero(int time)
    {
        return string.Format("{0}{1}", (time.ToString().Length < 2) ? "0" : "", time);
    }

    public static string correct {
        get{
            return (GameManager.Instance.language == GameEnum.Language.thai) ? "ตอบถูก" : "Correct";
        }
    }
    public static string incorrect {
        get{
            return (GameManager.Instance.language == GameEnum.Language.thai) ? "ตอบผิด" : "Incorrect";
        }
    }
    
    public static string timeout {
        get{
            return (GameManager.Instance.language == GameEnum.Language.thai) ? "หมดเวลา" : "Timeout";
        }
    }
    
    public static string time {
        get{
            return (GameManager.Instance.language == GameEnum.Language.thai) ? "เวลา" : "Time";
        }
    }
    
    public static string score {
        get{
            return (GameManager.Instance.language == GameEnum.Language.thai) ? "คะแนน" : "score";
        }
    }
    
    public static string letStart {
        get{
            return (GameManager.Instance.language == GameEnum.Language.thai) ? "เริ่มเล่น" : "Let's Start";
        }
    }
    
    public static string scan {
        get{
            return (GameManager.Instance.language == GameEnum.Language.thai) ? "กดเพื่อสแกน" : "Press to scan";
        }
    }
    
    public static string missioncompleted {
        get{
            return (GameManager.Instance.language == GameEnum.Language.thai) ? "คุณเดินทางครบทุก Station แล้ว" : "Your mission completed";
        }
    }
    
    public static string next {
        get{
            return (GameManager.Instance.language == GameEnum.Language.thai) ? "ต่อไป" : "Next";
        }
    }
    
    public static string specialQuestion {
        get{
            return (GameManager.Instance.language == GameEnum.Language.thai) ? "คำถามชุดพิเศษ" : "Special Question";
        }
    }
    public static string back {
        get{
            return (GameManager.Instance.language == GameEnum.Language.thai) ? "ย้อนกลับ" : "Back";
        }
    }
    

}
