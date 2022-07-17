using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HoverPreview: MonoBehaviour
{
    // PUBLIC FIELDS
    public GameObject TurnThisOffWhenPreviewing;  // if this is null, will not turn off anything 
    public GameObject TurnThisOffWhenHighlighting;
    public Vector3 TargetPosition_vector = new Vector3(0,2,0);
    public GameObject TargetPosition_gameobject;
  
    public GameObject HighlightGameObject;
    public GameObject previewGameObject;
    public bool ActivateInAwake = false;

    public float HighlightTargetScale = 1.5f;
    public float PreviewTargetScale = 2.5f;

    public bool disablePreview;

    // PRIVATE FIELDS
    private static HoverPreview currentlyViewing = null;
    private Vector3 StartingPosition;

    private WhereIsTheCardOrCreature w;

    static private float DoMoveUpTime = 1f;
    static private float DoMoveDownTime = 0.5f;

    static private float DoScaleUpTime = 1f;
    static private float DoScaleDownTime = 1f;

    private bool activePreview;

    bool OneTimePreview = true;

    private static bool _PreviewsAllowed = true;
    public static bool PreviewsAllowed
    {
        get { return _PreviewsAllowed;}

        set 
        { 
            //Debug.Log("Hover Previews Allowed is now: " + value);
            _PreviewsAllowed= value;
            if (!_PreviewsAllowed)
                StopAllPreviews();
        }
    }

    private bool _thisPreviewEnabled = false;
    public bool ThisPreviewEnabled
    {
        get { return _thisPreviewEnabled;}

        set 
        { 
            _thisPreviewEnabled = value;
            if (!_thisPreviewEnabled)
                StopThisPreview();
        }
    }

    public bool OverCollider { get; set;}
 
    void Awake()
    {
        ThisPreviewEnabled = ActivateInAwake;
        w = GetComponent<WhereIsTheCardOrCreature>();
        if (w == null)
            w = GetComponentInChildren<WhereIsTheCardOrCreature>();

        if (w == null)
            w = GetComponentInParent<WhereIsTheCardOrCreature>();
    }
            
    void OnMouseEnter()
    {
        
        OverCollider = true;

        if (PreviewsAllowed && ThisPreviewEnabled && !disablePreview)
        {
            HighlightCard();
        }
        else if (PreviewsAllowed && ThisPreviewEnabled && disablePreview)
        {
            activePreview = true;
         
        }
       
           
      
    }
        
    void OnMouseExit()
    {

        OverCollider = false;
        OneTimePreview = true;

        if (!PreviewingSomeCard())
        {
            activePreview = false;
            StopAllPreviews();
        }
            
    }


    private void Update()
    {
        if (activePreview == true && Input.GetMouseButtonDown(1) && OneTimePreview)
        {
            PreviewCard();
        }

        if (Time.timeScale == 0)
        {
            _PreviewsAllowed = false;
            StopAllPreviews();
        }
        else if (Time.timeScale == 1)
        {
            _PreviewsAllowed = true;
        }
    }

    void HighlightCard()
    {
        activePreview = true;
        w.BringToFront();
     
        StopAllPreviews();
    
        currentlyViewing = this;
    
        HighlightGameObject.SetActive(true);
  
        if (TurnThisOffWhenHighlighting != null)
            TurnThisOffWhenHighlighting.SetActive(false); 
    
        HighlightGameObject.transform.localPosition = Vector3.zero;
        HighlightGameObject.transform.localScale = Vector3.one;

        HighlightGameObject.transform.DOKill();
     
        HighlightGameObject.transform.DOLocalMove(TargetPosition_vector, DoMoveUpTime).SetEase(Ease.OutQuint);
        HighlightGameObject.transform.DOScale(HighlightTargetScale, DoScaleUpTime).SetEase(Ease.OutQuint);
      
      
    }

    void PreviewCard()
    {
        OneTimePreview = false;
        w.BringToFront();
    
        StopAllPreviews();
        currentlyViewing = this;
        previewGameObject.SetActive(true);

        if (TurnThisOffWhenPreviewing != null)
            TurnThisOffWhenPreviewing.SetActive(false);

        previewGameObject.transform.localPosition = Vector3.zero;
        previewGameObject.transform.localScale = Vector3.one;

        previewGameObject.transform.DOKill();
        previewGameObject.transform.DOMove(TargetPosition_gameobject.transform.position, DoMoveUpTime).SetEase(Ease.OutQuint);
        previewGameObject.transform.DOScale(PreviewTargetScale, DoScaleUpTime).SetEase(Ease.OutQuint);
    }

    void StopThisPreview()
    {
        previewGameObject.transform.DOKill();
        w.SetHandSortingOrder();
        if (TargetPosition_gameobject == null)
        {
            HighlightGameObject.transform.DOLocalMove(Vector3.zero, DoMoveDownTime).SetEase(Ease.OutQuint);
            HighlightGameObject.transform.DOScale(1f, DoScaleDownTime).SetEase(Ease.OutQuint).OnComplete(() =>
            {
                HighlightGameObject.SetActive(false);
                previewGameObject.SetActive(false);
                if (TurnThisOffWhenPreviewing != null)
                    TurnThisOffWhenPreviewing.SetActive(true);
                if (TurnThisOffWhenHighlighting != null)
                    TurnThisOffWhenHighlighting.SetActive(true);
            });
        }
        else 
        {
            HighlightGameObject.SetActive(false);
            previewGameObject.SetActive(false);
            if (TurnThisOffWhenPreviewing != null)
                TurnThisOffWhenPreviewing.SetActive(true);
            if (TurnThisOffWhenHighlighting != null)
                TurnThisOffWhenHighlighting.SetActive(true);
        }
    }

    private static void StopAllPreviews()
    {
        if (currentlyViewing != null)
        {
            currentlyViewing.StopThisPreview();
        }

    }

    private static bool PreviewingSomeCard()
    {
        if (!PreviewsAllowed)
            return false;

        HoverPreview[] allHoverBlowups = GameObject.FindObjectsOfType<HoverPreview>();

        foreach (HoverPreview hb in allHoverBlowups)
        {
            if (hb.OverCollider && hb.ThisPreviewEnabled)
                return true;
        }

        return false;
    }

   
}
