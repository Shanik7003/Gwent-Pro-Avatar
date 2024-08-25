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
                CardTheft = 6, //robar una carta del deck y agregarla a tu mano 
                None = 7, 
                Personalized = 8,
            #endregion
            
            #region SpecialCard
                Lure = 9,//(señuelo)
                Eclipse = 10,//(clima)Cambia la fuerza de todas las cartas de combate cuerpo a cuerpo de ambos jugadores a 1.
                Clearence = 13,// (despeje)Descarta todas las cartas de clima que haya en el campo de batalla y anula sus efectos.    
            #endregion


        }
}