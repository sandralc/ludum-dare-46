using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Transform[] closeSpawnPoints;
    public Transform[] midSpawnPoints;
    public Transform[] hardSpawnPoints;

    //Cada uno de los spawnpoints, cada X tiempo, si no tiene item, re-spawnea uno.
    //Si tiene, no hace anda.
    //Los cercanos lo hacen mas a menudo que los lejanos.
    //Esto puede cambiar con el nivel de dificultad.



}
