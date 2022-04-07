using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.AIControl;
using System.Threading;
using System.Threading.Tasks;

namespace TroopSpawning
{
    public class TroopSpawner : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] int secondsBetweenWaves = 20;
        [SerializeField] GameObject swordmanEastPrefab;
        [SerializeField] GameObject swordmanWestPrefab;
        [SerializeField] GameObject archerEastPrefab;
        [SerializeField] GameObject archerWestPrefab;

        [Header("Player")]
        [SerializeField] Transform spawnPointPlayer;
        [SerializeField] PatrolPath roadPathPlayerA;
        [SerializeField] PatrolPath roadPathPlayerB;
        [SerializeField] public int nbOfSwordmenPlayer;
        [SerializeField] public int nbOfArchersPlayer;

        [Header("Hotrion")]
        [SerializeField] Transform spawnPointHotrion;
        [SerializeField] PatrolPath roadPathHotrionA;
        [SerializeField] PatrolPath roadPathHotrionB;
        [SerializeField] public int nbOfSwordmenHotrion;
        [SerializeField] public int nbOfArchersHotrion;

        private CancellationTokenSource source;

        void Start()
        {
            source = new CancellationTokenSource();
            BeginSpawning(source.Token);
        }

        void OnApplicationQuit()
        {
            source.Cancel();
            source.Dispose();
        }

        private async void BeginSpawning(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                SpawnTroopsWest();
                SpawnTroopsEast();

                await Task.Delay(secondsBetweenWaves * 1000);
            }
        }

        private async void SpawnTroopsWest()
        {
            for (int i = 0; i < nbOfSwordmenPlayer; i++)
            {
                GameObject newSwordman = Instantiate(swordmanWestPrefab, spawnPointPlayer);
                if (i % 2 == 0)
                {
                    newSwordman.GetComponent<AIController>().SetPatrolPath(roadPathPlayerA);
                    await Task.Delay(400);
                    continue;
                }
                newSwordman.GetComponent<AIController>().SetPatrolPath(roadPathPlayerB);
                await Task.Delay(400);
            }

            for (int i = 0; i < nbOfArchersPlayer; i++)
            {
                GameObject newArcher = Instantiate(archerWestPrefab, spawnPointPlayer);
                if (i % 2 == 0)
                {
                    newArcher.GetComponent<AIController>().SetPatrolPath(roadPathPlayerA);
                    await Task.Delay(400);
                    continue;
                }
                newArcher.GetComponent<AIController>().SetPatrolPath(roadPathPlayerB);
                await Task.Delay(400);
            }
        }

        private async void SpawnTroopsEast()
        {
            for (int i = 0; i < nbOfSwordmenHotrion; i++)
            {
                GameObject newSwordman = Instantiate(swordmanEastPrefab, spawnPointHotrion);
                if (i % 2 == 0)
                {
                    newSwordman.GetComponent<AIController>().SetPatrolPath(roadPathHotrionA);
                    await Task.Delay(400);
                    continue;
                }
                newSwordman.GetComponent<AIController>().SetPatrolPath(roadPathHotrionB);
                await Task.Delay(400);
            }

            for (int i = 0; i < nbOfArchersHotrion; i++)
            {
                GameObject newArcher = Instantiate(archerEastPrefab, spawnPointHotrion);
                if (i % 2 == 0)
                {
                    newArcher.GetComponent<AIController>().SetPatrolPath(roadPathHotrionA);
                    await Task.Delay(400);
                    continue;
                }
                newArcher.GetComponent<AIController>().SetPatrolPath(roadPathHotrionB);
                await Task.Delay(400);
            }
        }
    }
}
