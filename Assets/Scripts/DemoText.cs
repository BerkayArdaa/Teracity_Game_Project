using UnityEngine;
using TMPro;

public class DemoText : MonoBehaviour
{
    // yap�m a�amas�nda....
    [SerializeField]
    public GameObject[] objectsToCheck; // Kontrol edilecek objeler
    public TextMeshProUGUI textMeshPro; // TextMesh Pro nnesnesi

    void Start()
    {
        // Ba�lang��ta TextMesh Pro'yu gizle
        textMeshPro.gameObject.SetActive(false);
    }

    void Update()
    {
        // T�m objeler yok olduysa TextMesh Pro'yu g�ster
        if (AllObjectsDestroyed())
        {
            textMeshPro.gameObject.SetActive(true);
        }
    }

    bool AllObjectsDestroyed()
    {
        // Obje listesindeki t�m objelerin yok olup olmad���n� kontrol et
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj != null)
            {
                return false;
            }
        }
        return true;
    }
}
