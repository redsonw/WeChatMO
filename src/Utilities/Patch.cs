using System.Diagnostics;

namespace YueHuan
{
    public class LimitRemover(WeChatWin weChatWin, ListBox lstBox)
    {
        private readonly WeChatWin weChatWin = weChatWin;
        private readonly LogMessage logger = new(lstBox);
        private readonly string fileName = "WeChatWin.dll";

        public async void RemoveLimit()
        {
            string filePath = Path.Combine(weChatWin.WeChatPath, $"[{weChatWin.WeChatVersion}]", fileName);
            string? version = FileVersionInfo.GetVersionInfo(filePath).FileVersion;

            if (version == null)
            {
                logger.Add($"获取版本失败，请查看是微信是否安装！！");
                return;
            }

            if (!weChatWin.CheckVersion())
            {
                logger.Add($"当前版本：[{version}] 不支持解除限制，请先安装微信 [{weChatWin.WechatVer[0]}] 或以上版本");
                return;
            }

            bool backupSuccess = Backup(filePath);

            if (!backupSuccess)
            {
                // loggerListBox("备份文件失败，无法继续。");
            }

            bool closeSuccess = CloseWeChat();

            if (!closeSuccess)
            {
                logger.Add("关闭微信进程失败，重启计算机后重试。");
                return;
            }

            (long offset, byte oldValue, byte newValue) values = await GetVersionValues(version);

            if (values == default)
            {
                logger.Add($"当前版本：[{version}] 不支持解除限制，请先安装微信 [{weChatWin.WechatVer[0]}] 或以上版本");
                return;
            }

            logger.Add($"初始化[ {version} ]十六进制偏移量完成...");

            try
            {
                logger.Add("开始载入文件");
                using FileStream fs = new(filePath, FileMode.Open, FileAccess.ReadWrite);
                fs.Seek(values.offset, SeekOrigin.Begin);  // 设定文件开始位置
                logger.Add($"载入成功:{filePath}");

                byte currentValue = (byte)fs.ReadByte(); // 读取指定位置的字节数据
                logger.Add("读取原始数据...");

                if (currentValue == values.oldValue)
                {
                    // 将文件流的位置重新设置为指定的偏移量
                    fs.Seek(values.offset, SeekOrigin.Begin);
                    logger.Add($"返回文件起点准备开始替换数据...");

                    // 写入新的字节数据
                    fs.WriteByte(values.newValue);
                    logger.Add($"修改{fileName} 完成");

                    // 刷新文件流，确保数据写入文件
                    fs.Flush();
                    logger.Add("解除双开限制补丁完成");
                }
                else
                {
                    logger.Add("未找到指定位置的字节数据或数据已被修改！");
                }
            }
            catch (Exception ex)
            {
                logger.Add("文件修改出现错误：" + ex.Message);
            }
        }

        private bool Backup(string filePath)
        {
            string backFile = Path.ChangeExtension(filePath, "bak");

            if (File.Exists(backFile))
            {
                DialogResult result = MessageBox.Show("备份文件已经存在，是否继续备份，选择“是”则继续备份，选择“否”则跳过备份。\r\n请注意：跳过备份会造成不可预知的后果！！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (!weChatWin.Backup())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    logger.Add("跳过备份，如微信无法启动时，请重新安装微信即可修复！");
                    return true;
                }
            }
            else
            {
                logger.Add($"备份已存在：{backFile}");
                return true;
            }
        }

        private bool CloseWeChat()
        {
            string chatName = "WeChat";
            Process[] processes = Process.GetProcessesByName(chatName);
            if (processes.Length > 0)
            {
                DialogResult result = MessageBox.Show("微信已经运行，是否要继续解除限制，选择“是”关闭微信继续，选择“否”则放弃解除限制。", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    foreach (Process process in processes)
                    {
                        try
                        {
                            process.Kill();
                            logger.Add("正在关闭微信，请稍候...");
                            Thread.Sleep(500);
                            logger.Add("关闭微信成功...");
                        }
                        catch (Exception ex)
                        {
                            logger.Add("关闭微信进程失败：" + ex.Message);
                            return false;
                        }
                    }
                }
                else
                {
                    logger.Add("放弃解除限制");
                }
            }

            return true;
        }

        private static async Task<(long offset, byte oldValue, byte newValue)> GetVersionValues(string version)
        {
            string url = "https://www.redsonw.com/WeChat/Update.json";
            WeChatUpdate chatUpdate = await WeChatUpdate.ParseAsync(url);
            if (chatUpdate != null)
            {
                WeChatUpdate.WeChatInfo.VersionInfo versions = chatUpdate.WeChat.Version[version];
                long offset = versions.Offset;
                byte oldValue = versions.OldValue;
                byte newValue = versions.NewValue;
                return (offset, oldValue, newValue);
            }
            else
            {
                return (0, 0, 0);
            }
        }
    }
}
