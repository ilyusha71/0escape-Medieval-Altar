/*****************************************************************************
 * Wakaka Studio 2017
 * Author: iLYuSha Dawa-mumu Wakaka Kocmocovich Kocmocki KocmocA
 * Project:: 0escape Medieval - Magic Altar
 * Tools: Unity 5/C# + Arduino/C++
 * Last Updated: 2017/07/21
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MagicAltar : MonoBehaviour
{
    public enum AltarState
    {
        Ready=0,
        Opening =2,
        HolyGrail =3,
        RoundTable=4,
        DragonFang=5,
        Crown=6,
        MagicAltar=7,
        Destruction=12,
        Redemption=14,
        Prophet=17,
    }
    public AltarState altarState = new AltarState();
    private List<int> read = new List<int>();
    private float timerOpen;
    private float durationOpen = 2.37f;
    private float timerFlip;
    private float durationFlip = 10.0f;
    public MegaBookBuilder book;
    public Transform bookCamera;
    private AudioSource audioSource;
    public AudioClip[] clips;
    bool page7;
    bool page9;
    bool page15;
    bool page17;

    void Awake()
    {
        //if (ArduinoController.instance == null)
        //{
        //    PlayerPrefs.SetInt("lastScene", SceneManager.GetActiveScene().buildIndex + 100);
        //    SceneManager.LoadScene("Arduino Controller");
        //}

        //bookCamera.position += new Vector3(0.4f, 0, 0);
        //audioSource = GetComponent<AudioSource>();
    }

    void OpenPage(int target)
    {
        if (read.Contains(target))
            return;
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

    void FlipPage(int target)
    {
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
    }

    void Update ()
    {
        //if (Time.time > timerOpen)
        //{
        //    if (altarState == AltarState.Ready)
        //    {
        //        if (ArduinoController.command == "HolyGrail" || Input.GetKey(KeyCode.Alpha1))
        //            altarState = AltarState.Opening;
        //        else if (ArduinoController.command == "RoundTable" || Input.GetKey(KeyCode.Alpha2))
        //            altarState = AltarState.Opening;
        //        else if (ArduinoController.command == "DragonFang" || Input.GetKey(KeyCode.Alpha3))
        //            altarState = AltarState.Opening;
        //        else if (ArduinoController.command == "Crown" || Input.GetKey(KeyCode.Alpha4))
        //            altarState = AltarState.Opening;
        //        else if(ArduinoController.command == "MagicAltar" || Input.GetKey(KeyCode.Alpha5))
        //            altarState = AltarState.Opening;

        //        //Opening

        //        if (altarState != AltarState.Ready)
        //        {
        //            timerOpen = Time.time + durationOpen;
        //            bookCamera.transform.DOMoveX(0, 1.5f).OnComplete(() => { OpenPage((int)altarState); });
        //        }
        //    }
        //    else if (altarState == AltarState.Opening)
        //    {
        //        if (ArduinoController.command == "HolyGrail" || Input.GetKey(KeyCode.Alpha1))
        //            altarState = AltarState.HolyGrail;
        //        else if (ArduinoController.command == "RoundTable" || Input.GetKey(KeyCode.Alpha2))
        //            altarState = AltarState.RoundTable;
        //        else if (ArduinoController.command == "DragonFang" || Input.GetKey(KeyCode.Alpha3))
        //            altarState = AltarState.DragonFang;
        //        else if (ArduinoController.command == "Crown" || Input.GetKey(KeyCode.Alpha4))
        //            altarState = AltarState.Crown;
        //        else if (ArduinoController.command == "MagicAltar" || Input.GetKey(KeyCode.Alpha5))
        //            altarState = AltarState.HolyGrail;

        //        if (altarState != AltarState.Opening)
        //        {
        //            timerOpen = Time.time + durationOpen;
        //            bookCamera.transform.DOMoveX(0, 1.5f).OnComplete(() => { OpenPage((int)altarState); });
        //        }
        //    }
        //    else if (altarState == AltarState.HolyGrail || altarState == AltarState.RoundTable || altarState == AltarState.DragonFang || altarState == AltarState.Crown)
        //    {
        //        if (ArduinoController.command == "MagicAltar" || Input.GetKey(KeyCode.Alpha5))
        //        {
        //            if (read.Contains((int)AltarState.HolyGrail) &&
        //                read.Contains((int)AltarState.RoundTable) &&
        //                read.Contains((int)AltarState.DragonFang) &&
        //                read.Contains((int)AltarState.Crown))
        //            {
        //                altarState = AltarState.MagicAltar;
        //                if (ArduinoController.arduinoSerialPort.IsOpen)
        //                    ArduinoController.arduinoSerialPort.WriteLine("5");
        //            }
        //            else if (!read.Contains((int)AltarState.HolyGrail))
        //                altarState = AltarState.HolyGrail;
        //            else if (!read.Contains((int)AltarState.RoundTable))
        //                altarState = AltarState.RoundTable;
        //            else if (!read.Contains((int)AltarState.DragonFang))
        //                altarState = AltarState.DragonFang;
        //            else if (!read.Contains((int)AltarState.Crown))
        //                altarState = AltarState.Crown;
        //        }
        //        else if (ArduinoController.command == "HolyGrail" || Input.GetKey(KeyCode.Alpha1))
        //        {
        //            if (!read.Contains((int)AltarState.HolyGrail))
        //                altarState = AltarState.HolyGrail;
        //        }
        //        else if (ArduinoController.command == "RoundTable" || Input.GetKey(KeyCode.Alpha2))
        //        {
        //            if (!read.Contains((int)AltarState.RoundTable))
        //                altarState = AltarState.RoundTable;
        //        }
        //        else if (ArduinoController.command == "DragonFang" || Input.GetKey(KeyCode.Alpha3))
        //        {
        //            if (!read.Contains((int)AltarState.DragonFang))
        //                altarState = AltarState.DragonFang;
        //        }
        //        else if (ArduinoController.command == "Crown" || Input.GetKey(KeyCode.Alpha4))
        //        {
        //            if (!read.Contains((int)AltarState.Crown))
        //                altarState = AltarState.Crown;
        //        }

        //        OpenPage((int)altarState);
        //    }
        //    else if (altarState == AltarState.MagicAltar || altarState == AltarState.Destruction)
        //    {
        //        if (!read.Contains((int)AltarState.Destruction))
        //        {
        //            if (ArduinoController.command == "Destruction" || Input.GetKey(KeyCode.Alpha7))
        //                altarState = AltarState.Destruction;
        //        }
        //        if (!read.Contains((int)AltarState.Redemption))
        //        {
        //            if (ArduinoController.command == "Redemption" || Input.GetKey(KeyCode.Alpha8))
        //            {
        //                altarState = AltarState.Redemption;
        //                if (ArduinoController.arduinoSerialPort.IsOpen)
        //                    ArduinoController.arduinoSerialPort.WriteLine("9");
        //            }
        //        }
        //        OpenPage((int)altarState);
        //    }
        //    else if (altarState == AltarState.Redemption)
        //    {
        //        if (!read.Contains((int)AltarState.Prophet))
        //        {
        //            if (ArduinoController.command == "Prophet" || Input.GetKey(KeyCode.Alpha9))
        //                altarState = AltarState.Prophet;
        //        }

        //        OpenPage((int)altarState);
        //    }
        //}

        //if (Time.time > timerFlip)
        //{
        //    timerFlip = Time.time + durationFlip;
        //    if (altarState == AltarState.MagicAltar)
        //    {
        //        if (book.page < (int)AltarState.Destruction - 1)
        //        {
        //            book.NextPage();
        //            audioSource.clip = clips[Random.Range(0, 3)];
        //            audioSource.Play();

        //             if (book.page == 9 && !page9)
        //                StartCoroutine(Page9());
        //        }
        //        else
        //            FlipPage((int)altarState);
        //    }
        //    else if (altarState == AltarState.Destruction)
        //    {
        //        if (book.page < (int)AltarState.Redemption-1)
        //        {
        //            book.NextPage();
        //            audioSource.clip = clips[Random.Range(0, 3)];
        //            audioSource.Play();
        //        }
        //        else
        //            FlipPage((int)altarState);
        //    }
        //    else if (altarState == AltarState.Redemption)
        //    {
        //        if (book.page < (int)AltarState.Prophet-1)
        //        {
        //            book.NextPage();
        //            audioSource.clip = clips[Random.Range(0, 3)];
        //            audioSource.Play();

        //            if (book.page == 15 && !page15)
        //                StartCoroutine(Page15());
        //        }
        //        else
        //            FlipPage((int)altarState);
        //    }
        //    else if (altarState == AltarState.Prophet)
        //    {
        //        if (book.page < 18)
        //        {
        //            book.NextPage();
        //            audioSource.clip = clips[Random.Range(0, 3)];
        //            audioSource.Play();
        //        }
        //        else
        //            FlipPage((int)altarState);
        //    }
        //}	
	}

    IEnumerator Page7()
    {
        timerFlip = Time.time + durationFlip + 15;
        yield return new WaitForSeconds(1.37f);
        audioSource.PlayOneShot(clips[3], 1);
        page7 = true;
    }
    IEnumerator Page9()
    {
        timerFlip = Time.time + durationFlip + 15;
        yield return new WaitForSeconds(1.37f);
        audioSource.PlayOneShot(clips[4], 1);
        page9 = true;
    }
    IEnumerator Page15()
    {
        yield return new WaitForSeconds(1.37f);
        audioSource.PlayOneShot(clips[5], 1);
        page15 = true;
    }
    IEnumerator Page17()
    {
        timerFlip = Time.time + durationFlip + 15;
        yield return new WaitForSeconds(1.37f);
        audioSource.PlayOneShot(clips[6], 1);
        page17 = true;
    }
}
