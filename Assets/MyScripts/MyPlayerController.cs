using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyPlayerController : MonoBehaviour
{
    //Pole przechowuj�ce HP
    public int hp = 100;

    //Pole przechowuj�ce liczb� posiadanych monet
    public int coinCounter = 0;

    public GameObject deathPrefab;

    //Obs�uga powiadomieniu o wej�ciu collidera tego GameObjectu w stref� trigera.
    //Referencja do komponentu Collider trigera jest przekazana jako parametr "other"
    private void OnTriggerEnter(Collider other)
    {
        //Poprzez referencj� do dowolnego komponentu, mo�eny sprawdzi�, czy jego GameObject ma okre�lony tag
        //Je�eli triger jest elementem GameObject'u otagowanego "Coin"...
        if (other.CompareTag("Coin"))
        {
            //... to zwi�kszamy liczb� monet przechowywan� w polu "coinCounter" o 1
            coinCounter++;
        }
    }

    //Publiczna metoda o nazwie "TakeDamage". Wymaga podania parametru "damage" - liczby ca�kowitej.
    //Mo�na j� wywo�a� poprzez referencj� do komponentu MyPlayerComponent, np. playerController.TakeDamage(10);
    public void TakeDamage(int damage)
    {
        //odejmujemy od warto�ci przechowywanej w polu "hp" warto� przekazan� w parametrze "damage"
        hp -= damage;

        //je�li warto�� w polu "hp" jest <= 0 to player umiera
        if (hp <= 0)
        {
            //Camera.main zwraca referencj� do komponentu Camera GameObiektu otagowanego jako "MainCamera" -> u nas to GameObject MainCamera ...
            //pobieramy referencj� do komponentu MyCameraMovement tego obiektu...
            //i poprzez ni� wywo�ujemy metod� "DetachCamera".
            Camera.main.GetComponent<MyCameraMovement>().DetachCamera();

            //w lokalizacji playera spawnujemy na scenie prefabrykat wskazywany przez referencj� w polu "deatchPrefab"
            Instantiate(deathPrefab, transform.position, transform.rotation);

            //Deaktywacja komponentu "MyPlayerMovement" - aby martwy player si� nie rusza�
            GetComponent<MyPlayerMovement>().enabled = false;

            //Deaktywacja komponentu "MyPlayerShooting" - aby martwy player nie strzela�
            GetComponent<MyPlayerShooting>().enabled = false;

            //Deaktywacja komponentu "CharacterController" - aby martwy player w �wiecie fizyki nie by� "CharacterControllerem"...
            GetComponent<CharacterController>().enabled = false;

            //... a by� Rigidbody
            //Pobieramy do zmiennej lokalnej "rb" referencj� do komponentu Rigidbody           
            var rb = GetComponent<Rigidbody>();

            //Wy��czamy flag� IsKinamatic, aby ruchem obiektu sterowa� silnik fizyki
            rb.isKinematic = false;

            //Delikatnie popychamy playera poprzez przy�o�enie losowej si�y do Rigidbody
            rb.AddForce(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized * 5.0f);

            //Wykonaj metod� "RestartGame" za 6 sekund.
            Invoke(nameof(RestartGame), 6);
        }
    }

    //Prywatna metoda, mo�liwa do wywo�ania tylko w tej klasie.
    private void RestartGame()
    {
        //LoadScene("aaa") to metoda �aduj�ca scen� o nazwie "aaa"
        //Tutaj jako parametr podaj� nazw� aktualnie otwartej sceny
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}