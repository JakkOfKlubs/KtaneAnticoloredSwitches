using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using KModkit;
using System.Text.RegularExpressions;

public class AnticoloredSwitchesScript : MonoBehaviour
{
    public KMBombModule Module;
    public KMBombInfo BombInfo;
    public KMAudio Audio;
    public KMSelectable[] SwitchSelectables;
    public GameObject[] Switches;
    public GameObject[] TopLeds, BottomLeds;

    private static int _moduleIdCounter = 1;
    private int _moduleId;
    private bool _moduleSolved;
    private bool[] _switchStates = new bool[5];
    private const int _switchAngle = 55;
    private int[] swLog = new int[5];

    private void Start()
    {
        _moduleId = _moduleIdCounter++;
        for (int i = 0; i < SwitchSelectables.Length; i++)
        {
            int j = i;
            SwitchSelectables[i].OnInteract += delegate ()
            {
                if (!_moduleSolved)
                    FlipSwitch(j);
                return false;
            };
        }
    }

    private void FlipSwitch(int sw)
    {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Switches[sw].transform);
        _switchStates[sw] = !_switchStates[sw];
        StartCoroutine(SwitchAnimation(sw));
    }

    private IEnumerator SwitchAnimation(int sw)
    {
        var switchFrom = _switchStates[sw] ? -55f : 55f;
        var switchTo = _switchStates[sw] ? 55f : -55f;
        var duration = 0.2f;
        var elapsed = 0f;
        while (elapsed < duration)
        {
            Switches[sw].transform.localEulerAngles = new Vector3(Easing.InOutQuad(elapsed, switchFrom, switchTo, duration), 0f, 0f);
            yield return null;
            elapsed += Time.deltaTime;
        }
        Switches[sw].transform.localEulerAngles = new Vector3(switchTo, 0f, 0f);
    }
}
