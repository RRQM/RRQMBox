﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using RRQMBox.Client.Common;
using RRQMMVVM;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.FileTransfer;

namespace RRQMBox.Client.Win
{
    /// <summary>
    /// FileClientWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FileClientWindow : RRQMWindow
    {
        public FileClientWindow()
        {
            InitializeComponent();
            this.Loaded += this.FileClientWindow_Loaded;
        }

        private void FileClientWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.fileList = new RRQMList<UrlFileInfo>();
            this.Lb_FileList.ItemsSource = this.fileList;
        }

        private FileClient fileClient;
        private RRQMList<UrlFileInfo> fileList;

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectService();
        }
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            this.BeginDownload();
        }
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            this.BeginUploadFile();
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            this.DisconnectService();
        }

        private void ShowMsg(string msg)
        {
            this.UIInvoke(() =>
            {
                this.msgBox.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]:{msg}\r\n");
            });
        }

        private void UIInvoke(Action action)
        {
            this.Dispatcher.Invoke(() =>
            {
                action.Invoke();
            });
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Pause();
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Resume();
        }

        private void StopThisButton_Click(object sender, RoutedEventArgs e)
        {
            this.StopThis();
        }

        private void StopAllButton_Click(object sender, RoutedEventArgs e)
        {
            this.StopAll();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Lb_FileList.SelectedItem != null)
            {
                UrlFileInfo fileInfo = (UrlFileInfo)this.Lb_FileList.SelectedItem;
                this.Cancel(fileInfo);
            }
            else
            {
                ShowMsg("请先选中传输项");
            }
        }

        private void RequestDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            this.RequestDelete();
        }

        private void RequestFileInfoButton_Click(object sender, RoutedEventArgs e)
        {
            this.RequestFileInfo();
        }

        private void SendSysMsgButton_Click(object sender, RoutedEventArgs e)
        {
            this.SendSystemMessage(this.mesBox.Text);
        }

        private void SelectUrlButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectPathFile();
        }

        #region 事件方法

        private void FileClient_FileTransferCollectionChanged(object sender, MesEventArgs e)
        {
            UIInvoke(()=> 
            {
                this.Lb_FileList.ItemsSource = new RRQMList<UrlFileInfo>(this.fileClient.FileTransferCollection);
            });
            
        }

        private void FileClient_DisConnectedService(object sender, MesEventArgs e)
        {
            UIInvoke(() =>
            {
                this.Tb_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
            });
        }

        private void FileClient_ConnectedService(object sender, MesEventArgs e)
        {
            UIInvoke(() =>
            {
                this.Tb_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6CE26C"));
            });
            ShowMsg("连接成功");
        }

        private void FileClient_FinishedFileTransfer(object sender, TransferFileMessageArgs e)
        {
            FileClient fileClient = sender as FileClient;//客户端中事件的sender实例均为FileClient
            RRQMSocket.FileTransfer.FileInfo fileInfo = e.FileInfo;//通过事件参数值，可获得完成的文件信息
            if (e.TransferType == TransferType.Download)
            {
                ShowMsg(string.Format("文件：{0}下载完成", e.FileInfo.FileName));
            }
            else
            {
                ShowMsg(string.Format("文件：{0}上传完成", e.FileInfo.FileName));
            }
        }

        private void FileClient_BeforeFileTransfer(object sender, FileOperationEventArgs e)
        {
            if (e.TransferType == TransferType.Download)
            {
                if (!Directory.Exists("ClientReceiveDir"))
                {
                    Directory.CreateDirectory("ClientReceiveDir");
                }

                DialogResult dialogResult = new DialogResult();
                dialogResult.Path = @"ClientReceiveDir\" + e.FileInfo.FileName;
                dialogResult.Visibility = System.Windows.Visibility.Visible;
                dialogResult.WaitHandle = new System.Threading.AutoResetEvent(false);

                UIInvoke(()=> 
                {
                    this.SaveDialog.DialogResult = dialogResult;
                });
               
                dialogResult.WaitHandle.WaitOne();
                e.TargetPath = dialogResult.Path;
            }
        }

        private void FileClient_ReceiveSystemMes(object sender, MesEventArgs e)
        {
            ShowMsg(e.Message);
        }

        private void FileClient_TransferFileError(object sender, TransferFileMessageArgs e)
        {
            if (e.TransferType == TransferType.Download)
            {
                ShowMsg(e.Message);
            }
            else
            {
                ShowMsg("服务器拒绝上传");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (fileClient != null)
            {
                this.Speed.Speed = fileClient.TransferSpeed / (1024 * 1024.0);
                this.Progress.Value = fileClient.TransferProgress;
            }
            else
            {
                this.Speed.Speed = 0;
                this.Progress.Value = 0;
            }
        }

        #endregion 事件方法



        #region 绑定方法
        private void BeginDownload()
        {
            if (fileClient != null && fileClient.Online)
            {
                if (string.IsNullOrEmpty(this.Tb_Url.Text))
                {
                    ShowMsg("Url不能为空");
                    return;
                }

                try
                {
                    fileClient.RequestTransfer(UrlFileInfo.CreatDownload(this.Tb_Url.Text));
                }
                catch (Exception e)
                {
                    ShowMsg(e.Message);
                }

            }
            else
            {
                ShowMsg("未连接");
            }
        }
        private void SendSystemMessage(string mes)
        {
            if (fileClient != null && fileClient.Online)
            {
                fileClient.SendSystemMessage(mes);
            }
        }
        private void StopAll()
        {
            if (fileClient != null)
            {
                fileClient.StopAllTransfer();
                ShowMsg("已停止下载");
            }
        }
        private void Resume()
        {
            if (fileClient != null)
            {
                fileClient.ResumeTransfer();
            }
        }
        private void BeginUploadFile()
        {
            if (fileClient != null && fileClient.Online)
            {
                if (File.Exists(this.Tb_Url.Text))
                {
                    try
                    {
                        fileClient.RequestTransfer(UrlFileInfo.CreatUpload(this.Tb_Url.Text, (bool)this.Cb_Restart.IsChecked, this.fileClient.BreakpointResume));
                    }
                    catch (Exception e)
                    {
                        ShowMsg(e.Message);
                    }
                }
                else
                {
                    ShowMsg("文件不存在");
                }
            }
        }
        private void SelectPathFile()
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();

            if (fileDialog.FileName != null && fileDialog.FileName.Length > 0)
            {
                this.Tb_Url.Text = fileDialog.FileName;
            }
        }

        private void StopThis()
        {
            if (fileClient != null)
            {
                fileClient.StopThisTransfer();
                ShowMsg("停止当前成功");
            }
        }

        private void Cancel(UrlFileInfo urlFileInfo)
        {
            if (fileClient != null)
            {
                if (fileClient.CancelTransfer(urlFileInfo))
                {
                    ShowMsg($"成功取消文件：{urlFileInfo.FileName}");
                }
            }
        }

        private void Pause()
        {
            if (fileClient != null)
            {
                this.fileClient.PauseTransfer();
            }
        }

        private void ConnectService()
        {
            if (fileClient != null)
            {
                ShowMsg("请勿重复连接");
                return;
            }

            try
            {
                fileClient = new FileClient();
                fileClient.Logger = new MsgLog(this.ShowMsg);
                fileClient.TransferFileError += this.FileClient_TransferFileError;
                fileClient.BeforeFileTransfer += this.FileClient_BeforeFileTransfer; ;
                fileClient.FinishedFileTransfer += this.FileClient_FinishedFileTransfer; ;
                fileClient.DisconnectedService += this.FileClient_DisConnectedService;
                fileClient.ReceiveSystemMes += this.FileClient_ReceiveSystemMes;
                fileClient.ConnectedService += this.FileClient_ConnectedService;
                fileClient.FileTransferCollectionChanged += this.FileClient_FileTransferCollectionChanged;
                fileClient.Connect(new IPHost(this.Tb_iPHost.Text), this.Tb_VerifyToken.Text);
            }
            catch (Exception ex)
            {
                if (fileClient != null && fileClient.Online)
                {
                    fileClient.Dispose();
                }

                fileClient = null;
                this.Tb_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
                ShowMsg(ex.Message);
            }
        }

        private void DisconnectService()
        {
            if (fileClient != null && fileClient.Online)
            {
                fileClient.Dispose();
            }

            fileClient = null;
            this.Tb_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
        }

        private void RequestDelete()
        {
            try
            {
                this.fileClient.RequestDelete(new UrlFileInfo(this.Tb_Url.Text));
                ShowMsg("请求成功");
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }

        private void RequestFileInfo()
        {
            try
            {
                RRQMSocket.FileTransfer.FileInfo fileInfo = this.fileClient.RequestFileInfo(new UrlFileInfo(this.Tb_Url.Text));
                ShowMsg($"请求成功,文件长度：{fileInfo.FileLength}");
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }



        #endregion 绑定方法


    }
}