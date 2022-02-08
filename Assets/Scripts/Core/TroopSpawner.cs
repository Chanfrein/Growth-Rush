using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Control.AIControl;

public class TroopSpawner : MonoBehaviour
{
    [Header("General")]
    [SerializeField] int secondsBetweenWaves = 20;
    [SerializeField] GameObject swordmanEastPrefab;
    [SerializeField] GameObject swordmanWestPrefab;
    [SerializeField] GameObject archerEastPrefab;
    [SerializeField] GameObject archerWestPrefab;


    [Header("West")]
    [SerializeField] Transform spawnPointWest;
    [SerializeField] PatrolPath roadPathWestA;
    [SerializeField] PatrolPath roadPathWestB;
    [SerializeField] float nbOfSwordmenWest;
    [SerializeField] float nbOfArchersWest;


    [Header("East")]
    [SerializeField] Transform spawnPointEast;
    [SerializeField] PatrolPath roadPathEastA;
    [SerializeField] PatrolPath roadPathEastB;
    [SerializeField] float nbOfSwordmenEast;
    [SerializeField] float nbOfArchersEast;

    bool shouldSpawn = true;

    void Start()
    {
        BeginSpawning();
    }

    void OnApplicationQuit() 
    {
        shouldSpawn = false;
    }

    private async void BeginSpawning()
    {
        while(shouldSpawn)
        {
            SpawnTroopsWest();
            SpawnTroopsEast();

            await Task.Delay(secondsBetweenWaves * 1000);
        }
    }

    private async void SpawnTroopsWest()
    {
        for (int i = 0; i < nbOfSwordmenWest; i++)
        {
            GameObject newSwordman = Instantiate(swordmanWestPrefab, spawnPointWest);
            if(i % 2 == 0)
            {
                newSwordman.GetComponent<AIController>().SetPatrolPath(roadPathWestA);
                await Task.Delay(400);
                continue;
            }
            newSwordman.GetComponent<AIController>().SetPatrolPath(roadPathWestB);
            await Task.Delay(400);
        }

        for (int i = 0; i < nbOfArchersWest; i++)
        {
            GameObject newArcher = Instantiate(archerWestPrefab, spawnPointWest);
            if (i % 2 == 0)
            {
                newArcher.GetComponent<AIController>().SetPatrolPath(roadPathWestA);
                await Task.Delay(400);
                continue;
            }
            newArcher.GetComponent<AIController>().SetPatrolPath(roadPathWestB);
            await Task.Delay(400);
        }
    }

    private async void SpawnTroopsEast()
    {
        for (int i = 0; i < nbOfSwordmenEast; i++)
        {
            GameObject newSwordman = Instantiate(swordmanEastPrefab, spawnPointEast);
            if (i % 2 == 0)
            {
                newSwordman.GetComponent<AIController>().SetPatrolPath(roadPathEastA);
                await Task.Delay(400);
                continue;
            }
            newSwordman.GetComponent<AIController>().SetPatrolPath(roadPathEastB);
            await Task.Delay(400);
        }

        for (int i = 0; i < nbOfArchersEast; i++)
        {
            GameObject newArcher = Instantiate(archerEastPrefab, spawnPointEast);
            if (i % 2 == 0)
            {
                newArcher.GetComponent<AIController>().SetPatrolPath(roadPathEastA);
                await Task.Delay(400);
                continue;
            }
            newArcher.GetComponent<AIController>().SetPatrolPath(roadPathEastB);
            await Task.Delay(400);
        }
    }
}
