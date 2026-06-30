using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace HtmlToDesktopConverter
{
    public class HtmlParser
    {
        public string Parse(string fileContent)
        {
            // تحديد نوع الملف
            if (fileContent.TrimStart().StartsWith("{"))
            {
                // ملف JSON
                return ParseJson(fileContent);
            }
            else if (fileContent.TrimStart().StartsWith("<!DOCTYPE") || fileContent.TrimStart().StartsWith("<html"))
            {
                // ملف HTML
                return ParseHtml(fileContent);
            }
            else
            {
                return $"<html><body style='color:red'><h1>❌ صيغة ملف غير مدعومة</h1></body></html>";
            }
        }

        private string ParseJson(string jsonContent)
        {
            try
            {
                dynamic data = JsonConvert.DeserializeObject(jsonContent);
                
                // توليد واجهة من JSON
                string html = "<html><head><meta charset='UTF-8'><title>JSON Viewer</title>";
                html += "<style>body{font-family:Arial;background:#f5f5f5;padding:20px}";
                html += ".container{max-width:1200px;margin:0 auto;background:white;padding:20px;border-radius:8px;box-shadow:0 2px 8px rgba(0,0,0,0.1)}";
                html += "h1{color:#333}pre{background:#f0f0f0;padding:10px;border-radius:4px;overflow-x:auto}</style>";
                html += "</head><body><div class='container'>";
                html += "<h1>📊 JSON Data Viewer</h1>";
                html += $"<pre>{JsonConvert.SerializeObject(data, Formatting.Indented)}</pre>";
                html += "</div></body></html>";

                return html;
            }
            catch
            {
                return "<html><body style='color:red'><h1>❌ خطأ في معالجة JSON</h1></body></html>";
            }
        }

        private string ParseHtml(string htmlContent)
        {
            // إضافة أنماط وتحسينات إذا لزم الأمر
            if (!htmlContent.Contains("</head>"))
            {
                htmlContent = htmlContent.Replace("<html>", "<html><head><meta charset='UTF-8'></head>");
            }

            // إضافة WebView2 Bridge للتواصل
            string bridge = "<script>\n" +
                "function sendToDesktop(action, data) {\n" +
                "  window.chrome.webview.postMessage({action: action, data: data});\n" +
                "}\n" +
                "</script>";

            htmlContent = htmlContent.Replace("</head>", bridge + "</head>");
            return htmlContent;
        }
    }
}