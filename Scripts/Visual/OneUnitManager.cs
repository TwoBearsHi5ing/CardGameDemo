using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OneUnitManager : MonoBehaviour 
{
    public CardAsset cardAsset;
    public OneCardManager PrevierManager;
    public OneUnitManager HighlightManager;
    [Header("Text Component References")]
    public Text HealthText;
    public Text AttackText;
    public Text MovePoints;
    [Header("Image References")]
    public Image CreatureGraphicImage;
    public Image CreatureGlowImage;
    public Image CreatureFaction;
    public Image CreatureBackgound;

    void Awake()
    {
        if (cardAsset != null)
            ReadCreatureFromAsset();
    }

    private bool canAttackNow = false;
    public bool CanAttackNow
    {
        get
        {
            return canAttackNow;
        }

        set
        {
            canAttackNow = value;

            CreatureGlowImage.enabled = true;
        }
    }

    private bool canMoveNow = false;
    public bool CanMoveNow
    {
        get
        {
            return canMoveNow;
        }

        set
        {
            canMoveNow = value;

            CreatureGlowImage.enabled = true;
        }
    }

    public void ReadCreatureFromAsset()
    {
        // Change the card graphic sprite
        CreatureGraphicImage.sprite = cardAsset.CardImage;

        AttackText.text = cardAsset.Attack.ToString();
        HealthText.text = cardAsset.MaxHealth.ToString();
        MovePoints.text = cardAsset.MovePoints.ToString();

        if (cardAsset.factionAsset != null)
        {
            CreatureBackgound.color = cardAsset.factionAsset.FactionBackgroundColor;
            CreatureFaction.sprite = cardAsset.factionAsset.FactionImage;
        }
      

        if (PrevierManager != null && HighlightManager != null)
        {
          //  Debug.Log("Read Creture form asset in Preview Manager");
            PrevierManager.cardAsset = cardAsset;
            PrevierManager.ReadCardFromAsset(false);

            HighlightManager.cardAsset = cardAsset;
            HighlightManager.ReadCreatureFromAsset();


           

            /*    Debug.Log("hihlight manager max health in card asset = " + HighlightManager.cardAsset.MaxHealth);
                Debug.Log("hihlight manager  health int text " + HighlightManager.HealthText.text);
                Debug.Log("preview manager max health in card asset = " + PrevierManager.cardAsset.MaxHealth);
                Debug.Log("preview manager  health int text " + PrevierManager.HealthText.text);
            */
        }
    }
   
    public void updateStats(int atk, int mp, int health)
    {
        AttackText.text = atk.ToString();
        MovePoints.text = mp.ToString();
        HealthText.text = health.ToString();

        HighlightManager.AttackText.text = atk.ToString();
        HighlightManager.MovePoints.text = mp.ToString();
        HighlightManager.HealthText.text = health.ToString();

    }
    public void TakeDamage(int amount, int healthAfter)
    {
        if (amount > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount, Visual_Effect.Damage);
            HealthText.text = healthAfter.ToString();
            HighlightManager.HealthText.text = healthAfter.ToString();
        }
    }

    public void Heal(int amount, int healthAfter)
    {
        if (amount > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount, Visual_Effect.Heal);
            HealthText.text = healthAfter.ToString();
            HighlightManager.HealthText.text = healthAfter.ToString();
        }
    }

    public void AddAttack(int amount, int attackAfter)
    {
        if (amount > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount, Visual_Effect.AttackBonus);
            AttackText.text = attackAfter.ToString();
            HighlightManager.AttackText.text = attackAfter.ToString();
        }
    }
    public void AddMovePoints(int amount, int movesAfter)
    {
        if (amount > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount, Visual_Effect.MovePointsBonus);
            MovePoints.text = movesAfter.ToString();
            HighlightManager.MovePoints.text = movesAfter.ToString();
        }
    }
}
