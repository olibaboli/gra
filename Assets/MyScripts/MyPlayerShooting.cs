using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerShooting : MonoBehaviour
{
    //Pole z referencj� do "prefabrykatu" pocisku
    public GameObject bulletPrefab;

    //Pole z referencj� do komponentu Transform, kt�ego pozycja i rotacja okre�laj�...
    //... ponkt spawnowania pocisk�w
    public Transform bulletSpawnPoint;

    //Si�a przyk�adana do pocisku
    public float bulletThrowingForce = 10;

    //Referencja do komponentu ParticleSystem (miotacz ognia)
    //Ustawiana w edytorze przez przeci�gni�cie odpowiedniego obiektu.
    public ParticleSystem flamethrower;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //Metoda (funkcja) Instantiate tworzy na podstawieprefaba nowy GameObiect i umieszcza go w hierarchii.
            //Referencja do utworzonego obiektu zapami�tana jest w zmiennej lokalnej o nazwie "bullet" (zmienna...
            //... lokalna to co� jak pole (definiowane na pocz�tku klasy, linia 8 czy 12), ale dost�pna tylko pomi�dzy ...
            //... otaczaj�cymi nawiasami klamrowymi tj. mi�dzy { w linii 24 a } w linii 37. 
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            //Z nowego GameObject'u pobieramy referencj� do komponentu Rigidbody i w nim (poprzez t� referencj�) wywo�ujemy...
            //... metod� AddForce - przy�o�enie wektora si�y (w przestrzeni 3D si�a jest definiowana przez wektor okre�laj�cy kierunek...
            //... jej dzia�ania; d�ugo�� tego wektora okre�la wielko�� si�y. My przyk�adamy si�� dzia�aj�c� w kierunku osi Z ("forward") komponentu
            //... Transform definiuj�cego spawn point pocisk�w. FOrward jest wektorem o d�ugo�ci 1, wi�c mno�ymy ten wektor przez warto�� wielko�ci si�y,...
            //... jak� chcemy "popchn��" pocisk.
            bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPoint.forward * bulletThrowingForce, ForceMode.Impulse);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            //W��czamy odgrywanie systemu cz�steczkowego
            flamethrower.Play();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            //Wy��czamy odgrywanie systemu czasteczkowego
            flamethrower.Stop();
        }
    }

    private void FixedUpdate()
    {
        //Je�eli system cz�steczkowy miotacza ognia jest odgrywany (dzia�a)...
        if (flamethrower.isPlaying)
        { 
            //...to:
            //Je�eli zapytanie do sceny typu "rzu� pi�eczk�"...
            //wykryje kolizj� z jakim� obiektem...
            if (Physics.SphereCast(bulletSpawnPoint.position, 0.25f, bulletSpawnPoint.forward, out var hitInfo, 4.0f))
            {
                //...to:
                //Je�eli obiekt zawieraj�cy kolider jest otagowany jako "Enemy"...
                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    //...to:
                    //Za pomoc� referencji do kolidera pobierz referencj� do 
                    //komponentu "MyEnemyController"...
                    var enemyController = hitInfo.collider.GetComponent<MyEnemyController>();

                    //... i wywo�aj w nim funkcj� (metod�) TakeDamage, przekazuj�c parametr 3.
                    enemyController.TakeDamage(3);
                }
            }
        }
    }
}