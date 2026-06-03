using TMPro;
using Unity.AppUI.UI;
using UnityEngine;

public class DamagePopUpGenerator : Singleton<DamagePopUpGenerator>
{
    [SerializeField] private GameObject damagePopUp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatePopUp(Vector3 position, string text)
    {
        GameObject popUp = Instantiate(damagePopUp, position, Quaternion.identity);
        TextMeshProUGUI temp = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;

        Destroy(popUp, 1f);
    }
}
