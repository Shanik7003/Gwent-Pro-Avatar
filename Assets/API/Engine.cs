using System;
using System.Collections.Generic;

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
        public static Card waterLeader = new Card("Katara",  "Water Bender",Position.Leaderposition, 0,null);
        public static UnitCard waterCard2 = new ("Pakku",  "Water Bender",Position.M,2,Habilities.IncreaseRow,null);
        public static UnitCard waterCard3 = new ("Hama",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard4 = new ("Yue",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard5 = new ("Kya",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard6 = new ("Ming-Hua",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard7 = new ("Korra",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard8 = new ("Tonraq",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard9 = new ("Eska",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard10 = new ("Desna",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard11 = new ("Unalaq",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard12 = new ("Tarrlok",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard13 = new ("Hu",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard14 = new ("Thod",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard15 = new ("Vachir",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard16 = new ("Shirshu",  "Water Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard17 = new ("Admiral Zhao",  "Fire Bender",Position.R,20,Habilities.CardTheft,null);
        public static UnitCard waterCard18 = new ("June",   "Non-bender", Position.S,57,Habilities.CardTheft,null);
        public static UnitCard waterCard19 = new ("Song",  "Non-bender", Position.S,57,Habilities.CardTheft,null);
        public static UnitCard waterCard20 = new ("Bato",  "Non-bender", Position.S,57,Habilities.CardTheft,null);
        public static UnitCard waterCard21 = new ("Pipsqueak",  "Non-bender", Position.S,57,Habilities.CardTheft,null);
        public static UnitCard waterCard22 = new ("The Duke",  "Non-bender", Position.S,57,Habilities.CardTheft,null);
        public static UnitCard waterCard23 = new ("Tyro",  "Non-bender", Position.S,57,Habilities.CardTheft,null);
        public static UnitCard waterCard24 = new ("The Painted Lady",  "Non-bender", Position.S,57,Habilities.CardTheft,null);
        public static UnitCard waterCard25 = new ("Tui and La",  "Spirit", Position.S,57,Habilities.CardTheft,null);
        public static List<UnitCard> WaterDeck = new() { waterCard2, waterCard3, waterCard4, waterCard5, waterCard6, waterCard7, waterCard8, waterCard9, waterCard10, waterCard11, waterCard12, waterCard13, waterCard14, waterCard15, waterCard16, waterCard17, waterCard18, waterCard19, waterCard20, waterCard21, waterCard22, waterCard23, waterCard24, waterCard25};
        public static readonly WaterTribe  WaterTribe = new(WaterDeck,waterLeader);
        #endregion

        #region AirNomads
        public static Card airLeader = new Card ("Aang", "Air Bender",Position.Leaderposition, 0,null);
        public static UnitCard airCard2 = new ("Gyatso", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard3 = new ("Tenzin", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard4 = new ("Jinora", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard5 = new ("Ikki", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard6 = new ("Meelo", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard7 = new ("Opal", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard8 = new ("Kai", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard9 = new ("Rohan", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard10 = new ("Tashi", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard11 = new ("Avatar Yangchen", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard12 = new ("Yanchen", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard13 = new ("Roku", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard14 = new ("Kyoshi", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard15 = new ("Kuruk", "Air Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard16 = new ("Avatar Wan", "Fire Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard17 = new ("Avatar Korra", "Water Bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard18 = new ("Appa", "Sky Bison",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard19 = new ("Momos", "Flying Lemur",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard20 = new ("Hawky", "Messenger Hawk",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard21 = new ("Flying Boar", "Non-bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard22 = new ("Wan Shi Tong", "Spirit",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard23 = new ("The Lion Turtle", "Spirit",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard24 = new ("Lemur", "Non-bender",Position.M,35,Habilities.MultiPoints,null);
        public static UnitCard airCard25 = new ("Bosco", "Bear",Position.M,35,Habilities.MultiPoints,null);
        public static List<UnitCard> AirDeck = new(){airCard2, airCard3, airCard4, airCard5, airCard6, airCard7, airCard8, airCard9, airCard10, airCard11, airCard12, airCard13, airCard14, airCard15, airCard16, airCard17, airCard18, airCard19, airCard20, airCard21, airCard22, airCard23, airCard24, airCard25};
        public static readonly AirNomads AirNomads = new(AirDeck,airLeader);
        #endregion

        #region FireNation
        public static Card fireLeader = new Card("Zuko", "Fire Bender",Position.Leaderposition,0,null);
        public static UnitCard fireCard2 = new ("Azula", "Fire Bender",Position.S,56,Habilities.CardTheft,null);
        public static UnitCard fireCard3 = new ("Iroh", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard4 = new ("Ozai", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard5 = new ("Mai", "Non-bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard6 = new ("Ty Lee", "Non-bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard7 = new ("Zhao", "Non-bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard8 = new ("Ran and Shaw", "Fire Lion Turtle",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard9 = new ("Combustion Man", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard10 = new ("Lo and Li", "Non-bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard11 = new ("Jeong Jeong", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard12 = new ("Sozin", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard13 = new ("Azulon", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard14 = new ("Shyu", "Non-bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard15 = new ("Fire Lord Sozin", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard16 = new ("Fire Lord Azulon", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard17 = new ("Fire Lord Zuko", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard18 = new ("Fire Lord Ozai", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard19 = new ("Fire Lord Iroh", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard20 = new ("Fire Lord Azula", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard21 = new ("Fire Lord Sozin", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard22 = new ("Izumi", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard23 = new ("Fire Lord Azulon", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard24 = new ("Azulon", "Fire Bender",Position.S,56,Habilities.None,null);
        public static UnitCard fireCard25 = new ("Iroh II", "Fire Bender",Position.S,56,Habilities.None,null);
        public static List<UnitCard> FireDeck = new(){fireCard2, fireCard3, fireCard4, fireCard5, fireCard6, fireCard7, fireCard8, fireCard9, fireCard10, fireCard11, fireCard12, fireCard13, fireCard14, fireCard15, fireCard16, fireCard17, fireCard18, fireCard19, fireCard20, fireCard21, fireCard22, fireCard23, fireCard24, fireCard25};
       
        public static readonly FireNation FireNation = new(FireDeck,fireLeader);
        #endregion

        #region Earth Kingdom
        public static Card earthLeader = new Card("Toph", "Earth Bender",Position.Leaderposition,0,null);
        public static UnitCard earthCard2 = new ("King Bumi", "Earth Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard3 = new ("Lin Beifong", "Metal Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard4 = new ("Suyin Beifong", "Metal Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard5 = new ("Wei", "Earth Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard6 = new ("Wing", "Earth Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard7 = new ("Huan", "Earth Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard8 = new ("Kuvira", "Earth Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard9 = new ("Ghazan", "Lavabender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard10 = new ("Ming-Hua", "Water Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard11 = new ("P\'Li", "Combustion Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard12 = new ("Avatar Kyoshi", "Earth Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard13 = new ("Avatar Yangchen", "Air Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard14 = new ("Avatar Kuruk", "Water Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard15 = new ("Avatar Roku", "Fire Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard16 = new ("Avatar Wan", "Fire Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard17 = new ("Avatar Korra", "Water Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard18 = new ("Avatar Aang", "Air Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard19 = new ("Bosco", "Bear",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard20 = new ("The Lion Turtle", "Spirit",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard21 = new ("Wan Shi Tong", "Spirit",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard22 = new ("Hayao", "Non-bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard23 = new ("Bolin", "Earth Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard24 = new ("Wei", "Earth Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static UnitCard earthCard25 = new ("Wing", "Earth Bender",Position.R,43,Habilities.IncreaseRow,null);
        public static List<UnitCard> EarthDeck = new(){earthCard2, earthCard3, earthCard4, earthCard5, earthCard6, earthCard7, earthCard8, earthCard9, earthCard10, earthCard11, earthCard12, earthCard13, earthCard14, earthCard15, earthCard16, earthCard17, earthCard18, earthCard19, earthCard20, earthCard21, earthCard22, earthCard23, earthCard24, earthCard25};
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

            public Card(string name,string description, Position position,int points,Player player)
            {
                this.name = name;
                this.description= description;
                this.position = position;
                this.points = points;
                this.player=player;
            }
        }
        public class UnitCard : Card
        {
            public Habilities hability{get; private set;}
            public UnitCard(string name,string description,Position position,int points, Habilities hability,Player player) : base(name,description, position,points,player)
            {
                this.hability = hability;
            }  
        }
        public class Player
        {
            public string Name;
            public int Points;
            public Faction Faction;
            public List<UnitCard> Graveyard;
            public Board Board;
            public List<UnitCard> Hand{get;set;}
            public int Id;
            public Player(string name,int points, Faction faction, List<UnitCard> graveyard,List<UnitCard>hand)//constructor antiguo
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
                Graveyard = new List<UnitCard>();
                Hand = new List<UnitCard>();
                Board = new Board();
                Id = random.Next(0, 1000);
            }
            public List<UnitCard> GetHand()
            {
                Random random = new();
                for (int i = 0; i < 10; i++)
                {
                    UnitCard card = Faction.Deck[random.Next(0,14)];
                    Hand.Add(card);
                    Faction.Deck.Remove(card);
                }
                return Hand;
            }
        }
        public class Faction
        {
            public Card leader;
            public List<UnitCard> Deck = new();
            public Faction(List<UnitCard> Deck, Card leader)
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
            #region UnitCard
            IncreaseRow = 0,
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
       public FireNation (List<UnitCard> FireDeck, Card leader) : base (FireDeck, leader) {}
    }
    public class WaterTribe : Faction
    {
        public WaterTribe (List<UnitCard> WaterDeck,Card leader) : base (WaterDeck,leader){}   
    }
    public class AirNomads : Faction
    {
        public AirNomads (List<UnitCard> AirDeck,Card leader) : base(AirDeck,leader){}
    }
    public class EarthKingdom : Faction
    {
        public EarthKingdom(List<UnitCard> EarthDeck, Card leader): base (EarthDeck,leader){}
    }
    }

