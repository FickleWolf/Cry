using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Database/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float worldSpeed;
    public int spawnRate; // Worldマップ上の出現率
    public string spawnGround; // Worldマップ上で出現するGround名
    public float hp; // バトル時のHP
    public float attck;// バトル時の攻撃力
    public float defense; // バトル時の守備力
    public float spped;// バトル時の移動スピード
    public float attckRange; // バトル時の射程範囲[m]
    public float attckInterval; // バトル時の攻撃間隔[秒]
    public float attckChargeTime; // 攻撃準時間[m]
    public AudioClip attackSound;
    public AudioClip preAttackSound;
    public AudioClip footstepSound;
}
