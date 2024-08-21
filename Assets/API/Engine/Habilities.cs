namespace Engine
{
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
}