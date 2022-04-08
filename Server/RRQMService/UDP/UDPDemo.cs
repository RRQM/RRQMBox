//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://www.yuque.com/eo2w71/rrqm
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMSocket;
using System;
using System.Text;

namespace RRQMService.UDP
{
    public static class UDPDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.简单udp测试");
            Console.WriteLine("2.udp性能测试");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TestUdpSession();
                        break;
                    }
                case "2":
                    {
                        TestUdpPerformance();
                        break;
                    }
                default:
                    break;
            }
        }

        private static void TestUdpPerformance()
        {
            UdpSession udpSession = new UdpSession();

            udpSession.Received += (remote, byteBlock,requestInfo) =>
            {
                udpSession.Send(remote, byteBlock);
            };
          
            udpSession.Setup(new RRQMConfig()
                .SetBindIPHost(new IPHost(7789)))
                .Start();
            Console.WriteLine("等待接收");
        }

        private static void TestUdpSession()
        {
            UdpSession udpSession = new UdpSession();
            udpSession.Received += (remote, byteBlock, requestInfo) =>
            {
                udpSession.Send(remote, byteBlock);
                Console.WriteLine($"收到：{Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len)}");
            };
            udpSession.Setup(new RRQMConfig()
                 .SetBindIPHost(new IPHost(7789)))
                 .Start();
            Console.WriteLine("等待接收");
        }
    }
}
