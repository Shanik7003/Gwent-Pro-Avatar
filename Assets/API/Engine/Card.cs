using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;
namespace Engine
{
    public enum CardType
    {
        UnitCard = 0,
        WheatherCard = 1,
        IncreaseCard = 2,
        ClearanceCard = 3

    }
    public class Card
    {
        public string name{get;private set;}
        public string description{get;private set;}
        public Position position{get; set;}
        public double points{get; set;}
        public Guid Id {get; private set; }
        public Player player{get;set;}
        public Habilities hability{get; private set;}
        public CardType CardType{get;private set;}
        public Faction faction { get; private set; } 
        public Card(){}

        //Constructor para las cartas creadas por el usuario
        public Card(CardType type,string name, Faction faction, int points, Position position,Guid id)
        {
            CardType = type;
            this.name = name;
            this.faction = faction;
            this.points = points;
            this.position = position;
            Id = id;
            hability = Habilities.Personalized;
        }
        // Constructor sin puntos, sin posición, con habilidad y con tipo
        public Card(string name, string description, Player player, Habilities hability, Guid id, CardType cardType, Faction faction)
        {
            this.name = name;
            this.description = description;
            this.position = Position.MRS;
            this.points = 0;
            this.player = player;
            this.hability = hability;
            this.Id = id;
            this.CardType = cardType;
            this.faction = faction; // Asignar la nueva propiedad
        }

        // Constructor con puntos, con habilidad y tipo, sin posición (por defecto MRS) para construir las cartas especiales de aumento
        public Card(string name, string description, int points, Player player, Habilities hability, Guid id, CardType cardType, Faction faction)
        {
            this.name = name;
            this.description = description;
            this.position = Position.MRS;
            this.points = points;
            this.player = player;
            this.hability = hability;
            this.Id = id;
            this.CardType = cardType;
            this.faction = faction; // Asignar la nueva propiedad
        }

        // Constructor con habilidad y sin tipo (por defecto es UnitCard) para construir las cartas que tienen habilidades pero son normales
        public Card(string name, string description, Position position, int points, Player player, Habilities hability, Guid id, Faction faction)
        {
            this.name = name;
            this.description = description;
            this.position = position;
            this.points = points;
            this.player = player;
            this.hability = hability;
            this.Id = id;
            this.CardType = CardType.UnitCard;
            this.faction = faction; // Asignar la nueva propiedad
        }

        // Constructor sin habilidad (por defecto es None) y sin tipo (por defecto es UnitCard) para construir las cartas normales
        public Card(string name, string description, Position position, int points, Player player, Guid id, Faction faction)
        {
            this.name = name;
            this.description = description;
            this.position = position;
            this.points = points;
            this.player = player;
            this.hability = Habilities.None;
            this.Id = id;
            this.CardType = CardType.UnitCard;
            this.faction = faction; // Asignar la nueva propiedad
        }


