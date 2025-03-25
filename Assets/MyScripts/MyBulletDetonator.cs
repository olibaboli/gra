using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBulletDetonator : MonoBehaviour
{
    public int damage = 5;

    public GameObject explosionPrefab;

    private void Start()
    {
        //Wywo�anie metody "SelfDestroy" za 4 sekundy...
        //... chyba, �e wcze�niej pocisk uderzy w inny obiekt.
        Invoke(nameof(SelfDestroy), 4.0f);
    }

    //Unity wywo�a t� metod� w momencie wykrycia kolizji obiekt�w w �wiecie fizyki.
    //Jako parametr przekazuje informacje o kolizji (m.in. referencj� do kolidera obiektu, kt�ry wszed� w interakcj� z trigerem)
    private void OnCollisionEnter(Collision collision)
    {
        //Poprzez referencj� do kolidera (collision.collider), sprawdzamy, czy jest otagowany jako "Enenmy"
        if (collision.collider.CompareTag("Enemy"))
        {
            //Poprzez referencj� do komponentu Collider...
            //pobieramy referencj� do komponentu MyEnemyController (w tym samym GameObject'cie co collider).
            //Ka�dy z komponent�w: BoxCollider, SphereCollider, CapsuleCollider, MeshCollider, jest r�wnie� Collider'em - to nazywa si� "polimorfizmem" w obiektowych j�zykach programowania.
            var enemyController = collision.collider.GetComponent<MyEnemyController>();

            //W klasie MyEnemyController zdefiniowalili�my metod� TakeDamage - tutaj j� wywo�ujemy w konkretnym obiekcie ("wydrukowanym formularzu"), przyczepionym do konkretnego GameObject'u.
            //Jako parametr podajemy liczb� orzechowywan� w polu "damage".
            enemyController.TakeDamage(damage);
        }

        //Jak wy�ej, ale z Playerem:
        if (collision.collider.CompareTag("Player"))
        {
            var playerController = collision.collider.GetComponent<MyPlayerController>();

            playerController.TakeDamage(damage);
        }

        //Wywo�anie (wykonanie) metody "SelfDestroy"
        SelfDestroy();
    }

    //Prywatna metoda o nazwie "SelfDestroy" - samozniszczenie pocisku
    private void SelfDestroy()
    {
        //W miejsce pocisku tworzymy prefabrykat wybuchu
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        //Samozniszczenie pocisku
        Destroy(gameObject);
    }
}