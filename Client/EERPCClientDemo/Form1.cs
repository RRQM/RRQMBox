//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EERPCClientDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void ShowMsg(string msg)
        {
            this.Invoke((Action)(delegate () { this.textBox1.AppendText(msg + "\r\n"); }));
        }

        TcpRpcClient tcpRpcClient;
        private void button3_Click(object sender, EventArgs e)
        {
            this.tcpRpcClient = new TcpRpcClient();
            this.tcpRpcClient.Disconnected += TcpRpcClient_Disconnected;
            tcpRpcClient.Setup("127.0.0.1:7789").Connect();
            this.button3.Enabled = false;
            ShowMsg("连接成功");
        }

        private void TcpRpcClient_Disconnected(RRQMSocket.ITcpClient client, RRQMSocket.MesEventArgs e)
        {
            ShowMsg("已断开连接");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AccessType accessType = AccessType.Owner;
            if (this.checkBox1.Checked)
            {
                accessType = accessType | AccessType.Owner;
            }
            if (this.checkBox2.Checked)
            {
                accessType = accessType | AccessType.Service;
            }
            if (this.checkBox3.Checked)
            {
                accessType = accessType | AccessType.Everyone;
            }
            this.tcpRpcClient.PublishEvent(this.textBox2.Text, accessType);
            ShowMsg("发布成功");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] events = this.tcpRpcClient.GetAllEvents();

            this.listBox1.Items.Clear();
            this.listBox1.Items.AddRange(events);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.tcpRpcClient.SubscribeEvent<string>(this.textBox3.Text, SubscribeEvent);
            this.ShowMsg($"订阅成功");
        }
        private void SubscribeEvent(EventSender eventSender, string arg)
        {
            this.ShowMsg($"从{eventSender.RaiseSourceType}收到通知事件{eventSender.EventName}，信息：{arg}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is string eventName)
            {
                this.tcpRpcClient.RaiseEvent(eventName, this.textBox4.Text);
            }
            else
            {
                ShowMsg("请先选择事件");
            }
        }
    }
}