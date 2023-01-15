using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea_Ground : SpawnAreaScript
{
    public GameObject[] Enemy_List = new GameObject[1];//敵の種類
    public float[] enemy_spawn_time = new float[1];//敵それぞれのスポーン時間
    public float[] max_enemy = new float[1];//敵のそれぞれの最大数

    public float[] enemy_count;//敵のそれぞれの数
    private float[] enemy_time;//敵それぞれのスポーン時間(計測用)

    public GameObject range_A;
    public GameObject range_B;

    // Start is called before the first frame update
    void Start()
    {
        Setup();

        //五種類の敵それぞれのスポーン時間(計測用)
        enemy_time = new float[Enemy_List.Length];
        //敵のそれぞれの数
        enemy_count = new float[Enemy_List.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemode.State == GameModeStateEnum.Play)
        {
            //Spawncount(Enemy_List, enemy_spawn_time, enemy_count, enemy_time, max_enemy, range_A, range_B);

            time = GameModeController.Instance.GameTime;
            if (spawntime.Wave1(time)|| spawntime.Wave2(time)|| spawntime.Wave3(time))//始めから最後まで
            {
                Spawncount(Enemy_List, enemy_spawn_time, enemy_count, enemy_time, max_enemy, range_A, range_B);
            }
        }
    }
}
