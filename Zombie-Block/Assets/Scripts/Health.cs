using UnityEngine;

public class Health : MonoBehaviour
{
   public float health = 100f;
   public float disolveSpeed = 10f;
   public Material material;

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
   }
}
