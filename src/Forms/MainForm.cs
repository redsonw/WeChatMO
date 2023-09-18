using System.Diagnostics;
using System.Reflection;

namespace YueHuan
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
            DownloadLinkLabel.Text = " 测试版    正式版";
            DownloadLinkLabel.Links.Add(new LinkLabel.Link(1, 3, "https://www.redsonw.com/wechatwinbeta.html"));
            DownloadLinkLabel.Links.Add(new LinkLabel.Link(8, 12, "https://dldir1.qq.com/weixin/Windows/WeChatSetup.exe"));
        }

        private void DownloadLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link!.LinkData is string url)
            {
                WeChatWin.OpenURL(url);
            }
        }

        private void PatchesButton_Click(object sender, EventArgs e)
        {
            LimitRemover remover = new(weChatWin, LoggerListBox);
            remover.RemoveLimit();
        }
    }
}