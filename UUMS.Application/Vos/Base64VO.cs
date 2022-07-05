using System.Text.Json.Serialization;

namespace UUMS.Application.Vos
{
    public class Base64VO
    {
        /// <summary>
        /// base64串
        /// </summary>
        [JsonPropertyName("Content")]
        public string Content { get; set; }

        /// <summary>
        /// 后缀名
        /// </summary>
        [JsonPropertyName("Extension")]
        public string Extension { get; set; }

        /// <summary>
        /// mime类型
        /// </summary>
        [JsonPropertyName("MIME")]
        public string MIME { get; set; }
    }
}
