using System;
using UnityEngine;

public class RewardCanvas : MonoBehaviour
{
    public static RewardCanvas i;

    [SerializeField] private Transform target;
    [SerializeField] private GameObject rewardPrefab;

    /*
    public void Awake()
    {
        gameObject.SetActive(false);
    }

     public void GiveReward()
    {
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
    }
} 
*/
