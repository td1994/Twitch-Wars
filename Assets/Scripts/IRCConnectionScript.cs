using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class IRCConnectionScript : MonoBehaviour {
    public GameObject textBox;
    public GameObject loginCanvas;
    public GameObject errorCanvas;
    public GameObject errorMessage;
    public GameObject border;
    public GameObject enemy;
    public GameObject sniper;
    public GameObject player;
    public GameObject nameText;

    private string serverName = "irc.chat.twitch.tv", userName, password, channel;
    private int port = 6667;
    private IRC IrcObject;
    private bool stopThreads = false;
    private List<string> msgList;
    private string inputBoxText;

    private string ircUser, ircRealName;
    private bool isInvisible = false;
    private TcpClient ircConnection;
    private StreamReader ircReader;
    private StreamWriter ircWriter;
    private NetworkStream ircStream;
    Thread input, output;
    private bool sendMsg = false;
    private bool chatStarted = false;

    private float minX, minY, maxX, maxY;

    public void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void Start()
    {
        msgList = new List<string>();
        inputBoxText = "";
        minX = border.transform.position.x - border.GetComponent<PolygonCollider2D>().bounds.size.x / 2 + 0.5f;
        maxX = border.transform.position.x + border.GetComponent<PolygonCollider2D>().bounds.size.x / 2 - 0.5f;
        minY = border.transform.position.x - border.GetComponent<PolygonCollider2D>().bounds.size.y / 2 + 0.5f;
        maxY = border.transform.position.x + border.GetComponent<PolygonCollider2D>().bounds.size.y / 2 - 0.5f;
    }

    public void setUserName(string userName)
    {
        this.userName = userName;
    }

    public void setPort(string port)
    {
        this.port = Convert.ToInt32(port);
    }

    public void setServer(string serverName)
    {
        this.serverName = serverName;
    }

    public void setPassword(string password)
    {
        this.password = password;
    }

    public void setChannelName(string channelName)
    {
        channel = "#" + channelName.ToLower();
    }

    public void connect()
    {
        // Connect to server
        //IrcObject.Connect("efnet.xs4all.nl", 6667);	
        //IrcObject.Connect(serverName, port, password);
        // Connect with the IRC server.
        loginCanvas.SetActive(false);
        stopThreads = false;
        textBox.GetComponent<Text>().text += "Connecting to " + serverName + ":" + port + "\n";
        ircRealName = "Twitch Wars User: " + userName;
        if(channel == null)
        {
            channel = "#" + userName.ToLower();
        }
        ircConnection = new TcpClient(serverName, port);
        ircStream = ircConnection.GetStream();
        ircReader = new StreamReader(ircStream);
        ircWriter = new StreamWriter(ircStream);

        // Authenticate our user
        ircWriter.WriteLine(String.Format("USER {0} {1} * :{2}", userName, "0", ircRealName));
        ircWriter.Flush();
        ircWriter.WriteLine(String.Format("PASS {0}", password));
        ircWriter.Flush();
        ircWriter.WriteLine(String.Format("NICK {0}", userName));
        ircWriter.Flush();
        ircWriter.WriteLine(String.Format("JOIN {0}", channel));
        ircWriter.Flush();

        input = new Thread(() => inputThread());
        input.Start();
        output = new Thread(() => outputThread());
        output.Start();
    }

    private void IrcJoin(string IrcChan, string IrcUser)
    {
        Console.WriteLine(String.Format("{0} joins {1}", IrcUser, IrcChan));
        IrcObject.IrcWriter.WriteLine(String.Format("NOTICE {0} :Hello {0}, welcome to {1}!", IrcUser, IrcChan));
        IrcObject.IrcWriter.Flush();
    } /* IrcJoin */

    private void IrcPart(string IrcChan, string IrcUser)
    {
        Console.WriteLine(String.Format("{0} parts {1}", IrcUser, IrcChan));
    } /* IrcPart */

    private void IrcMode(string IrcChan, string IrcUser, string UserMode)
    {
        if (IrcUser != IrcChan)
        {
            Console.WriteLine(String.Format("{0} sets {1} in {2}", IrcUser, UserMode, IrcChan));
        }
    } /* IrcMode */

    private void IrcNickChange(string UserOldNick, string UserNewNick)
    {
        Console.WriteLine(String.Format("{0} changes nick to {1}", UserOldNick, UserNewNick));
    } /* IrcNickChange */

    private void IrcKick(string IrcChannel, string UserKicker, string UserKicked, string KickMessage)
    {
        Console.WriteLine(String.Format("{0} kicks {1} out {2} ({3})", UserKicker, UserKicked, IrcChannel, KickMessage));
    } /* IrcKick */

    private void IrcQuit(string UserQuit, string QuitMessage)
    {
        Console.WriteLine(String.Format("{0} has quit IRC ({1})", UserQuit, QuitMessage));
    } /* IrcQuit */

    public void showConnectionStatus()
    {

    }

    public void addInput(string i)
    {
        lock(inputBoxText)
        {
            inputBoxText = i;
        }
    }

    public void sendMessage()
    {
        sendMsg = true;
    }

    void outputThread()
    {
        //Debug.Log("Where is this updating?");
        while(!stopThreads)
        {
            string ircCommand;
            if ((ircCommand = ircReader.ReadLine()) != null)
            {
                lock (msgList)
                {
                    msgList.Add(ircCommand);
                    //Debug.Log("outputThread unlocked.");
                }
            }
        }
    }

    void inputThread()
    {
        while(!stopThreads)
        {
            if(sendMsg)
            {
                lock (inputBoxText)
                {
                    ircWriter.WriteLine(String.Format("PRIVMSG {0} :{1}", channel, inputBoxText));
                    ircWriter.Flush();
                    inputBoxText = "";
                }
                sendMsg = false;
            }
        }
    }

    public void Close()
    {
        Debug.Log("Closing game");
        stopThreads = true;

        ircWriter.Close();
        ircReader.Close();
        ircConnection.Close();
        textBox.GetComponent<Text>().text = "";

        errorCanvas.SetActive(false);
        loginCanvas.SetActive(true);
    }

    void Update()
    {
        // Detect if client disconnected
        if (chatStarted && ircConnection.Client.Poll(0, SelectMode.SelectRead))
        {
            byte[] buff = new byte[1];
            try
            {
                if (ircConnection.Client.Receive(buff, SocketFlags.Peek) == 0)
                {
                    // Client disconnected
                    chatStarted = false;
                    errorCanvas.SetActive(true);
                    errorMessage.GetComponent<Text>().text = "ERROR: You were disconnected from the chat.";
                }
            } catch(SocketException)
            {
                chatStarted = false;
                errorCanvas.SetActive(true);
                errorMessage.GetComponent<Text>().text = "ERROR: You were disconnected from the chat.";
            }
            
        }

        lock (msgList)
        {
            foreach(string msg in msgList)
            {
                //textBox.GetComponent<Text>().text += msg + "\n";
                if (!chatStarted && msg.Contains("NOTICE * :Improperly formatted auth"))
                {
                    errorCanvas.SetActive(true);
                    errorMessage.GetComponent<Text>().text = "ERROR: Failed to connect to IRC Server. Your username or oauth password were incorrect.";
                    //Debug.Log("ERROR: Failed to connect to IRC Server. Your username or oauth password were incorrect.");
                }
                else if (!chatStarted && msg.Contains("End of /NAMES list"))
                {
                    chatStarted = true;
                    textBox.GetComponent<Text>().text += "Connected to " + channel + "\n";
                    player.SetActive(true);

                } else if(chatStarted)
                {
                    string user = msg.Substring(msg.IndexOf(':') + 1, msg.IndexOf('!') - 1);
                    string message = msg.Substring(msg.IndexOf(':', msg.IndexOf(':') + 1) + 1);
                    textBox.GetComponent<Text>().text += user + ": " + message + "\n";
                    Vector2 pos = new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
                    Vector3 textPos = new Vector3(pos.x, pos.y, -1f);
                    GameObject gen;
                    if (message.Equals("sniper"))
                    {
                        gen = Instantiate(sniper, pos, Quaternion.identity) as GameObject;
                    } else
                    {
                        gen = Instantiate(enemy, pos, Quaternion.identity) as GameObject;
                    }
                    GameObject text = Instantiate(nameText, textPos, Quaternion.identity) as GameObject;
                    text.GetComponent<TextMesh>().text = user;
                    text.transform.parent = gen.transform;
                }
            }
            msgList.Clear();
        }
    }

    private void OnDestroy()
    {
        Close();
    }
}


