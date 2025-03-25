using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraMovement : MonoBehaviour
{
    public float smoothing = 5.0f; //f oznacza, ¿e podana liczba 5.0 jest typu "float"

    //Referencja na komponent Transform definiuj¹cy dalsz¹ pozycjê kamery za playerem
    public Transform farCameraPoint;

    //Referencja na komponent Transform definiuj¹cy bli¿sz¹ pozycjê kamery (przy playerze)
    public Transform nearCameraPoint;


    //Pola prywatne, niewidoczne w edytorze; mo¿liwoœæ odwo³ania siê do nich tylko w tej klasie:

    //Referencja na aktualnie wybran¹ pozycjê kamery (w zale¿noœci od stanu RMB: farCameraPoint lub nearCameraPoint)
    private Transform targetCameraPoint;

    //Punkt na który patrzy kamera
    private Vector3 currentLookAtPoint;

    private void Start()
    {
        //Ukrycie kursora myszy. Pokazanie kursora pod klawiszem Escape
        Cursor.visible = false;

        //pocz¹tkowe wyznaczenia punktu na który patrzy kamera:
        //pozycja kamery dalszej + 10 jednostek w kierunku osi Z tego komponentu Transform
        currentLookAtPoint = farCameraPoint.position + 10 * farCameraPoint.forward;
    }

    private void FixedUpdate()
    {
        //Je¿eli wciœniêty jest RMB
        if (Input.GetButton("Fire2"))
        {
            //Ukrycie kursora myszy. Pokazanie kursora pod klawiszem Escape
            Cursor.visible = false;

            targetCameraPoint = nearCameraPoint;
        }
        else //w przeciwnym wypadku
        {
            targetCameraPoint = farCameraPoint;
        }

        //Lerp(a, b, t) to kombinacja liniowa: a + (b - a) * t
        //https://docs.unity3d.com/2023.2/Documentation/ScriptReference/Vector3.Lerp.html
        //Po³o¿enie kamery (transform.position) "pod¹¿a" do aktualnego po³o¿enia targetCameraPoint.
        //TargetCameraPoint jest referencj¹ albo na bliski albo na daleki komponent Transform w obiekcie Player. Jak Player siê poruszy, to poprzez te referencje mamy dostêp
        //do aktualnych pozycji tych punktów i przesuwamy kamerê w odpowiednim kierunku.
        transform.position = Vector3.Lerp(transform.position, targetCameraPoint.position, smoothing * Time.deltaTime);

        currentLookAtPoint = Vector3.Lerp(currentLookAtPoint, targetCameraPoint.position + 10 * targetCameraPoint.forward, smoothing * Time.deltaTime);

        //Wywo³anie metody ustawiaj¹cej Transform tak, aby oœ Z wskazywa³a na punkt podany jako parametr wywo³ania.
        transform.LookAt(currentLookAtPoint);
    }

    //Publiczna metoda o nazwie "DetachCamera" - mo¿na j¹ wywo³aæ maj¹c referencjê do komponentu tej klasy...
    //... np. przy œmierci gracza.
    public void DetachCamera()
    {
        //do pól przechowuj¹cych dalek¹ i blisk¹ pozycjê kamery wpisujemu referencjê do w³asnego kompoinentu Transform (komponentu GameObjectu MainCamera).
        nearCameraPoint = transform;
        farCameraPoint = transform;
    }
}