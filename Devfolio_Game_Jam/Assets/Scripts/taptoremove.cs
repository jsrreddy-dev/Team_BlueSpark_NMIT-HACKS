using UnityEngine;

public class taptoremove : MonoBehaviour
{

    // remove the object when tapped on screen with mobile

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Destroy(gameObject);
            }
        }
    }

}
