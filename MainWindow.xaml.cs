using Microsoft.Win32;
using Microsoft.Web.WebView2.Core;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace HtmlToDesktopConverter
{
    public partial class MainWindow : Window
    {
        private DatabaseBridge? _dbBridge;
        private HtmlParser? _htmlParser;
        private string _currentFilePath = "";

        public MainWindow()
        {
            InitializeComponent();
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            try
            {
                await WebView.EnsureCoreWebView2Async(null);
                WebView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
                StatusText.Text = "✅ WebView2 متصل";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تهيئة WebView2: {ex.Message}");
            }
        }

        private void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "HTML و JSON Files (*.html;*.json)|*.html;*.json|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                FilePath.Text = dialog.FileName;
                _currentFilePath = dialog.FileName;
                FileInfoText.Text = $"الملف: {Path.GetFileName(dialog.FileName)}";
            }
        }

        private async void Convert_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                MessageBox.Show("اختر ملف أولاً");
                return;
            }

            try
            {
                StatusText.Text = "⏳ جاري المعالجة...";

                // قراءة الملف
                string fileContent = await File.ReadAllTextAsync(_currentFilePath);

                // تحليل الملف
                _htmlParser = new HtmlParser();
                string processedHtml = _htmlParser.Parse(fileContent);

                // ربط قاعدة البيانات إذا لزم الأمر
                if (ConnectDB.IsChecked == true)
                {
                    _dbBridge = new DatabaseBridge();
                    bool dbConnected = await _dbBridge.InitializeDatabasesFromPath(@"D:\db");
                    DBStatusText.Text = dbConnected ? "✅ قاعدة البيانات متصلة" : "❌ فشل الاتصال";
                }

                // عرض الملف في WebView2
                await WebView.CoreWebView2.ExecuteScriptAsync($"document.body.innerHTML = `{EscapeForJavaScript(processedHtml)}`;" );

                StatusText.Text = "✅ تم التحميل بنجاح";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ: {ex.Message}");
                StatusText.Text = "❌ حدث خطأ";
            }
        }

        private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(e.WebMessageAsJson);
                string action = data["action"];

                if (action == "queryDatabase")
                {
                    string query = data["query"];
                    var result = _dbBridge?.ExecuteQuery(query);
                    
                    var response = JsonConvert.SerializeObject(new { success = true, data = result });
                    WebView.CoreWebView2.PostWebMessageAsJson(response);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في معالجة الرسالة: {ex.Message}");
            }
        }

        private string EscapeForJavaScript(string text)
        {
            return text.Replace("\\", "\\\\").Replace("`", "\\`").Replace("$", "\\$");
        }
    }
}