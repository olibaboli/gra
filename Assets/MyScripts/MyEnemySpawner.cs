using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    
    //Co ile sekund spawnujemy wrogie npc
    public float spawnTime = 3f;

    //Tablica (kolekcja) referencji do obiektów typu Transform
    public Transform[] spawnPoints;

    private MyPlayerController playerController;

    void Start()
    {
        //Pobranie referencji do GameObject'u otagowanego "Player"
        var player = GameObject.FindGameObjectWithTag("Player");

        //Z pomoc¹ tej referencji, pobranie referencji do komponentu "MyPlayerController" i zapisanie jej w polu "playerCOntroller"
        playerController = player.GetComponent<MyPlayerController>();

        //Je¿eli w kolekcji "spawnPoints" jest co najmniej jeden element...
        if (spawnPoints.Length > 0)
        {
            //... po up³ywie liczby sekund w polu spawnTime, powtarzaj wywo³anie metody "Spawn" co liczbê sekund w polu "spawnTime"
            InvokeRepeating(nameof(Spawn), spawnTime, spawnTime);
        }
    }

    //Prywatna metoda o nazwie "Spawn"
    private void Spawn()
    {
        //Je¿eli player ¿yje (ma dodatni¹ liczbê HP)
        if (playerController.hp > 0)
        {
            //Do zmiennej lokalnej "spawnPointIndex" zapisz wygwnwrowan¹ liczbê losow¹ z zakresu od 0 do liczby elementów w kolekcji "spawnPoints" minus 1
            var spawnPointIndex = Random.Range(0, spawnPoints.Length);

            //Utwórz na scenie GameObject na podstawie prefabrykatu enemyPrefab, w lokalizacji wylosowanej pozycji z kolekcji i z zerow¹ rotacj¹.
            Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, Quaternion.identity);
        }
    }
}
