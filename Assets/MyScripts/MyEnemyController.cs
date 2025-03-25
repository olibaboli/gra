using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyEnemyController : MonoBehaviour
{
    //Punkty ¿ycia npc'ka
    public int hp = 100;

    public GameObject deathPrefab;

    //Ile czasu musi min¹æ pomiêdzy atakami (domyœlnie pó³ sekundy)
    public float timeBetweenAttacks = 0.5f;

    public int attackDamage = 10;

    private MyPlayerController playerController;

    private NavMeshAgent navMeshAgent;

    private Transform playerTransform;

    //Pole przechowuj¹ce informacjê, czy w strefie trigera znajduje siê player?
    //Typ bool przyjmuje jedn¹ z dwóch wartoœci: prawda lub fa³sz (true/false)
    private bool playerInRange = false;

    //W tym polu przechowujemy ile czasu up³yne³o od ostatniego ataku
    private float timer = 0;

    //Metoda "start" któr¹ Unity wywo³uje w ka¿dym komponencie ka¿dego obiektu na starcie gry
    void Start()
    {
        //Pobranie referencji do GameObject'u otagowanego "Player"
        var player = GameObject.FindGameObjectWithTag("Player");

        //Z pomoc¹ tej referencji, pobranie referencji do komponentu "MyPlayerController" i zapisanie jej w polu "playerCOntroller"
        playerController = player.GetComponent<MyPlayerController>();

        //Referencja do komponentu "Transform" jest zawsze pod rêk¹ (pole "transform" w GameObject lub w ka¿dym komponencie...
        //... zdefiniowane dla nas przez twórców Unity)
        playerTransform = player.transform;

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    //W trigerze pojawi³ siê inny collider (o referencji przekazanej w parametrze "other")
    void OnTriggerEnter(Collider other)
    {
        //Jeœli jest otagowany jako "Player"...
        if (other.CompareTag("Player"))
        {
            //... to w polu "playerInRange" zapisujemy wartoœæ "true"
            playerInRange = true;
        }
    }

    //Strefê trigera opuœci³ collider (o referencji przekazanej w parametrze "other")
    void OnTriggerExit(Collider other)
    {
        //Jeœli jest otagowany jako "Player"...
        if (other.CompareTag("Player"))
        {
            //... to w polu "playerInRange" zapisujemy wartoœæ "false"
            playerInRange = false;
        }
    }

    //Unity wywo³uje tê metod¹ co chwilê - tu dzieje siê symulacja naszego œwiata
    void Update()
    {
        //Do wartoœci w polu "timer" dodajemy wartoœæ "Time.deltaTime" - ile czasu (w sekundach, liczba zmiennoprzecinkowa) mine³o od poprzedniego wywo³ania metody Update
        timer += Time.deltaTime;

        //Je¿eli spe³nione s¹ wszystkie warunki (coœ oraz coœ oraz...) [&& oznacza "oraz"] wykonujemy atak. Warunki
        //Warunek jest spe³niony (w sensie logicznym), je¿eli odpowiada mu wartoœæ logiczna prawda (true); warunek nie jest spe³niony, je¿eli...
        //odpowiada mu wartoœæ logiczna fa³sz (false).
        //Np. warunek trzeci (hp > 0) jest spe³niony, je¿eli prawd¹ jest, ¿e liczba w polu hp jest wiêksza od 0 (NPC ¿yje)
        //W warunku 2 wynik logiczny jest wprost wartoœci¹ przechowywan¹ w polu playerInRange - typ bool (true/false) ..
        //Podobnie w warunku w linii 60: if (other.CompareTag("Player")) metoda CompareTag zwraca wynik true/false, który okreœla spe³nienie warunku.
        if (timer >= timeBetweenAttacks && playerInRange && hp > 0 && playerController.hp > 0)
        {
            //zerujemy czas od ostatniego ataku
            timer = 0f;

            //Za pomoc¹ referencji w polu "playerController" wywo³ujemy w komponencie MyPlayerController GameObject'u "Player" metodê "TakeDamage"...
            //... przekazuj¹c jako parametr wartoœæ przechowywan¹ w polu "attackDamage"
            playerController.TakeDamage(attackDamage);
        }

        //Je¿eli NPC ¿yje oraz player ¿yje to...
        if (hp > 0 && playerController.hp > 0)
        {
            //Ustaw cel agentowi jako aktualn¹ pozycjê playera (w polu playerTransform mamy referencjê do komponentu Transform GameObjectu Player'a...
            //i z jej pomoc¹ zawsze mo¿emy odczytaæ jego aktualn¹ pozycjê)
            navMeshAgent.destination = playerTransform.position;

            //Fajne filmy na YT o systemie NavMesh:
            //https://youtu.be/u2EQtrdgfNs
            //https://youtu.be/K6bBC0qkImI
        }
    }

    //Tê metodê bêdzie wywo³ywaæ wybuchaj¹cy pocisk
    public void TakeDamage(int damage)
    {
        //odejmujemy od wartoœci przechowywanej w polu "hp" wartoœ przekazan¹ w parametrze "damage"
        hp -= damage;

        //jeœli wartoœæ w polu "hp" jest <= 0 to npc umiera
        if (hp <= 0)
        {
            //w lokalizacji npc spawnujemy na scenie prefabrykat wskazywany przez referencjê w polu "deatchPrefab"
            Instantiate(deathPrefab, transform.position, transform.rotation);

            //Usuniêcie ze sceny GameObject'u - jako parametr metody Destroy przekazywana jest
            //referencja do swojego GameObject'a (utworzone przez twórców Unity pole "gameObject" jest dostêpne w ka¿dym komponencie)
            Destroy(gameObject);
        }
    }
}