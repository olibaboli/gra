using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerShooting : MonoBehaviour
{
    //Pole z referencj¹ do "prefabrykatu" pocisku
    public GameObject bulletPrefab;

    //Pole z referencj¹ do komponentu Transform, któego pozycja i rotacja okreœlaj¹...
    //... ponkt spawnowania pocisków
    public Transform bulletSpawnPoint;

    //Si³a przyk³adana do pocisku
    public float bulletThrowingForce = 10;

    //Referencja do komponentu ParticleSystem (miotacz ognia)
    //Ustawiana w edytorze przez przeci¹gniêcie odpowiedniego obiektu.
    public ParticleSystem flamethrower;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //Metoda (funkcja) Instantiate tworzy na podstawieprefaba nowy GameObiect i umieszcza go w hierarchii.
            //Referencja do utworzonego obiektu zapamiêtana jest w zmiennej lokalnej o nazwie "bullet" (zmienna...
            //... lokalna to coœ jak pole (definiowane na pocz¹tku klasy, linia 8 czy 12), ale dostêpna tylko pomiêdzy ...
            //... otaczaj¹cymi nawiasami klamrowymi tj. miêdzy { w linii 24 a } w linii 37. 
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            //Z nowego GameObject'u pobieramy referencjê do komponentu Rigidbody i w nim (poprzez tê referencjê) wywo³ujemy...
            //... metodê AddForce - przy³o¿enie wektora si³y (w przestrzeni 3D si³a jest definiowana przez wektor okreœlaj¹cy kierunek...
            //... jej dzia³ania; d³ugoœæ tego wektora okreœla wielkoœæ si³y. My przyk³adamy si³ê dzia³aj¹c¹ w kierunku osi Z ("forward") komponentu
            //... Transform definiuj¹cego spawn point pocisków. FOrward jest wektorem o d³ugoœci 1, wiêc mno¿ymy ten wektor przez wartoœæ wielkoœci si³y,...
            //... jak¹ chcemy "popchn¹æ" pocisk.
            bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPoint.forward * bulletThrowingForce, ForceMode.Impulse);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            //W³¹czamy odgrywanie systemu cz¹steczkowego
            flamethrower.Play();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            //Wy³¹czamy odgrywanie systemu czasteczkowego
            flamethrower.Stop();
        }
    }

    private void FixedUpdate()
    {
        //Je¿eli system cz¹steczkowy miotacza ognia jest odgrywany (dzia³a)...
        if (flamethrower.isPlaying)
        { 
            //...to:
            //Je¿eli zapytanie do sceny typu "rzuæ pi³eczk¹"...
            //wykryje kolizjê z jakimœ obiektem...
            if (Physics.SphereCast(bulletSpawnPoint.position, 0.25f, bulletSpawnPoint.forward, out var hitInfo, 4.0f))
            {
                //...to:
                //Je¿eli obiekt zawieraj¹cy kolider jest otagowany jako "Enemy"...
                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    //...to:
                    //Za pomoc¹ referencji do kolidera pobierz referencjê do 
                    //komponentu "MyEnemyController"...
                    var enemyController = hitInfo.collider.GetComponent<MyEnemyController>();

                    //... i wywo³aj w nim funkcjê (metodê) TakeDamage, przekazuj¹c parametr 3.
                    enemyController.TakeDamage(3);
                }
            }
        }
    }
}