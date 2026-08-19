using UnityEngine;

public class GameConfig
{
    public readonly float ScoreMultiplier;
    public readonly float DropCooldown;

    public GameConfig(float scoreMultiplier = 10f, float dropCooldown = 2f)
    {
        ScoreMultiplier = scoreMultiplier;
        DropCooldown = dropCooldown;
    }
}
