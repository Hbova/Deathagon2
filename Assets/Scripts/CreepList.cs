using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepList {
    public List<int> creepsToSpawn = new List<int>();

    public static string MakeCreepListJson( List<int> creepList ) {
        return JsonUtility.ToJson( new CreepList { creepsToSpawn = creepList } );
    }

    public static List<int> CreepsToSpawnFromJson( string json ) {
    	CreepList creepList = JsonUtility.FromJson<CreepList>( json );
    	return creepList.creepsToSpawn;
    }
}
