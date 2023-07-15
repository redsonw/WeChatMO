using System.Diagnostics;
using System.Reflection;
using WeChatMultiOpen;
using YueHuan;

namespace RemoveMulti
{
    public partial class MainForm : Form
    {

        private readonly WeChatWin weChatWin;
        public MainForm()
        {
            InitializeComponent();
            weChatWin = new WeChatWin();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 获取程序集版本号
            Assembly assembly = Assembly.GetEntryAssembly()!;
            Version assemblyVersion = assembly.GetName().Version!;
            Console.WriteLine("程序集版本号: " + assemblyVersion);

            // 获取文件版本号
            string filePath = Environment.ProcessPath!;
            FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(filePath);
            Version fileVer = new(fileVersion.FileVersion!);
            string version = $"{fileVer.Major}.{fileVer.Minor}.{fileVer.Build}.{fileVer.Revision}";
            // MessageBox.Show(version);

            string infoPath = Environment.ProcessPath!; // 获取可执行文件路径
            FileInfo fileInfo = new(infoPath);

            // 获取项目版本号（替换为实际的版本号）
            Version projectVersion = new();
            Console.WriteLine("项目版本号: " + projectVersion);

            this.Text = $" 解除微信多开限制 v{assemblyVersion}";

            VersionLabel.Text = $" {weChatWin.WechatVer.Last()}"; // 最新支持版本

            PatchInfoLabel.Text = " 解除微信多开限制补丁";
            ReleaseLabel.Text = $" {fileInfo.LastWriteTime:yyyy-MM-dd}"; // fileInfo.LastAccessTime  # 访问时间  fileInfo.CreationTime   # 文件创建时间

            if (!string.IsNullOrEmpty(weChatWin.WeChatVersion))
            {
                LoggerListBox.Items.Add($"工具版本：{version}");
                LoggerListBox.Items.Add($"当前版本：{weChatWin.WeChatVersion}");
                LoggerListBox.Items.Add($"安装路径：{weChatWin.WeChatPath}");
                // LoggerListBox.Items.Add("QQ交流群：913959002");
            }
            else
            {
                LoggerListBox.Items.Add($"您的电脑还未安装PC 微信，请到官网下载最新版本");
                PatchesButton.Enabled = false;
            }
            DownloadLinkLabel.Text = " 最新测试版    官网最新版";
            DownloadLinkLabel.Links.Add(new LinkLabel.Link(1, 5, "https://www.redsonw.com/pc/commsocial/wechatwin.html"));
            DownloadLinkLabel.Links.Add(new LinkLabel.Link(10, 15, "https://dldir1.qq.com/weixin/Windows/WeChatSetup.exe"));
        }

        private void DownloadLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // DownloadLinkLabel.Links[0].LinkData = DownloadLinkLabel.Text;
            // DownloadLinkLabel.LinkVisited = true;
            if (e.Link!.LinkData is string url)
            {
                WeChatWin.OpenURL(url);
            }
        }

        private void PatchesButton_Click(object sender, EventArgs e)
        {

            string fileName = "WeChatWin.dll";
            string filePath = Path.Combine(weChatWin.WeChatPath, $"[{weChatWin.WeChatVersion}]", fileName);

            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);
            string? version = fileVersionInfo.FileVersion;

            if (!weChatWin.CheckVersion())
            {
                LoggerListBox.Items.Add($"当前版本：[{version}] 不支持解除限制，，请先安装微信 [{weChatWin.WechatVer[0]}] 或以上版本");
                return;
            }

            string backFile = Path.Combine(filePath, ".bak");

            if (File.Exists(backFile))
            {
                DialogResult result = MessageBox.Show("备份文件已经存在，是否继续备份，选择“是”则继续备份，选择“否”则跳过备份。", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (!weChatWin.Backup())
                    {
                        LoggerListBox.Items.Add($"备份成功：{backFile}");
                    }
                }
            }
            else
            {
                LoggerListBox.Items.Add($"备份已存在：{backFile}");
            }

            string chatName = "WeChat";
            Process[] processes = Process.GetProcessesByName(chatName);
            if (processes.Length > 0)
            {
                DialogResult result = MessageBox.Show("微信已经运行，是否要继续解除限制，选择“是”关闭微信继续，选择“否”则放弃解除限制。", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    foreach (Process process in processes)
                    {
                        process.Kill();
                        LoggerListBox.Items.Add("正在关闭微信，请稍候...");
                        Thread.Sleep(500);
                        LoggerListBox.Items.Add("微信已经被关闭");
                    }
                }
                else
                {
                    LoggerListBox.Items.Add("放弃解除限制");
                }
            }

            LoggerListBox.Items.Add($"初始化十六进制偏移量...");
            (long offset, byte oldValue, byte newValue) values = version switch
            {
                "3.9.5.65" => (0x01CDFBD8, 0x85, 0x33),
                "3.9.5.73" => (0x01CE1C38, 0x85, 0x33),
                "3.9.5.81" => (0x01CE15A8, 0x85, 0x33),
                "3.9.5.91" => (0x01CE2E28, 0x85, 0x33),
                "3.9.6.22" => (0X01CCE808, 0x85, 0x31),
                _ => throw new ArgumentException($"当前版本：[{version}] 不支持解除限制，，请先安装微信 [{weChatWin.WechatVer[0]}] 或以上版本")
            };
            long offset = values.offset;        // 偏移量，十六进制表示 
            byte oldValue = values.oldValue;    // 原始字节数据
            byte newValue = values.newValue;    // 新的字节数据

            LoggerListBox.Items.Add($"初始化[ {version} ]十六进制偏移量完成...");

            try
            {
                LoggerListBox.Items.Add("开始载入文件");
                using FileStream fs = new(filePath, FileMode.Open, FileAccess.ReadWrite);
                fs.Seek(offset, SeekOrigin.Begin);
                LoggerListBox.Items.Add($"载入成功:{filePath}");

                byte currentValue = (byte)fs.ReadByte();        // 读取指定位置的字节数据
                LoggerListBox.Items.Add("读取原始数据...");

                if (currentValue == oldValue)
                {
                    // 将文件流的位置重新设置为指定的偏移量
                    fs.Seek(offset, SeekOrigin.Begin);
                    LoggerListBox.Items.Add($"返回文件起点准备开始替换数据...");

                    // 写入新的字节数据
                    fs.WriteByte(newValue);
                    LoggerListBox.Items.Add($"修改{fileName} 完成");

                    // 刷新文件流，确保数据写入文件
                    fs.Flush();
                    LoggerListBox.Items.Add("解除双开限制补丁完成");
                }
                else
                {
                    LoggerListBox.Items.Add("未找到指定位置的字节数据或数据已被修改！");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                // LoggerListBox.Items.Add("文件修改出现错误：" + ex.Message);
            }
            finally
            {
                LoggerListBox.SelectedIndex = LoggerListBox.Items.Count - 1;
                LoggerListBox.ClearSelected();
                LoggerListBox.TopIndex = LoggerListBox.Items.Count - 1;
            }
        }

        private void AboutLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // MessageBox.Show("微信");
            // 创建新进程并打开关于窗口
            AboutForm aboutForm = new();
            aboutForm.ShowDialog();
        }
    }
}