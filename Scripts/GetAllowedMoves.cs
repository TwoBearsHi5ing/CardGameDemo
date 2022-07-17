using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllowedMoves : MonoBehaviour
{
    public static GetAllowedMoves instance;
    
    private void Awake()
    {
        instance = this;

    }


    public List<int> GetAllowedSlots(int Position, int Range, MovementOptions moveOption)
    {

        List<int> possibleMoves = new List<int>();
        List<int> allowedMoves = new List<int>();
        int Count = 0;


        possibleMoves.Clear();
        allowedMoves.Clear();



        if (Range == 1)
        {
            if (moveOption == MovementOptions.Horizontal)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {
                        possibleMoves.Add(Position - 4);
                        possibleMoves.Add(Position + 4);
                        break;

                    }
                }

            }
            else if (moveOption == MovementOptions.Vertical)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {
                        possibleMoves.Add(Position - 1);
                        possibleMoves.Add(Position + 1);

                        if (Position % 4 == 0)
                        {
                            possibleMoves.RemoveAt(0);
                        }
                        else if (Position % 4 == 3)
                        {
                            possibleMoves.RemoveAt(1);
                        }
                        break;
                    }
                }
            }

            else if (moveOption == MovementOptions.Ver_Hor)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {
                        possibleMoves.Add(Position - 4);
                        possibleMoves.Add(Position + 4);
                        possibleMoves.Add(Position - 1);
                        possibleMoves.Add(Position + 1);

                        if (Position % 4 == 0)
                        {
                            possibleMoves.RemoveAt(2);
                        }
                        else if (Position % 4 == 3)
                        {
                            possibleMoves.RemoveAt(3);
                        }
                        break;
                    }
                }
            }

            else if (moveOption == MovementOptions.Diagonally)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {

                        possibleMoves.Add(Position - 3);
                        possibleMoves.Add(Position + 3);
                        possibleMoves.Add(Position - 5);
                        possibleMoves.Add(Position + 5);

                        if (Position % 4 == 0)
                        {
                            possibleMoves.RemoveAt(1);
                            possibleMoves.RemoveAt(1);
                        }
                        else if (Position % 4 == 3)
                        {
                            possibleMoves.RemoveAt(0);
                            possibleMoves.RemoveAt(2);

                        }
                        break;
                    }
                }
            }
            else if (moveOption == MovementOptions.All_Ways)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {
                        possibleMoves.Add(Position - 4);
                        possibleMoves.Add(Position + 4);
                        possibleMoves.Add(Position - 1);
                        possibleMoves.Add(Position + 1);
                        possibleMoves.Add(Position - 3);
                        possibleMoves.Add(Position + 3);
                        possibleMoves.Add(Position - 5);
                        possibleMoves.Add(Position + 5);

                        // jezeli karta znajduje sie w dolnym rzedzie (0,4,8,12,16,20)
                        if (Position % 4 == 0)
                        {
                            // usuwamy -1, +3, -5 
                            //Poni¿ej dwa razy wystepuje indeks 4 bo przez usuniêcie wczesniejszych obiektów pozostale zostaj¹ przesuniete
                            possibleMoves.RemoveAt(2);
                            possibleMoves.RemoveAt(4);
                            possibleMoves.RemoveAt(4);
                        }
                        else if (Position % 4 == 3)
                        {
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(5);
                        }
                        break;
                    }
                }
            }
        }

        else if (Range == 2)
        {
            if (moveOption == MovementOptions.Horizontal)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {
                        possibleMoves.Add(Position - 4);
                        possibleMoves.Add(Position + 4);
                        possibleMoves.Add(Position - 8);
                        possibleMoves.Add(Position + 8);
                        break;

                    }
                }

            }

            else if (moveOption == MovementOptions.Vertical)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {
                        possibleMoves.Add(Position - 1);
                        possibleMoves.Add(Position + 1);
                        possibleMoves.Add(Position - 2);
                        possibleMoves.Add(Position + 2);

                        if (Position % 4 == 0)
                        {
                            possibleMoves.RemoveAt(0);
                            possibleMoves.RemoveAt(1);
                        }
                        else if (Position % 4 == 3)
                        {
                            possibleMoves.RemoveAt(1);
                            possibleMoves.RemoveAt(2);
                        }
                        else if (Position % 4 == 1)
                        {
                            possibleMoves.RemoveAt(2);
                        }
                        else if (Position % 4 == 2)
                        {
                            possibleMoves.RemoveAt(3);
                        }
                        break;
                    }
                }
            }

            else if (moveOption == MovementOptions.Ver_Hor)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {
                        possibleMoves.Add(Position - 4);
                        possibleMoves.Add(Position + 4);
                        possibleMoves.Add(Position - 1);
                        possibleMoves.Add(Position + 1);
                        possibleMoves.Add(Position - 8);
                        possibleMoves.Add(Position + 8);
                        possibleMoves.Add(Position - 2);
                        possibleMoves.Add(Position + 2);

                        if (Position % 4 == 0)
                        {
                            possibleMoves.RemoveAt(2);
                            possibleMoves.RemoveAt(5);
                        }
                        else if (Position % 4 == 3)
                        {
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(6);
                        }
                        else if (Position % 4 == 1)
                        {
                            possibleMoves.RemoveAt(6);
                        }
                        else if (Position % 4 == 2)
                        {
                            possibleMoves.RemoveAt(7);
                        }
                        break;
                    }
                }
            }

            else if (moveOption == MovementOptions.Diagonally)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {

                        possibleMoves.Add(Position - 3);
                        possibleMoves.Add(Position + 3);
                        possibleMoves.Add(Position - 5);
                        possibleMoves.Add(Position + 5);
                        possibleMoves.Add(Position - 6); //4
                        possibleMoves.Add(Position + 6);
                        possibleMoves.Add(Position - 10);
                        possibleMoves.Add(Position + 10);

                        if (Position % 4 == 0)
                        {
                            possibleMoves.RemoveAt(1);
                            possibleMoves.RemoveAt(1);
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(3);
                        }
                        else if (Position % 4 == 3)
                        {
                            possibleMoves.RemoveAt(0);
                            possibleMoves.RemoveAt(2);
                            possibleMoves.RemoveAt(2);
                            possibleMoves.RemoveAt(4);
                        }
                        else if (Position % 4 == 1)
                        {
                            // -10 i +6
                            possibleMoves.RemoveAt(5);
                            possibleMoves.RemoveAt(5);
                        }
                        else if (Position % 4 == 2)
                        {
                            // + 10 i - 6

                            possibleMoves.RemoveAt(4);
                            possibleMoves.RemoveAt(6);
                        }
                        break;
                    }
                }
            }
            else if (moveOption == MovementOptions.All_Ways)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {
                        possibleMoves.Add(Position - 4);
                        possibleMoves.Add(Position + 4);
                        possibleMoves.Add(Position - 1);
                        possibleMoves.Add(Position + 1);
                        possibleMoves.Add(Position - 3);
                        possibleMoves.Add(Position + 3);
                        possibleMoves.Add(Position - 5);
                        possibleMoves.Add(Position + 5);

                        // jezeli karta znajduje sie w dolnym rzedzie (0,4,8,12,16,20)
                        if (Position % 4 == 0)
                        {
                            // usuwamy -1, +3, -5 
                            //Ponizej dwa razy wystepuje indeks 4 bo przez usuniecie wczesniejszych obiektow pozostale zostaja przesuniete
                            possibleMoves.RemoveAt(2);
                            possibleMoves.RemoveAt(4);
                            possibleMoves.RemoveAt(4);
                        }
                        else if (Position % 4 == 3)
                        {
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(5);
                        }
                        break;
                    }
                }
            }
        }

        else if (Range == 3)
        {
            if (moveOption == MovementOptions.Horizontal)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {
                        possibleMoves.Add(Position - 4);
                        possibleMoves.Add(Position + 4);
                        possibleMoves.Add(Position - 8);
                        possibleMoves.Add(Position + 8);
                        possibleMoves.Add(Position - 12);
                        possibleMoves.Add(Position + 12);
                        break;

                    }
                }

            }

            else if (moveOption == MovementOptions.Vertical)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {
                        possibleMoves.Add(Position - 1);
                        possibleMoves.Add(Position + 1);
                        possibleMoves.Add(Position - 2);
                        possibleMoves.Add(Position + 2);
                        possibleMoves.Add(Position - 3);
                        possibleMoves.Add(Position + 3);

                        if (Position % 4 == 0)
                        {
                            possibleMoves.RemoveAt(0);
                            possibleMoves.RemoveAt(1);
                            possibleMoves.RemoveAt(2);
                        }
                        else if (Position % 4 == 3)
                        {
                            possibleMoves.RemoveAt(1);
                            possibleMoves.RemoveAt(2);
                            possibleMoves.RemoveAt(3);
                        }
                        else if (Position % 4 == 1)
                        {
                            possibleMoves.RemoveAt(2);
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(3);
                        }
                        else if (Position % 4 == 2)
                        {
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(3);
                        }
                        break;
                    }
                }
            }

            else if (moveOption == MovementOptions.Ver_Hor)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {
                        possibleMoves.Add(Position - 4);
                        possibleMoves.Add(Position + 4);
                        possibleMoves.Add(Position - 1);
                        possibleMoves.Add(Position + 1);
                        possibleMoves.Add(Position - 8);
                        possibleMoves.Add(Position + 8);
                        possibleMoves.Add(Position - 2);
                        possibleMoves.Add(Position + 2);

                        if (Position % 4 == 0)
                        {
                            possibleMoves.RemoveAt(2);
                            possibleMoves.RemoveAt(5);
                        }
                        else if (Position % 4 == 3)
                        {
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(6);
                        }
                        else if (Position % 4 == 1)
                        {
                            possibleMoves.RemoveAt(6);
                        }
                        else if (Position % 4 == 2)
                        {
                            possibleMoves.RemoveAt(7);
                        }
                        break;
                    }
                }
            }

            else if (moveOption == MovementOptions.Diagonally)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {

                        possibleMoves.Add(Position - 3);
                        possibleMoves.Add(Position + 3);
                        possibleMoves.Add(Position - 5);
                        possibleMoves.Add(Position + 5);
                        possibleMoves.Add(Position - 6); //4
                        possibleMoves.Add(Position + 6);
                        possibleMoves.Add(Position - 10);
                        possibleMoves.Add(Position + 10);
                        possibleMoves.Add(Position - 15); //8
                        possibleMoves.Add(Position + 15);
                        possibleMoves.Add(Position - 9);
                        possibleMoves.Add(Position + 9);

                        if (Position % 4 == 0)
                        {
                            possibleMoves.RemoveAt(1);
                            possibleMoves.RemoveAt(1);
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(4);
                            possibleMoves.RemoveAt(5);
                        }
                        else if (Position % 4 == 3)
                        {
                            possibleMoves.RemoveAt(0);
                            possibleMoves.RemoveAt(2);
                            possibleMoves.RemoveAt(2);
                            possibleMoves.RemoveAt(4);
                            possibleMoves.RemoveAt(5);
                            possibleMoves.RemoveAt(5);
                        }
                        else if (Position % 4 == 1)
                        {
                            // -10 i +6 -9 + 9 -15 +15
                            possibleMoves.RemoveAt(5);
                            possibleMoves.RemoveAt(5);
                            possibleMoves.RemoveAt(6);
                            possibleMoves.RemoveAt(6);
                            possibleMoves.RemoveAt(6);
                            possibleMoves.RemoveAt(6);
                        }
                        else if (Position % 4 == 2)
                        {
                            // + 10 i - 6

                            possibleMoves.RemoveAt(4);
                            possibleMoves.RemoveAt(6);
                            possibleMoves.RemoveAt(6);
                            possibleMoves.RemoveAt(6);
                            possibleMoves.RemoveAt(6);
                            possibleMoves.RemoveAt(6);
                        }
                        break;
                    }
                }
            }

            else if (moveOption == MovementOptions.All_Ways)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == Position)
                    {
                        possibleMoves.Add(Position - 4);
                        possibleMoves.Add(Position + 4);
                        possibleMoves.Add(Position - 1);
                        possibleMoves.Add(Position + 1);
                        possibleMoves.Add(Position - 3);
                        possibleMoves.Add(Position + 3);
                        possibleMoves.Add(Position - 5);
                        possibleMoves.Add(Position + 5);

                        // jezeli karta znajduje sie w dolnym rzedzie (0,4,8,12,16,20)
                        if (Position % 4 == 0)
                        {
                            // usuwamy -1, +3, -5 
                            //Ponizej dwa razy wystepuje indeks 4 bo przez usuniecie wczesniejszych obiektow pozostale zostaja przesuniete
                            possibleMoves.RemoveAt(2);
                            possibleMoves.RemoveAt(4);
                            possibleMoves.RemoveAt(4);
                        }
                        else if (Position % 4 == 3)
                        {
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(3);
                            possibleMoves.RemoveAt(5);
                        }
                        break;
                    }
                }
            }
        }

        for (int j = 0; j < possibleMoves.Count; j++)
        {
            if (possibleMoves[j] >= 0 && possibleMoves[j] < 24)
            {
                Count++;
            }
            else
            {
                possibleMoves[j] = -1;
            }
        }
        for (int h = 0; h < possibleMoves.Count; h++)
        {
            if (possibleMoves[h] == -1)
            {
                possibleMoves.RemoveAt(h);
            }
        }

        allowedMoves.Capacity = Count;
        for (int k = 0; k < Count; k++)
        {
            if (possibleMoves[k] >= 0 && possibleMoves[k] < 24)
            {
                allowedMoves.Add(possibleMoves[k]);
            }
        }
        return allowedMoves;


    }
}
