using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraMovement : MonoBehaviour
{
    public float smoothing = 5.0f; //f oznacza, �e podana liczba 5.0 jest typu "float"

    //Referencja na komponent Transform definiuj�cy dalsz� pozycj� kamery za playerem
    public Transform farCameraPoint;

    //Referencja na komponent Transform definiuj�cy bli�sz� pozycj� kamery (przy playerze)
    public Transform nearCameraPoint;


    //Pola prywatne, niewidoczne w edytorze; mo�liwo�� odwo�ania si� do nich tylko w tej klasie:

    //Referencja na aktualnie wybran� pozycj� kamery (w zale�no�ci od stanu RMB: farCameraPoint lub nearCameraPoint)
    private Transform targetCameraPoint;

    //Punkt na kt�ry patrzy kamera
    private Vector3 currentLookAtPoint;

    private void Start()
    {
        //Ukrycie kursora myszy. Pokazanie kursora pod klawiszem Escape
        Cursor.visible = false;

        //pocz�tkowe wyznaczenia punktu na kt�ry patrzy kamera:
        //pozycja kamery dalszej + 10 jednostek w kierunku osi Z tego komponentu Transform
        currentLookAtPoint = farCameraPoint.position + 10 * farCameraPoint.forward;
    }

    private void FixedUpdate()
    {
        //Je�eli wci�ni�ty jest RMB
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
        //Po�o�enie kamery (transform.position) "pod��a" do aktualnego po�o�enia targetCameraPoint.
        //TargetCameraPoint jest referencj� albo na bliski albo na daleki komponent Transform w obiekcie Player. Jak Player si� poruszy, to poprzez te referencje mamy dost�p
        //do aktualnych pozycji tych punkt�w i przesuwamy kamer� w odpowiednim kierunku.
        transform.position = Vector3.Lerp(transform.position, targetCameraPoint.position, smoothing * Time.deltaTime);

        currentLookAtPoint = Vector3.Lerp(currentLookAtPoint, targetCameraPoint.position + 10 * targetCameraPoint.forward, smoothing * Time.deltaTime);

        //Wywo�anie metody ustawiaj�cej Transform tak, aby o� Z wskazywa�a na punkt podany jako parametr wywo�ania.
        transform.LookAt(currentLookAtPoint);
    }

    //Publiczna metoda o nazwie "DetachCamera" - mo�na j� wywo�a� maj�c referencj� do komponentu tej klasy...
    //... np. przy �mierci gracza.
    public void DetachCamera()
    {
        //do p�l przechowuj�cych dalek� i blisk� pozycj� kamery wpisujemu referencj� do w�asnego kompoinentu Transform (komponentu GameObjectu MainCamera).
        nearCameraPoint = transform;
        farCameraPoint = transform;
    }
}