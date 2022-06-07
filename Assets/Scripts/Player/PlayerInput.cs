using UnityEngine;

[RequireComponent(typeof(PlayerMover))]

public class PlayerInput : MonoBehaviour
{
    private PlayerMover _payerMover;
    private CameraShaker _shaker;

    private void Start()
    {
        _payerMover = GetComponent<PlayerMover>();
        _shaker = GetComponentInChildren<CameraShaker>();
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0) && Physics.Raycast(ray, out RaycastHit hit))
        {
            if (!hit.collider.gameObject.GetComponent<Pickable>())
            {
                _payerMover.Move();
                _shaker.Begin();
            }
        }
        else
        {
            _shaker.End();
        }
    }
}
