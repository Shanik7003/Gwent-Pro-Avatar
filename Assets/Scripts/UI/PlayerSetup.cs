using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Engine;
using System.Text;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using JetBrains.Annotations;
using System.Collections.Generic; // Asegúrate de incluir este namespace para trabajar con TextMeshPro.

public class PlayerSetup : MonoBehaviour
{
    public Vector3 expandedScale = new(1.2f, 1.2f, 1.2f);
    public Vector3 normalScale = new(1f, 1f, 1f);
    private Button currentlySelectedButton = null;
    public TMP_InputField nameInputField; // Referencia al InputField de TMP.
    private Faction selectedFaction; // Para almacenar el nombre de la facción seleccionada.
    private Faction Player1Faction,Player2Faction;
    private string player1Name, player2Name;
    private bool isSettingPlayer1 = true; // Indica si estamos configurando el Jugador 1


    public Button WaterButton, FireButton, AirButton, EarthButton;

    // Método para asignar al evento de clic de cada botón
    public void SelectFaction(Button selectedButton)
    {
        
        if (selectedButton == WaterButton)
        {
            selectedFaction = Faction.WaterTribe;
            if (!isSettingPlayer1 && selectedFaction == Game.GameInstance.Player1.Faction)
            {
                selectedButton = null;
                return;
            }
            //Debug.Log("Seleccionaste la Faccion WaterTribe");
        }
        if (selectedButton == FireButton)
        {
            selectedFaction = Faction.FireNation;
            Faction prueba = Game.GameInstance.Player1.Faction;
            if (!isSettingPlayer1 && selectedFaction == Game.GameInstance.Player1.Faction)
            {
                selectedButton = null;
                return;
            }
            
            //Debug.Log("Seleccionaste la Faccion FireNation");
        }
        if (selectedButton == EarthButton)
        {
            selectedFaction = Faction.EarthKingdom;
            if (!isSettingPlayer1 && selectedFaction == Game.GameInstance.Player1.Faction)
            {
                selectedButton =null;
                return;
            }
          
            //Debug.Log("Seleccionaste la Faccion EarthKindom");
        }
        if (selectedButton == AirButton)
        {
        
            selectedFaction = Faction.AirNomads;
            
            if (!isSettingPlayer1 && selectedFaction == Game.GameInstance.Player1.Faction)
            {
                selectedButton = null;
                return;
            }
            
            //Debug.Log("Seleccionaste la Faccion AirNomads");
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
            //Debug.Log("You must fill in your name and select a faction to continue.");
            // Aquí podrías también mostrar algún mensaje en la UI informando al jugador.
            return;
        }
        else
        {
            if (isSettingPlayer1)
            {
                // Guardar los datos del Jugador 1
                player1Name = nameInputField.text;
                Player1Faction = selectedFaction;
                ResetUI(); //*! Resetear la interfaz para el Jugador 2
                isSettingPlayer1 = false;
            }
            else
            {
                // Guardar los datos del Jugador 2 y procesar
                player2Name = nameInputField.text;
                Player2Faction = selectedFaction;
        
                GameManagerWrapper.Instance.SetPlayersInfo(player1Name,Player1Faction,player2Name,Player2Faction);

                if (GameManagerWrapper.Instance == null)
                {
                    //Debug.LogError("GameManagerWrapper instance is null");
                }
                else
                {
           
                    // Game.GameInstance.Player1.GetHand();
                    // Game.GameInstance.Player2.GetHand();
               
                    //*?esto es para ver todas las cartas que hay en el deck y la mano del jugador 1 
                    // foreach (var card in Game.GameInstance.Player1.Deck)
                    // {
                    //     //Debug.Log($"DeckCard: {card.name}");
                    // }
                    // foreach (var card in Game.GameInstance.Player1.Hand)
                    // {
                    //     //Debug.Log($"HandCard: {card.name}");
                    // }

                    SceneManager.LoadScene("Board",LoadSceneMode.Single);
                }
               
            }
        }
    }  
     private void ResetUI()
    {
        nameInputField.text = "";
        currentlySelectedButton.transform.localScale = Vector3.one; // Resetear la escala si es necesario
        currentlySelectedButton.interactable = false;
        currentlySelectedButton = null;
        selectedFaction = Faction.None; 
        //Debug.Log("escena reseteada: introduce los datos de player2"); 
    }
}