        #region Habilidades de las Cartas Especiales
        public void Eclipse (Card card, int row, Player enemy)//para las cartas de clima 
        {
            System.Console.WriteLine("Entre a Eclipse del Engine");
            Game.GameInstance.WheatherSpace.Spaces[row] = card;//añade la carta a lso espacios de clima del engine
            foreach (var item in card.player.Board.rows[row])
            {
                if (item.CardType == CardType.IncreaseCard)
                {
                    continue;
                }
                double discount = item.points/2;
                item.points = item.points/2;//reduce sus puntos a la mitad 
                card.player.Points -= discount;//quitale la misma cantidad de puntos que le qutaste a la carta al jugador  
            }
            foreach (var item in enemy.Board.rows[row])//aplica tambien el clima en la fila correspondiente del otro jugador 
            {
                if (item.CardType == CardType.IncreaseCard)
                {
                    continue;
                }
                double discount = item.points/2;
                item.points = item.points/2;//reduce sus puntos a la mitad 
                enemy.Points -= discount;//quitale la misma cantidad de puntos que le qutaste a la carta al jugador 
            }

        }
        public  void Clearence(Card card,int row,Player enemy)//habilidad para las cartas de despeje
        {
            if(Game.GameInstance.WheatherSpace.Spaces[row] != null)//si existe una carta de clima afectando esa fila : despeja lo que su habilidad estaba ocasionando 
            {
                double count = 0;
                foreach (var item in card.player.Board.rows[row])
                {
                    if (item.CardType == CardType.IncreaseCard)
                    {
                        continue;
                    }
                    item.points += item.points;//duplica los puntos para eliminar el efecto del eclipse 
                    count += item.points/2;//para ponerle al jugador los puntos que se esta recuperando 
                }
                card.player.Points += count;//actualiza lo spuntos del juagdor 
                count = 0;//actualizo count para el jugador 2
                foreach (var item in enemy.Board.rows[row])
                {
                    if (item.CardType == CardType.IncreaseCard)
                    {
                        continue;
                    }
                    item.points += item.points;
                    count += item.points/2;//para ponerle al jugador los puntos que se esta recuperando 
                }
                enemy.Points += count;//actualiza lo spuntos del juagdor 
                Game.GameInstance.WheatherSpace.Spaces[row] = null; //quitar la carta de clima 
            }
        }
        #endregion
    
        #region Habilidades de las Unidades
        public static Card CardTheft(Player player) //robar una carta 
        {
            System.Random random = new System.Random();
            Card stolenCard = player.Deck[random.Next(1,player.Deck.Count)];
            player.Hand.Add(stolenCard);
            player.Deck.Remove(stolenCard);
            return stolenCard;
        }
        public  void IncreaseMyRow(Card card, int row)
        {
            int count = 0; //para saber cuantas cartas hay en esa fila 
            foreach (var item in card.player.Board.rows[row])
            {
                if (item.Id != card.Id)//para que no pueda contar la propia carta de aumento
                {
                    item.points += card.points;//sumale 5 ptos a cada carta
                    count ++;
                }
            
            }
            card.player.Points += count * card.points; //+5 por cada carta 
        }
        public static Guid EliminateMostPowerful(Player enemy)
        {
            double maxPoints = 0;
            Guid mostPowerfulID = new();
            for (int i = 0; i < enemy.Board.rows.Length ; i++)
            {
                foreach (var item in enemy.Board.rows[i])//este ciclo identifica la mayor catidad de puntos que hay en el tablero, pero no la carta 
                {
                    if (item.CardType == CardType.IncreaseCard)
                    {
                        continue;
                    }
                    if (item.points > maxPoints)//si la carta por la que vas iterando es mayor que la q tienes 
                    {
                        maxPoints = item.points;
                        mostPowerfulID = item.Id;
                    }
                }
            }
        
            for (int i = 0; i < enemy.Board.rows.Length; i++)
            {
                foreach (var item in enemy.Board.rows[i])
                {
                    if(item.Id == mostPowerfulID)
                    {
                        enemy.Board.RemoveCard(enemy.Board.rows[i],item);
                        //enemy.Board.rows[i].Remove(item);
                        enemy.Points -= item.points;//reduce los puntos del jugador 
                        enemy.Graveyard.Add(item);
                        mostPowerfulID = item.Id;
                        break;
                    }
                }
            }
            return mostPowerfulID;//en la interfaz anejar el caso en el que mostPowerfulID sea cero que sognifica que no habia ninguna carta en al campo del rival y entonces no se hace nada visualmente 
        }
        public static Guid  EliminateLeastPowerful(Player enemy)
        {
            double minPoints = Int32.MaxValue;
            Guid lessPowerfulID = new();
            for (int i = 0; i < enemy.Board.rows.Length ; i++)
            {
                foreach (var item in enemy.Board.rows[i])//este ciclo identifica la mayor catidad de puntos que hay en el tablero, pero no la carta 
                {
                    if (item.CardType == CardType.IncreaseCard)
                    {
                        continue;
                    }
                    if (item.points < minPoints)//si la carta por la que vas iterando es mayor que la q tienes 
                    {
                        minPoints = item.points;
                    }
                }
            }
            for (int i = 0; i < enemy.Board.rows.Length; i++)
            {
                foreach (var item in enemy.Board.rows[i])
                {
                
                    if(item.points == minPoints)
                    {
                        enemy.Board.RemoveCard(enemy.Board.rows[i],item);
                        enemy.Graveyard.Add(item);
                        enemy.Points -= item.points;//reduce los puntos del jugador 
                        lessPowerfulID = item.Id;
                        break;
                    }
                }
            }
            return lessPowerfulID;
        
        }
        public static double Multipoints(Card card)
        {
            int sameCard = 0;
            for (int i = 0; i < card.player.Board.rows.Length; i++)
                {
                    foreach (var item in card.player.Board.rows[i])
                    {
                    if (item.CardType == CardType.IncreaseCard)
                    {
                        continue;
                    }
                        if (item.name == card.name)
                        {
                            sameCard += 1;
                        }
                    }
                }
                card.player.Points = card.player.Points - card.points;//restale al jugador los puntos originales que tenia la carta 
                card.points *= sameCard;//multiplica los puntos de card por la cantidad de cartas iguales a ella en el tablero
                card.player.Points += card.points; //sumale los puntos que en realidad la carta va a aportar despues de ejecutada su habilidad
                return card.points;
        }

