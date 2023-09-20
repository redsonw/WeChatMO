using Newtonsoft.Json;

namespace YueHuan
{
    public class WeChatUpdate
    {
        [JsonProperty(nameof(WeChat))]
        public required WeChatInfo WeChat { get; set; }
        public string[] WechatVer = Array.Empty<string>();

        /// <summary>
        /// 特征码信息结构
        /// </summary>
        public class WeChatInfo
        {
            public Dictionary<string, VersionInfo>? Version { get; set; }

            public class VersionInfo
            {
                [JsonProperty("offset")]
                public long Offset { get; set; }

                [JsonProperty("oldValue")]
                public byte OldValue { get; set; }

                [JsonProperty("newValue")]
                public byte NewValue { get; set; }
            }
        }

        /// <summary>
        /// 解析在线JSON文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<WeChatUpdate> ParseAsync(string url)
        {
            using var http = new HttpClient();
            var json = await http.GetStringAsync(url);
            return JsonConvert.DeserializeObject<WeChatUpdate>(json)!; // 反序列JSON
        }

        /// <summary>
        /// 获取所有支持的版本列表
        /// </summary>
        /// <returns></returns>
        public string[] UpdateWeChatVersions()
        {
            if (WeChat != null && WeChat.Version != null)
            {
                return WeChat.Version.Keys.ToArray();
            }

            return Array.Empty<string>();
        }
    }
}
