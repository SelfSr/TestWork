using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public float health; 
    public float damage; 

    public float speed;
    public float attackSpeed;
    public float attackRange;
}