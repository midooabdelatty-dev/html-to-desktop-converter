# 🎨 HTML to Desktop Converter

أداة احترافية لتحويل ملفات HTML و JSON إلى تطبيقات سطح مكتب Windows باستخدام WPF و WebView2.

## ✨ المميزات

- ✅ تحويل HTML إلى تطبيق سطح مكتب فوراً
- ✅ دعم ملفات JSON وعرضها بصيغة مرتبة
- ✅ ربط تلقائي مع قواعد البيانات (D:\db)
- ✅ دعم ONNX Models للتنبؤات
- ✅ WebView2 Bridge للتواصل بين C# و JavaScript
- ✅ واجهة داكنة احترافية

## 🚀 البدء السريع

### المتطلبات
- .NET 8.0 Windows Desktop
- Visual Studio 2022 أو أعلى
- WebView2 Runtime

### التثبيت

```bash
git clone https://github.com/midooabdelatty-dev/html-to-desktop-converter.git
cd html-to-desktop-converter
dotnet restore
dotnet build
```

### التشغيل

```bash
dotnet run
```

## 📖 الاستخدام

1. **اختر ملف HTML أو JSON**
2. **اضغط "تحويل وتشغيل"**
3. **سيتم عرض التطبيق في النافذة مباشرة**

### ربط قاعدة البيانات

اختر "ربط قاعدة البيانات" وسيتم البحث تلقائياً عن جميع ملفات `.db` في `D:\db`

## 🏗️ البنية

```
.
├── MainWindow.xaml          # الواجهة الرئيسية
├── MainWindow.xaml.cs       # منطق الواجهة
├── Core/
│   ├── HtmlParser.cs        # معالج HTML/JSON
│   └── DatabaseBridge.cs    # ربط قاعدة البيانات
└── README.md
```

## 🔧 التطوير

### إضافة ميزة جديدة

1. انشئ فرع جديد: `git checkout -b feature/your-feature`
2. اكتب الكود
3. اعمل commit: `git commit -m "Add feature"`
4. ادفع التغييرات: `git push origin feature/your-feature`
5. افتح Pull Request

## 📝 الترخيص

هذا المشروع مرخص تحت MIT License - انظر ملف LICENSE للتفاصيل.

## 👨‍💻 المطور

محمد عبد العظيم - [@midooabdelatty-dev](https://github.com/midooabdelatty-dev)

## 📞 التواصل

للأسئلة والاقتراحات، افتح issue على GitHub.
