using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    private static BattleHandler instance;

    public static BattleHandler GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
}
