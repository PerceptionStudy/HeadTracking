/*

   -----------------------
   UDP-Receive (send to)
   -----------------------
   // http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC
   
   
   // > receive
   // 127.0.0.1 : 8051
   
   // send
   // nc -u 127.0.0.1 8051

*/
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour {
   
    // receiving Thread
    Thread receiveThread;

   // udpclient object
   UdpClient client;

   // public
   // public string IP = "127.0.0.1"; default local
    public int port; // define > init

   // infos
   public string lastReceivedUDPPacket="";
   public string allReceivedUDPPackets=""; // clean up this from time to time!
   // FaceAPI
   public float xPos;
   public float yPos;
   public float zPos;

	public float oldPitch = 0.0f;
	public float oldYaw = 0.0f;

	public float pitch = 0.0f;
	public float yaw = 0.0f;
	public float roll = 0.0f;
   
   // start from shell
    private static void Main()
    {
       UDPReceive receiveObj=new UDPReceive();
       receiveObj.init();

      string text="";
      do
      {
          text = Console.ReadLine();
      }
      while(!text.Equals("exit"));
    }
    // start from unity3d
    public void Start()
    {
       
       init();   
    }
	
	public string getLastPacket()
	{
		return lastReceivedUDPPacket;
	}
	
	
	/*
    // OnGUI
    void OnGUI()
   {
      Rect rectObj=new Rect(40,10,200,400);
         GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperLeft;
      GUI.Box(rectObj,"# UDPReceive\n127.0.0.1 "+port+" #\n"
               + "shell> nc -u 127.0.0.1 : "+port+" \n"
               + "\nLast Packet: \n"+ lastReceivedUDPPacket
               + "\n\nAll Messages: \n"+allReceivedUDPPackets
            ,style);
   }
      */
    // init
    private void init()
    {		
       xPos = 0;
       yPos = 0;
       zPos = 0;
       pitch = 0;
       yaw = 0;
       roll = 0;

        print("UDPSend.init()");
       
        // define port
        //port = 6100;

        // status
        print("Sending to 127.0.0.1 : "+port);
        print("Test-Sending to this Port: nc -u 127.0.0.1  "+port+"");
 
   
       // ----------------------------
      // Abhören
      // ----------------------------
        // Lokalen Endpunkt definieren (wo Nachrichten empfangen werden).
        // Einen neuen Thread für den Empfang eingehender Nachrichten erstellen.
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
 
    }

   // receive thread
    private  void ReceiveData()
    {

        client = new UdpClient(port);
        while (true)
        {

            try
            {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

                // Bytes mit der UTF8-Kodierung in das Textformat kodieren.
                string text = Encoding.UTF8.GetString(data);

                parseString(text);

				smoothValues();
                //print(">> " + text);

                // latest UDPpacket
                lastReceivedUDPPacket=text;
               
                // ....
                //allReceivedUDPPackets=allReceivedUDPPackets+text;
               
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
    
	private void smoothValues()
	{
		yaw = oldYaw + (yaw - oldYaw) * 0.25f;
		pitch = oldPitch + (pitch - oldPitch) * 0.25f;
	}

    private void parseString(String text)
    {
        String[] str = text.Split(' ');
        int index = 0;

		oldPitch = pitch;
		oldYaw = yaw;

        xPos = float.Parse(str[index++]);
        yPos = float.Parse(str[index++]);
        zPos = float.Parse(str[index++]);
        pitch = float.Parse(str[index++]);
        yaw = float.Parse(str[index++]);
        roll = float.Parse(str[index++]);
    }

    // getLatestUDPPacket
    // cleans up the rest
    public string getLatestUDPPacket()
    {
       allReceivedUDPPackets="";
       return lastReceivedUDPPacket;
    }
	
	void OnDisable()
	{
		if ( receiveThread!= null)
		receiveThread.Abort();

		client.Close();
	} 
} 