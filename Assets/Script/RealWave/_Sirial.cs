using UnityEngine;
using System.Threading;
using System.IO.Ports;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class _Sirial : MonoBehaviour
{
    private Thread serialThread;
    bool isSerialWorking = false;
    public String Map="";
    private SerialPort port;
    private bool portIS = false;
    private int idx = 0;
    private string indata = "";
    private string mcoomend = "";
    private string commend = "";
    public int mapcode =0;
    private PostQueue instance = null;
    private BoardCheckManager ccboard = new BoardCheckManager();
    public static GameObject single = null;

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);
    [DllImport("user32.dll")]
    private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll")]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
    [DllImport("user32.dll")]
    static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
    [DllImport("user32.dll")]
    public static extern void BringWindowToTop(IntPtr hWnd);

    public bool debug = false;
    float timer;
    int check;

    public bool isMouseLock = true;
    public int alignSpeed;

    void Awake()
    {
        if (single == null)
            single = gameObject;
        else if (single != gameObject)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Screen.SetResolution(1920, 1080, true); // 해상도 고정(가끔씩 윈도우 기본해상도를 잘못가져올때가 있어서 고정시킴)

        WindowToTop(Application.productName);

        if (isMouseLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        alignSpeed = int.Parse(IniFileManager.instance.ReadValue("Speed & Time", "alignSpeed"));
    }

    void WindowToTop(string windowTitle)
    {
        IntPtr hWnd = FindWindow(null, windowTitle);

        IntPtr ForegroundHWnd = GetForegroundWindow();

        if (ForegroundHWnd != hWnd)
        {
            if (ForegroundHWnd != null)
            {
                uint foregroundThreadID = GetWindowThreadProcessId(ForegroundHWnd, IntPtr.Zero);
                uint targetThreadID = GetWindowThreadProcessId(hWnd, IntPtr.Zero);

                if (targetThreadID != foregroundThreadID)
                {
                    if (AttachThreadInput(targetThreadID, foregroundThreadID, true))
                    {
                        BringWindowToTop(hWnd);
                        AttachThreadInput(targetThreadID, foregroundThreadID, false);
                    }
                }
            }
        }
    }

    void Start()
    {
        instance = PostQueue.GetInstance;
        port = new SerialPort(IniFileManager.instance.ReadValue("COM", "PORT"), 38400, Parity.None, 8, StopBits.One);
        port.ReadTimeout = 100;
        Connect();
        serialThread = new Thread(new ThreadStart(OnSerialThreadStart));
        isSerialWorking = true;
        serialThread.Start();

        while (!serialThread.IsAlive) ; // wait for thread to start
        UnityEngine.Debug.Log("Thread started : " + Time.realtimeSinceStartup);

    }
    private void Update()
    {
        timer += Time.deltaTime;
        
        if (check != (int)timer / 60 * 10)
        {
            check = (int)timer / 60 * 10;
            SendLockCode();
        }
    }

    private void OnApplicationQuit()
    {
        AlignMachine(alignSpeed);
        StopSerialThread();
        SendMessageToServer("Serial On");
        WindowToTop("RealWave");
    }

    void StopSerialThread()
    {
        isSerialWorking = false;

        try
        {
            if (serialThread.IsAlive)
            {
                if (port.IsOpen)
                    port.Close();

                serialThread.Join();
            }
        }
        catch
        {
            UnityEngine.Debug.LogError("Thread join failed!!");
        }
    }

    void SendMessageToServer(string sendMessage)
    {
        Socket socket;
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1208);

        try
        {
            socket.Connect(localEndPoint);
        }
        catch
        {
            UnityEngine.Debug.LogError("Unable to connect to remote end point!");
        }

        string text = sendMessage;
        byte[] data = Encoding.UTF8.GetBytes(text);

        socket.Send(data);

        socket.Close();
    }

    protected void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        string indata = port.ReadExisting();


        //Debug.Log(idx++ + "ㅁ Data Recevied : " + indata);


    }
    void OnSerialThreadStart()
    {
        while (isSerialWorking)
        {
            try
            {
                int intRecSize = port.BytesToRead;

                string strRecData = "";
                string strRecHex = "";

                if (intRecSize > 1)
                {
                    byte[] buff = new byte[2];

                    port.Read(buff, 0, 2);

                    for (int iTemp = 0; iTemp < 2; iTemp++)
                    {
                        strRecHex += buff[iTemp].ToString("X2") + " ";
                        strRecData += Convert.ToChar(buff[iTemp]);
                    }

                    if (strRecData.IndexOf("I") == 0 || strRecData.IndexOf("J") == 0)
                    {
                        if (debug)
                            UnityEngine.Debug.Log(strRecData);
                                
                        PostQueue.GetInstance.PushData(strRecData);
                    }

                    idx++;
                }
                else
                {
                    System.Threading.Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex);
            }
        }
    }






    public void StartMotor(char pos, char rot, int t) //모터 시작 t 범위(0 ~ 99) 0일땐 무한 동작 (pos : F 앞, M 중간, N 뒤, D 드롭) (rot : L 왼쪽, R 오른쪽) 드롭모터는 rot 무조건 R
    {
        if (!port.IsOpen)
            return;

        byte[] str = new byte[6];
        str[0] = (byte)'M';
        str[1] = (byte)(~str[0]);
        str[2] = (byte)(pos);
        str[3] = (byte)(rot);
        str[4] = (byte)('0' + t / 10);
        str[5] = (byte)('0' + t % 10);

        //write(ByteToString(str));
        port.Write(str, 0, 6);
        UnityEngine.Debug.Log("StartMotor " + pos + " " + rot + " " + t);
    }

    public void StopMotor(char pos, char rot) //모터 멈추기 (pos : F 앞, M 중간, N 뒤, D 드롭) (rot : L 왼쪽, R 오른쪽) 드롭모터는 rot 무조건 R
    {
        if (!port.IsOpen)
            return;

        byte[] str = new byte[4];
        str[0] = (byte)'m';
        str[1] = (byte)(~str[0]);
        str[2] = (byte)(pos);
        str[3] = (byte)(rot);

        //write(ByteToString(str));
        port.Write(str, 0, 4);
        UnityEngine.Debug.Log("StopMotor " + pos + " " + rot);
    }


    public void Connect()
    {
        if (!debug)
            while (!portIS)
            {
                try
                {
                    port.Open();

                    if (port.IsOpen)
                    {
                        portIS = true;
                        UnityEngine.Debug.Log("연결되었습니다.");
                        ccboard.Check_System();
                        SendLockCode();
                        InitControl();
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("오류 " + ex.Message);
                    UnityEngine.Debug.Log("포트에 이상이 있으므로 재연결합니다.");
                    Thread.Sleep(1000);
                }

            }
    }
    private void InitControl()
    {
        this.SendBrake(0);
        this.SendVal('G', 6);
        this.SendVal('I', 6);
        this.SendVal('J', 6);
    }
    private void SendBrake(int val)
    {
        int val1 = val;
        if (val1 > 100)
            val1 = 100;
        if (val1 < 0)
            val1 = 0;
        this.SendVal('B', val1);
    }
    public void SendVal(char d, int val)
    {
        byte[] str = new byte[4];
        str[0] = (byte)d;
        str[1] = (byte)~str[0];
        str[2] = (byte)val;
        str[3] = (byte)~str[2];
        //Debug.Log(str[0] + " : " + str[1] + " : " + str[2] + " : " + str[3] + " SendVal ");
        port.Write(str, 0, 4);
    }

    public void SendPitch(int val)
    {
        //Debug.Log("SendPitch");
        int val1 = val;
        if (val1 > 100)
            val1 = 100;
        if (val1 < 0)
            val1 = 0;
        this.SendVal('a', val1);

    }

    public void SendRoll(int val)
    {
        //Debug.Log("SendRoll");
        int val1 = val;
        if (val1 > 100)
            val1 = 100;
        if (val1 < 0)
            val1 = 0;
        this.SendVal('D', val1);
    }

    public void SendYaw(int val)
    {
        //Debug.Log("SendYaw");
        int val1 = val;
        if (val1 > 95)
            val1 = 95;
        if (val1 < 5)
            val1 = 5;
        this.SendVal('d', val1);
    }
    public void ChangeRot()
    {
        byte[] str = new byte[4];
        str[0] = (byte)'A';
        str[1] = (byte)(~str[0]);
        str[2] = (byte)80;
        str[3] = (byte)(~str[2]);
        //Debug.Log(str[0] + str[1] + str[2] + str[3] + " ChangeRot ");
        port.Write(str, 0, 4);
    }
    public void CheckLock() //일정 시간마다 락 체크하는 부분.
    {

        byte[] str = new byte[4];
        str[0] = (byte)'H';
        str[1] = (byte)(~str[0]);
        str[2] = (byte)('W');
        str[3] = (byte)(~str[2]);
        //Debug.Log(str[0] + str[1] + str[2] + str[3] + " CheckLock ");
        port.Write(str, 0, 4);
    }


    public void SendLockCode()
    {
        //string str = String.Format("{0:X}", Convert.ToInt32(aa));
        byte[] str = new byte[16];

        char c = 'C';
        str[0] = (byte)c;
        str[1] = (byte)(~c);
        str[2] = (byte)(ccboard.reg24);
        str[3] = (byte)(~ccboard.reg24);
        str[4] = (byte)(ccboard.vidH);
        str[5] = (byte)(~ccboard.vidH);
        str[6] = (byte)(ccboard.reg26);
        str[7] = (byte)(~ccboard.reg26);
        str[8] = (byte)(ccboard.vidL);
        str[9] = (byte)(~ccboard.vidL);
        str[10] = (byte)(ccboard.cidH);
        str[11] = (byte)(~ccboard.cidH);
        str[12] = (byte)(ccboard.cidL);
        str[13] = (byte)(~ccboard.cidL);
        c = (char)0x23;
        str[14] = (byte)(c);
        str[15] = (byte)(~c);

        //write(ByteToString(str));
        //port.Write(str, 0, 16);
        for (int i = 0; i < 16; i++)
        {
            //Console.Write(str[i]);
            port.Write(str, i, 1);
        }
       
    }

    public void AlignMachine(int speed)
    {
        if (port.IsOpen)
        {
            SendVal('G', speed);
            SendVal('I', speed);
            SendVal('J', speed);
            SendPitch(50);
            SendRoll(50);
        }
    }
}
