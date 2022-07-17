using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragSpellOnTarget : DraggingActions {

    public TargetingOptions Targets = TargetingOptions.AllUnits;
    private SpriteRenderer sr;
    private LineRenderer lr;
    private WhereIsTheCardOrCreature whereIsThisCard;
    private VisualStates tempVisualState;
    private Transform triangle;
    private SpriteRenderer triangleSR;
    private GameObject Target;
    private OneCardManager manager;

    public override bool CanDrag
    {
        get
        {          
            return base.CanDrag && manager.CanBePlayedNow;
        }
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Arrow");
        triangleSR = triangle.GetComponent<SpriteRenderer>();


        manager = GetComponentInParent<OneCardManager>();
        whereIsThisCard = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    public override void OnStartDrag()
    {
        tempVisualState = whereIsThisCard.VisualState;
        whereIsThisCard.VisualState = VisualStates.Dragging;
        sr.enabled = true;
        lr.enabled = true;
    }

    public override void OnDraggingInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction*2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            lr.SetPositions(new Vector3[]{ transform.parent.position, transform.position - direction });
            lr.enabled = true;

            triangleSR.enabled = true;
            triangleSR.transform.parent.position = transform.position - direction;      
        }
        else
        {          
            lr.enabled = false;  
        }

    }

    public override void OnEndDrag()
    {
        Target = null;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin: Camera.main.transform.position, 
            direction: (-Camera.main.transform.position + this.transform.position).normalized, 
            maxDistance: 30f) ;

        foreach (RaycastHit h in hits)
        {
            if (h.transform.tag.Contains("Player"))
            {
                Target = h.transform.gameObject;
            }
            else if (h.transform.tag.Contains("Creature"))
            {
                Target = h.transform.parent.gameObject;
            }
        }

        bool targetValid = false;

        if (Target != null)
        {
            Player owner = null; 
            if (tag.Contains("Low"))
                owner = GlobalSettings.Instance.LowPlayer;
            else
                owner = GlobalSettings.Instance.TopPlayer;

            int targetID = Target.GetComponent<IDHolder>().UniqueID;
            switch (Targets)
            {
               
                case TargetingOptions.AllUnits:
                    if (Target.tag.Contains("Creature"))
                    {
                        owner.PlaySpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                        targetValid = true;
                    }
                    break;
                
                case TargetingOptions.EnemyUnits:
                    if (Target.tag.Contains("Creature"))
                    {
                       
                        if ((tag.Contains("Low") && Target.tag.Contains("Top"))
                            || (tag.Contains("Top") && Target.tag.Contains("Low")))
                        {
                            owner.PlaySpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                            targetValid = true;
                        }
                    }
                    break;
                   
                case TargetingOptions.YourUnits:
                    if (Target.tag.Contains("Creature"))
                    {
                        
                        if ((tag.Contains("Low") && Target.tag.Contains("Low"))
                            || (tag.Contains("Top") && Target.tag.Contains("Top")))
                        {
                            owner.PlaySpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                            targetValid = true;
                        }
                    }
                    break;
                default:
                    Debug.LogWarning("Reached default case in DragSpellOnTarget! Suspicious behaviour!!");
                    break;
            }
        }

        if (!targetValid)
        {
            // not a valid target, return
            whereIsThisCard.VisualState = tempVisualState;
            whereIsThisCard.SetHandSortingOrder();
        }

        // return target and arrow to original position
        // this position is special for spell cards to show the arrow on top
        transform.localPosition = new Vector3(0f, 0f, 0.4f);
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;

    }

    // NOT USED IN THIS SCRIPT
    protected override bool DragSuccessful()
    {
        return true;
    }
}
