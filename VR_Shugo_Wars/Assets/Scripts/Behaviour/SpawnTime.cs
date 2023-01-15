using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTime : MonoBehaviour
{

    [System.Serializable]
    public struct SpawnTimer
    {
        public int start;//開始時間
        public int end;//終了時間
    }

    [Header("要素0=wave1、要素1=wave2、要素2=wave3")]
    public SpawnTimer[] wavetime = new SpawnTimer[3];
   
    public bool Wave1(float time)
    {
        return (time >= wavetime[0].start && time <= wavetime[0].end);
    }

    public bool Wave2(float time)
    {
        return (time >= wavetime[1].start && time <= wavetime[1].end);
    }

    public bool Wave3(float time)
    {
        return (time >= wavetime[2].start && time <= wavetime[2].end);
    }
}
