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
        public readonly string[] WechatVer = new string[] { "3.9.5.65", "3.9.5.73", "3.9.5.81", "3.9.5.91", "3.9.6.22", "3.9.6.29", "3.9.6.33", "3.9.6.43","3.9.6.47" };

        // WeChatPath字段的属性
        public string WeChatPathValue
        {
            get { return WeChatPath; }
        }

        // WeChatVersion字段的属性
        public string WeChatVersionValue
        {
            get { return WeChatVersion; }
        }

        public string WechatTitleValue
        {
            get { return WeChatTitle; }
        }

        public string UninstallStringValue
        {
            get { return UninstallString; }
        }

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

        public bool CheckVersion()
        {
            string fileName = Path.Combine(WeChatPath, $"[{WeChatVersion}]", "WeChatWin.dll");
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(fileName);
            string? version = fileVersionInfo.FileVersion;
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
                string filePath = Path.Combine(WeChatPath, $"[{WeChatVersionValue}]", "WeChatWin.dll");
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
