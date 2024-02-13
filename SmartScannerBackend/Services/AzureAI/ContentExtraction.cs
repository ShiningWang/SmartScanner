using Azure;
using Azure.AI.Vision.ImageAnalysis;

namespace SmartScannerBackend.Services.AzureAI
{
    public interface IContentExtraction
    {
        public Task<long> SetImage(Stream image);

        public Task Resolve();

        public string ReadFullContent();
    }

    public class ContentExtraction : IContentExtraction
    {
        private Stream _stream = new MemoryStream();

        private readonly ImageAnalysisClient _imageAnalysisClient;

        protected string _decodedLines = string.Empty;

        public ContentExtraction()
        {
            _imageAnalysisClient = new ImageAnalysisClient(
                new Uri(System.Configuration.ConfigurationManager.AppSettings["AzureEndpoint"]!),
                new AzureKeyCredential(System.Configuration.ConfigurationManager.AppSettings["AzureKey"]!));
        }

        public async Task<long> SetImage(Stream image)
        {
            await image.CopyToAsync(_stream);

            return _stream.Length;
        }

        public async Task Resolve()
        {
            _stream.Seek(0, SeekOrigin.Begin);

            ImageAnalysisResult readResult =
                await _imageAnalysisClient.AnalyzeAsync(
                    BinaryData.FromStream(_stream), VisualFeatures.Read);

            foreach (var lineData in readResult.Read.Blocks.SelectMany(b => b.Lines))
            {
                _decodedLines += lineData.Text + Environment.NewLine;
            };
        }

        public string ReadFullContent()
        {
            return _decodedLines;
        }
    }
}