using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyEnemyController : MonoBehaviour
{
    //Punkty �ycia npc'ka
    public int hp = 100;

    public GameObject deathPrefab;

    //Ile czasu musi min�� pomi�dzy atakami (domy�lnie p� sekundy)
    public float timeBetweenAttacks = 0.5f;

    public int attackDamage = 10;

    private MyPlayerController playerController;

    private NavMeshAgent navMeshAgent;

    private Transform playerTransform;

    //Pole przechowuj�ce informacj�, czy w strefie trigera znajduje si� player?
    //Typ bool przyjmuje jedn� z dw�ch warto�ci: prawda lub fa�sz (true/false)
    private bool playerInRange = false;

    //W tym polu przechowujemy ile czasu up�yne�o od ostatniego ataku
    private float timer = 0;

    //Metoda "start" kt�r� Unity wywo�uje w ka�dym komponencie ka�dego obiektu na starcie gry
    void Start()
    {
        //Pobranie referencji do GameObject'u otagowanego "Player"
        var player = GameObject.FindGameObjectWithTag("Player");

        //Z pomoc� tej referencji, pobranie referencji do komponentu "MyPlayerController" i zapisanie jej w polu "playerCOntroller"
        playerController = player.GetComponent<MyPlayerController>();

        //Referencja do komponentu "Transform" jest zawsze pod r�k� (pole "transform" w GameObject lub w ka�dym komponencie...
        //... zdefiniowane dla nas przez tw�rc�w Unity)
        playerTransform = player.transform;

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    //W trigerze pojawi� si� inny collider (o referencji przekazanej w parametrze "other")
    void OnTriggerEnter(Collider other)
    {
        //Je�li jest otagowany jako "Player"...
        if (other.CompareTag("Player"))
        {
            //... to w polu "playerInRange" zapisujemy warto�� "true"
            playerInRange = true;
        }
    }

    //Stref� trigera opu�ci� collider (o referencji przekazanej w parametrze "other")
    void OnTriggerExit(Collider other)
    {
        //Je�li jest otagowany jako "Player"...
        if (other.CompareTag("Player"))
        {
            //... to w polu "playerInRange" zapisujemy warto�� "false"
            playerInRange = false;
        }
    }

    //Unity wywo�uje t� metod� co chwil� - tu dzieje si� symulacja naszego �wiata
    void Update()
    {
        //Do warto�ci w polu "timer" dodajemy warto�� "Time.deltaTime" - ile czasu (w sekundach, liczba zmiennoprzecinkowa) mine�o od poprzedniego wywo�ania metody Update
        timer += Time.deltaTime;

        //Je�eli spe�nione s� wszystkie warunki (co� oraz co� oraz...) [&& oznacza "oraz"] wykonujemy atak. Warunki
        //Warunek jest spe�niony (w sensie logicznym), je�eli odpowiada mu warto�� logiczna prawda (true); warunek nie jest spe�niony, je�eli...
        //odpowiada mu warto�� logiczna fa�sz (false).
        //Np. warunek trzeci (hp > 0) jest spe�niony, je�eli prawd� jest, �e liczba w polu hp jest wi�ksza od 0 (NPC �yje)
        //W warunku 2 wynik logiczny jest wprost warto�ci� przechowywan� w polu playerInRange - typ bool (true/false) ..
        //Podobnie w warunku w linii 60: if (other.CompareTag("Player")) metoda CompareTag zwraca wynik true/false, kt�ry okre�la spe�nienie warunku.
        if (timer >= timeBetweenAttacks && playerInRange && hp > 0 && playerController.hp > 0)
        {
            //zerujemy czas od ostatniego ataku
            timer = 0f;

            //Za pomoc� referencji w polu "playerController" wywo�ujemy w komponencie MyPlayerController GameObject'u "Player" metod� "TakeDamage"...
            //... przekazuj�c jako parametr warto�� przechowywan� w polu "attackDamage"
            playerController.TakeDamage(attackDamage);
        }

        //Je�eli NPC �yje oraz player �yje to...
        if (hp > 0 && playerController.hp > 0)
        {
            //Ustaw cel agentowi jako aktualn� pozycj� playera (w polu playerTransform mamy referencj� do komponentu Transform GameObjectu Player'a...
            //i z jej pomoc� zawsze mo�emy odczyta� jego aktualn� pozycj�)
            navMeshAgent.destination = playerTransform.position;

            //Fajne filmy na YT o systemie NavMesh:
            //https://youtu.be/u2EQtrdgfNs
            //https://youtu.be/K6bBC0qkImI
        }
    }

    //T� metod� b�dzie wywo�ywa� wybuchaj�cy pocisk
    public void TakeDamage(int damage)
    {
        //odejmujemy od warto�ci przechowywanej w polu "hp" warto� przekazan� w parametrze "damage"
        hp -= damage;

        //je�li warto�� w polu "hp" jest <= 0 to npc umiera
        if (hp <= 0)
        {
            //w lokalizacji npc spawnujemy na scenie prefabrykat wskazywany przez referencj� w polu "deatchPrefab"
            Instantiate(deathPrefab, transform.position, transform.rotation);

            //Usuni�cie ze sceny GameObject'u - jako parametr metody Destroy przekazywana jest
            //referencja do swojego GameObject'a (utworzone przez tw�rc�w Unity pole "gameObject" jest dost�pne w ka�dym komponencie)
            Destroy(gameObject);
        }
    }
}