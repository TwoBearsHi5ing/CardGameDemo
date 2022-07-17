using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum Visual_Effect
{ 
    Heal,
    Damage,
    AttackBonus,
    MovePointsBonus,
    ManaBonus
}

public class DamageEffect : MonoBehaviour {

    // an array of sprites with different blood splash graphics
    public Sprite[] Damage_sprites;
    public Sprite Attack_sprite;
    public Sprite Move_sprite;
    public Sprite Mana_sprite;
    public Sprite Heal_sprite;
    Sprite Damage_spr;

    // a UI image to show the blood splashes
    public Image EffectImage;

    // CanvasGropup should be attached to the Canvas of this damage effect
    // It is used to fade away the alpha value of this effect
    public CanvasGroup cg;

    // The text component to show the amount of damage taken by target like: "-2"
    public Text AmountText;


    // A Coroutine to control the fading of this damage effect
    private IEnumerator ShowDamageEffect()
    {
        // make this effect non-transparent
        cg.alpha = 1f;
        // wait for 1 second before fading
        yield return new WaitForSeconds(1f);
        // gradually fade the effect by changing its alpha value
        while (cg.alpha > 0)
        {
            cg.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        // after the effect is shown it gets destroyed.
        Destroy(this.gameObject);
    }
   
    public static void CreateDamageEffect(Vector3 position, int amount, Visual_Effect ef)
    {
        if (amount == 0)
            return;
        // Instantiate a DamageEffect from prefab
        GameObject newDamageEffect = GameObject.Instantiate(GlobalSettings.Instance.DamageEffectPrefab, position, Quaternion.identity) as GameObject;
        // Get DamageEffect component in this new game object
        DamageEffect de = newDamageEffect.GetComponent<DamageEffect>();
        de.EffectImage.color = new Color32(255, 255, 255, 255);

       de.Damage_spr = de.Damage_sprites[Random.Range(0, de.Damage_sprites.Length)];

        if (ef == Visual_Effect.AttackBonus)
        {
            de.EffectImage.sprite = de.Attack_sprite;
            de.AmountText.text = "+" + amount.ToString();
        }
        else if (ef == Visual_Effect.MovePointsBonus)
        {
            de.EffectImage.sprite = de.Move_sprite;
            de.AmountText.text = "+" + amount.ToString();
        }
        else if (ef == Visual_Effect.ManaBonus)
        {
            de.EffectImage.sprite = de.Mana_sprite;
            de.AmountText.text = "+" + amount.ToString();
        }
        else if (ef == Visual_Effect.Damage)
        {
            de.EffectImage.color = new Color32(144, 35, 40, 255);
            de.EffectImage.sprite = de.Damage_spr;
                     
                de.AmountText.text = "-" + amount.ToString();
           
        }
        else if (ef == Visual_Effect.Heal)
        {
            de.EffectImage.sprite = de.Heal_sprite;
            de.AmountText.text = "+" + amount.ToString();
        }

       
        // start a coroutine to fade away and delete this effect after a certain time
        de.StartCoroutine(de.ShowDamageEffect());
    }
}
