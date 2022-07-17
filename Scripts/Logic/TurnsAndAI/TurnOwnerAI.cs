using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//this class will take all decisions for AI. 

public class TurnOwnerAI : TurnOwner
{

    int NumberOfPlayerUnits = 0;
    int NumberOfAIUnits = 0;

    private void Start()
    {

    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        new ShowMessageCommand("Enemy`s Turn!", 3f).AddToQueue();
        PlayerOwner.DrawACard();
        StartCoroutine(MakeAITurn());
    }

    // THE LOGIC FOR AI
    IEnumerator MakeAITurn()
    {
        int cardInHand = 0;
        foreach (CardInLogic c in PlayerOwner.hand.CardsInHand)
        {
            cardInHand++;  
            
        }
        if (cardInHand == 0)
        {
            InsertDelay(3f);
        }

        foreach (UnitInLogic cl in Table.instance.UnitsOnTable)
        {
            if (cl.owner != PlayerOwner)
            {
                NumberOfPlayerUnits++;
            }
            else if (cl.owner == PlayerOwner)
            {
                NumberOfAIUnits++;
            }
        }

        bool strategyAttackFirst = false;
        if (Random.Range(0, 2) == 0)
            strategyAttackFirst = true;

        while (MakeOneAIMove(strategyAttackFirst))
        {
            yield return null;
        }
       
        InsertDelay(1f);
        StartCoroutine(Wait(2));
        if (cardInHand == 0)
        {
            StartCoroutine(Wait(3));
        }

        new EndATurnCommand().AddToQueue();
    }

    bool MakeOneAIMove(bool attackFirst)
    {
        if (Command.CardDrawPending())
            return true;
        else if (attackFirst)
            return AttackOrMoveUnit() || PlayCardsFromHand();
        else
            return PlayCardsFromHand() || AttackOrMoveUnit();
    }

    bool PlayCardsFromHand()
    {
        foreach (CardInLogic c in PlayerOwner.hand.CardsInHand)
        {
            //Debug.LogWarning("karta: " + c._cardAsset.name);
            if (c.CanBePlayed)
            {
                if (c._cardAsset.MaxHealth == 0)
                {
                    if (c._cardAsset.Targets == TargetingOptions.NoTarget)
                    {
                        bool SlotIsFree = false;
                        int tablePos = -1;

                        for (int i = 2; i < 4; i++)
                        {
                            if (Table.instance.ChechkIfSpellSlotIsFree(i))
                            {
                                tablePos = i;
                            }
                        }

                        if (Table.instance.ChechkIfSpellSlotIsFree(tablePos))
                        {
                            SlotIsFree = true;
                        }


                        if (SlotIsFree)
                        {
                            PlayerOwner.PlaySpellOnTable(c.ID, tablePos);
                            InsertDelay(1.5f);
                            return true;
                        }

                    }
                    else
                    {
                        int TargetID;
                        int TargetPosition;
                        List<UnitInLogic> EnemyCreatures = new List<UnitInLogic>();
                        List<UnitInLogic> AICreatures = new List<UnitInLogic>();
                        foreach (UnitInLogic cl in Table.instance.UnitsOnTable)
                        {
                            if (cl.owner != PlayerOwner && cl.owner != null)
                            {
                                EnemyCreatures.Add(cl);
                                //Debug.LogWarning("Added enemy creature");
                            }
                            else if (cl.owner == PlayerOwner)
                            {
                                AICreatures.Add(cl);
                                //Debug.LogWarning("Added friendly creature");
                            }
                        }
                        if (c._cardAsset.Targets == TargetingOptions.AllUnits || c._cardAsset.Targets == TargetingOptions.EnemyUnits)
                        {

                            bool attackHero = false;
                            if (Random.Range(0, 2) == 0)
                                attackHero = true;

                            if (attackHero)
                            {
                                TargetID = 6;
                                PlayerOwner.PlaySpellFromHand(c.UniqueCardID, TargetID);
                                InsertDelay(1.5f);
                                return true;
                            }
                            else
                            {
                                
                               

                                int attackValue = 1;
                                int unitID=0;
                                foreach (UnitInLogic ul in EnemyCreatures)
                                {
                                    if (ul.Attack > attackValue)
                                    {
                                        attackValue = ul.Attack;
                                        unitID = ul.ID;
                                        
                                    }
                                   
                                }

                                if (unitID > 0)
                                {
                                    PlayerOwner.PlaySpellFromHand(c.UniqueCardID, unitID);
                                    InsertDelay(1.5f);
                                    return true;
                                }
                                else
                                {
                                    int range = Random.Range(0, EnemyCreatures.Count);
                                    //Debug.LogWarning("range " + range);
                                    TargetPosition = EnemyCreatures[range].Position;

                                    //Debug.LogWarning("Target Position : " + TargetPosition);
                                    TargetID = Table.instance.FindUnitByPosition(TargetPosition);
                                    PlayerOwner.PlaySpellFromHand(c.UniqueCardID, TargetID);
                                    InsertDelay(1.5f);
                                    return true;
                                }

                               
                            }

                            
                        }
                        else
                        {
                            int range = Random.Range(0, AICreatures.Count);
                            TargetPosition = AICreatures[range].Position;
                            TargetID = Table.instance.FindUnitByPosition(TargetPosition);
                            PlayerOwner.PlaySpellFromHand(c.UniqueCardID, TargetID);
                            InsertDelay(1.5f);
                            return true;
                        }

                    }
                }
                else //if (NumberOfAICreatures < 5)
                {
                    int TargetID;
                    int TargetPosition;
                    List<int> AvaliablePositions = new List<int>();
                    for (int i = 12; i < 24; i++)
                    {
                        if (Table.instance.ChechkIfUnitSlotIsFree(i))
                        {
                            AvaliablePositions.Add(i);
                        }
                    }

                    if (c._cardAsset.attackType == AttackType.Range)
                    {
                        List<int> randomPositions = new List<int>();
                        int bestPositon;
                        for (int i = 0; i < 6; i++)
                        {
                            int k = Random.Range(0, AvaliablePositions.Count);
                            randomPositions.Add(k);
                        }
                        if (randomPositions.Count > 0)
                        {
                            bestPositon = randomPositions[0];

                            foreach (int r in randomPositions)
                            {
                                if (r > bestPositon)
                                {
                                    bestPositon = r;
                                }
                            }
                            TargetPosition = AvaliablePositions[bestPositon];
                            PlayerOwner.PlayUnitFromHand(c, TargetPosition);
                            InsertDelay(1.5f);
                            return true;
                        }

                    }
                    else 
                    {
                        int index = Random.Range(0, AvaliablePositions.Count);
                        TargetPosition = AvaliablePositions[index];
                        PlayerOwner.PlayUnitFromHand(c, TargetPosition);
                        InsertDelay(1.5f);
                        return true;
                    }
                    
                }

            }
        }
        return false;
    }

