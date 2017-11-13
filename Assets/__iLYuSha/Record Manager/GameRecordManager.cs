using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TextCustom
{
    public static string TextColor(string colorHex, string text)
    {
        return "<color=" + colorHex + ">" + text + "</color>";
    }

    public static string TextSize(int size, string text)
    {
        return "<size=" + size + ">" + text + "</size>";
    }
}

public class GameRecordManager : MonoBehaviour
{
    public GameObject recordCanvas;
    public Text textRecord, textRealtime;
    public int iniRecord = 3600;
    internal int[] loadRecord = new int[9];
    internal int[] newRecord = new int[9];
    internal string[] loadDate = new string[9];
    internal string[] newDate = new string[9];
    internal string dateNow;
}
