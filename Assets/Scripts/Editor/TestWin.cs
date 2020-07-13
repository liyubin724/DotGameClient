using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DotEditor.BehaviourLine;
using DotEngine.BehaviourLine.Line;
using DotEngine.BehaviourLine.Track;
using Newtonsoft.Json;

public class Person
{
    public string name;
    public int age;
}

public class Student : Person
{
    public string className;
}

public class Teacher : Person
{
    public string tName;
}

public class TestWin : EditorWindow
{
    [MenuItem("Test/Test Char")]
    public static void TestChar()
    {
        char c = '中';
        Debug.Log((int)c);

        int ci = (int)c;

        Debug.Log((char)ci);
    }

    [MenuItem("Test/Test Json")]
    public static void TestJson()
    {
        Person person = new Student()
        {
            name = "Test1",
            age = 11,
            className = "Class",
        };
        string json = JsonConvert.SerializeObject(person, new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
        });
        Debug.Log(json);

        Person p = (Person)JsonConvert.DeserializeObject(json, new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
        });
        Debug.Log(p.name);
        Debug.Log(((Student)p).className);
    }

    [MenuItem("Test/Test Timeline")]
    public static void ShowWin()
    {
        GetWindow<TestWin>().Show();
    }

    private TimelineData data = null;
    private TimelineDrawer drawer = null;

    private void OnGUI()
    {
        if(data == null)
        {
            data = new TimelineData();
            data.Tracks.Add(new TracklineData());
            data.Tracks.Add(new TracklineData());
            data.Tracks.Add(new TracklineData());
            data.Tracks.Add(new TracklineData());
            data.Tracks.Add(new TracklineData());
            data.Tracks.Add(new TracklineData());
            data.Tracks.Add(new TracklineData());
            data.Tracks.Add(new TracklineData());
            data.Tracks.Add(new TracklineData());
            data.Tracks.Add(new TracklineData());
            data.TimeLength = 10;
            drawer = new TimelineDrawer(this,"Test");
            drawer.SetData(data);
        }

        drawer.OnGUI(new Rect(0, 0, position.width, position.height));
    }
}
