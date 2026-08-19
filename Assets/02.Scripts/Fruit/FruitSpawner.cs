using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] private List<Fruit> _fruitPrefabs = new();
    [SerializeField] private Vector2 _spawnPosition;

    private void Start()
    {
        Spawn();
    }
    private void Spawn()
    {
        int randomIndex = Random.Range(0, 5);
        Instantiate(_fruitPrefabs[randomIndex], _spawnPosition, Quaternion.identity);
    }
    public void Spawn(int level, Vector2 postion)
    {
        if(level >= _fruitPrefabs.Count) return;
        Fruit fruit = Instantiate(_fruitPrefabs[level], postion, Quaternion.identity);
        fruit.Dropped = true;
    }

    public async void SpawnAfterDelay()
    {

        float delay = GameManager.Instance.Config.DropCooldown;
        await Awaitable.WaitForSecondsAsync(delay);

        Spawn();
    }
}
