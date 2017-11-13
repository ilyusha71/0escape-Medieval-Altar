using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public partial class Altar : MonoBehaviour
{
    [Header("【日記控制】")]
    private List<int> read = new List<int>();
    private float timerOpen, timerFlip;
    private float durationOpen = 2.37f;
    private float durationFlip = 10.0f;
    public MegaBookBuilder book;
    public Transform bookCamera;
    bool page7, page9, page15, page17;

    void KeyboardOpen()
    {
        if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1))
            commands[1] += "HolyGrail.";
        else if (Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2))
            commands[1] += "RoundTable.";
        else if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3))
            commands[1] += "DragonFang.";
        else if (Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4))
            commands[1] += "Crown.";
        else if (Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Keypad5))
            commands[1] += "Altar.";
        else if (Input.GetKey(KeyCode.Alpha7) || Input.GetKey(KeyCode.Keypad7))
            commands[1] = "Destruction";
        else if (Input.GetKey(KeyCode.Alpha8) || Input.GetKey(KeyCode.Keypad8))
            commands[1] = "Redemption";
        else if (Input.GetKey(KeyCode.Alpha9) || Input.GetKey(KeyCode.Keypad9))
            commands[1] = "Prophet";

    }
    void OpenCheck()
    {
        if (altarState == AltarState.Ready)
        {
            // 打開序章
            if (commands[1].Contains("HolyGrail") ||
                commands[1].Contains("RoundTable") ||
                commands[1].Contains("DragonFang") ||
                commands[1].Contains("Crown") ||
                commands[1].Contains("Altar"))
            {
                GameObject.Find("Altar BGM").GetComponent<AudioSource>().Play();
                altarState = AltarState.Opening;
                timerOpen = Time.time + durationOpen;
                bookCamera.transform.DOMoveX(0, 1.5f).OnComplete(() => { OpenPage((int)altarState); });
            }
        }
        else if (altarState == AltarState.Opening)
        {
            // 打開前四章
            if (commands[1].Contains("HolyGrail"))
                altarState = AltarState.HolyGrail;
            else if (commands[1].Contains("RoundTable"))
                altarState = AltarState.RoundTable;
            else if (commands[1].Contains("DragonFang"))
                altarState = AltarState.DragonFang;
            else if (commands[1].Contains("Crown"))
                altarState = AltarState.Crown;
            else if (commands[1].Contains("Altar"))
                altarState = AltarState.HolyGrail;

            if (altarState != AltarState.Opening)
            {
                timerOpen = Time.time + durationOpen;
                bookCamera.transform.DOMoveX(0, 1.5f).OnComplete(() => { OpenPage((int)altarState); });
            }
        }
        else if (altarState == AltarState.HolyGrail ||
                   altarState == AltarState.RoundTable ||
                   altarState == AltarState.DragonFang ||
                   altarState == AltarState.Crown)
        {
            // 打開第五章
            if (commands[1].Contains("Altar"))
            {
                if (read.Contains((int)AltarState.HolyGrail) &&
                    read.Contains((int)AltarState.RoundTable) &&
                    read.Contains((int)AltarState.DragonFang) &&
                    read.Contains((int)AltarState.Crown))
                {
                    altarState = AltarState.Altar;
                    timerManager.MissionRecord(AltarState.Altar.ToString());
                    timerManager.MissionStart(AltarState.Destruction.ToString());
                    timerManager.MissionStart(AltarState.Redemption.ToString());

                    if (ArduinoController.instance.connectAruidnoCompleted)
                        ArduinoController.arduinoSerialPort.WriteLine("5");
                }
                else if (!read.Contains((int)AltarState.HolyGrail))
                    altarState = AltarState.HolyGrail;
                else if (!read.Contains((int)AltarState.RoundTable))
                    altarState = AltarState.RoundTable;
                else if (!read.Contains((int)AltarState.DragonFang))
                    altarState = AltarState.DragonFang;
                else if (!read.Contains((int)AltarState.Crown))
                    altarState = AltarState.Crown;
            }
            else
            {
                if (commands[1].Contains("HolyGrail"))
                {
                    if (!read.Contains((int)AltarState.HolyGrail))
                        altarState = AltarState.HolyGrail;
                }
                if (commands[1].Contains("RoundTable"))
                {
                    if (!read.Contains((int)AltarState.RoundTable))
                        altarState = AltarState.RoundTable;
                }
                if (commands[1].Contains("DragonFang"))
                {
                    if (!read.Contains((int)AltarState.DragonFang))
                        altarState = AltarState.DragonFang;
                }
                if (commands[1].Contains("Crown"))
                {
                    if (!read.Contains((int)AltarState.Crown))
                        altarState = AltarState.Crown;
                }
            } 
            OpenPage((int)altarState);
        }
        else if (altarState == AltarState.Altar || altarState == AltarState.Destruction)
        {
            // 打開第七章
            if (!read.Contains((int)AltarState.Destruction))
            {
                if (commands[1] == "Destruction")
                {
                    altarState = AltarState.Destruction;
                    timerManager.MissionRecord(AltarState.Destruction.ToString());
                }
            }
            // 打開第八章
            if (!read.Contains((int)AltarState.Redemption))
            {
                if (commands[1] == "Redemption")
                {
                    altarState = AltarState.Redemption;
                    timerManager.MissionRecord(AltarState.Redemption.ToString());
                    timerManager.MissionStart(AltarState.Prophet.ToString());

                    if (ArduinoController.instance.connectAruidnoCompleted)
                        ArduinoController.arduinoSerialPort.WriteLine("9");
                }
            }
            OpenPage((int)altarState);
        }
        else if (altarState == AltarState.Redemption)
        {
            // 打開第九章
            if (!read.Contains((int)AltarState.Prophet))
            {
                if (commands[1].Contains("Prophet"))
                {
                    altarState = AltarState.Prophet;
                    GameObject.Find("Altar BGM").GetComponent<AudioSource>().DOFade(0, 5.0f);
                    timerManager.MissionRecord(AltarState.Prophet.ToString());
                    timerManager.GameFinish();
                }
            }
            OpenPage((int)altarState);
        }
    }
    void OpenPage(int target)
    {
        if (read.Contains(target))
            return;
        ArduinoController.instance.msgBox.Keyword("<color=cyan>日記開啟-"+(AltarState)target + "</color>");
        timerOpen = Time.time + durationOpen;
        read.Add(target);
        int diff = target - (int)book.page;
        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                book.NextPage();
                audioSource.clip = clips[Random.Range(0, 3)];
                audioSource.Play();
            }
        }
        else
        {
            diff = -diff;
            for (int i = 0; i < diff; i++)
            {
                book.PrevPage();
                audioSource.clip = clips[Random.Range(0, 3)];
                audioSource.Play();
            }
        }
        timerFlip = Time.time + durationFlip;

        if (book.page == 7 && !page7)
            StartCoroutine(Page7());
        if (book.page == 17 && !page17)
            StartCoroutine(Page17());
    }
    void ForcePage(int target)
    {
        if (altarState == (AltarState)target)
            return;
        bookCamera.transform.DOMoveX(0, 1.5f).OnComplete(() => { OpenPage((int)altarState); });
        altarState = (AltarState)target;
        timerOpen = Time.time + durationOpen;
        read.Add(target);
        int diff = target - (int)book.page;
        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                book.NextPage();
                audioSource.clip = clips[Random.Range(0, 3)];
                audioSource.Play();
            }
        }
        else
        {
            diff = -diff;
            for (int i = 0; i < diff; i++)
            {
                book.PrevPage();
                audioSource.clip = clips[Random.Range(0, 3)];
                audioSource.Play();
            }
        }
        timerFlip = Time.time + durationFlip;

        if (book.page == 7 && !page7)
            StartCoroutine(Page7());
        if (book.page == 17 && !page17)
            StartCoroutine(Page17());
    }
    void FlipCheck()
    {
        timerFlip = Time.time + durationFlip;
        if (altarState == AltarState.Altar)
        {
            if (book.page < (int)AltarState.Destruction - 1)
            {
                NextPage();
                if (book.page == 9 && !page9)
                    StartCoroutine(Page9());
            }
            else
                FlipPage((int)altarState);
        }
        else if (altarState == AltarState.Destruction)
        {
            if (book.page < (int)AltarState.Redemption - 1)
            {
                NextPage();
            }
            else
                FlipPage((int)altarState);
        }
        else if (altarState == AltarState.Redemption)
        {
            if (book.page < (int)AltarState.Prophet - 1)
            {
                NextPage();
                if (book.page == 15 && !page15)
                    StartCoroutine(Page15());
            }
            else
                FlipPage((int)altarState);
        }
        else if (altarState == AltarState.Prophet)
        {
            //if (book.page < 17)
            //{
            //    NextPage();
            //}
            //else
            //    FlipPage((int)altarState);
        }
    }
    void FlipPage(int target)
    {
        int diff = target - (int)book.page;
        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                NextPage();
            }
        }
        else
        {
            diff = -diff;
            for (int i = 0; i < diff; i++)
            {
                PrevPage();
            }
        }
    }
    void NextPage()
    {
        book.NextPage();
        audioSource.clip = clips[Random.Range(0, 3)];
        audioSource.Play();
    }
    void PrevPage()
    {
        book.PrevPage();
        audioSource.clip = clips[Random.Range(0, 3)];
        audioSource.Play();
    }
    IEnumerator Page7()
    {
        timerFlip = Time.time + durationFlip + 15;
        yield return new WaitForSeconds(1.37f);
        if (book.page == 7)
        {
            audioSource.PlayOneShot(clips[3], 1);
            page7 = true;
        }
    }
    IEnumerator Page9()
    {
        timerFlip = Time.time + durationFlip + 15;
        yield return new WaitForSeconds(1.37f);
        if (book.page == 9)
        {
            audioSource.PlayOneShot(clips[4], 1);
            page9 = true;
        }
    }
    IEnumerator Page15()
    {
        timerFlip = Time.time + durationFlip + 15;
        yield return new WaitForSeconds(1.37f);
        if (book.page == 15)
        {
            audioSource.PlayOneShot(clips[5], 1);
            page15 = true;
        }
    }
    IEnumerator Page17()
    {
        timerFlip = Time.time + durationFlip + 15;
        yield return new WaitForSeconds(1.37f);
        if (book.page == 17)
        {
            audioSource.PlayOneShot(clips[6], 1);
            page17 = true;
        }
    }
    IEnumerator Page18()
    {
        yield return new WaitForSeconds(600.0f);
        ForcePage((int)AltarState.Epilogue);
    }
}
