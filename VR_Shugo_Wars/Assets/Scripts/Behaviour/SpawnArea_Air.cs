using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea_Air : SpawnAreaScript
{
    public GameObject[] Enemy_List = new GameObject[1];//�G�̎��
    public float[] enemy_spawn_time = new float[1];//�G���ꂼ��̃X�|�[������
    public float[] max_enemy = new float[1];//�G�̂��ꂼ��̍ő吔 (�D�̏���̓R�[�X�|�C���g�̊֌W��ő�4�܂�)

    public float[] enemy_count;//�G�̂��ꂼ��̐�
    private float[] enemy_time;//�G���ꂼ��̃X�|�[������(�v���p)

    public GameObject range_A;
    public GameObject range_B;

    //public List<GameObject> enemyshiplist = new List<GameObject>();

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
            // Debug.Log(time);

            if (spawntime.Wave2(time))//1������(�c��2������) 0=�~�T�C���̂�
            {
                Spawnpickup(Enemy_List, 0, enemy_spawn_time, enemy_count, enemy_time, max_enemy, range_A, range_B);
            }
            else if (spawntime.Wave3(time))//2������(�c��1������) �~�T�C���ƃK�g�����O
            {
                Spawncount(Enemy_List, enemy_spawn_time, enemy_count, enemy_time, max_enemy, range_A, range_B);
            }
        }
    }

}
