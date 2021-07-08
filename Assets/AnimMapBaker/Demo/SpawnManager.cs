using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// demo使用，分配大量的活动的角色
/// </summary>
public class SpawnManager : MonoBehaviour {

    #region 字段

    public GameObject spawnPrefab;
    public int gridWidth;
    public int gridHeight;

    #endregion


    #region 方法

    void Start()
    {
        for(var i = 0; i < gridWidth; i++)
        {
            for(var j = 0; j < gridHeight; j++)
            {
                Instantiate<GameObject>(spawnPrefab, new Vector3(i, 0, j), Quaternion.identity);
            }
        }
    }


    #endregion

}
