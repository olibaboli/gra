using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBulletDetonator : MonoBehaviour
{
    public int damage = 5;

    public GameObject explosionPrefab;

    private void Start()
    {
        //Wywo³anie metody "SelfDestroy" za 4 sekundy...
        //... chyba, ¿e wczeœniej pocisk uderzy w inny obiekt.
        Invoke(nameof(SelfDestroy), 4.0f);
    }

    //Unity wywo³a tê metodê w momencie wykrycia kolizji obiektów w œwiecie fizyki.
    //Jako parametr przekazuje informacje o kolizji (m.in. referencjê do kolidera obiektu, który wszed³ w interakcjê z trigerem)
    private void OnCollisionEnter(Collision collision)
    {
        //Poprzez referencjê do kolidera (collision.collider), sprawdzamy, czy jest otagowany jako "Enenmy"
        if (collision.collider.CompareTag("Enemy"))
        {
            //Poprzez referencjê do komponentu Collider...
            //pobieramy referencjê do komponentu MyEnemyController (w tym samym GameObject'cie co collider).
            //Ka¿dy z komponentów: BoxCollider, SphereCollider, CapsuleCollider, MeshCollider, jest równie¿ Collider'em - to nazywa siê "polimorfizmem" w obiektowych jêzykach programowania.
            var enemyController = collision.collider.GetComponent<MyEnemyController>();

            //W klasie MyEnemyController zdefiniowaliliœmy metodê TakeDamage - tutaj j¹ wywo³ujemy w konkretnym obiekcie ("wydrukowanym formularzu"), przyczepionym do konkretnego GameObject'u.
            //Jako parametr podajemy liczbê orzechowywan¹ w polu "damage".
            enemyController.TakeDamage(damage);
        }

        //Jak wy¿ej, ale z Playerem:
        if (collision.collider.CompareTag("Player"))
        {
            var playerController = collision.collider.GetComponent<MyPlayerController>();

            playerController.TakeDamage(damage);
        }

        //Wywo³anie (wykonanie) metody "SelfDestroy"
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