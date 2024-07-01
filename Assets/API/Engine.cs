using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

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
        public static Card waterLeader = new Card("Katara",  "Roba una carta extra del Mazo",Position.M, 0,null,Habilities.CardTheft,1);
        public static Card waterCard2 = new ("Pakku",  "Roba una carta extra del Mazo",Position.M,2,null,Habilities.CardTheft,2);
        public static Card waterCard3 = new ("Hama",  "Roba una carta extra del Mazo",Position.R,3,null,Habilities.CardTheft,3);
        public static Card waterCard4 = new ("Yue",  "Roba una carta extra del Mazo",Position.R,3,null,Habilities.CardTheft,4);
        public static Card waterCard5 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,5,CardType.WheatherCard);
        public static Card waterCard6 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,6,CardType.WheatherCard);
        public static Card waterCard7 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,7,CardType.WheatherCard  );
        public static Card waterCard8 = new ("Yue", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,8);
        public static Card waterCard9 = new ("Yue", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,9);
        public static Card waterCard10 = new ("Yue", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,10);
        public static Card waterCard11 = new ("Katara", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto",Position.R,4,null,Habilities.Clearence,11);
        public static Card waterCard12 = new ("Katara", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto",Position.R,4,null,Habilities.Clearence,12);
        public static Card waterCard13 = new ("Katara", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto",Position.R,4,null,Habilities.Clearence,13);
        public static Card waterCard14 = new ("Spirits", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo",Position.RS,2,null,Habilities.CleanRow,14);
        public static Card waterCard15 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",4,null,Habilities.IncreaseMyRow,15,CardType.IncreaseCard);
        public static Card waterCard16 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",4,null,Habilities.IncreaseMyRow,16,CardType.IncreaseCard);
        public static Card waterCard17 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",2,null,Habilities.IncreaseMyRow,17,CardType.IncreaseCard);
        public static Card waterCard18 = new ("Spirits", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo",Position.RS,2,null,Habilities.CleanRow,18);
        public static Card waterCard19 = new ("Spirits", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo",Position.RS,2,null,Habilities.CleanRow,19);
        public static Card waterCard20 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,20);
        public static Card waterCard21 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,21);
        public static Card waterCard22 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,22);
        public static Card waterCard23 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,23);
        public static Card waterCard24 = new ("Avatar Kuruk",  "Roba una carta extra del Mazo", Position.S,1,null,Habilities.CardTheft,24);
        public static Card waterCard25 = new ("The Painted Lady",  "Roba una carta extra del Mazo", Position.S,1,null,Habilities.CardTheft,25);
        public static List<Card> WaterDeck = new() { waterCard2, waterCard3, waterCard4, waterCard5, waterCard6, waterCard7, waterCard8, waterCard9, waterCard10, waterCard11, waterCard12, waterCard13, waterCard14, waterCard15, waterCard16, waterCard17, waterCard18, waterCard19, waterCard20, waterCard21, waterCard22, waterCard23, waterCard24, waterCard25};
        public static readonly WaterTribe  WaterTribe = new(WaterDeck,waterLeader);
        #endregion

        #region AirNomads
        public static Card airLeader = new Card ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,51);
        public static Card airCard2 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,52);
        public static Card airCard3 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",2,null,Habilities.IncreaseMyRow,53,CardType.IncreaseCard);
        public static Card airCard4 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",2,null,Habilities.IncreaseMyRow,54,CardType.IncreaseCard);
        public static Card airCard5 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",2,null,Habilities.IncreaseMyRow,55,CardType.IncreaseCard);
        public static Card airCard6 = new ("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo",Position.RS,2,null,Habilities.CleanRow,56);
        public static Card airCard7 = new ("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo",Position.S,2,null,Habilities.CleanRow,57);
        public static Card airCard8 = new ("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo",Position.S,2,null,Habilities.CleanRow,58);
        public static Card airCard9 = new ("Aang",  "Roba una carta extra del Mazo",Position.M, 0,null,Habilities.CardTheft,59);
        public static Card airCard10 = new ("Aang",  "Roba una carta extra del Mazo",Position.M, 0,null,Habilities.CardTheft,60);
        public static Card airCard11 = new ("Aang",  "Roba una carta extra del Mazo",Position.M, 0,null,Habilities.CardTheft,61);
        public static Card airCard12 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,62,CardType.WheatherCard);
        public static Card airCard13 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,63,CardType.WheatherCard);
        public static Card airCard14 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,64,CardType.WheatherCard);
        public static Card airCard15 = new ("Appa", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,65);
        public static Card airCard16 = new ("Appa", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,66);
        public static Card airCard17 = new ("Appa", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,67);
        public static Card airCard18 = new ("Monk Giatso", "Elimina la carta con mas poder del campo del rival",Position.R,4,null,Habilities.EliminateMostPowerful,68);
        public static Card airCard19 = new ("Monk Giatso", "Elimina la carta con mas poder del campo del rival",Position.R,4,null,Habilities.EliminateMostPowerful,69);
        public static Card airCard20 = new ("Monk Giatso", "Elimina la carta con mas poder del campo del rival",Position.R,4,null,Habilities.EliminateMostPowerful,70);
        public static Card airCard21 = new ("Appa", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto",Position.R,4,null,Habilities.Clearence,71);
        public static Card airCard22 = new ("Appa", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto",Position.R,4,null,Habilities.Clearence,72);
        public static Card airCard23 = new ("Appa", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto",Position.R,4,null,Habilities.Clearence,73);
        public static Card airCard24 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,74);
        public static Card airCard25 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,75);
        public static List<Card> AirDeck = new(){airCard2, airCard3, airCard4, airCard5, airCard6, airCard7, airCard8, airCard9, airCard10, airCard11, airCard12, airCard13, airCard14, airCard15, airCard16, airCard17, airCard18, airCard19, airCard20, airCard21, airCard22, airCard23, airCard24, airCard25};
        public static readonly AirNomads AirNomads = new(AirDeck,airLeader);
        #endregion

        #region FireNation
        public static Card fireLeader = new Card("Zuko", "Ninguna habilidad especial",Position.MR,0,null,Habilities.None,26);
        public static Card fireCard2 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",4,null,Habilities.IncreaseMyRow,27,CardType.IncreaseCard);
        public static Card fireCard3 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",4,null,Habilities.IncreaseMyRow,28,CardType.IncreaseCard);
        public static Card fireCard4 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",4,null,Habilities.IncreaseMyRow,29,CardType.IncreaseCard);
        public static Card fireCard5 = new ("Azula", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla",Position.MRS,1,null,Habilities.MultiPoints,30);
        public static Card fireCard6 = new ("Azula", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla",Position.MRS,1,null,Habilities.MultiPoints,31);
        public static Card fireCard7 = new ("Zhao", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla",Position.MRS,1,null,Habilities.MultiPoints,32);
        public static Card fireCard8 = new ("Zuko",  "Roba una carta extra del Mazo",Position.M, 0,null,Habilities.CardTheft,33);
        public static Card fireCard9 = new ("Zuko",  "Roba una carta extra del Mazo",Position.M, 0,null,Habilities.CardTheft,34);
        public static Card fireCard10 = new ("Zuko",  "Roba una carta extra del Mazo",Position.M, 0,null,Habilities.CardTheft,35);
        public static Card fireCard11 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,36);
        public static Card fireCard12 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,37);
        public static Card fireCard13 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,38,CardType.WheatherCard);
        public static Card fireCard14 = new ("Roku", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,39);
        public static Card fireCard15 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,40);
        public static Card fireCard16 = new ("Roku", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,41);
        public static Card fireCard17 = new ("Iroh", "Elimina la carta con mas poder del campo del rival",Position.R,4,null,Habilities.EliminateMostPowerful,42);
        public static Card fireCard18 = new ("Iroh", "Elimina la carta con mas poder del campo del rival",Position.R,4,null,Habilities.EliminateMostPowerful,43);
        public static Card fireCard19 = new ("Iroh", "Elimina la carta con mas poder del campo del rival",Position.R,4,null,Habilities.EliminateMostPowerful,44);
        public static Card fireCard20 = new ("Piandao", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto",Position.R,4,null,Habilities.Clearence,45);
        public static Card fireCard21 = new ("Piandao", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto",Position.R,4,null,Habilities.Clearence,46);
        public static Card fireCard22 = new ("Piandao", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto",Position.R,4,null,Habilities.Clearence,47);
        public static Card fireCard23 = new ("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo",Position.RS,2,null,Habilities.CleanRow,48);
        public static Card fireCard24 = new ("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo",Position.RS,2,null,Habilities.CleanRow,49);
        public static Card fireCard25 = new ("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo",Position.RS,2,null,Habilities.CleanRow,50);
        public static List<Card> FireDeck = new(){fireCard2, fireCard3, fireCard4, fireCard5, fireCard6, fireCard7, fireCard8, fireCard9, fireCard10, fireCard11, fireCard12, fireCard13, fireCard14, fireCard15, fireCard16, fireCard17, fireCard18, fireCard19, fireCard20, fireCard21, fireCard22, fireCard23, fireCard24, fireCard25};
       
        public static readonly FireNation FireNation = new(FireDeck,fireLeader);
        #endregion

        #region Earth Kingdom
        public static Card earthLeader = new Card("Toph", "Elimina la carta con mas poder del campo del rival",Position.S,0,null,Habilities.EliminateMostPowerful,77);
        public static Card earthCard2 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,20);
        public static Card earthCard3 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,79);
        public static Card earthCard4 = new ("Sozin Comet",  "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee",4,null,Habilities.IncreaseMyRow,80,CardType.IncreaseCard);
        public static Card earthCard5 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,81,CardType.WheatherCard);
        public static Card earthCard6 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,20);
        public static Card earthCard7 = new ("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad",null,Habilities.Eclipse,83,CardType.WheatherCard);
        public static Card earthCard8 = new ("Kuvira", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,84);
        public static Card earthCard9 = new ("Ghazan", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,85);
        public static Card earthCard10 = new ("Ming-Hua", "Elimina la carta con menos poder del campo del rival",Position.R,2,null,Habilities.EliminateLeastPowerful,86);
        public static Card earthCard11 = new ("Earth King", "Elimina la carta con menos poder del campo del rival",Position.R,1,null,Habilities.EliminateLeastPowerful,87);
        public static Card earthCard12 = new ("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo",Position.RS,2,null,Habilities.CleanRow,88);
        public static Card earthCard13 = new ("Avatar Kyoshi", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto",Position.R,1,null,Habilities.Clearence,89);
        public static Card earthCard14 = new ("Avatar Kyoshi", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto",Position.R,1,null,Habilities.Clearence,90);
        public static Card earthCard15 = new ("Long Feng", "Elimina la carta con mas poder del campo del rival",Position.R,1,null,Habilities.EliminateMostPowerful,91);
        public static Card earthCard16 = new ("Long Feng", "Elimina la carta con mas poder del campo del rival",Position.R,1,null,Habilities.EliminateMostPowerful,92);
        public static Card earthCard17 = new ("Bumi",  "Roba una carta extra del Mazo",Position.M, 0,null,Habilities.CardTheft,93);
        public static Card earthCard18 = new ("Bumi",  "Roba una carta extra del Mazo",Position.M, 0,null,Habilities.CardTheft,94);
        public static Card earthCard19 = new ("Bumi",  "Roba una carta extra del Mazo",Position.M, 0,null,Habilities.CardTheft,95);
        public static Card earthCard20 = new ("Bumi", "Elimina la carta con mas poder del campo del rival",Position.R,1,null,Habilities.EliminateMostPowerful,96);
        public static Card earthCard21 = new ("Bumi", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto",Position.R,4,null,Habilities.Clearence,97);
        public static Card earthCard22 = new ("Spirits",  "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S,1,null,Habilities.MultiPoints,20);
        public static Card earthCard23 = new ("Tejones Topo", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto",Position.R,4,null,Habilities.Clearence,99);
        public static Card earthCard24 = new ("Bumi", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo",Position.RS,2,null,Habilities.CleanRow,100);
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
            public double points{get; set;}
            public Player player{get;set;}
            public Habilities hability{get; private set;}
            public int ID{get;private set;} 
            public CardType CardType{get;private set;}
             public Card(){}
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
                double maxPoints = 0;
                int mostPowerfulID = 0;
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
                            mostPowerfulID = item.ID;
                        }
                    }
                }
                
                for (int i = 0; i < enemy.Board.rows.Length; i++)
                {
                    foreach (var item in enemy.Board.rows[i])
                    {
                        if(item.ID == mostPowerfulID)
                        {
                            enemy.Board.rows[i].Remove(item);
                            enemy.Points -= item.points;//reduce los puntos del jugador 
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
                double minPoints = Int32.MaxValue;
                int lessPowerfulID = 0;
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
                            enemy.Board.rows[i].Remove(item);
                            enemy.Board.cemetery.Add(item);
                            enemy.Points -= item.points;//reduce los puntos del jugador 
                            lessPowerfulID = item.ID;
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
   
        public class Player
        {
            public string Name;
            public double Points;
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
                System.Random random = new();
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
                System.Random random = new();
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
            Clearence = 13,// (despeje)Descarta todas las cartas de clima que haya en el campo de batalla y anula sus efectos.    
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

