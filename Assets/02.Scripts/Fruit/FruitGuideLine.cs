using UnityEngine;

public class FruitGuideLine : MonoBehaviour
{
    private float _startY;

    private void Start()
    {
        _startY = transform.position.y;
    }

    private void Update()
    {
        Vector2 mousePostion = Input.mousePosition;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePostion);
        worldPosition.y = _startY;

        transform.position = worldPosition;
    }
}
