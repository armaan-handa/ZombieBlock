using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    public float health = 100f;
    public Text healthText;
    public GameObject damageTextPF;
    public Animation deathAnim;
    Camera mainCam;
    public bool isDead = false;

    private void Start() 
    {
        mainCam = Camera.main;
    }
    private void Update() 
    {
        if (gameObject.tag == "Player")
        {
            healthText.text = health.ToString();
        }
    }
    public void TakeDamage(float damage)
    {
        if(isDead)
            return;
        
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        if (gameObject.tag == "Enemy")
        {
            GameObject damageText = Instantiate(damageTextPF, transform.position, mainCam.transform.rotation, transform);
            damageText.GetComponent<TextMeshPro>().text = damage.ToString();
            Destroy(damageText, 1f);
            gameObject.GetComponent<EnemyController>().timeTillNextWander = 0;
        }
    }
    void Die()
    {   
        isDead = true;
        
        if (gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Main");
        }
        deathAnim.Play();
        Destroy(gameObject, 2f);
    }
}
