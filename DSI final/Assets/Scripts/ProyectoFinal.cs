using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

class ProyectoFinal : MonoBehaviour
{
    VisualElement button; //boton de confirmar personaje

    VisualElement selection; //personaje seleccionado

    List<VisualElement> charList = new List<VisualElement>(); //lista de personajes

    VisualElement tarjetaImagen; //retrato del personaje seleccionado

    List<VisualElement> players = new List<VisualElement>(); //lista de jugadores
    int currentPlayerIndex; //indice en la lista del jugador al que le toca elegir
    
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        button = root.Q<Button>("boton"); 
        tarjetaImagen = root.Q<VisualElement>("foto");

        charList = root.Query(className: "character").ToList();

        players = root.Query(className: "player").ToList();
        currentPlayerIndex = 0;
        highlightPlayer(players[currentPlayerIndex]);
        

        charList.ForEach(elem =>
        {
            elem.RegisterCallback<ClickEvent>(SelectCharacter);
        });

        button.RegisterCallback<ClickEvent>(ConfirmCharacter);
    }

    void SelectCharacter(ClickEvent ev)
    {
        VisualElement a = ev.target as VisualElement;

        if(selection != null)
            selection.RemoveFromClassList("characterSelected");
        selection = a;
        selection.AddToClassList("characterSelected");

        string aux = a.name + "Portrait";

        tarjetaImagen.style.backgroundImage = (StyleBackground)Resources.Load(aux); //cuando hacemos click sobre un personaje le seleccionamos
    }

    void ConfirmCharacter(ClickEvent ev)
    {
        if(selection != null)
        {
            string aux = selection.name;

            selection.RemoveFromClassList("characterSelected");
            selection.AddToClassList("charBlocked"); 
            selection.pickingMode = PickingMode.Ignore; //se bloquea el personaje confirmado para que no se pueda elegir mas tarde

            selection = null;
            tarjetaImagen.style.backgroundImage = null; //se quita el personaje seleccionado para evitar bugs de seleccionar varias veces un mismo personaje

            players[currentPlayerIndex].style.backgroundImage = (StyleBackground)Resources.Load(aux); //se cambia el retrato de la tarjeta
            nextPlayer();
        }
    }

    void nextPlayer()
    {
        if(currentPlayerIndex < players.Count - 1) //evitamos que salte un error
        {
            players[currentPlayerIndex].RemoveFromClassList("playerHighlight"); //quitamos el resaltado del jugador
            players[currentPlayerIndex].AddToClassList("player");
            currentPlayerIndex++; //avanzamos al siguiente de la lista
            highlightPlayer(players[currentPlayerIndex]); 
        }
        else //para bloquear que el ultimo jugador siga pudiendo cambiar
        {
            players[currentPlayerIndex].RemoveFromClassList("playerHighlight");
            players[currentPlayerIndex].AddToClassList("player");
            button.pickingMode = PickingMode.Ignore;
        }
    }

    void highlightPlayer(VisualElement ve)
    {
        ve.RemoveFromClassList("player"); //resaltamos el jugador
        ve.AddToClassList("playerHighlight");
    }
}