using OpenCvSharp;
using Sdcb.PaddleOCR.KnownModels;

namespace Sdcb.PaddleOCR.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task FastCheckOCR()
        {
            OCRModel model = KnownOCRModel.EnglishPPOcrV3;
            await model.EnsureAll();

            byte[] sampleImageData;
            string sampleImageUrl = @"https://visualstudio.microsoft.com/wp-content/uploads/2021/11/Home-page-extension-visual-updated.png";
            using (HttpClient http = new HttpClient())
            {
                Console.WriteLine("Download sample image from: " + sampleImageUrl);
                sampleImageData = await http.GetByteArrayAsync(sampleImageUrl);
            }

            using (PaddleOcrAll all = new PaddleOcrAll(model.RootDirectory, model.KeyPath)
            {
                AllowRotateDetection = true, /* ����ʶ���нǶȵ����� */
                Enable180Classification = false, /* ����ʶ����ת�Ƕȴ���90�ȵ����� */
            })
            {
                // Load local file by following code:
                // using (Mat src2 = Cv2.ImRead(@"C:\test.jpg"))
                using (Mat src = Cv2.ImDecode(sampleImageData, ImreadModes.Color))
                {
                    PaddleOcrResult result = all.Run(src);
                    Console.WriteLine("Detected all texts: \n" + result.Text);
                    foreach (PaddleOcrResultRegion region in result.Regions)
                    {
                        Console.WriteLine($"Text: {region.Text}, Score: {region.Score}, RectCenter: {region.Rect.Center}, RectSize:    {region.Rect.Size}, Angle: {region.Rect.Angle}");
                    }
                }
            }
        }
    }
}