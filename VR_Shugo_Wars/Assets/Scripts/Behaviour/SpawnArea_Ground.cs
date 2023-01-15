using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea_Ground : SpawnAreaScript
{
    public GameObject[] Enemy_List = new GameObject[1];//�G�̎��
    public float[] enemy_spawn_time = new float[1];//�G���ꂼ��̃X�|�[������
    public float[] max_enemy = new float[1];//�G�̂��ꂼ��̍ő吔

    public float[] enemy_count;//�G�̂��ꂼ��̐�
    private float[] enemy_time;//�G���ꂼ��̃X�|�[������(�v���p)

    public GameObject range_A;
    public GameObject range_B;

    // Start is called before the first frame update
    void Start()
    {
        Setup();

        //�܎�ނ̓G���ꂼ��̃X�|�[������(�v���p)
        enemy_time = new float[Enemy_List.Length];
        //�G�̂��ꂼ��̐�
        enemy_count = new float[Enemy_List.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemode.State == GameModeStateEnum.Play)
        {
            //Spawncount(Enemy_List, enemy_spawn_time, enemy_count, enemy_time, max_enemy, range_A, range_B);

            time = GameModeController.Instance.GameTime;
            if (spawntime.Wave1(time)|| spawntime.Wave2(time)|| spawntime.Wave3(time))//�n�߂���Ō�܂�
            {
                Spawncount(Enemy_List, enemy_spawn_time, enemy_count, enemy_time, max_enemy, range_A, range_B);
            }
        }
    }
}
