using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;
using DatabaseControl;

public class MyNetworkManager : NetworkManager
{

    public delegate void ResponseDelegate(string response);
    public ResponseDelegate loginResponseDelegate;
    public ResponseDelegate registerResponseDelegate;

    public bool serverMode;

    public delegate void RegisterServerHandlerDelegate();
    public RegisterServerHandlerDelegate serverRegisterHandler;
    public delegate void RegisterClientHandlerDelegate(NetworkClient client);
    public RegisterClientHandlerDelegate clientRegisterHandler;

    void Start()
    {
        if (serverMode)
        {
            StartServer();
            NetworkServer.UnregisterHandler(MsgType.Connect);
            NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnectCustom);
            NetworkServer.RegisterHandler(MsgType.Highest + (short)NetMsgType.Login, OnServerLogin);
            NetworkServer.RegisterHandler(MsgType.Highest + (short)NetMsgType.Register, OnServerRegister);
            if (serverRegisterHandler != null) serverRegisterHandler.Invoke();
        }

    }

    void OnServerLogin(NetworkMessage netMsg)
    {
        StartCoroutine(LoginUser(netMsg));
    }

    void OnServerRegister(NetworkMessage netMsg)
    {
        StartCoroutine(RegisterUser(netMsg));
    }

    void OnClientLogin(NetworkMessage netMsg)
    {
        loginResponseDelegate.Invoke(netMsg.reader.ReadString());
    }

    void OnClientRegister(NetworkMessage netMsg)
    {
        registerResponseDelegate.Invoke(netMsg.reader.ReadString());
    }

    void OnServerConnectCustom(NetworkMessage netMsg)
    {
        if (LogFilter.logDebug) { Debug.Log("NetworkManager:OnServerConnectCustom"); }
        netMsg.conn.SetMaxDelay(maxDelay);
        OnServerConnect(netMsg.conn);
    }

    IEnumerator LoginUser(NetworkMessage netMsg)
    {
        UserAccount account = new UserAccount(netMsg.conn);
        UserMessage msg = netMsg.ReadMessage<UserMessage>();
        IEnumerator e = account.Login(msg.login, msg.pass);
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string;

        if (response == "Success")
        {
            if (account.data.characterHash.IsValid())
            {
                AccountEnter(account);
            }
            else
            {
                netMsg.conn.Send(MsgType.Highest + 1 + (short)NetMsgType.Login, new StringMessage("CharacterNotSelect"));
                netMsg.conn.Send(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, new EmptyMessage());
            }
        }
        else
        {
            netMsg.conn.Send(MsgType.Highest + 1 + (short)NetMsgType.Login, new StringMessage(response));
        }
    }

    IEnumerator RegisterUser(NetworkMessage netMsg)
    {
        UserMessage msg = netMsg.ReadMessage<UserMessage>();
        IEnumerator e = DCF.RegisterUser(msg.login, msg.pass, "");

        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string;

        Debug.Log("server register done");
        netMsg.conn.Send(MsgType.Highest + (short)NetMsgType.Register, new StringMessage(response));
    }

    public void Login(string login, string pass)
    {
        ClientConnect();
        StartCoroutine(SendLogin(login, pass));
    }

    public void Register(string login, string pass)
    {
        ClientConnect();
        StartCoroutine(SendRegister(login, pass));
    }

    IEnumerator SendLogin(string login, string pass)
    {
        while (!client.isConnected) yield return null;
        Debug.Log("client login");
        client.connection.Send(MsgType.Highest + (short)NetMsgType.Login, new UserMessage(login, pass));
    }

    IEnumerator SendRegister(string login, string pass)
    {
        while (!client.isConnected) yield return null;
        Debug.Log("client register");
        client.connection.Send(MsgType.Highest + (short)NetMsgType.Register, new UserMessage(login, pass));
    }

    void ClientConnect()
    {
        NetworkClient client = this.client;
        if (client == null)
        {
            client = StartClient();
            client.RegisterHandler(MsgType.Highest + (short)NetMsgType.Login, OnClientLogin);
            client.RegisterHandler(MsgType.Highest + (short)NetMsgType.Register, OnClientRegister);
            if (clientRegisterHandler != null) clientRegisterHandler.Invoke(client);
        }
    }

    public void AccountEnter(UserAccount account)
    {
        account.conn.Send(MsgType.Scene, new StringMessage(onlineScene));
    }

}

public enum NetMsgType { Login, Register, SelectCharacter }

public class UserMessage : MessageBase
{
    public string login;
    public string pass;

    public UserMessage()
    {
    }

    public UserMessage(string login, string pass)
    {
        this.login = login;
        this.pass = pass;
    }

    public override void Deserialize(NetworkReader reader)
    {
        login = reader.ReadString();
        pass = reader.ReadString();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(login);
        writer.Write(pass);
    }
}
