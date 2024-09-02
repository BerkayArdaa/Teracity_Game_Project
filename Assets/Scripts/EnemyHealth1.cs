using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth1 : MonoBehaviour
{
    public float health = 100f;  // D��man�n ba�lang�� can�

    public void TakeDamage(float damage)
    {
        health -= damage;  // Al�nan zarar kadar can� azalt
        Debug.Log(gameObject.name + " has " + health + " health left.");  // G�ncel can durumunu logla
        if (health <= 0)
        {
            Die();  // Can� 0 veya alt�na d��t�yse �l�m fonksiyonunu �a��r
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " �ld�.");  // �l�m logu
        // Burada �l�m animasyonu veya patlama efekti tetiklenebilir.
        Destroy(gameObject);  // D��man objesini yok et
    }
}
