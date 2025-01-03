using UnityEngine;

[CreateAssetMenu(fileName = "NewGround", menuName = "Database/Ground")]

public class GroundData : ScriptableObject
{
    public string groundName;
    public AudioClip footstepSound;
}
