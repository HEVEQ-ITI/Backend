using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.AI
{
  
    public sealed class AiSettings
    {
        public AwsBedrockSettings AwsBedrock { get; set; } = new();
        public QdrantSettings Qdrant { get; set; } = new();
    }

    public sealed class AwsBedrockSettings
    {
        public string GatewayUrl { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string ModelId { get; set; } = "anthropic.claude-3-5-sonnet-20240620-v1:0"; // مثال لموديل متوقع
    }

    public sealed class QdrantSettings
    {
        public string Host { get; set; } = "http://localhost:6333";
        public string ApiKey { get; set; } = string.Empty;
    }
}
