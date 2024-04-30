using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.VisualScripting;

namespace Engine
{
    public class Game
    {
        // Única instancia estática privada
        private static Game _instance;
        
        // Propiedad pública estática para acceder a la instancia
        public static Game GameInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Game();  // Crear la nueva instancia si no existe
                }
                return _instance;
            }
        }

        // Propiedades para los jugadores
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public WheatherSpace WheatherSpace;

        // Constructor privado para prevenir instanciación externa
        private Game()
        {
            // Inicializar los jugadores dentro del constructor privado
            Player1 = new Player();
            Player2 = new Player();
            WheatherSpace = new();
        }
        #region WaterTribe 
        public static Card waterLeader = new Card("Katara",  "Roba una carta extra del Mazo",Position.Leaderposition, 0,null,Habilities.CardTheft,1);
        public static Card waterCard2 = new ("Pakku",  "Roba una carta extra del Mazo",Position.M,2,null,Habilities.CardTheft,2);
        public static Card waterCard3 = new ("Hama",  "Roba una carta extra del Mazo",Position.R,3,null,Habilities.CardTheft,3);
        public static Card waterCard4 = new ("Yue",  "Roba una carta extra del Mazo",Position.R,3,null,Habilities.CardTheft,4);
        public static Card waterCard5 = new ("Avatar Kuruk",  "Roba una carta extra del Mazo",Position.R,2,null,Habilities.CardTheft,5);
        public static Card waterCard6 = new ("Hakoda",  "Roba una carta extra del Mazo",Position.R,3,null,Habilities.CardTheft,6);
        public static Card waterCard7 = new ("Korra",  "Roba una carta extra del Mazo",Position.R,3,null,Habilities.CardTheft,7);
        public static Card waterCard8 = new ("Tonraq",  "Roba una carta extra del Mazo",Position.R,3,null,Habilities.CardTheft,8);
        public static Card waterCard9 = new ("Cocky Sokka",  "Roba una carta extra del Mazo",Position.R,3,null,Habilities.CardTheft,9);
        public static Card waterCard10 = new ("Fighter Katara",  "Roba una carta extra del Mazo",Position.R,2,null,Habilities.CardTheft,10);
        public static Card waterCard11 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",2,null,Habilities.IncreaseMyRow,11,CardType.IncreaseCard);
        public static Card waterCard12 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla",Position.R,4,null,Habilities.MultiPoints,12);
        public static Card waterCard13 = new ("Katara",  "Roba una carta extra del Mazo",Position.R,2,null,Habilities.CardTheft,13);
        public static Card waterCard14 = new ("Spirits",  "Roba una carta extra del Mazo",Position.R,2,null,Habilities.CardTheft,14);
        public static Card waterCard15 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",4,null,Habilities.IncreaseMyRow,15,CardType.IncreaseCard);
        public static Card waterCard16 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",4,null,Habilities.IncreaseMyRow,16,CardType.IncreaseCard);
        public static Card waterCard17 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",2,null,Habilities.IncreaseMyRow,17,CardType.IncreaseCard);
        public static Card waterCard18 = new ("Sozin Comet",  " CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",1,null,Habilities.IncreaseMyRow,18,CardType.IncreaseCard);
        public static Card waterCard19 = new ("Katara",  "Roba una carta extra del Mazo", Position.S,1,null,Habilities.CardTheft,19);
        public static Card waterCard20 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,2);
        public static Card waterCard21 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,21);
        public static Card waterCard22 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,22);
        public static Card waterCard23 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,23);
        public static Card waterCard24 = new ("Avatar Kuruk",  "Roba una carta extra del Mazo", Position.S,1,null,Habilities.CardTheft,24);
        public static Card waterCard25 = new ("The Painted Lady",  "Roba una carta extra del Mazo", Position.S,1,null,Habilities.CardTheft,25);
        public static List<Card> WaterDeck = new() { waterCard2, waterCard3, waterCard4, waterCard5, waterCard6, waterCard7, waterCard8, waterCard9, waterCard10, waterCard11, waterCard12, waterCard13, waterCard14, waterCard15, waterCard16, waterCard17, waterCard18, waterCard19, waterCard20, waterCard21, waterCard22, waterCard23, waterCard24, waterCard25};
        public static readonly WaterTribe  WaterTribe = new(WaterDeck,waterLeader);
        #endregion

        #region AirNomads
        public static Card airLeader = new Card ("Aang", "Air Bender",Position.Leaderposition, 0,null,51);
        public static Card airCard2 = new ("Monk Gyatso", "Air Bender",Position.M,2,null,52);
        public static Card airCard3 = new ("Tenzin", "Air Bender",Position.M,2,null,53);
        public static Card airCard4 = new ("Tenzin", "Air Bender",Position.M,2,null,54);
        public static Card airCard5 = new ("Appa", "Air Bender",Position.M,2,null,55);
        public static Card airCard6 = new ("Appa", "Air Bender",Position.M,2,null,56);
        public static Card airCard7 = new ("Appa", "Air Bender",Position.M,2,null,57);
        public static Card airCard8 = new ("Appa", "Air Bender",Position.M,2,null,58);
        public static Card airCard9 = new ("Aang", "Air Bender",Position.M,2,null,59);
        public static Card airCard10 = new ("Aang", "Air Bender",Position.M,2,null,60);
        public static Card airCard11 = new ("Avatar Yangchen", "Air Bender",Position.M,2,null,61);
        public static Card airCard12 = new ("Monk Gyatso", "Air Bender",Position.M,2,null,62);
        public static Card airCard13 = new ("Roku", "Air Bender",Position.M,1,null,63);
        public static Card airCard14 = new ("Kyoshi", "Air Bender",Position.M,1,null,64);
        public static Card airCard15 = new ("Momo", "Air Bender",Position.M,1,null,65);
        public static Card airCard16 = new ("Momo", "Fire Bender",Position.M,1,null,66);
        public static Card airCard17 = new ("Korra", "Water Bender",Position.M,1,null,67);
        public static Card airCard18 = new ("Appa", "Sky Bison",Position.M,1,null,68);
        public static Card airCard19 = new ("Momo", "Flying Lemur",Position.M,1,null,69);
        public static Card airCard20 = new ("Momo", "Messenger Hawk",Position.M,3,null,70);
        public static Card airCard21 = new ("Momo", "Non-bender",Position.M,3,null,71);
        public static Card airCard22 = new ("Tenzin", "Spirit",Position.M,3,null,72);
        public static Card airCard23 = new ("Tenzin", "Spirit",Position.M,3,null,73);
        public static Card airCard24 = new ("Tenzin", "Non-bender",Position.M,3,null,74);
        public static Card airCard25 = new ("Tenzin", "Bear",Position.M,3,null,75);
        public static List<Card> AirDeck = new(){airCard2, airCard3, airCard4, airCard5, airCard6, airCard7, airCard8, airCard9, airCard10, airCard11, airCard12, airCard13, airCard14, airCard15, airCard16, airCard17, airCard18, airCard19, airCard20, airCard21, airCard22, airCard23, airCard24, airCard25};
        public static readonly AirNomads AirNomads = new(AirDeck,airLeader);
        #endregion

        #region FireNation
        public static Card fireLeader = new Card("Zuko", "Ninguna habilidad especial",Position.Leaderposition,0,null,Habilities.None,26);
        public static Card fireCard2 = new ("Azula", "Ninguna habilidad especial",Position.MRS,1,null,Habilities.None,27);
        public static Card fireCard3 = new ("Iroh", "Aumenta los puntos de las unidades de su fila(no pasivamente)",Position.MRS,1,null,Habilities.IncreaseMyRow,28);
        public static Card fireCard4 = new ("Piandao", "Aumenta los puntos de las unidades de su fila(no pasivamente)",Position.MRS,1,null,Habilities.IncreaseMyRow,29);
        public static Card fireCard5 = new ("Azula", "Aumenta los puntos de las unidades de su fila(no pasivamente)",Position.MRS,1,null,Habilities.IncreaseMyRow,30);
        public static Card fireCard6 = new ("Azula", "Aumenta los puntos de las unidades de su fila(no pasivamente)",Position.MRS,1,null,Habilities.IncreaseMyRow,31);
        public static Card fireCard7 = new ("Zhao", "Aumenta los puntos de las unidades de su fila(no pasivamente)",Position.MRS,1,null,Habilities.IncreaseMyRow,32);
        public static Card fireCard8 = new ("Azula", "Ninguna habilidad especial",Position.MRS,1,null,Habilities.None,33);
        public static Card fireCard9 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,34,CardType.WheatherCard);
        public static Card fireCard10 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad ",null,Habilities.Eclipse,35,CardType.WheatherCard);
        public static Card fireCard11 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,36,CardType.WheatherCard);
        public static Card fireCard12 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,37,CardType.WheatherCard);
        public static Card fireCard13 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,38,CardType.WheatherCard);
        public static Card fireCard14 = new ("Roku", "Ninguna habilidad especial",Position.MRS,2,null,Habilities.None,39);
        public static Card fireCard15 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,40,CardType.WheatherCard);
        public static Card fireCard16 = new ("Fire Lord Ozai", "Ninguna habilidad especial",Position.MRS,2,null,Habilities.None,41);
        public static Card fireCard17 = new ("Fire Lord Zuko", "Ninguna habilidad especial",Position.MRS,2,null,Habilities.None,42);
        public static Card fireCard18 = new ("Fire Lord Ozai", "Ninguna habilidad especial",Position.MRS,2,null,Habilities.None,43);
        public static Card fireCard19 = new ("Fire Lord Iroh", "Ninguna habilidad especial",Position.MRS,2,null,Habilities.None,44);
        public static Card fireCard20 = new ("Fire Lord Azula", "Ninguna habilidad especial",Position.MRS,3,null,Habilities.None,45);
        public static Card fireCard21 = new ("Espiritu Azul", "Ninguna habilidad especial",Position.MRS,3,null,Habilities.None,46);
        public static Card fireCard22 = new ("Iroh", "Ninguna habilidad especial",Position.MRS,3,null,Habilities.None,47);
        public static Card fireCard23 = new ("Piandao", "Ninguna habilidad especial",Position.MRS,3,null,Habilities.None,48);
        public static Card fireCard24 = new ("Piandao", "Ninguna habilidad especial",Position.MRS,3,null,Habilities.None,49);
        public static Card fireCard25 = new ("Iroh", "Ninguna habilidad especial",Position.MRS,3,null,Habilities.None,50);
        public static List<Card> FireDeck = new(){fireCard2, fireCard3, fireCard4, fireCard5, fireCard6, fireCard7, fireCard8, fireCard9, fireCard10, fireCard11, fireCard12, fireCard13, fireCard14, fireCard15, fireCard16, fireCard17, fireCard18, fireCard19, fireCard20, fireCard21, fireCard22, fireCard23, fireCard24, fireCard25};
       
        public static readonly FireNation FireNation = new(FireDeck,fireLeader);
        #endregion

        #region Earth Kingdom
        public static Card earthLeader = new Card("Toph", "Elimina la carta con mas poder del campo del rival",Position.Leaderposition,0,null,Habilities.EliminateMostPowerful,77);
        public static Card earthCard2 = new ("Bumi", "Elimina la carta con mas poder del campo del rival",Position.R,2,null,Habilities.EliminateMostPowerful,78);
        public static Card earthCard3 = new ("Lin Beifong", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,79);
        public static Card earthCard4 = new ("Toph", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,80);
        public static Card earthCard5 = new ("Wei", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,81);
        public static Card earthCard6 = new ("Wing", "Elimina la carta con mas poder del campo del rival",Position.R,2,null,Habilities.EliminateMostPowerful,82);
        public static Card earthCard7 = new ("Huan", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,83);
        public static Card earthCard8 = new ("Kuvira", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,84);
        public static Card earthCard9 = new ("Ghazan", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,85);
        public static Card earthCard10 = new ("Ming-Hua", "Elimina la carta con mas poder del campo del rival",Position.R,2,null,Habilities.EliminateMostPowerful,86);
        public static Card earthCard11 = new ("Earth King", "Elimina la carta con menos poder del campo del rival",Position.R,1,null,Habilities.EliminateLeastPowerful,87);
        public static Card earthCard12 = new ("Avatar Kyoshi", "Elimina la carta con menos poder del campo del rival",Position.R,1,null,Habilities.EliminateLeastPowerful,88);
        public static Card earthCard13 = new ("Avatar Kyoshi", "Air Bender",Position.R,1,null,89);
        public static Card earthCard14 = new ("Avatar Kyoshi", "Water Bender",Position.R,1,null,90);
        public static Card earthCard15 = new ("Long Feng", "Elimina la carta con mas poder del campo del rival",Position.R,1,null,Habilities.EliminateMostPowerful,91);
        public static Card earthCard16 = new ("Long Feng", "Elimina la carta con mas poder del campo del rival",Position.R,1,null,Habilities.EliminateMostPowerful,92);
        public static Card earthCard17 = new ("Korra", "Water Bender",Position.R,1,null,93);
        public static Card earthCard18 = new ("Aang", "Elimina la carta con mas poder del campo del rival",Position.R,1,null,Habilities.EliminateMostPowerful,94);
        public static Card earthCard19 = new ("Bumi", "Bear",Position.R,1,null,95);
        public static Card earthCard20 = new ("Bumi", "Elimina la carta con mas poder del campo del rival",Position.R,1,null,Habilities.EliminateMostPowerful,96);
        public static Card earthCard21 = new ("Bumi", "Spirit",Position.R,4,null,97);
        public static Card earthCard22 = new ("Tejones Topo", "Non-bender",Position.R,4,null,98);
        public static Card earthCard23 = new ("Tejones Topo", "Earth Bender",Position.R,4,null,99);
        public static Card earthCard24 = new ("Wei", "Elimina la carta con mas poder del campo del rival",Position.R,4,null,Habilities.EliminateMostPowerful,100);
        public static Card earthCard25 = new ("Wing", "Elimina la carta con mas poder del campo del rival",Position.R,4,null,Habilities.EliminateMostPowerful,101);
        public static List<Card> EarthDeck = new(){earthCard2, earthCard3, earthCard4, earthCard5, earthCard6, earthCard7, earthCard8, earthCard9, earthCard10, earthCard11, earthCard12, earthCard13, earthCard14, earthCard15, earthCard16, earthCard17, earthCard18, earthCard19, earthCard20, earthCard21, earthCard22, earthCard23, earthCard24, earthCard25};
        public static readonly EarthKingdom EarthKingdom = new(EarthDeck,earthLeader);
        #endregion
    }    
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
            public int points{get; set;}
            public Player player{get;set;}
            public Habilities hability{get; private set;}
            public int ID{get;private set;} 
            public CardType CardType{get;private set;}
             //constructor sin puntos sin posisicion con habilidad con tipo 
             public Card(string name,string description,Player player,Habilities hability,int id,CardType cardType)
             {
                this.name = name;
                this.description= description;
                this.position = Position.MRS;
                this.points = 0;
                this.player=player;
                this.hability=hability;
                this.ID = id;
                this.CardType = cardType;
             }
            // constructor con puntos  con habilidad y tipo sin posicion(por defecto MRS)para construir las cartas especiales de aumento
            public Card(string name,string description,int points,Player player,Habilities hability,int id,CardType cardType)
            {
                this.name = name;
                this.description= description;
                this.position = Position.MRS;
                this.points = points;
                this.player=player;
                this.hability=hability;
                this.ID = id;
                this.CardType = cardType;
            }
            //constructor con habilidad y sin tipo (por defecto es UnitCard)para construir las cartas que tienen habilidades pero son normales
            public Card(string name,string description, Position position,int points,Player player,Habilities hability,int id)
            {
                this.name = name;
                this.description= description;
                this.position = position;
                this.points = points;
                this.player=player;
                this.hability=hability;
                this.ID = id;
                this.CardType = CardType.UnitCard;
            }
            //constructor sin habilidad (por defecto es None) sin tipo (por defecto es UnitCard)para construir las cartas normales
            public Card(string name,string description, Position position,int points,Player player,int id)
            {
                this.name = name;
                this.description= description;
                this.position = position;
                this.points = points;
                this.player=player;
                this.hability = Habilities.None;
                this.ID = id;
                this.CardType = CardType.UnitCard;

            }
            #region Habilidades de las Cartas Especiales
            public  void Eclipse (Card card, int row, Player enemy)//para las cartas de clima 
            {
                System.Console.WriteLine("Entre a Eclipse del Engine");
                foreach (var item in card.player.Board.rows[row])
                {
                    item.points = item.points/2;//reduce sus puntos a la mitad 
                    card.player.Points -= item.points/2;//quitale la misma cantidad de puntos que le qutaste a la carta al jugador  
                }
                foreach (var item in enemy.Board.rows[row])//aplica tambien el clima en la fila correspondiente del otro jugador 
                {
                    item.points = item.points/2;//reduce sus puntos a la mitad 
                    enemy.Points -= item.points/2;//quitale la misma cantidad de puntos que le qutaste a la carta al jugador 
                } 
            }
            #endregion
          
            #region Habilidades de las Unidades
            public static Card CardTheft(Player player) //robar una carta 
            {
                Random random = new Random();
                Card stolenCard = player.Faction.Deck[random.Next(1,player.Faction.Deck.Count)];
                player.Hand.Add(stolenCard);
                player.Faction.Deck.Remove(stolenCard);
                return stolenCard;
            }
            public  void IncreaseMyRow(Card card, int row)
            {
                int count = 0; //para saber cuantas cartas hay en esa fila 
                foreach (var item in card.player.Board.rows[row])
                {
                    if (item.ID != card.ID)//para que no pueda contar la propia carta de aumento
                    {
                        item.points += card.points;//sumale 5 ptos a cada carta
                        count ++;
                    }
                   
                }
                card.player.Points += count * card.points; //+5 por cada carta 
            }
            public static int EliminateMostPowerful(Player enemy)
            {
                int maxPoints = 0;
                int mostPowerfulID = 0;
                for (int i = 0; i < enemy.Board.rows.Length ; i++)
                {
                    foreach (var item in enemy.Board.rows[i])//este ciclo identifica la mayor catidad de puntos que hay en el tablero, pero no la carta 
                    {
                        if (item.points > maxPoints)//si la carta por la que vas iterando es mayor que la q tienes 
                        {
                            maxPoints = item.points;
                        }
                    }
                }
                for (int i = 0; i < enemy.Board.rows.Length; i++)
                {
                    foreach (var item in enemy.Board.rows[i])
                    {
                        if(item.points == maxPoints)
                        {
                            enemy.Board.rows[i].Remove(item);
                            enemy.Board.cemetery.Add(item);
                            mostPowerfulID = item.ID;
                            break;
                        }
                    }
                }
                return mostPowerfulID;//en la interfaz anejar el caso en el que mostPowerfulID sea cero que sognifica que no habia ninguna carta en al campo del rival y entonces no se hace nada visualmente 
            }
            public static int  EliminateLeastPowerful(Player enemy)
            {
                int minPoints = Int32.MaxValue;
                int lessPowerfulID = 0;
                for (int i = 0; i < enemy.Board.rows.Length ; i++)
                {
                    foreach (var item in enemy.Board.rows[i])//este ciclo identifica la mayor catidad de puntos que hay en el tablero, pero no la carta 
                    {
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
                            enemy.Board.rows[i].Remove(item);
                            enemy.Board.cemetery.Add(item);
                            lessPowerfulID = item.ID;
                            break;
                        }
                    }
                }
                return lessPowerfulID;
               
            }
            public static int Multipoints(Card card)
            {
                int sameCard = 0;
                for (int i = 0; i < card.player.Board.rows.Length; i++)
                    {
                        foreach (var item in card.player.Board.rows[i])
                        {
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

            // public static void CleanRow(Card card)
            // {
            //     int count = 0;
            //     int cardsAmountPerRow = int.MaxValue; //catntidad de cartas en esa fila 
            //     int row = -1 ;
            //     for (int i = 0; i < card.enemy.Board.board.GetLength(0); i++)
            //     {
            //         for (int j = 0; j < card.enemy.Board.board.GetLength(1); j++)
            //         {
            //             if (card.enemy.Board.board[i,j] != null)
            //             {
            //                 count += 1;
            //             }
            //         }
            //         if (cardsAmountPerRow > count && count != 0)
            //         {
                    
            //             cardsAmountPerRow = count; //se queda con la fila que menor cantidad de cartas tenga 
            //             row = i;
            //         }
            //         count = 0;
            //     }  
            //     if (cardsAmountPerRow != int.MaxValue)//si hay alguna fila que no este vacia 
            //     {
            //         //en la fila que menos cartas hay, elimina todas las cartas 
            //         for (int i = 0; i < card.enemy.Board.board.GetLength(1); i++)
            //         {
            //             if (card.enemy.Board.board[row,i] != null)
            //             {
            //                 Game.EliminateCard(card.enemy.Board.board[row,i]);
            //             }
            //         }        
            //     }
                
            // } 
            #endregion
            
        }
   
        public class Player
        {
            public string Name;
            public int Points;
            public Faction Faction;
            public List<Card> Graveyard;
            public Board Board;
            public List<Card> Hand{get;set;}
            public int Id;
            public List<int> Gems;
            public bool AlreadyPass;
            public Player(string name,int points, Faction faction, List<Card> graveyard,List<Card>hand)//constructor antiguo
            {
                Name = name;
                Points = points;
                Faction = faction;
                Graveyard = graveyard;
                Hand = hand;
                Gems = new();
            }
            public Player()//nuevo constructor para poder inicializar los players sin tener todos sus datos
            {
                Random random = new();
                Name = "DefaultName";
                Points = 0;
                Graveyard = new List<Card>();
                Hand = new List<Card>();
                Gems = new();
                Board = new Board();
                Id = random.Next(0, 1000);
            }
            public List<Card> GetHand()
            {
                Random random = new();
                for (int i = 0; i < 10; i++)
                {
                    Card card = Faction.Deck[random.Next(0,14)];
                    Hand.Add(card);
                    Faction.Deck.Remove(card);
                }
                return Hand;
            }
        }
        public class Faction
        {
            public Card leader;
            public List<Card> Deck = new();
            public Faction(List<Card> Deck, Card leader)
            {
                for (int i = 0; i < Deck.Count; i++)
                {
                this.Deck.Add(Deck[i]);   
                } 
                this.leader = leader;
            }
        }
        public class Board
        {
            public List<Card>[] rows = new List<Card>[3];
            public List<Card> cemetery = new();
            public Board()
            {
                rows = new List<Card>[3];  // Asume 3 filas, ajusta según sea necesario
                for (int i = 0; i < rows.Length; i++)
                {
                    rows[i] = new List<Card>();
                }
            }
            public void ResetEngineBoard()
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    rows[i] = new List<Card>();
                }
            }
        }  
        public class WheatherSpace
        {
            public Card[] Spaces = new Card[3];
            public WheatherSpace(){}
        }
        public enum Position
        {
            M = 0,//cuerpo a cuerpo
            R = 1,//distancia
            S = 2,//asedio
            MR = 3,
            MS = 4,
            RS = 5,
            MRS = 6,
            Leaderposition = 7,
        }
        public enum Habilities
        {
            #region 
            IncreaseMyRow = 0,
            EliminateMostPowerful = 1,
            EliminateLeastPowerful = 2,
            MultiPoints = 3,
            CleanRow = 4,
            WheatherSet = 5,
            CardTheft = 6, //robar una carta del deck y agregarla a tu mano 
            None = 7, 
            #endregion
            #region SpecialCard
            CommanderHorn = 8,//(aumento)duplica la fuerza de todas las cartas de la fila en la q se coloque
            Lure = 9,//(señuelo)
            Eclipse = 10,//(clima)Cambia la fuerza de todas las cartas de combate cuerpo a cuerpo de ambos jugadores a 1.
            Fog = 11,//(clima)Cambia la fuerza de todas las cartas de combate a distancia de ambos jugadores a 1.
            Rain = 12,//(clima)Cambia la fuerza de todas las cartas de combate de asedio de ambos jugadores a 1.
            ClearWheather = 13,// (despeje)Descarta todas las cartas de clima que haya en el campo de batalla y anula sus efectos.    
            #endregion
            #region Leader
        ExtraCardFirstRound = 14,//robar una carta extra al inicio de la segunda ronda
        ExtraCardSecondRound = 15,//robar una carta extra al inicio de la segunda ronda
        TieWon = 16,//empate ganado
        StayBetweenRounds = 17, // mantener una carta aleatoria entre rondas   
        #endregion 

        }
    public class FireNation : Faction
    {
       public FireNation (List<Card> FireDeck, Card leader) : base (FireDeck, leader) {}
    }
    public class WaterTribe : Faction
    {
        public WaterTribe (List<Card> WaterDeck,Card leader) : base (WaterDeck,leader){}   
    }
    public class AirNomads : Faction
    {
        public AirNomads (List<Card> AirDeck,Card leader) : base(AirDeck,leader){}
    }
    public class EarthKingdom : Faction
    {
        public EarthKingdom(List<Card> EarthDeck, Card leader): base (EarthDeck,leader){}
    }
    }

