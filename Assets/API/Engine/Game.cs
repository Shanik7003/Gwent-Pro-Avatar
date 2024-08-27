using System;
using System.Collections.Generic;
using UnityEngine;

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
        public List<Card> Board { get; set; }
        public WheatherSpace WheatherSpace;

        // Diccionario para almacenar todas las cartas, incluyendo las creadas por el usuario
        public static Dictionary<Guid, Card> AllCards { get; set; }

        // Constructor privado para prevenir instanciación externa
        private Game()
        {
            //Debug.Log("Creando el Game del Engine");
            Player1 = new Player();
            Player2 = new Player();
            WheatherSpace = new();

            // Inicializar el diccionario de cartas
            AllCards = new Dictionary<Guid, Card>();
            Board = new List<Card>();
            // Agregar las cartas fijas al diccionario
            InitializeDefaultCards();


        }

        private void InitializeDefaultCards()
        {
            // Cartas de la Tribu del Agua
            AddCard(new Card("Katara", "Roba una carta extra del Mazo", Position.M, 0, null, Habilities.CardTheft, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Pakku", "Roba una carta extra del Mazo", Position.M, 2, null, Habilities.CardTheft, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Hama", "Roba una carta extra del Mazo", Position.R, 3, null, Habilities.CardTheft, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Yue", "Roba una carta extra del Mazo", Position.R, 3, null, Habilities.CardTheft, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad", null, Habilities.Eclipse, GenerateGuid(), CardType.WheatherCard, Faction.WaterTribe));
            AddCard(new Card("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad", null, Habilities.Eclipse, GenerateGuid(), CardType.WheatherCard, Faction.WaterTribe));
            AddCard(new Card("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad", null, Habilities.Eclipse, GenerateGuid(), CardType.WheatherCard, Faction.WaterTribe));
            AddCard(new Card("Yue", "Elimina la carta con menos poder del campo del rival", Position.R, 2, null, Habilities.EliminateLeastPowerful, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Yue", "Elimina la carta con menos poder del campo del rival", Position.R, 2, null, Habilities.EliminateLeastPowerful, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Yue", "Elimina la carta con menos poder del campo del rival", Position.R, 2, null, Habilities.EliminateLeastPowerful, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Katara", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto", Position.R, 4, null, Habilities.Clearence, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Katara", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto", Position.R, 4, null, Habilities.Clearence, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Katara", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto", Position.R, 4, null, Habilities.Clearence, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Spirits", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo", Position.RS, 2, null, Habilities.CleanRow, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Sozin Comet", "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee", 4, null, Habilities.IncreaseMyRow, GenerateGuid(), CardType.IncreaseCard, Faction.WaterTribe));
            AddCard(new Card("Sozin Comet", "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee", 4, null, Habilities.IncreaseMyRow, GenerateGuid(), CardType.IncreaseCard, Faction.WaterTribe));
            AddCard(new Card("Sozin Comet", "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee", 2, null, Habilities.IncreaseMyRow, GenerateGuid(), CardType.IncreaseCard, Faction.WaterTribe));
            AddCard(new Card("Spirits", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo", Position.RS, 2, null, Habilities.CleanRow, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Spirits", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo", Position.RS, 2, null, Habilities.CleanRow, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("Avatar Kuruk", "Roba una carta extra del Mazo", Position.S, 1, null, Habilities.CardTheft, GenerateGuid(), Faction.WaterTribe));
            AddCard(new Card("The Painted Lady", "Roba una carta extra del Mazo", Position.S, 1, null, Habilities.CardTheft, GenerateGuid(), Faction.WaterTribe));

            // Cartas de los Nómadas del Aire
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Sozin Comet", "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee", 2, null, Habilities.IncreaseMyRow, GenerateGuid(), CardType.IncreaseCard, Faction.AirNomads));
            AddCard(new Card("Sozin Comet", "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee", 2, null, Habilities.IncreaseMyRow, GenerateGuid(), CardType.IncreaseCard, Faction.AirNomads));
            AddCard(new Card("Sozin Comet", "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee", 2, null, Habilities.IncreaseMyRow, GenerateGuid(), CardType.IncreaseCard, Faction.AirNomads));
            AddCard(new Card("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo", Position.RS, 2, null, Habilities.CleanRow, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo", Position.S, 2, null, Habilities.CleanRow, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo", Position.S, 2, null, Habilities.CleanRow, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Aang", "Roba una carta extra del Mazo", Position.M, 0, null, Habilities.CardTheft, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Aang", "Roba una carta extra del Mazo", Position.M, 0, null, Habilities.CardTheft, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Aang", "Roba una carta extra del Mazo", Position.M, 0, null, Habilities.CardTheft, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad", null, Habilities.Eclipse, GenerateGuid(), CardType.WheatherCard, Faction.AirNomads));
            AddCard(new Card("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad", null, Habilities.Eclipse, GenerateGuid(), CardType.WheatherCard, Faction.AirNomads));
            AddCard(new Card("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad", null, Habilities.Eclipse, GenerateGuid(), CardType.WheatherCard, Faction.AirNomads));
            AddCard(new Card("Appa", "Elimina la carta con menos poder del campo del rival", Position.R, 2, null, Habilities.EliminateLeastPowerful, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Appa", "Elimina la carta con menos poder del campo del rival", Position.R, 2, null, Habilities.EliminateLeastPowerful, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Appa", "Elimina la carta con menos poder del campo del rival", Position.R, 2, null, Habilities.EliminateLeastPowerful, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Monk Giatso", "Elimina la carta con más poder del campo del rival", Position.R, 4, null, Habilities.EliminateMostPowerful, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Monk Giatso", "Elimina la carta con más poder del campo del rival", Position.R, 4, null, Habilities.EliminateMostPowerful, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Monk Giatso", "Elimina la carta con más poder del campo del rival", Position.R, 4, null, Habilities.EliminateMostPowerful, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Appa", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto", Position.R, 4, null, Habilities.Clearence, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Appa", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto", Position.R, 4, null, Habilities.Clearence, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Appa", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto", Position.R, 4, null, Habilities.Clearence, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.AirNomads));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.AirNomads));

            // Cartas de la Nación del Fuego
            AddCard(new Card("Zuko", "Ninguna habilidad especial", Position.MR, 0, null, Habilities.None, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Sozin Comet", "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee", 4, null, Habilities.IncreaseMyRow, GenerateGuid(), CardType.IncreaseCard, Faction.FireNation));
            AddCard(new Card("Sozin Comet", "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee", 4, null, Habilities.IncreaseMyRow, GenerateGuid(), CardType.IncreaseCard, Faction.FireNation));
            AddCard(new Card("Sozin Comet", "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee", 4, null, Habilities.IncreaseMyRow, GenerateGuid(), CardType.IncreaseCard, Faction.FireNation));
            AddCard(new Card("Azula", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.MRS, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Azula", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.MRS, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Zhao", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.MRS, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Zuko", "Roba una carta extra del Mazo", Position.M, 0, null, Habilities.CardTheft, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Zuko", "Roba una carta extra del Mazo", Position.M, 0, null, Habilities.CardTheft, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Zuko", "Roba una carta extra del Mazo", Position.M, 0, null, Habilities.CardTheft, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad", null, Habilities.Eclipse, GenerateGuid(), CardType.WheatherCard, Faction.FireNation));
            AddCard(new Card("Roku", "Elimina la carta con menos poder del campo del rival", Position.R, 2, null, Habilities.EliminateLeastPowerful, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Roku", "Elimina la carta con menos poder del campo del rival", Position.R, 2, null, Habilities.EliminateLeastPowerful, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Iroh", "Elimina la carta con más poder del campo del rival", Position.R, 4, null, Habilities.EliminateMostPowerful, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Iroh", "Elimina la carta con más poder del campo del rival", Position.R, 4, null, Habilities.EliminateMostPowerful, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Iroh", "Elimina la carta con más poder del campo del rival", Position.R, 4, null, Habilities.EliminateMostPowerful, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Piandao", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto", Position.R, 4, null, Habilities.Clearence, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Piandao", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto", Position.R, 4, null, Habilities.Clearence, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Piandao", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto", Position.R, 4, null, Habilities.Clearence, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo", Position.RS, 2, null, Habilities.CleanRow, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo", Position.RS, 2, null, Habilities.CleanRow, GenerateGuid(), Faction.FireNation));
            AddCard(new Card("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo", Position.RS, 2, null, Habilities.CleanRow, GenerateGuid(), Faction.FireNation));

            // Cartas del Reino Tierra
            AddCard(new Card("Toph", "Elimina la carta con más poder del campo del rival", Position.S, 0, null, Habilities.EliminateMostPowerful, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Sozin Comet", "CARTA DE AUMENTO Le suma a las cartas de su fila de batalla los puntos que posee", 4, null, Habilities.IncreaseMyRow, GenerateGuid(), CardType.IncreaseCard, Faction.EarthKingdom));
            AddCard(new Card("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad", null, Habilities.Eclipse, GenerateGuid(), CardType.WheatherCard, Faction.EarthKingdom));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Eclipse", "CARTA DE CLIMA Reduce los puntos de las unidades a la mitad", null, Habilities.Eclipse, GenerateGuid(), CardType.WheatherCard, Faction.EarthKingdom));
            AddCard(new Card("Kuvira", "Elimina la carta con menos poder del campo del rival", Position.R, 2, null, Habilities.EliminateLeastPowerful, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Ghazan", "Elimina la carta con menos poder del campo del rival", Position.R, 2, null, Habilities.EliminateLeastPowerful, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Ming-Hua", "Elimina la carta con menos poder del campo del rival", Position.R, 2, null, Habilities.EliminateLeastPowerful, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Earth King", "Elimina la carta con menos poder del campo del rival", Position.R, 1, null, Habilities.EliminateLeastPowerful, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Appa", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo", Position.RS, 2, null, Habilities.CleanRow, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Avatar Kyoshi", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto", Position.R, 1, null, Habilities.Clearence, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Avatar Kyoshi", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto", Position.R, 1, null, Habilities.Clearence, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Long Feng", "Elimina la carta con más poder del campo del rival", Position.R, 1, null, Habilities.EliminateMostPowerful, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Long Feng", "Elimina la carta con más poder del campo del rival", Position.R, 1, null, Habilities.EliminateMostPowerful, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Bumi", "Roba una carta extra del Mazo", Position.M, 0, null, Habilities.CardTheft, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Bumi", "Roba una carta extra del Mazo", Position.M, 0, null, Habilities.CardTheft, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Bumi", "Roba una carta extra del Mazo", Position.M, 0, null, Habilities.CardTheft, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Bumi", "Elimina la carta con más poder del campo del rival", Position.R, 1, null, Habilities.EliminateMostPowerful, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Bumi", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto", Position.R, 4, null, Habilities.Clearence, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Spirits", "Multiplica sus puntos por la cantidad de cartas iguales a ella que hay en el campo de batalla", Position.S, 1, null, Habilities.MultiPoints, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Tejones Topo", "CARTA DE DESPEJE: Si hay un clima presente lo elimina y elimina su efecto", Position.R, 4, null, Habilities.Clearence, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Bumi", "LIMPIA UNA FILA: Elimina todas las cartas de la fila que menos cartas tenga de todo el campo", Position.RS, 2, null, Habilities.CleanRow, GenerateGuid(), Faction.EarthKingdom));
            AddCard(new Card("Wing", "Elimina la carta con más poder del campo del rival", Position.R, 4, null, Habilities.EliminateMostPowerful, GenerateGuid(), Faction.EarthKingdom));
        }


        // Método para agregar cartas al diccionario
        public void AddCard(Card card)
        {
            if (card != null && !AllCards.ContainsKey(card.Id))
            {
                AllCards.Add(card.Id, card);
            }
            else
            {
                //Debug.LogError("La carta ya existe o el ID es inválido.");
            }
        }

        // Método para generar un nuevo UUID (GUID)
        public Guid GenerateGuid()
        {
            return Guid.NewGuid();
        }

        // Método para agregar cartas creadas por el usuario
        public void AddUserCard(Card userCard)
        {
            AddCard(userCard);
        }

        // Método para obtener una carta por su ID
        public Card GetCardById(Guid id)
        {
            if (AllCards.TryGetValue(id, out var card))
            {
                return card;
            }

            //Debug.LogError("No se encontró ninguna carta con ese ID.");
            return null;
        }
    }
}
