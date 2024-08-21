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
}

