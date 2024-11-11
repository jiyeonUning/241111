using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] float randomRange;

    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log("플레이어 참가");

        if (player != Runner.LocalPlayer) return;

        Vector3 spawnPos = new Vector3(Random.Range(-randomRange, randomRange), 1, Random.Range(-randomRange, randomRange));
        Runner.Spawn(PlayerPrefab, spawnPos, Quaternion.identity);
    }

    public void PlayerLeft(PlayerRef player)
    {
        Debug.Log("플레이어 나감");
    }
}