using Microsoft.Win32;
using System.Diagnostics;

namespace YueHuan
{
    public class WeChatWin
    {
        // 全局变量：WeChatPath、WeChatVersion
        public string WeChatPath = string.Empty;
        public string WeChatVersion = string.Empty;
        public string WeChatTitle = string.Empty;
        public string UninstallString = string.Empty;
        public string[] WechatVer = Array.Empty<string>();

        public WeChatWin()
        {
            // 查找注册表中的主键
            using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\WeChat");
            if (key != null)
            {
                // 读取键值：InstallLocation、DisplayVersion
                WeChatPath = (string)key.GetValue("InstallLocation")!; // 读取微信安装目录
                WeChatVersion = (string)key.GetValue("DisplayVersion")!; // 读取当前微信版本
                WeChatTitle = (string)key.GetValue("DisplayName")!; // 读取微信名称
                UninstallString = (string)key.GetValue("UninstallString")!; // 读取微信卸载文件路径
            }
        }

        public async Task InitializeAsync()
        {
            string url = "https://www.redsonw.com/WeChat/Update.json";
            WeChatUpdate chatUpdate = await WeChatUpdate.ParseAsync(url);
            WechatVer = chatUpdate.WeChat.Version.Keys.ToArray();
        }

        public bool CheckVersion()
        {
            string fileName = Path.Combine(WeChatPath, $"[{WeChatVersion}]", "WeChatWin.dll"); // 获取微信动态库

            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(fileName); // 获取动态库的版本号

            string? version = fileVersionInfo.FileVersion; // 设置动态库版本号

            if (version != null)
            {
                foreach (string v in WechatVer)
                {
                    if (version == v)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // 备份WeChatPath路径下的WeChatWin.dll文件
        public bool Backup()
        {
            try
            {
                string filePath = Path.Combine(WeChatPath, $"[{WeChatVersion}]", "WeChatWin.dll");
                string backupPath = Path.ChangeExtension(filePath, "bak");

                File.Copy(filePath, backupPath, true);
                return true; // 备份成功，返回 true
            }
            catch (Exception ex)
            {
                return false; // 备份失败，返回 false
                throw new Exception($"错误信息：{ex.Message}{ex}");
            }
        }

        // 跳转到指定链接
        public static void OpenURL(string url)
        {
            // 使用默认浏览器打开链接
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }

}
