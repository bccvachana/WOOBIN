using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class python : MonoBehaviour
{
    Thread mThread;
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25001;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    public int[] data = new int[5];
    public int detection = 0;

    bool running;

    private void Start()
    {
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();

        client = listener.AcceptTcpClient();

        running = true;
        while (running)
        {
            Connection();
        }
        listener.Stop();
    }

    void Connection()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        if (dataReceived != null)
        {
            if (dataReceived == "stop")
            {
                running = false;
            }
            else
            {
                data = StringToArray(dataReceived);
                nwStream.Write(buffer, 0, bytesRead);
                detection = data[0];
            }
        }
    }

    int[] StringToArray(string pythondata)
    {
        // Remove the parentheses
        if (pythondata.StartsWith("(") && pythondata.EndsWith(")"))
        {
            pythondata = pythondata.Substring(1, pythondata.Length - 2);
        }

        // split the items
        string[] sArray = pythondata.Split(',');

        // store as a Vector3
        int[] result =
        {
            int.Parse(sArray[0]),
            int.Parse(sArray[1]),
            int.Parse(sArray[2]),
            int.Parse(sArray[3]),
            int.Parse(sArray[4])
        };
        return result;
    }
}