using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    
    //Co ile sekund spawnujemy wrogie npc
    public float spawnTime = 3f;

    //Tablica (kolekcja) referencji do obiekt�w typu Transform
    public Transform[] spawnPoints;

    private MyPlayerController playerController;

    void Start()
    {
        //Pobranie referencji do GameObject'u otagowanego "Player"
        var player = GameObject.FindGameObjectWithTag("Player");

        //Z pomoc� tej referencji, pobranie referencji do komponentu "MyPlayerController" i zapisanie jej w polu "playerCOntroller"
        playerController = player.GetComponent<MyPlayerController>();

        //Je�eli w kolekcji "spawnPoints" jest co najmniej jeden element...
        if (spawnPoints.Length > 0)
        {
            //... po up�ywie liczby sekund w polu spawnTime, powtarzaj wywo�anie metody "Spawn" co liczb� sekund w polu "spawnTime"
            InvokeRepeating(nameof(Spawn), spawnTime, spawnTime);
        }
    }

    //Prywatna metoda o nazwie "Spawn"
    private void Spawn()
    {
        //Je�eli player �yje (ma dodatni� liczb� HP)
        if (playerController.hp > 0)
        {
            //Do zmiennej lokalnej "spawnPointIndex" zapisz wygwnwrowan� liczb� losow� z zakresu od 0 do liczby element�w w kolekcji "spawnPoints" minus 1
            var spawnPointIndex = Random.Range(0, spawnPoints.Length);

            //Utw�rz na scenie GameObject na podstawie prefabrykatu enemyPrefab, w lokalizacji wylosowanej pozycji z kolekcji i z zerow� rotacj�.
            Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, Quaternion.identity);
        }
    }
}
