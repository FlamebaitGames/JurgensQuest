using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class HighScorePanel : MonoBehaviour {
    public class IntComp : IComparer<int>
    {
        public int Compare(int a, int b)
        {
            return b.CompareTo(a);
        }
    }
    SortedList<int, int> sortedList = new SortedList<int, int>(new IntComp());
    
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            int t = PlayerPrefs.GetInt("HS" + i, 50 * (i+1));
            sortedList.Add(t, t);
        }
        UpdateScoreTable();
        
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void AddNewScore(int newScore)
    {
        sortedList.Add(newScore, newScore);
        UpdateScoreTable();
    }

    private void UpdateScoreTable()
    {
        string str = "High Scores:\n";
        for (int i = 0; i < 10 && i < sortedList.Values.Count; i++)
        {
            str += sortedList.Values[i] + "\n";
        }
        GetComponentInChildren<Text>().text = str;
    }

    void OnApplicationQuit()
    {
        for (int i = 0; i < 10 && i < sortedList.Values.Count; i++)
        {
            PlayerPrefs.SetInt("HS"+i, sortedList.Values[i]);
        }
        PlayerPrefs.Save();
    }
}
