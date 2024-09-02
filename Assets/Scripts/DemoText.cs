using UnityEngine;
using TMPro;

public class DemoText : MonoBehaviour
{
    // yapým aþamasýnda....
    [SerializeField]
    public GameObject[] objectsToCheck; // Kontrol edilecek objeler
    public TextMeshProUGUI textMeshPro; // TextMesh Pro nnesnesi

    void Start()
    {
        // Baþlangýçta TextMesh Pro'yu gizle
        textMeshPro.gameObject.SetActive(false);
    }

    void Update()
    {
        // Tüm objeler yok olduysa TextMesh Pro'yu göster
        if (AllObjectsDestroyed())
        {
            textMeshPro.gameObject.SetActive(true);
        }
    }

    bool AllObjectsDestroyed()
    {
        // Obje listesindeki tüm objelerin yok olup olmadýðýný kontrol et
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
