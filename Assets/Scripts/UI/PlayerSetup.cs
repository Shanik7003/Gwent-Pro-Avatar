using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Engine;
using System.Text;
using System;
using UnityEngine.SceneManagement;
using System.Linq; // Asegúrate de incluir este namespace para trabajar con TextMeshPro.

public class PlayerSetup : MonoBehaviour
{
    public Vector3 expandedScale = new(1.2f, 1.2f, 1.2f);
    public Vector3 normalScale = new(1f, 1f, 1f);
    private Button currentlySelectedButton = null;
    public TMP_InputField nameInputField; // Referencia al InputField de TMP.
    private Faction selectedFaction; // Para almacenar el nombre de la facción seleccionada.
    private Faction Player1Faction,Player2Faction;private string player1Name, player2Name;
    private bool isSettingPlayer1 = true; // Indica si estamos configurando el Jugador 1

    public Button WaterButton, FireButton, AirButton, EarthButton;

    // Método para asignar al evento de clic de cada botón
    public void SelectFaction(Button selectedButton)
    {
        if (selectedButton == WaterButton)
        {
            selectedFaction = Game.WaterTribe;
            Debug.Log("Seleccionaste la Faccion WaterTribe");
        }
        if (selectedButton == FireButton)
        {
            selectedFaction = Game.FireNation;
            Debug.Log("Seleccionaste la Faccion FireNation");
        }
        if (selectedButton == EarthButton)
        {
            selectedFaction = Game.EarthKingdom;
            Debug.Log("Seleccionaste la Faccion EarthKindom");
        }
        if (selectedButton == AirButton)
        {
            selectedFaction = Game.AirNomads;
            Debug.Log("Seleccionaste la Faccion AirNomads");
        }
        if (currentlySelectedButton != null && currentlySelectedButton != selectedButton)
        {
            currentlySelectedButton.transform.localScale = normalScale;
        }
        selectedButton.transform.localScale = expandedScale;
        currentlySelectedButton = selectedButton;
    }


    // Método para manejar el clic en el botón Continue
    public void OnContinueButtonClicked()
    {
        if (string.IsNullOrEmpty(nameInputField.text) || currentlySelectedButton == null)
        {
            Debug.Log("You must fill in your name and select a faction to continue.");
            // Aquí podrías también mostrar algún mensaje en la UI informando al jugador.
        }
        else
        {
             if (isSettingPlayer1)
            {
                // Guardar los datos del Jugador 1
                player1Name = nameInputField.text;
                Player1Faction = selectedFaction;
                Debug.Log("Player 1 set: " + player1Name + ", " + Player1Faction);
                // Resetear la interfaz para el Jugador 2
                ResetUI();
                isSettingPlayer1 = false;
            }
            else
            {
                // Guardar los datos del Jugador 2 y procesar
                player2Name = nameInputField.text;
                Player2Faction = selectedFaction;
                Debug.Log("Player 2 set: " + player2Name + ", " + Player2Faction);
        
                if (GameManagerWrapper.Instance == null)
                {
                    Debug.LogError("GameManagerWrapper instance is null");
                }
                else
                {
                    GameManagerWrapper.Instance.SetPlayersInfo(player1Name, player2Name, Player1Faction, Player2Faction);
                    Debug.Log("nombre del player1:" + Game.GameInstance.Player1.Name);
                    Debug.Log("nombre del player2:" + Game.GameInstance.Player2.Name);
                    Debug.Log("faccion del player1:" + Game.GameInstance.Player1.Faction);
                    Debug.Log("faccion del player2:" + Game.GameInstance.Player2.Faction);
                    Game.GameInstance.Player1.GetHand();
                    Debug.Log("Hand1 Añadida al Player1");
                    Game.GameInstance.Player2.GetHand();
                    Debug.Log("Hand2 Añadida al Player2");
                    SceneManager.LoadScene("Board",LoadSceneMode.Single);
                }
               
                // Opcionalmente, cargar otra escena o reiniciar la escena actual para otros propósitos
            }
        }
    }  
     private void ResetUI()
    {
        nameInputField.text = "";
        currentlySelectedButton.transform.localScale = Vector3.one; // Resetear la escala si es necesario
        currentlySelectedButton = null;
        selectedFaction = null; 
        Debug.Log("escena reseteada: introduce los datos de player2"); 
    }
}