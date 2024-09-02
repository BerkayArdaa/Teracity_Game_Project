using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth1 : MonoBehaviour
{
    public float health = 100f;  // Düþmanýn baþlangýç caný

    public void TakeDamage(float damage)
    {
        health -= damage;  // Alýnan zarar kadar caný azalt
        Debug.Log(gameObject.name + " has " + health + " health left.");  // Güncel can durumunu logla
        if (health <= 0)
        {
            Die();  // Caný 0 veya altýna düþtüyse ölüm fonksiyonunu çaðýr
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " öldü.");  // Ölüm logu
        // Burada ölüm animasyonu veya patlama efekti tetiklenebilir.
        Destroy(gameObject);  // Düþman objesini yok et
    }
}
