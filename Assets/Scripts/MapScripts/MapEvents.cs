using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapEvents : MonoBehaviour
{
    public static MapEvents instance;
    
    [Header("Dependencies"), SerializeField] private TMP_Text textBox;
    [SerializeField]  private Image image;
    [SerializeField] private GameObject choicePrefab, choiceTarget;

    [Header("Settings"), Space(7), SerializeField]
    private float spaceBetweenChoices = 20;

    private MapEventData _data;
    private List<GameObject> _choiceList = new List<GameObject>();
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        gameObject.SetActive(false);
    }
    
    public void LoadEvent(int tier)
    {
        gameObject.SetActive(true);
        _data = CombatScenesHolder.instance.tierSettings[tier].mapEventData[CombatScenesHolder.instance.tierSettings[tier].eventCounter];
        textBox.text = _data.textBox;
        image.sprite = _data.image;
        for (var i = 0; i < _data.choices.Length; i++)
        {
            var currentChoice = Instantiate(choicePrefab, new Vector3(choiceTarget.transform.position.x, choiceTarget.transform.position.y + (i * -spaceBetweenChoices), choiceTarget.transform.position.z), Quaternion.identity, this.transform);
            currentChoice.SetActive(true);
            var currentLogic = currentChoice.GetComponent<ChoiceLogic>();
            Debug.Log(_data.choices[i].Text);
            currentLogic.text.text = _data.choices[i].Text;
            currentLogic.choices = _data.choices[i];
            _choiceList.Add(currentChoice);
        }

        CombatScenesHolder.instance.tierSettings[tier].eventCounter++;
    }

    public void OnClick()
    {
        for (var i = _data.choices.Length - 1; i >= 0; i--)
        {
            Destroy(_choiceList[i]);
        }
        _choiceList.Clear();
        gameObject.SetActive(false);
    }
}
