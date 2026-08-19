using UnityEngine;

public class FruitMerger : MonoBehaviour
{
    public void RequestMerge(Fruit fruit1, Fruit fruit2)
    {
        if(fruit1.Level != fruit2.Level) return;

        if(fruit1.Merging || fruit2.Merging) return;


        fruit1.Merging = true;
        fruit2.Merging = true;

        int mergeLevel = fruit1.Level + 1;
        FindAnyObjectByType<FruitSpawner>().Spawn(
            mergeLevel,
            Vector2.Lerp(fruit1.transform.position, fruit2.transform.position, 0.5f));
        

        int score = Mathf.RoundToInt(mergeLevel * GameManager.Instance.Config.ScoreMultiplier);
        GameManager.Instance.AddScore(score);

        Destroy(fruit1.gameObject);
        Destroy(fruit2.gameObject);
    }
}
