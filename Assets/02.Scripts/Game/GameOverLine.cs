using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverLine : MonoBehaviour
{
    private float _gameOverTime = 2.0f;
    private float _timer;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Fruit")) return;

        Fruit fruit = collision.gameObject.GetComponent<Fruit>();
       if(fruit == null) return;
       if(!fruit.Dropped) return;

       _timer += Time.deltaTime;
        if( _timer >= _gameOverTime )
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Fruit")) return;

        _timer = 0f;
    }
}