    bool AttackOrMoveUnit()
    {

        List<int> AllovedSlotsToMove = new List<int>();
        List<int> AllovedSlotsToAttack = new List<int>();
       

        int TargetID;
        int TargetPosition;
        List<UnitInLogic> EnemyCreatures = new List<UnitInLogic>();
        List<UnitInLogic> AICreatures = new List<UnitInLogic>();
        List<UnitInLogic> CreaturesInRange = new List<UnitInLogic>();

        foreach (UnitInLogic cl in Table.instance.UnitsOnTable)
        {
            if (cl.owner != PlayerOwner)
            {
                EnemyCreatures.Add(cl);
            }
            else if (cl.owner == PlayerOwner)
            {
                AICreatures.Add(cl);
            }
        }
        foreach (UnitInLogic cll in AICreatures)
        {
            MovementOptions movementOptions;
            bool Move = false;
            bool RandomMove = false;
            if (cll.owner != null)
            {
                if (cll.UniqueUnitID == 4 || cll.UniqueUnitID == 6)
                {
                    AllovedSlotsToMove = GetAllowedMoves.instance.GetAllowedSlots(cll.Position, cll.ha.MoveRange, cll.ha.moveOption);
                    AllovedSlotsToAttack = GetAllowedMoves.instance.GetAllowedSlots(cll.Position, cll.ha.AttackRange, cll.ha.attackOption);
                    movementOptions = cll.ha.moveOption;
                }
                else
                {
                    AllovedSlotsToMove = GetAllowedMoves.instance.GetAllowedSlots(cll.Position, cll.ca.MoveRange, cll.ca.moveOption);
                    AllovedSlotsToAttack = GetAllowedMoves.instance.GetAllowedSlots(cll.Position, cll.ca.AttackRange, cll.ca.attackOption);
                    movementOptions = cll.ca.moveOption;
                }


                if (cll.MovePoints > 0)
                {
                    foreach (int i in AllovedSlotsToAttack)
                    {
                        if (!Table.instance.ChechkIfUnitSlotIsFree(i))
                        {
                            int CreatureID = Table.instance.FindUnitByPosition(i);
                            UnitInLogic creature = UnitInLogic.FindUnitLogicByID(CreatureID);
                            if (creature.owner != PlayerOwner)
                            {
                                CreaturesInRange.Add(creature);
                            }

                        }
                    }

                    if (CreaturesInRange.Count > 0)
                    {
                        for (int i = 0; i < cll.MovePoints; i++)
                        {
                            int index = Random.Range(0, CreaturesInRange.Count);
                            TargetID = CreaturesInRange[index].UniqueUnitID;
                            cll.AttackUnitWithID(TargetID);
                            InsertDelay(1.5f);
                        }

                        return true;
                    }
                    else
                    {
                        if (AllovedSlotsToMove.Count > 0)
                        {
                            int currentPosition = cll.Position;
                            int currentEnemyHeroPosition = UnitInLogic.FindUnitLogicByID(6).Position;

                            if (cll.UniqueUnitID == 4)
                            {
                              
                                int r = Random.Range(0, 20);
                                if (r == 5 || r == 6 || r == 7)
                                {
                                    Move = true;
                                }
                                else if (r == 12 || r == 13)
                                {
                                    RandomMove = true;
                                }
                            }
                            else
                            {
                                
                                int r = Random.Range(0, 10);
                                if (r == 0 || r == 2 || r == 4 || r == 5 || r == 6 || r == 7 || r == 8)
                                {
                                    Move = true;
                                }
                                else
                                {
                                    RandomMove = true;
                                }
                            }



                            if (movementOptions != MovementOptions.Vertical && Move)
                            {
                                bool skip = false;
                                List<int> TempAllovedSlotsToMove = AllovedSlotsToMove.ToList();

                                if (NumberOfPlayerUnits < 2)
                                {
                                    
                                    int clostest = TempAllovedSlotsToMove.Aggregate((x, y) => Mathf.Abs(x - currentEnemyHeroPosition) < Mathf.Abs(y - currentEnemyHeroPosition) ? x : y);
                                    Debug.LogWarning("closest : " + clostest + " dla " + cll.ID);
                                    if (Table.instance.ChechkIfUnitSlotIsFree(clostest))
                                    {
                                        currentPosition = clostest;
                                        skip = true;
                                        Debug.LogWarning("Gowno");
                                    }
                                    /* else
                                     {
                                         int index = TempAllovedSlotsToMove.IndexOf(clostest);
                                         TempAllovedSlotsToMove[index] = currentPosition;
                                     }
                                    */
                                }
                                if (!skip)
                                {
                                    foreach (int i in AllovedSlotsToMove)
                                    {
                                        if (currentEnemyHeroPosition == 3 || currentEnemyHeroPosition == 7 || currentEnemyHeroPosition == 11 || currentEnemyHeroPosition == 15 || currentEnemyHeroPosition == 19 || currentEnemyHeroPosition == 23)
                                        {

                                        }
                                        else
                                        {
                                            if (i < currentPosition)
                                            {
                                                if (Table.instance.ChechkIfUnitSlotIsFree(i))
                                                {
                                                    currentPosition = i;
                                                }

                                            }
                                        }
                                    }
                                }

                                if (Table.instance.ChechkIfUnitSlotIsFree(currentPosition))
                                {
                                    TargetPosition = currentPosition;
                                    Vector3 tablePos = TabeSlots.Instance.All_Creature_slots[TargetPosition].transform.position;
                                    cll.MoveUnit(cll.UniqueUnitID, TargetPosition, tablePos);
                                    InsertDelay(1.5f);
                                    return true;
                                }

                            }
                            else if (RandomMove)
                            {
                                bool stay = true;

                                int rand = Random.Range(0, 2);
                                if (rand == 0)
                                    stay = false;


                                if (!stay)
                                {
                                    int RandomPosition = -1;
                                    foreach (int i in AllovedSlotsToMove)
                                    {
                                        int index = Random.Range(0, AllovedSlotsToMove.Count);

                                        if (Table.instance.ChechkIfUnitSlotIsFree(AllovedSlotsToMove[index]))
                                        {
                                            RandomPosition = AllovedSlotsToMove[index];
                                            break;
                                        }
                                    }
                                    if (Table.instance.ChechkIfUnitSlotIsFree(RandomPosition))
                                    {
                                        TargetPosition = RandomPosition;
                                        Vector3 tablePos = TabeSlots.Instance.All_Creature_slots[TargetPosition].transform.position;
                                        cll.MoveUnit(cll.UniqueUnitID, TargetPosition, tablePos);
                                        InsertDelay(1.5f);
                                        return true;
                                    }

                                }

                            }


                        }

                    }
                }
            }
        }

       

        return false;
    }



    void InsertDelay(float delay)
    {
        new DelayCommand(delay).AddToQueue();
    }
    IEnumerator Wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
