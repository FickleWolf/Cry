using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGroundDatabase", menuName = "Database/GroundDatabase")]
public class GroundDatabase : ScriptableObject
{
    public GroundData defaultGround;
    public GroundData[] grounds;

    public GroundData GetGroundByName(string groundName)
    {
        GroundData ground = grounds.FirstOrDefault(ground => ground.groundName == groundName);

        if (ground == null) return defaultGround;

        return ground;
    }
}
