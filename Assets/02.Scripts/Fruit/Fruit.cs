using UnityEngine;

public class Fruit : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    [SerializeField] private int _level;
    public int Level => _level;
    private bool _merging;
    public bool Merging
    {
        get => _merging;
        set
        {
            _merging = value;
        }
    }

    private bool _dropped = false;
    public bool Dropped
    {
        get => _dropped;
        set
        {
            _dropped = value;
            _rigidbody.gravityScale = value ? 1f : 0f;
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        
        Dropped = false;
        Merging = false;
    }

    private void Update()
    {
        if(_dropped) return;

        if(Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            float extentsX = _collider.bounds.extents.x;

            worldPosition.x = Mathf.Clamp(worldPosition.x, -6.5f + extentsX, 6.5f - extentsX); // 너비 제한
            worldPosition.y = 6.7f; // 높이 제한

            transform.position =  worldPosition;
        }

        if(Input.GetMouseButtonUp(0))
        {
            Dropped = true;
            FindAnyObjectByType<FruitSpawner>().SpawnAfterDelay(); 
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Fruit"))
        {
            Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();
            if(otherFruit == null) return;
            if(!otherFruit.Dropped) return;   
            
            FindAnyObjectByType<FruitMerger>().RequestMerge(this, otherFruit);
        }
    }
}
