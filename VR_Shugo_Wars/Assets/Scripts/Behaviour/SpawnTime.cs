using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTime : MonoBehaviour
{

    [System.Serializable]
    public struct SpawnTimer
    {
        public int start;//�J�n����
        public int end;//�I������
    }

    [Header("�v�f0=wave1�A�v�f1=wave2�A�v�f2=wave3")]
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
