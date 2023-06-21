using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ColliderAsButton : MonoBehaviour
{
    [SerializeField] private Camera TheCamera;
    [SerializeField] private UnityEvent OnClick;

    private Collider ThisCollider;

    private void Awake()
    {
        ThisCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray RayForCheckingIfTheButtonWasPressed = TheCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] AllRaycastHits = Physics.RaycastAll(RayForCheckingIfTheButtonWasPressed);
            for(int i = 0; i < AllRaycastHits.Length; i++)
            {
                if (AllRaycastHits[i].collider.Equals(ThisCollider))
                {
                    OnClick.Invoke();
                }
            }
        }
    }
}