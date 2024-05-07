using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

///<summary>
///
///</summary>
public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count==0?null:enemyList[Random.Range(0, enemyList.Count)];
    public int WaveNumber => waveNumber;
    public float TimeBetweenWaves => timeBetweenWaves;
    [SerializeField] bool spawnEnemy = true;
    [SerializeField]GameObject waveUI;
    [SerializeField]GameObject[] enemyPrefabs;
    [SerializeField]float timeBetweenSpawns = 1f;
    [SerializeField]float timeBetweenWaves= 1f;

    [SerializeField] int minEnemyAmout=4;
    [SerializeField] int maxEnemyAmout=10;

    WaitForSeconds waitTimeBetweenSpawns;
    WaitUntil waituntilNoEnemy;
    WaitForSeconds waitTimeBetweenWaves;

    int waveNumber = 1;
    int enemyAmount;
    List<GameObject> enemyList;
    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waituntilNoEnemy = new WaitUntil( ()=>enemyList.Count==0);
        waitTimeBetweenWaves=new WaitForSeconds(timeBetweenWaves);
    }
     IEnumerator Start()
    {
        while(spawnEnemy)
        {
            

            waveUI.SetActive(true);
            yield return waitTimeBetweenWaves;
            waveUI.SetActive(false);
            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
        
    }

    IEnumerator RandomlySpawnCoroutine()
    {
        enemyAmount= Mathf.Clamp(enemyAmount, minEnemyAmout+waveNumber/3, maxEnemyAmout);
        for(int i=0;i<enemyAmount;i++)
        {
            enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));
            yield return waitTimeBetweenSpawns; 
        }
        yield return waituntilNoEnemy;
        waveNumber++;
    }
    public void RemoveFromList(GameObject enemy)=>enemyList.Remove(enemy);
    
}
