using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
   public float health = 100f;
   public float disolveSpeed = 10f;
   public Text healthText;


    private void Update() {
        if (gameObject.tag == "Player")
        {
            healthText.text = health.ToString();
        }
    }
   public void TakeDamage(float damage)
   {
       health -= damage;
       if (health <= 0)
       {
           Die();
       }
   }
   void Die()
   {
       Destroy(gameObject);
       if (gameObject.tag == "Player")
       {
           SceneManager.LoadScene("Main");
       }
   }
}
