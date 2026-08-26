using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public Vector3 targetPos;
    public float moveSpeed = 5f;
    public bool IsSelected { get; private set; }

    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    public void Selected()
    {
        IsSelected = true;
        Debug.Log($"º±≈√ µ : {gameObject.name}");
    }
    public void DeSelected()
    {
        IsSelected = false;
    }
}