        public static  (Position,Player) CleanRow(Card card,Player enemy)//ademas de limpiar la fila que menos cartas tenga, devuelve el Row para que la UI trabaje con eso 
        {
            Player ChoosePlayer = card.player;
            int count = 0;
            double aux = 0;
            double discountPointsPlayer = 0;//puntos que le tengo que quitar al jugador 
            double discountPointsEnemy = 0;
            int MinCardsPerRow = int.MaxValue; //catntidad de cartas en esa fila 
            int rowPlayer = -1 ;
            int rowEnemy = -1 ;
            Card IncreaseCardPlayer = new();
            Card IncreaseCardEnemy = new();
        
            for (int i = 0; i < card.player.Board.rows.Length; i++)
            {
                foreach (var item in card.player.Board.rows[i])//cuenta la cantidad de cartas en el board del juagador 
                {
                    if (item.CardType == CardType.IncreaseCard)
                    {
                        IncreaseCardPlayer = item;
                        continue;
                    }
                    count ++;
                    aux += item.points;
                }
            
                if (MinCardsPerRow > count && count != 0)
                {
                    MinCardsPerRow = count; //se queda con la fila que menor cantidad de cartas tenga 
                    rowPlayer = i;
                    ChoosePlayer = card.player;
                    discountPointsPlayer = aux;
                    aux = 0;
                }
                count = 0;                    
            }  
        
            for (int i = 0; i < enemy.Board.rows.Length; i++)
            {
                foreach (var item in enemy.Board.rows[i])//cuenta la cantidad de cartas en el board del juagador 
                {
                    if (item.CardType == CardType.IncreaseCard)
                    {
                        IncreaseCardEnemy = item;
                        continue;
                    }
                    count ++;
                    aux += item.points;
                }
            
                if (MinCardsPerRow > count && count != 0)
                {
                    MinCardsPerRow = count; //se queda con la fila que menor cantidad de cartas tenga 
                    rowEnemy = i;
                    ChoosePlayer = enemy;
                    discountPointsEnemy = aux;
                    aux = 0;
                } 
                count = 0;                  
            } 
            if (ChoosePlayer == card.player)
            {
                card.player.Board.rows[rowPlayer] = new List<Card>();//vacia la lista de cartas 
                card.player.Board.rows[rowPlayer].Add(IncreaseCardPlayer);
                card.player.Points -= discountPointsPlayer;
                return ((Position)rowPlayer,card.player);
            }
            else
            {
                enemy.Board.rows[rowEnemy] = new List<Card>();//vacia la lista de cartas 
                enemy.Board.rows[rowEnemy].Add(IncreaseCardEnemy);
                enemy.Points -= discountPointsEnemy;
                return ((Position)rowEnemy,enemy);
            }
        } 
        #endregion
    }
}
