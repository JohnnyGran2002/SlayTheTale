using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapEvents : MonoBehaviour
{
    public static MapEvents mapEvents;
    
    [Header("Dependencies"), SerializeField] private TMP_Text textBox;
    [SerializeField]  private Image image;
    [SerializeField] private GameObject choicePrefab, choiceTarget;

    [Header("Settings"), Space(7), SerializeField]
    private float spaceBetweenChoices = 20;

    private MapEventData _data;
    private List<GameObject> _choiceList = new List<GameObject>();
    
    private void Awake()
    {
        if (mapEvents != null && mapEvents != this)
        {
            Destroy(this);
        }
        else
        {
            mapEvents = this;
            DontDestroyOnLoad(this);
        }

        gameObject.SetActive(false);
    }
    
    public void LoadEvent(int tier)
    {
        gameObject.SetActive(true);
        _data = CombatScenesHolder.combatScenesHolder.tierSettings[tier].mapEventData[CombatScenesHolder.combatScenesHolder.eventCounter];
        textBox.text = _data.textBox;
        image.sprite = _data.image;
        for (var i = 0; i < _data.choices.Length; i++)
        {
            var currentChoice = Instantiate(choicePrefab, new Vector3(choiceTarget.transform.position.x, choiceTarget.transform.position.y + (i * -spaceBetweenChoices), choiceTarget.transform.position.z), Quaternion.identity, this.transform);
            currentChoice.SetActive(true);
            var currentLogic = currentChoice.GetComponent<ChoiceLogic>();
            Debug.Log(_data.choices[i].Text);
            currentLogic.text.text = _data.choices[i].Text;
            _choiceList.Add(currentChoice);
        }
    }

    public void OnClick(GameObject button)
    {
        gameObject.SetActive(false);
    }
}
