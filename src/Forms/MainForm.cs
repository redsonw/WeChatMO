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

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await weChatWin.InitializeAsync();

            DisplayAssemblyVersion();   // 获取程序集版本号
            DisplayFileVersion();       // 获取当前程序集版本号
            DisplayProjectVersion();    // 替换版本号
            SetLabels();                // 设置程序标题
            SetLoggerListBox();         // 程序日志写入
            SetDownloadLinks();         // 设置下载路径
        }

        /// <summary>
        /// 获取程序集的版本号
        /// </summary>
        private static void DisplayAssemblyVersion()
        {
            // 获取当前正在执行的程序集
            Assembly? assembly = Assembly.GetEntryAssembly();

            // 从程序集中获取其版本信息
            Version? assemblyVersion = assembly?.GetName().Version;

            // 将获取到的版本信息输出到控制台，如果版本信息为null，则输出"未知"
            Console.WriteLine("程序集版本号： " + (assemblyVersion?.ToString() ?? "未知"));

        }

        /// <summary>
        /// 获取当前程序版本号
        /// </summary>
        private async void DisplayFileVersion()
        {
            // 获取当前进程的路径
            string? filePath = Environment.ProcessPath;

            // 获取文件的版本信息
            FileVersionInfo? fileVersion = FileVersionInfo.GetVersionInfo(filePath!);

            // 从文件版本信息中提取出版本号的各个部分，并创建一个新的Version对象
            Version? fileVer = new(fileVersion.FileVersion!);

            // 将版本号的各个部分拼接成一个字符串
            string version = $"{fileVer.Major}.{fileVer.Minor}.{fileVer.Build}.{fileVer.Revision}";

            var githubUpdate = new GithubUpdate("redsonw", "WeChatMO");
            var latestRelease = await githubUpdate.GetLatestRelease();
            string newversion = latestRelease.TagName.Replace("v", "");
            string exeDownloadUrl = await githubUpdate.GetExeDownloadUrl();
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "new" + GetFileNameFromUrl(exeDownloadUrl);
            IProgress<double> progress = new Progress<double>(percent =>
            {
                Console.WriteLine($"Downloaded: {percent}%");
            });

            if (newversion != version)
            {
                bool result = MessageBox.Show("发现新的版本，请更新！！！", "版本更新", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;

                if (result)
                {
                    double finalProgress = await githubUpdate.DownloadExe(fileName, progress);
                    LoggerListBox.Items.Add($"已经下载：{finalProgress}%");
                }
            }
        }

        static string GetFileNameFromUrl(string url)
        {
            string fileName = Path.GetFileName(new Uri(url).LocalPath);
            return fileName;
        }

        private static void DisplayProjectVersion()
        {
            // 这里需要替换为实际的项目版本号获取逻辑
            Version projectVersion = new("1.0.0.0"); // 例如：假设版本号是 1.0.0.0
            Console.WriteLine("项目版本号: " + projectVersion);
        }

        /// <summary>
        /// 设置程序标题
        /// </summary>
        private void SetLabels()
        {
            // 获取当前进程的路径
            string? infoPath = Environment.ProcessPath;
            // 根据路径创建一个FileInfo对象
            FileInfo fileInfo = new(infoPath!);

            // 设置窗体的标题，显示版本信息和解除微信多开限制的信息
            this.Text = $" 解除微信多开限制 v{Assembly.GetEntryAssembly()?.GetName()?.Version}";

            // 设置版本标签的文本，显示最新支持的版本信息
            VersionLabel.Text = $" {weChatWin.WechatVer.Last()}"; // 最新支持版本

            // 设置补丁信息标签的文本，显示补丁的信息
            PatchInfoLabel.Text = " 解除微信多开限制补丁";

            // 设置发布标签的文本，显示文件的最后写入时间
            ReleaseLabel.Text = $" {fileInfo.LastWriteTime:yyyy-MM-dd}";
        }

        /// <summary>
        /// 写入日志信息
        /// </summary>
        private void SetLoggerListBox()
        {
            if (!string.IsNullOrEmpty(weChatWin.WeChatVersion))
            {
                LoggerListBox.Items.Add($"工具版本：{GetFileVersion()}");
                LoggerListBox.Items.Add($"当前版本：{weChatWin.WeChatVersion}");
                LoggerListBox.Items.Add($"安装路径：{weChatWin.WeChatPath}");
            }
            else
            {
                LoggerListBox.Items.Add($"您的电脑还未安装PC 微信，请到官网下载最新版本");
                PatchesButton.Enabled = false;
            }
        }

        /// <summary>
        /// 获取当前程序文件的版本信息
        /// </summary>
        /// <returns></returns>
        private static string GetFileVersion()
        {
            // 获取当前程序的路径
            string? filePath = Environment.ProcessPath;

            // 使用FileVersionInfo类获取文件的版本信息
            FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(filePath!);

            // 将版本信息封装到Version对象中
            Version fileVer = new(fileVersion.FileVersion!);

            // 格式化版本信息并返回字符串形式
            return $"{fileVer.Major}.{fileVer.Minor}.{fileVer.Build}.{fileVer.Revision}";
        }

        /// <summary>
        /// 设置微信下载路径
        /// </summary>
        private void SetDownloadLinks()
        {
            DownloadLinkLabel.Text = " [ --- 测试版 - 正式版 - 更新日志 --- ]";
            DownloadLinkLabel.Links.Add(new LinkLabel.Link(7, 3, "https://www.redsonw.com/wechatwinbeta.html"));
            DownloadLinkLabel.Links.Add(new LinkLabel.Link(13, 3, "https://dldir1.qq.com/weixin/Windows/WeChatSetup.exe"));
            DownloadLinkLabel.Links.Add(new LinkLabel.Link(19, 4, "https://www.redsonw.com/wechatmo.html"));
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