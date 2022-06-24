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
        /// 文件名(不带后缀)
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; }
    }
}
