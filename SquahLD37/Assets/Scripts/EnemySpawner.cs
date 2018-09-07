using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public float cooldown;

    public GameObject[] Enemies;
    public Wave[] Waves;

    public Rect SpawnAreaRect;
    public Rect NonSpawnRect;

    private int waveNum;

    private int remainingToCont;

	public Transform[] spawnAreas;

	public float minDistFromPlayer;

	private float waitForVOTime;
	private Wave waveToSpawnAfterVO;
	private bool spawnWaveAfterVO;

    // Use this for initialization
    void Start()
    {
        waveNum = 0;
		
		//SpawnAreaRect = new Rect(spawnOuter.position, new Vector2(spawnOuter.transform.localScale.x, spawnOuter.transform.localScale.y));
		//NonSpawnRect = new Rect(spawnInner.position, new Vector2(spawnInner.transform.localScale.x, spawnInner.transform.localScale.y));
    }

    // Update is called once per frame
    void Update()
    {


        var enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Count(item => item.GetComponent<Enemy>().health > 0);
	    waitForVOTime -= Time.deltaTime;
		//Debug.Log(waitForVOTime);

        if (enemyCount <= remainingToCont && waitForVOTime <= 0)
        {
            if (waveNum >= Waves.Length)
            {
                return;
            }

	        Wave wave = Waves[waveNum];

	        if (wave.vo.enabled && !spawnWaveAfterVO)
	        {
		        bool soundPlayed = true;
		        if (wave.vo.playOnlyOnce)
		        {
			        if (!AudioManager.instance.PlaySpecialSound(wave.vo.clip))
			        {
				        waitForVOTime = 0;
				        soundPlayed = false;
			        }
		        }
		        else
		        {
			        AudioManager.instance.PlaySound(wave.vo.clip);
		        }

				//Should happen even if sound is not played, so wave is spawned after 0-time delay
		        spawnWaveAfterVO = true;
		        waveToSpawnAfterVO = wave;

		        if (soundPlayed)
		        {
			        SubtitleHandler.instance.AddLines(wave.vo.lines, wave.vo.delays);
			        AudioManager.instance.DampenMusic();
					waitForVOTime = wave.vo.totalTime;
		        }
	        }
	        else if (!spawnWaveAfterVO)
	        {
		        SpawnWave(wave);
		        ++waveNum;
	        }

	        if (spawnWaveAfterVO && waitForVOTime <= 0)
	        {
		        SpawnWave(waveToSpawnAfterVO);
		        ++waveNum;
				AudioManager.instance.UndampenMusic();
		        spawnWaveAfterVO = false;
	        }
        }

		//Cheats
		//if (Input.GetKeyDown("space"))
		//	KillAll();
    }

	public void KillAll()
	{
		IEnumerable<GameObject> enemies = GameObject.FindGameObjectsWithTag("Enemy").Where(item => item.GetComponent<Enemy>().health != 0);
		foreach (GameObject enemy in enemies)
		{
			enemy.GetComponent<Enemy>().Damage(1000);
		}
	}

    public void SpawnWave(Wave _wave)
    {
        for (int i = 0; i < _wave.Enemies.Length; ++i)
	        for (int j = 0; j < _wave.Enemies[i].count; ++j)
		        spawn(_wave.Enemies[i].type);

        remainingToCont = _wave.remainingTillNext;

    }


    void spawn(EnemyType type)
    {
        Transform area = spawnAreas[Random.Range(0, 4)];
        //float xTrans = 0;
        //float yTrans = 0;

        //float xPot = Random.value;
        //float yPot = Random.value;

        //switch (area)
        //{
        //    case 0:
        //        xTrans = xPot * (NonSpawnRect.xMin - SpawnAreaRect.xMin) + SpawnAreaRect.xMin;
        //        yTrans = yPot * (SpawnAreaRect.yMax - SpawnAreaRect.yMin) + SpawnAreaRect.yMin;
        //        break;
        //    case 1:
        //        xTrans = xPot * (NonSpawnRect.xMax - NonSpawnRect.xMin) + NonSpawnRect.xMin;
        //        yTrans = yPot * (SpawnAreaRect.yMax - NonSpawnRect.yMax) + NonSpawnRect.yMax;
        //        break;
        //    case 2:
        //        xTrans = xPot * (SpawnAreaRect.xMax - NonSpawnRect.xMax) + NonSpawnRect.xMax;
        //        yTrans = yPot * (SpawnAreaRect.yMax - SpawnAreaRect.yMin) + SpawnAreaRect.yMin;
        //        break;
        //    case 3:
        //        xTrans = xPot * (NonSpawnRect.xMax - NonSpawnRect.xMax) + NonSpawnRect.xMin;
        //        yTrans = yPot * (SpawnAreaRect.yMax - NonSpawnRect.yMax) + SpawnAreaRect.yMax;
        //        break;
        //}
		
	    float width = area.lossyScale.x;
	    float height = area.lossyScale.y;
	    float xTrans = area.position.x - width/2f;
	    float yTrans = area.position.y - height/2f;
	    xTrans += Random.value*width;
	    yTrans += Random.value*height;

		Vector3 pos = new Vector3(xTrans, yTrans, 0);
	    float distFromPlayer = (pos - Player.instance.gameObject.transform.position).magnitude;
		if (distFromPlayer > minDistFromPlayer)
			Instantiate(Enemies[(int) type], new Vector3(xTrans, yTrans, 0), Quaternion.identity);
		else
			spawn(type);
    }

}

[Serializable]
public struct Wave
{
    public EnemyWaveType[] Enemies;
    public int remainingTillNext;
	public VoiceOver vo;
}

public enum EnemyType
{
	Stool,
	Table,
	Lamp,
	Bed
}


[Serializable]
public struct EnemyWaveType
{
	public EnemyType type;
    public int count;
}

[Serializable]
public struct VoiceOver
{
	public bool enabled;
	public AudioClip clip;
	public List<String> lines;
	public List<float> delays;
	public float totalTime;
	public bool playOnlyOnce;
}