using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyPlayerController : MonoBehaviour
{
    //Pole przechowuj¹ce HP
    public int hp = 100;

    //Pole przechowuj¹ce liczbê posiadanych monet
    public int coinCounter = 0;

    public GameObject deathPrefab;

    //Obs³uga powiadomieniu o wejœciu collidera tego GameObjectu w strefê trigera.
    //Referencja do komponentu Collider trigera jest przekazana jako parametr "other"
    private void OnTriggerEnter(Collider other)
    {
        //Poprzez referencjê do dowolnego komponentu, mo¿eny sprawdziæ, czy jego GameObject ma okreœlony tag
        //Je¿eli triger jest elementem GameObject'u otagowanego "Coin"...
        if (other.CompareTag("Coin"))
        {
            //... to zwiêkszamy liczbê monet przechowywan¹ w polu "coinCounter" o 1
            coinCounter++;
        }
    }

    //Publiczna metoda o nazwie "TakeDamage". Wymaga podania parametru "damage" - liczby ca³kowitej.
    //Mo¿na j¹ wywo³aæ poprzez referencjê do komponentu MyPlayerComponent, np. playerController.TakeDamage(10);
    public void TakeDamage(int damage)
    {
        //odejmujemy od wartoœci przechowywanej w polu "hp" wartoœ przekazan¹ w parametrze "damage"
        hp -= damage;

        //jeœli wartoœæ w polu "hp" jest <= 0 to player umiera
        if (hp <= 0)
        {
            //Camera.main zwraca referencjê do komponentu Camera GameObiektu otagowanego jako "MainCamera" -> u nas to GameObject MainCamera ...
            //pobieramy referencjê do komponentu MyCameraMovement tego obiektu...
            //i poprzez ni¹ wywo³ujemy metodê "DetachCamera".
            Camera.main.GetComponent<MyCameraMovement>().DetachCamera();

            //w lokalizacji playera spawnujemy na scenie prefabrykat wskazywany przez referencjê w polu "deatchPrefab"
            Instantiate(deathPrefab, transform.position, transform.rotation);

            //Deaktywacja komponentu "MyPlayerMovement" - aby martwy player siê nie rusza³
            GetComponent<MyPlayerMovement>().enabled = false;

            //Deaktywacja komponentu "MyPlayerShooting" - aby martwy player nie strzela³
            GetComponent<MyPlayerShooting>().enabled = false;

            //Deaktywacja komponentu "CharacterController" - aby martwy player w œwiecie fizyki nie by³ "CharacterControllerem"...
            GetComponent<CharacterController>().enabled = false;

            //... a by³ Rigidbody
            //Pobieramy do zmiennej lokalnej "rb" referencjê do komponentu Rigidbody           
            var rb = GetComponent<Rigidbody>();

            //Wy³¹czamy flagê IsKinamatic, aby ruchem obiektu sterowa³ silnik fizyki
            rb.isKinematic = false;

            //Delikatnie popychamy playera poprzez przy³o¿enie losowej si³y do Rigidbody
            rb.AddForce(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized * 5.0f);

            //Wykonaj metodê "RestartGame" za 6 sekund.
            Invoke(nameof(RestartGame), 6);
        }
    }

    //Prywatna metoda, mo¿liwa do wywo³ania tylko w tej klasie.
    private void RestartGame()
    {
        //LoadScene("aaa") to metoda ³aduj¹ca scenê o nazwie "aaa"
        //Tutaj jako parametr podajê nazwê aktualnie otwartej sceny
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}