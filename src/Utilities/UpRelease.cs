using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace YueHuan
{
    public class GithubRelease
    {
        public string Name { get; set; }
        public string TagName { get; set; }
        public string Body { get; set; }
        public List<Asset> Assets { get; set; }

        public class Asset
        {
            public string Url { get; set; }
            public string Name { get; set; }
            public string ContentType { get; set; }
            public long Size { get; set; }
            public int DownloadCount { get; set; }
            public string BrowserDownloadUrl { get; set; }
        }
    }

    public class GithubUpdate
    {
        private readonly HttpClient _httpClient;
        private readonly string _owner;
        private readonly string _repository;

        public GithubUpdate(string owner, string repository)
        {
            _owner = owner;
            _repository = repository;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.github.com/")
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
        }

        public async Task<GithubRelease> GetLatestRelease()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"repos/{_owner}/{_repository}/releases/latest");
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                dynamic releaseData = JsonConvert.DeserializeObject(content);

                GithubRelease release = new GithubRelease
                {
                    Name = releaseData.name,
                    TagName = releaseData.tag_name,
                    Body = releaseData.body,
                    Assets = new List<GithubRelease.Asset>()
                };

                foreach (var assetData in releaseData.assets)
                {
                    GithubRelease.Asset asset = new GithubRelease.Asset
                    {
                        Url = assetData.url,
                        Name = assetData.name,
                        ContentType = assetData.content_type,
                        Size = assetData.size,
                        DownloadCount = assetData.download_count,
                        BrowserDownloadUrl = assetData.browser_download_url
                    };
                    release.Assets.Add(asset);
                }

                return release;
            }
            catch (HttpRequestException ex)
            {
                // Handle the HTTP request exception here
                throw ex; // Re-throw or handle as appropriate
            }
        }

        public async Task<List<GithubRelease.Asset>> GetReleaseAssets()
        {
            GithubRelease release = await GetLatestRelease();
            return release.Assets;
        }

        public async Task<double> DownloadExe(string filePath, IProgress<double> progress)
        {
            List<GithubRelease.Asset> assets = await GetReleaseAssets();
            GithubRelease.Asset exeAsset = assets.Find(asset => asset.Name.EndsWith(".exe"));

            if (exeAsset != null)
            {
                string exeUrl = exeAsset.BrowserDownloadUrl;

                using HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(exeUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                long? contentLength = response.Content.Headers.ContentLength;

                using Stream stream = await response.Content.ReadAsStreamAsync();
                using FileStream fileStream = File.Create(filePath);

                const int bufferSize = 81920;
                byte[] buffer = new byte[bufferSize];
                long totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer.AsMemory(0, bufferSize))) > 0)
                {
                    await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
                    totalBytesRead += bytesRead;

                    if (contentLength.HasValue)
                    {
                        double percentComplete = ((double)totalBytesRead / contentLength.Value) * 100;
                        progress.Report(percentComplete);
                    }
                }

                return 100; // 下载完成
            }

            return 0; // 如果下载失败，可以返回适当的错误码
        }



        public async Task<string> GetExeDownloadUrl()
        {
            List<GithubRelease.Asset> assets = await GetReleaseAssets();
            GithubRelease.Asset exeAsset = assets.Find(asset => asset.Name.EndsWith(".exe"));
            return exeAsset?.BrowserDownloadUrl;
        }
    }
}
