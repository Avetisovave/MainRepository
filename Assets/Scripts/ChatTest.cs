using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;

public class ChatTest : MonoBehaviour
{
    [SerializeField] private Button sendButton;
    [SerializeField] private Text allMessageText;
    [SerializeField] private InputField inputField;
    private HubConnection _hubConnection;
    private string _stringMessages;

    private void Start()
    {
        _hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:5225/chatHub").Build();
        _hubConnection.Closed += async e =>
        {
            await Task.Delay(1000);
            await _hubConnection.StartAsync();
        };
        Connect();
        sendButton.onClick.AddListener(SendMessage);
        
        
    }

    private void Update()
    {
        allMessageText.text = _stringMessages;
    }

    private async void Connect()
    {
        _hubConnection.On<string, string>("ReceiveMessage", ReceiveMessage);
        try
        {
            await _hubConnection.StartAsync();
            Debug.Log("Connected");
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            
        }
    }

    private void ReceiveMessage(string user , string message )
    {
        Debug.Log($"{user}: {message}");
        _stringMessages += $"{user}: {message}\n";
    }

    private void SendMessage()
    {
        _hubConnection.SendAsync("SendMessage", "Vlad", inputField.text);

    }
}
