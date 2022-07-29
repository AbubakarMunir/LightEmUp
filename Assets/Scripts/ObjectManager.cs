using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    MeshRenderer _renderer;
    BoxCollider2D boxCollider;
    [SerializeField] Material simple;
    [SerializeField] Material highlighted;
    CharacterBehaviour characterBehaviour;
    bool caughtOnce;
    // Start is called before the first frame update
    void Start()
    {
        characterBehaviour = GameManager.player.GetComponent<CharacterBehaviour>();
        boxCollider = GetComponent<BoxCollider2D>();
        _renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PointInBoxCollider(GameManager.player.transform.position,boxCollider))
        {
            if (caughtOnce)
                return;
            caughtOnce = true;
            HighLight();
            characterBehaviour.StopHere();
            characterBehaviour.currentState = CharacterBehaviour.STATE.STATIC;
        }
        else
        {
            caughtOnce = false;
            UnHighLight();
        }
    }

    

    public void HighLight()
    {
        _renderer.material = highlighted;
    }

    public void UnHighLight()
    {
        _renderer.material = simple;
    }

    bool PointInBoxCollider(Vector3 point, BoxCollider2D box)
    {
        Vector2 center = box.bounds.center;
        Vector2 extents = box.bounds.extents;

        Vector2 rightTop = center + extents;
        Vector2 leftTop = center + new Vector2(-extents.x, extents.y);
        Vector2 rightBottom = center + new Vector2(extents.x, -extents.y);
        Vector2 leftBottom = center - extents;
        if (point.x < rightTop.x && point.x > leftTop.x && point.y < rightTop.y && point.y > rightBottom.y)
            return true;
        return false;
    }
}
