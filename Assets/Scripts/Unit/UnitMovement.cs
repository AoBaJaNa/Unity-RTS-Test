using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public Vector3 targetPos;
    public float moveSpeed = 5f;
    MaterialPropertyBlock MaterialPropertyBlock;
    Renderer renderer;
    public bool IsSelected { get; private set; }

    private void Awake()
    {
        targetPos = transform.position;
        renderer = GetComponent<Renderer>();
        MaterialPropertyBlock = new MaterialPropertyBlock();
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    public void Selected()
    {
        SetColor(true);
        IsSelected = true;
        Debug.Log($"º±≈√ µ : {gameObject.name}");
    }
    public void DeSelected()
    {
        SetColor(false);
        IsSelected = false;
    }
    void SetColor(bool isSelected)
    {
        renderer.GetPropertyBlock(MaterialPropertyBlock);
        Color color = isSelected ? Color.red : Color.white;
        MaterialPropertyBlock.SetColor("_BaseColor", color);
        renderer.SetPropertyBlock(MaterialPropertyBlock);
    }
}
