using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class Server : MonoBehaviour {

    public static Server instance;
    public static Server Instance
    { 
        get 
        {
            return instance;
        } 
        set 
        {
            instance = value;
        }
    }

    [SerializeField] private string deviceID;

    private DatabaseReference reference;

    [SerializeField] private List<User> listUserData;
    
    private string queryDeviceID;
    private string groupName;
    private int score;
    // private List<int> station = new List<int>(new int[6]);
    private int specialScore;
    private int kinectScore;
    private const string normal = "score";
    private const string kinect = "kinectScore";

    void Awake() {
        if(instance == null) instance = this;
        listUserData = new List<User>();
        deviceID = SystemInfo.deviceUniqueIdentifier;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://meiji-questions-game.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.GetReference("user-data");
        reference.ValueChanged += HandleValueChanged;
        // print("deviceUniqueIdentifier : " + deviceID);
        // WriteNewUser("eiei", 100, 200);
        // reference.Child(deviceID).Child("score").SetValueAsync(700);
        // GetUpdateScoreBoard();
        
    }

    public string GetDeviceId()
    {
        return deviceID;
    }

    private void InsertUpdateListUserData(User currentUserData)
    {
        User user = listUserData.Find(u => u.deviceID == currentUserData.deviceID);
        if(user != null)
        {
            user.UpdateUserData(currentUserData);
            return;
        }

        listUserData.Add(currentUserData);
    }

    public List<User> GetListUser()
    {
        listUserData.Sort();
        return listUserData;
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot

        DataSnapshot snapshot = args.Snapshot;

        // string json = snapshot.GetRawJsonValue();
        // print(json);
        // listUserData = new List<User>();
        foreach (DataSnapshot s in snapshot.Children)
        {
            // print(s.Key);
            queryDeviceID = s.Key;
            List<int> stationScore = new List<int>(new int[6]);
            foreach (DataSnapshot item in s.Children)
            {
                switch (item.Key)
                {
                    case "groupName":
                        groupName = item.Value.ToString();
                        break;
                    case "score":
                        score = int.Parse(item.Value.ToString());
                        break;
                    case "kinectScore":
                        kinectScore = int.Parse(item.Value.ToString());
                        break;
                    case "station":
                        foreach (var stationIndex in item.Children)
                        {
                            // Debug.LogError(stationIndex.Key);
                            int index = int.Parse(stationIndex.Key);
                            int value = int.Parse(stationIndex.Value.ToString());
                            stationScore[index] = value;
                        }
                        break;
                    case "specialScore":
                        specialScore = int.Parse(item.Value.ToString());
                        break;
                }
            }
            // print(s.GetRawJsonValue());
            InsertUpdateListUserData(new User(queryDeviceID, groupName, score, kinectScore, specialScore, stationScore));
            // print((User)s.Value);
        }

        GameManager.Instance.CheckLoadingPanel();

        // foreach (User item in listUserData)
        // {
        //     print(item.groupName);
        //     print(item.score);
        //     print(item.kinectScore);
        // }
    }

    public void GetUpdateScoreBoard()
    {
        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                print("Error");
            }
            else if (task.IsCompleted)
            {

                DataSnapshot snapshot = task.Result;

                // string json = snapshot.GetRawJsonValue();
                // print(json);
                listUserData = new List<User>();
                foreach (DataSnapshot s in snapshot.Children)
                {
                    // print(s.Key);
                    queryDeviceID = s.Key;
                    foreach (DataSnapshot item in s.Children)
                    {
                        switch (item.Key)
                        {
                            case "groupName":
                                groupName = item.Value.ToString();
                                break;
                            case "score":
                                score = int.Parse(item.Value.ToString());
                                break;
                            case "kinectScore":
                                kinectScore = int.Parse(item.Value.ToString());
                                break;
                            case "station1":
                                score = int.Parse(item.Value.ToString());
                                break;
                            case "station2":
                                score = int.Parse(item.Value.ToString());
                                break;
                            case "station3":
                                score = int.Parse(item.Value.ToString());
                                break;
                            case "station4":
                                score = int.Parse(item.Value.ToString());
                                break;
                            case "station5":
                                score = int.Parse(item.Value.ToString());
                                break;
                            case "station6":
                                score = int.Parse(item.Value.ToString());
                                break;
                            case "special":
                                score = int.Parse(item.Value.ToString());
                                break;

                        }
                    }
                    // print(s.GetRawJsonValue());
                    InsertUpdateListUserData(new User(queryDeviceID, groupName, score, kinectScore));
                    // print((User)s.Value);
                }

                // foreach (User item in listUserData)
                // {
                //     print(item.groupName);
                //     print(item.score);
                //     print(item.kinectScore);
                // }
            }
        });
    }

    public void WriteNewUser (string groupName, int score = 0, int kinectScore = 0)
    {
        User user = new User(deviceID, groupName, score, kinectScore);
        string json = JsonUtility.ToJson(user);
        print(json);
        reference.Child(deviceID).SetRawJsonValueAsync(json);
    }

    public void UpdateScore(int score, GameEnum.ScoreType scoreType)
    {
        reference.Child(deviceID).Child((scoreType == GameEnum.ScoreType.normal)?normal:kinect).SetValueAsync(score);
    }

    public void UpdateStationScore(int stationIndex, int score)
    {
        reference.Child(deviceID).Child("station").Child(stationIndex.ToString()).SetValueAsync(score);
    }

    public string GetGroupName()
    {
        try
        {
            return listUserData.Find(user => user.deviceID == deviceID).groupName;    
        }
        catch (System.Exception)
        {
            WriteNewUser("NewUser");
            return "New User Restart game for genarate user.";
        }
        
    }
	
}
