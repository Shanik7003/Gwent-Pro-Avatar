using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

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

        // Constructor privado para prevenir instanciación externa
        private Game()
        {
            // Inicializar los jugadores dentro del constructor privado
            Player1 = new Player();
            Player2 = new Player();
        }
        #region WaterTribe 
        public static Card waterLeader = new Card("Katara",  "Water Bender",Position.Leaderposition, 0,null,Habilities.CardTheft);
        public static Card waterCard2 = new ("Pakku",  "Water Bender",Position.M,2,null,Habilities.CardTheft);
        public static Card waterCard3 = new ("Hama",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard4 = new ("Yue",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard5 = new ("Kya",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard6 = new ("Ming-Hua",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard7 = new ("Korra",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard8 = new ("Tonraq",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard9 = new ("Eska",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard10 = new ("Desna",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard11 = new ("Unalaq",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard12 = new ("Tarrlok",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard13 = new ("Hu",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard14 = new ("Thod",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard15 = new ("Vachir",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard16 = new ("Shirshu",  "Water Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard17 = new ("Admiral Zhao",  "Fire Bender",Position.R,20,null,Habilities.CardTheft);
        public static Card waterCard18 = new ("June",   "Non-bender", Position.S,57,null,Habilities.CardTheft);
        public static Card waterCard19 = new ("Song",  "Non-bender", Position.S,57,null,Habilities.CardTheft);
        public static Card waterCard20 = new ("Bato",  "Non-bender", Position.S,57,null,Habilities.CardTheft);
        public static Card waterCard21 = new ("Pipsqueak",  "Non-bender", Position.S,57,null,Habilities.CardTheft);
        public static Card waterCard22 = new ("The Duke",  "Non-bender", Position.S,57,null,Habilities.CardTheft);
        public static Card waterCard23 = new ("Tyro",  "Non-bender", Position.S,57,null,Habilities.CardTheft);
        public static Card waterCard24 = new ("The Painted Lady",  "Non-bender", Position.S,57,null,Habilities.CardTheft);
        public static Card waterCard25 = new ("Tui and La",  "Spirit", Position.S,57,null,Habilities.CardTheft);
        public static List<Card> WaterDeck = new() { waterCard2, waterCard3, waterCard4, waterCard5, waterCard6, waterCard7, waterCard8, waterCard9, waterCard10, waterCard11, waterCard12, waterCard13, waterCard14, waterCard15, waterCard16, waterCard17, waterCard18, waterCard19, waterCard20, waterCard21, waterCard22, waterCard23, waterCard24, waterCard25};
        public static readonly WaterTribe  WaterTribe = new(WaterDeck,waterLeader);
        #endregion

        #region AirNomads
        public static Card airLeader = new Card ("Aang", "Air Bender",Position.Leaderposition, 0,null);
        public static Card airCard2 = new ("Gyatso", "Air Bender",Position.M,35,null);
        public static Card airCard3 = new ("Tenzin", "Air Bender",Position.M,35,null);
        public static Card airCard4 = new ("Jinora", "Air Bender",Position.M,35,null);
        public static Card airCard5 = new ("Ikki", "Air Bender",Position.M,35,null);
        public static Card airCard6 = new ("Meelo", "Air Bender",Position.M,35,null);
        public static Card airCard7 = new ("Opal", "Air Bender",Position.M,35,null);
        public static Card airCard8 = new ("Kai", "Air Bender",Position.M,35,null);
        public static Card airCard9 = new ("Rohan", "Air Bender",Position.M,35,null);
        public static Card airCard10 = new ("Tashi", "Air Bender",Position.M,35,null);
        public static Card airCard11 = new ("Avatar Yangchen", "Air Bender",Position.M,35,null);
        public static Card airCard12 = new ("Yanchen", "Air Bender",Position.M,35,null);
        public static Card airCard13 = new ("Roku", "Air Bender",Position.M,35,null);
        public static Card airCard14 = new ("Kyoshi", "Air Bender",Position.M,35,null);
        public static Card airCard15 = new ("Kuruk", "Air Bender",Position.M,35,null);
        public static Card airCard16 = new ("Avatar Wan", "Fire Bender",Position.M,35,null);
        public static Card airCard17 = new ("Avatar Korra", "Water Bender",Position.M,35,null);
        public static Card airCard18 = new ("Appa", "Sky Bison",Position.M,35,null);
        public static Card airCard19 = new ("Momos", "Flying Lemur",Position.M,35,null);
        public static Card airCard20 = new ("Hawky", "Messenger Hawk",Position.M,35,null);
        public static Card airCard21 = new ("Flying Boar", "Non-bender",Position.M,35,null);
        public static Card airCard22 = new ("Wan Shi Tong", "Spirit",Position.M,35,null);
        public static Card airCard23 = new ("The Lion Turtle", "Spirit",Position.M,35,null);
        public static Card airCard24 = new ("Lemur", "Non-bender",Position.M,35,null);
        public static Card airCard25 = new ("Bosco", "Bear",Position.M,35,null);
        public static List<Card> AirDeck = new(){airCard2, airCard3, airCard4, airCard5, airCard6, airCard7, airCard8, airCard9, airCard10, airCard11, airCard12, airCard13, airCard14, airCard15, airCard16, airCard17, airCard18, airCard19, airCard20, airCard21, airCard22, airCard23, airCard24, airCard25};
        public static readonly AirNomads AirNomads = new(AirDeck,airLeader);
        #endregion

        #region FireNation
        public static Card fireLeader = new Card("Zuko", "Fire Bender",Position.Leaderposition,0,null);
        public static Card fireCard2 = new ("Azula", "Fire Bender",Position.S,56,null);
        public static Card fireCard3 = new ("Iroh", "Fire Bender",Position.S,56,null);
        public static Card fireCard4 = new ("Ozai", "Fire Bender",Position.S,56,null);
        public static Card fireCard5 = new ("Mai", "Non-bender",Position.S,56,null);
        public static Card fireCard6 = new ("Ty Lee", "Non-bender",Position.S,56,null);
        public static Card fireCard7 = new ("Zhao", "Non-bender",Position.S,56,null);
        public static Card fireCard8 = new ("Ran and Shaw", "Fire Lion Turtle",Position.S,56,null);
        public static Card fireCard9 = new ("Combustion Man", "Fire Bender",Position.S,56,null);
        public static Card fireCard10 = new ("Lo and Li", "Non-bender",Position.S,56,null);
        public static Card fireCard11 = new ("Jeong Jeong", "Fire Bender",Position.S,56,null);
        public static Card fireCard12 = new ("Sozin", "Fire Bender",Position.S,56,null);
        public static Card fireCard13 = new ("Azulon", "Fire Bender",Position.S,56,null);
        public static Card fireCard14 = new ("Shyu", "Non-bender",Position.S,56,null);
        public static Card fireCard15 = new ("Fire Lord Sozin", "Fire Bender",Position.S,56,null);
        public static Card fireCard16 = new ("Fire Lord Azulon", "Fire Bender",Position.S,56,null);
        public static Card fireCard17 = new ("Fire Lord Zuko", "Fire Bender",Position.S,56,null);
        public static Card fireCard18 = new ("Fire Lord Ozai", "Fire Bender",Position.S,56,null);
        public static Card fireCard19 = new ("Fire Lord Iroh", "Fire Bender",Position.S,56,null);
        public static Card fireCard20 = new ("Fire Lord Azula", "Fire Bender",Position.S,56,null);
        public static Card fireCard21 = new ("Fire Lord Sozin", "Fire Bender",Position.S,56,null);
        public static Card fireCard22 = new ("Izumi", "Fire Bender",Position.S,56,null);
        public static Card fireCard23 = new ("Fire Lord Azulon", "Fire Bender",Position.S,56,null);
        public static Card fireCard24 = new ("Azulon", "Fire Bender",Position.S,56,null);
        public static Card fireCard25 = new ("Iroh II", "Fire Bender",Position.S,56,null);
        public static List<Card> FireDeck = new(){fireCard2, fireCard3, fireCard4, fireCard5, fireCard6, fireCard7, fireCard8, fireCard9, fireCard10, fireCard11, fireCard12, fireCard13, fireCard14, fireCard15, fireCard16, fireCard17, fireCard18, fireCard19, fireCard20, fireCard21, fireCard22, fireCard23, fireCard24, fireCard25};
       
        public static readonly FireNation FireNation = new(FireDeck,fireLeader);
        #endregion

        #region Earth Kingdom
        public static Card earthLeader = new Card("Toph", "Earth Bender",Position.Leaderposition,0,null);
        public static Card earthCard2 = new ("King Bumi", "Earth Bender",Position.R,43,null);
        public static Card earthCard3 = new ("Lin Beifong", "Metal Bender",Position.R,43,null);
        public static Card earthCard4 = new ("Suyin Beifong", "Metal Bender",Position.R,43,null);
        public static Card earthCard5 = new ("Wei", "Earth Bender",Position.R,43,null);
        public static Card earthCard6 = new ("Wing", "Earth Bender",Position.R,43,null);
        public static Card earthCard7 = new ("Huan", "Earth Bender",Position.R,43,null);
        public static Card earthCard8 = new ("Kuvira", "Earth Bender",Position.R,43,null);
        public static Card earthCard9 = new ("Ghazan", "Lavabender",Position.R,43,null);
        public static Card earthCard10 = new ("Ming-Hua", "Water Bender",Position.R,43,null);
        public static Card earthCard11 = new ("P\'Li", "Combustion Bender",Position.R,43,null);
        public static Card earthCard12 = new ("Avatar Kyoshi", "Earth Bender",Position.R,43,null);
        public static Card earthCard13 = new ("Avatar Yangchen", "Air Bender",Position.R,43,null);
        public static Card earthCard14 = new ("Avatar Kuruk", "Water Bender",Position.R,43,null);
        public static Card earthCard15 = new ("Avatar Roku", "Fire Bender",Position.R,43,null);
        public static Card earthCard16 = new ("Avatar Wan", "Fire Bender",Position.R,43,null);
        public static Card earthCard17 = new ("Avatar Korra", "Water Bender",Position.R,43,null);
        public static Card earthCard18 = new ("Avatar Aang", "Air Bender",Position.R,43,null);
        public static Card earthCard19 = new ("Bosco", "Bear",Position.R,43,null);
        public static Card earthCard20 = new ("The Lion Turtle", "Spirit",Position.R,43,null);
        public static Card earthCard21 = new ("Wan Shi Tong", "Spirit",Position.R,43,null);
        public static Card earthCard22 = new ("Hayao", "Non-bender",Position.R,43,null);
        public static Card earthCard23 = new ("Bolin", "Earth Bender",Position.R,43,null);
        public static Card earthCard24 = new ("Wei", "Earth Bender",Position.R,43,null);
        public static Card earthCard25 = new ("Wing", "Earth Bender",Position.R,43,null);
        public static List<Card> EarthDeck = new(){earthCard2, earthCard3, earthCard4, earthCard5, earthCard6, earthCard7, earthCard8, earthCard9, earthCard10, earthCard11, earthCard12, earthCard13, earthCard14, earthCard15, earthCard16, earthCard17, earthCard18, earthCard19, earthCard20, earthCard21, earthCard22, earthCard23, earthCard24, earthCard25};
        public static readonly EarthKingdom EarthKingdom = new(EarthDeck,earthLeader);
        #endregion
    }    
        public class Card
        {
            public string name{get;private set;}
            public string description{get;private set;}
            public Position position{get;private set;}
            public int points{get;private set;}
            public Player player{get;set;}
            public Habilities hability{get; private set;}

            public Card(string name,string description, Position position,int points,Player player,Habilities hability)
            {
                this.name = name;
                this.description= description;
                this.position = position;
                this.points = points;
                this.player=player;
                this.hability=hability;
            }
              public Card(string name,string description, Position position,int points,Player player)
            {
                this.name = name;
                this.description= description;
                this.position = position;
                this.points = points;
                this.player=player;
                this.hability = Habilities.None;

            }
              #region Habilidades de las Unidades
            public static void CardTheft(Card card) //robar una carta 
            {
                Random random = new Random();
                Card stolenCard = card.player.Faction.Deck[random.Next(1,24)];
                card.player.Hand.Add(stolenCard);
            }
            // public static void IncreaseMyRow(Card card)
            // {
            //     int row = (int)card.position;
            //     int count = 0;//para saber cuantas cartas hay en esa fila 
            //     for (int i = 0; i < card.player.Board.board.GetLength(1); i++)//9 porq esa es la medida de la fila, tengo que busacr una mejor forma de poner esto
            //     {
            //         if (card.player.Board.board[row,i] != null)
            //         {
            //             card.player.Board.board[row,i].points += 5;
            //             count++;
            //         }
            //     }
            //     card.player.Points += count * 5; //+5 por cada carta 
            // }
            // public static void EliminateMostPowerful(Card card)
            // {
            //     Card powerfull = new("blabla","blabla",Position.MRS,0,card.player,card.enemy);
                
            //     for (int i = 0; i < card.enemy.Board.board.GetLength(0) ; i++)
            //     {
            //         for (int j = 0; j < card.enemy.Board.board.GetLength(1); j++)
            //         {
            //             if (card.enemy.Board.board[i,j] == null)
            //             {
            //                 continue;
            //             }
            //             if (card.enemy.Board.board[i,j].points > powerfull.points)//si la carta por la que vas iterando es mayor que la q tienes 
            //             {
            //                 powerfull = card.enemy.Board.board[i,j];
            //             }
            //         }
            //     }
            //     card.enemy.Graveyard.Add(powerfull);
            //     Game.EliminateCard(powerfull);
            // }
            // public static void EliminateLeastPowerful(Card card)
            // {
            //     Card lesspowerfull = null!;
            //     for (int i = 0; i < card.enemy.Board.board.GetLength(0) ; i++)
            //     {
            //         for (int j = 0; j < card.enemy.Board.board.GetLength(1); j++)
            //         {
            //             if (card.enemy.Board.board[i,j] == null)
            //             {
            //                 continue;
            //             }
            //             if (card.enemy.Board.board[i,j].points < lesspowerfull.points)//si la carta por la que vas iterando es mayor que la q tienes 
            //             {
            //                 lesspowerfull = card.enemy.Board.board[i,j];
            //             }
            //         }
            //     }
            //     card.enemy.Graveyard.Add(lesspowerfull);
            //     Game.EliminateCard(lesspowerfull);
            // }
            // public static void Multipoints(Card card)
            // {
            //     int sameCard = 1;
            //     for (int i = 0; i < card.player.Board.board.GetLength(0); i++)
            //         {
            //             for (int j = 0; j < card.player.Board.board.GetLength(1); j++)
            //             {
            //                 if (card.player.Board.board[i,j]== card)
            //                 {
            //                     sameCard += 1;
            //                 }
            //             }
            //         }
            //         card.points *= sameCard;//multiplica los puntos de card por la cantidad de cartas iguales a ella en el tablero
            // }
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
            public Player(string name,int points, Faction faction, List<Card> graveyard,List<Card>hand)//constructor antiguo
            {
                Name = name;
                Points = points;
                Faction = faction;
                Graveyard = graveyard;
                Hand = hand;

            }
            public Player()//nuevo constructor para poder inicializar los players sin tener todos sus datos
            {
                Random random = new();
                Name = "DefaultName";
                Points = 0;
                Graveyard = new List<Card>();
                Hand = new List<Card>();
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
            public Board()
            {
                rows = new List<Card>[3];  // Asume 3 filas, ajusta según sea necesario
                for (int i = 0; i < rows.Length; i++)
                {
                    rows[i] = new List<Card>();
                }
            }
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
            #region  = 0,
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
            Frost = 10,//(clima)Cambia la fuerza de todas las cartas de combate cuerpo a cuerpo de ambos jugadores a 1.
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

