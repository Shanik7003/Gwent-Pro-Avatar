using System;
using System.Collections.Generic;

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
        private double Points;
        public Position position{get; set;}
        public Guid Id {get; private set; }
        public Player player{get;set;}
        public Habilities hability{get; private set;}
        public CardType CardType{get;private set;}
        public Faction faction { get; private set; } 
        private List<IObserver> observers = new List<IObserver>();
        public List<Card> Ubication {get; set;}

        public double points
        {
            get => Points;
            set
            {
                Points = value;
                NotifyObservers(EventType.CardPointsChanged, this); // Notifica que los puntos de la carta han cambiado
            }
        }

        public void AddObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            observers = new List<IObserver>();
            //observers.Remove(observer);
        }

        private void NotifyObservers(EventType eventType, object data)
        {
            foreach (var observer in observers)
            {
                    observer.OnNotify(eventType, data);
            }
        }

        public void MoveCard(List<Card>Destino)
        {
            // Mover la carta a una nueva posición en el tablero
            Ubication.Remove(this);
            Destino.Add(this);
            this.Ubication = Destino;
            NotifyObservers(EventType.CardMoved, Destino); // Notifica que la carta se ha movido
        }
        public void MoveCardAndDesapeare(List<Card>Destino)
        {
            // Mover la carta a una nueva posición en el tablero
            Ubication.Remove(this);
            Destino.Add(this);
            this.Ubication = Destino;
            NotifyObservers(EventType.CardMovedAndDesapeare, Destino); // Notifica que la carta se ha movido
        }
        public void MoveCardToRight(List<Card>Destino)
        {
            // Mover la carta a una nueva posición en el tablero
            Ubication.Remove(this);
            Destino.Add(this);
            this.Ubication = Destino;
            NotifyObservers(EventType.CardMovedToRight, Destino); // Notifica que la carta se ha movido
        }
        public void RemoveCard()//*?siempre la manda para el cementerio
        {
            Ubication.Remove(this);

            player.Field.Remove(this);
            Game.GameInstance.Board.Remove(this);

            this.player.Graveyard.Add(this);
            this.Ubication = this.player.Graveyard;
            NotifyObservers(EventType.CardRemoved, this.player); 
        }


        #region Constructors
        public Card(){
            Ubication = new List<Card>();
        }

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
            Ubication = new List<Card>();
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
            Ubication = new List<Card>();
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
            Ubication = new List<Card>();
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
            Ubication = new List<Card>();
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
            Ubication = new List<Card>();
        }
    
        #endregion
        

        #region Habilidades de las Cartas Especiales
        public void Eclipse (Card card, int row)//para las cartas de clima 
        {
            //Game.GameInstance.WheatherSpace.Spaces[row] = card;//añade la carta a los espacios de clima del engine
            foreach (var item in Game.GameInstance.Player1.Board.rows[row])
            {
                if (item.CardType == CardType.IncreaseCard)
                {
                    continue;
                }
                if (item.points == 0) continue;
                // double discount = item.points/2;
                item.points /= 2;//reduce sus puntos a la mitad 
                Game.GameInstance.Player1.Points -= item.points;//quitale la misma cantidad de puntos que le qutaste a la carta al jugador  
            }
            foreach (var item in Game.GameInstance.Player2.Board.rows[row])//aplica tambien el clima en la fila correspondiente del otro jugador 
            {
                if (item.CardType == CardType.IncreaseCard)
                {
                    continue;
                }
                if (item.points==0) continue;
                // double discount = item.points/2;
                item.points /= 2;//reduce sus puntos a la mitad 
                Game.GameInstance.Player2.Points -= item.points;//quitale la misma cantidad de puntos que le qutaste a la carta al jugador 
            }

        }
        public  void Clearence(Card card,int row,Player enemy)//habilidad para las cartas de despeje
        {
            if(Game.GameInstance.WheatherSpace.Spaces[row] != null)//si existe una carta de clima afectando esa fila : despeja lo que su habilidad estaba ocasionando 
            {
               
                foreach (var item in card.player.Board.rows[row])
                {
                    if (item.CardType == CardType.IncreaseCard)
                    {
                        continue;
                    }
                    item.points += item.points; //duplica los puntos para eliminar el efecto del eclipse 
                    item.player.Points += item.points/2; //para ponerle al jugador los puntos que se esta recuperando 
                }
             
                foreach (var item in enemy.Board.rows[row])
                {
                    if (item.CardType == CardType.IncreaseCard)
                    {
                        continue;
                    }
                    item.points += item.points;
                    item.player.Points += item.points/2;//para ponerle al jugador los puntos que se esta recuperando 
                }
                
                Game.GameInstance.WheatherSpace.Spaces[row].RemoveCard(); //quitar la carta de clima 
            }
        }
        #endregion
    
        #region Habilidades de las Unidades
        public static void CardTheft(Player player) //robar una carta extra del mazo
        {
            System.Random random = new();
            Card stolenCard = player.Deck[random.Next(1,player.Deck.Count)];//escoge una carta random para robar del deck
            stolenCard.MoveCard(player.Hand);//la mueve a la mano del jugador
        }

        public  void IncreaseMyRow(Card card, int row)
        {
            int count = 0; //para saber cuantas cartas hay en esa fila 
            foreach (var item in card.player.Board.rows[row])
            {
                if (item.Id != card.Id)//para que no pueda contar la propia carta de aumento
                {
                    item.points += card.points;//sumale a cada una los puntos de la carta de aumento 
                    count ++;
                }
            }
            card.player.Points += count * card.points;
        }

        public static void EliminateMostPowerful(Player enemy)
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
                        enemy.Points -= item.points;//reduce los puntos del jugador 
                        item.RemoveCard();
                        return;
                    }
    
                }
            }
                return;
        }

        public static void  EliminateLeastPowerful(Player enemy)
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
                        lessPowerfulID = item.Id;
                    }
                }
            }
            for (int i = 0; i < enemy.Board.rows.Length; i++)
            {
                foreach (var item in enemy.Board.rows[i])
                {
                
                    if(item.Id == lessPowerfulID)
                    {
                        enemy.Points -= item.points;//reduce los puntos del jugador 
                        item.RemoveCard();
                        return;
                    }
                }
            }
            return;
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

        public static void CleanRow(Card card)//ademas de limpiar la fila que menos cartas tenga, devuelve el Row para que la UI trabaje con eso 
        {
            List<Card> minRow = new();

            for (int i = 0; i < Game.GameInstance.Player1.Board.rows.Length; i++)
            {
                if (Game.GameInstance.Player1.Board.rows[i].Count > 0 && Game.GameInstance.Player1.Board.rows[i] != card.Ubication)
                {
                    minRow = Game.GameInstance.Player1.Board.rows[i];//se que da con la primera fila que tenga mas de cero 
                    break;
                }
            }
            if (minRow.Count == 0)
            {
                for (int i = 0; i < Game.GameInstance.Player2.Board.rows.Length; i++)
                {
                    if (Game.GameInstance.Player2.Board.rows[i].Count > 0 && Game.GameInstance.Player2.Board.rows[i] != card.Ubication)
                    {
                        minRow = Game.GameInstance.Player2.Board.rows[i];//se que da con la primera fila que tenga mas de cero 
                        break;
                    }
                }
                
            }

            if (minRow.Count == 0)//si entra aqui es porque todas las filas tienen cero cartas 
            {
                return;
            }
            

            for(int i = 0; i < Game.GameInstance.Player1.Board.rows.Length; i++)//saca la fila que menos cartas tiene 
            {
    
                if (minRow.Count > Game.GameInstance.Player1.Board.rows[i].Count && Game.GameInstance.Player1.Board.rows[i].Count > 0 &&  Game.GameInstance.Player1.Board.rows[i] != card.Ubication)
                {
                    minRow = Game.GameInstance.Player1.Board.rows[i];
                }
      
                if (minRow.Count > Game.GameInstance.Player2.Board.rows[i].Count && Game.GameInstance.Player2.Board.rows[i].Count > 0 && Game.GameInstance.Player2.Board.rows[i] != card.Ubication)
                {
                    minRow = Game.GameInstance.Player2.Board.rows[i];
                }
            } 

            if (minRow.Contains(card))//si la fila que menos crtas tiene contiene a la carta que eliminaba no se aplica el e
            {
                return;
            }

            for (int i = 0; i < minRow.Count; i++)
            {
                if (minRow[i] != null)
                {
                    minRow[i].player.Points -= minRow[i].Points;
                    minRow[i].RemoveCard();//elimina la carta 
                }
            }

        }

        #endregion
    }
}
 
        
          
