using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public int health; 
    public int damage; 

    public float speed;
    public float attackSpeed;
    public float attackRange;
}