using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCreatureOnNewPosition : DraggingActions
{
    // reference to the sprite with a round "Target" graphic
    private SpriteRenderer sr;
    // LineRenderer that is attached to a child game object to draw the arrow
    private LineRenderer lr;
    // reference to WhereIsTheCardOrCreature to track this object`s state in the game
    private WhereIsTheCardOrCreature whereIsThisCreature;
    // the pointy end of the arrow, should be called "Triangle" in the Hierarchy
    private Transform triangle;
    // SpriteRenderer of triangle. We need this to disable the pointy end if the target is too close.
    private SpriteRenderer triangleSR;
    // when we stop dragging, the gameObject that we were targeting will be stored in this variable.
    private GameObject Target;
    // Reference to creature manager, attached to the parent game object
    private OneUnitManager manager;
    private OneHeroManager h_manager;


    void Awake()
    {
        // establish all the connections
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Arrow");
        triangleSR = triangle.GetComponent<SpriteRenderer>();

        manager = GetComponentInParent<OneUnitManager>();
        h_manager = GetComponentInParent<OneHeroManager>();
        whereIsThisCreature = GetComponentInParent<WhereIsTheCardOrCreature>();
    }
    public override bool CanDrag
    {
        get
        {
          

            
            if (manager == null)
            {
                return base.CanDrag && h_manager.CanMoveNow;
            }
            else
            {
                return base.CanDrag && manager.CanMoveNow;
            }

        }
    }
    public override void OnDraggingInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction * 2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            // draw a line between the creature and the target
            lr.SetPositions(new Vector3[] { transform.parent.position, transform.position - direction * 2.3f });
            lr.enabled = true;

            // position the end of the arrow between near the target.
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.5f * direction;

            // proper rotarion of arrow end
            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            // if the target is not far enough from creature, do not show the arrow
            lr.enabled = false;
            triangleSR.enabled = false;
        }
    }

    public override void OnEndDrag()
    {

        Target = null;
        bool targetValid = false;


        int tablePos = playerOwner.PArea.DualTableVisual.TablePositionForNewUnit(Camera.main.ScreenToWorldPoint(
           new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).x, (Camera.main.ScreenToWorldPoint(
           new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).y));

       
       
        



        if (!targetValid)
        {
            // not a valid target, return
         
            whereIsThisCreature.VisualState = VisualStates.Table;
            whereIsThisCreature.SetTableSortingOrder();
        }

        // return target and arrow to original position
        transform.localPosition = Vector3.zero;
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;












    }

    public override void OnStartDrag()
    {
        whereIsThisCreature.VisualState = VisualStates.Dragging;
        // enable target graphic
        sr.enabled = true;
        // enable line renderer to start drawing the line.
        lr.enabled = true;
    }

    protected override bool DragSuccessful()
    {
        return true;
    }
}
