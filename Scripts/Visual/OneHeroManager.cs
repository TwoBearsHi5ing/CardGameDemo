using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneHeroManager : MonoBehaviour
{
    public HeroAsset heroAsset;
    public OneHeroCardManager HighlightManager;
    public OneHeroManager PreviewManager;
    [Header("Text Component References")]
    public Text HealthText;
    public Text AttackText;
    public Text MovePoints;
    [Header("Image References")]
    public Image CreatureGraphicImage;
    public Image CreatureGlowImage;
    public Image AvatarBGTint;
    public Image PowerButtonGraphic;
    public Image Factionimage;


    void Awake()
    {
        if (heroAsset != null)
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
        CreatureGraphicImage.sprite = heroAsset.HeroImage;

        AttackText.text = heroAsset.Attack.ToString();
        HealthText.text = heroAsset.MaxHealth.ToString();
        MovePoints.text = heroAsset.AP.ToString();

        if (heroAsset.faction != null)
        {
            AvatarBGTint.color = heroAsset.faction.FactionBackgroundColor;
            Factionimage.sprite = heroAsset.faction.FactionImage;
        }
        else
        {
            AvatarBGTint.color = GlobalSettings.Instance.DefaultBackgroundColor;
            Factionimage.sprite = GlobalSettings.Instance.DefaultFactionImage;
        }
      

        if (PreviewManager != null)
        {
            PreviewManager.heroAsset = heroAsset;
            PreviewManager.ReadCreatureFromAsset();
            HighlightManager.heroAsset = heroAsset;
            HighlightManager.ReadCardFromAsset();
        }
    }
    public void updateStats(int atk, int mp, int health)
    {
        AttackText.text = atk.ToString();
        MovePoints.text = mp.ToString();
        HealthText.text = health.ToString();

        PreviewManager.AttackText.text = atk.ToString();
        PreviewManager.MovePoints.text = mp.ToString();
        PreviewManager.HealthText.text = health.ToString();

    }
    public void TakeDamage(int amount, int healthAfter)
    {
        if (amount > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount, Visual_Effect.Damage);
            HealthText.text = healthAfter.ToString();
            PreviewManager.HealthText.text = healthAfter.ToString();
        }
    }

    public void Heal(int amount, int healthAfter)
    {
        if (amount > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount, Visual_Effect.Heal);
            
            HealthText.text = healthAfter.ToString();
            PreviewManager.HealthText.text = healthAfter.ToString();
        }
    }
    public void AddAttack(int amount, int attackAfter)
    {
        if (amount > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount, Visual_Effect.AttackBonus);

            AttackText.text = attackAfter.ToString();
            PreviewManager.AttackText.text = attackAfter.ToString();
        }
    }
    public void AddMovePoints(int amount, int movesAfter)
    {
        if (amount > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount, Visual_Effect.MovePointsBonus);

            MovePoints.text = movesAfter.ToString();
            PreviewManager.MovePoints.text = movesAfter.ToString();
        }
    }

    public void Explode()
    {
        Instantiate(GlobalSettings.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
        Sequence s = DOTween.Sequence();
        s.PrependInterval(2f);
        s.OnComplete(() =>
        {
            GlobalSettings.Instance.GameOverPanel.SetActive(true);
            GlobalSettings.Instance.GamOverText.SetActive(true);
            GlobalSettings.Instance.GameOver = true;
            Time.timeScale = 0;
            Command.CommandExecutionComplete();
        });
       
    }

}
