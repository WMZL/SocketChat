using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;
using System;

public class ChatManager : MonoBehaviour
{
    private Socket m_ClientSocket;
    public Button m_SendBtn;
    public Text m_ShowText;
    public InputField m_Input;
    /// <summary>
    /// 为什么是1024TODO
    /// </summary>
    private byte[] data = new byte[1024];
    private string m_Msg = string.Empty;

    void Start()
    {
        m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_ClientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10001));

        Application.runInBackground = true;
        StartRecv();
    }

    /// <summary>
    /// 接收到服务器的消息
    /// </summary>
    private void OnReceiveServerMsg(IAsyncResult ar)
    {
        try
        {
            if (m_ClientSocket.Connected == false)
            {
                m_ClientSocket.Close();
                return;
            }

            int length = m_ClientSocket.EndReceive(ar);
            string msg = Encoding.UTF8.GetString(data, 0, length);
            m_Msg = msg;
            StartRecv();
        }
        catch (Exception e)
        {
            Debug.LogError("出现错误：" + e);
        }
    }

    void Update()
    {
        if (m_Msg != null && m_Msg != "")
        {
            m_ShowText.text += m_Msg + "\n";
            m_Msg = "";
        }
    }

    /// <summary>
    /// 点击发送按钮
    /// </summary>
    public void OnClickSend()
    {
        byte[] info = Encoding.UTF8.GetBytes(m_Input.text);
        m_ClientSocket.Send(info);
        m_Input.text = string.Empty;
    }

    /// <summary>
    /// 开始接受服务器的消息
    /// </summary>
    private void StartRecv()
    {
        m_ClientSocket.BeginReceive(data, 0, 1024, SocketFlags.None, OnReceiveServerMsg, null);
    }

    private void OnDestroy()
    {
        m_ClientSocket.Close();
    }
}
