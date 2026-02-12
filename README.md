# 🚌 İzmit Transit - Akıllı Ulaşım Planlayıcı

Modern, kullanıcı dostu bir web tabanlı ulaşım rotası planlama uygulaması. İzmit şehri için optimize edilmiş, gerçek zamanlı rota planlama, çoklu ulaşım türleri desteği ve kullanıcı deneyimine odaklanmış tasarımıyla öne çıkar.

![İzmit Transit Ana Ekran](screenshots/Ekran%20görüntüsü%202026-02-11%20022831.png)

## ✨ Özellikler

### 🎯 Rota Planlama

- **Akıllı Rota Algoritması**: Otobüs, tramvay, taksi ve yürüme seçeneklerini birleştirerek en uygun rotayı bulur
- **Çoklu Alternatif Rotalar**: En hızlı, en ucuz ve dengeli rota seçenekleri
- **Gerçek Zamanlı Hesaplama**: Mesafe, süre ve maliyet bilgileriyle detaylı rota analizi
- **Aktarma Optimizasyonu**: Minimum aktarma ve yürüme mesafesi ile optimize edilmiş rotalar

### 🗺️ Harita Entegrasyonu

- **Google Maps Tabanlı**: Tam ekran, interaktif harita görünümü
- **Görsel Rota Çizimi**: Farklı ulaşım türleri için renkli ve kesikli çizgi desteği
  - 🚶 Yeşil kesikli çizgi: Yürüme
  - 🚌 Mavi kesikli çizgi: Otobüs
  - 🚊 Mor düz çizgi: Tramvay
  - 🚕 Sarı düz çizgi: Taksi
- **Interaktif Durak İşaretleri**: Tıklanabilir durak bilgileri ve detayları
- **Harita Üzerinden Nokta Seçimi**: Başlangıç ve hedef noktalarını haritadan doğrudan seçebilme

![Rota Detayları](screenshots/Ekran%20görüntüsü%202026-02-11%20022843.png)

### 🎨 Modern Kullanıcı Arayüzü

- **Google Maps Tarzı Tasarım**: Sezgisel ve tanıdık kullanıcı deneyimi
- **Yan Panel Menü**: Daraltılabilir, 3 sekmeli (Rota Planla, Duraklar, Favoriler)
- **Responsive Tasarım**: Masaüstü ve mobil cihazlarda mükemmel görünüm
- **Koyu Tema Entegrasyonu**: Modern, göz yormayan renk paleti
- **Animasyonlu Geçişler**: Akıcı ve profesyonel kullanıcı deneyimi

### 👥 Yolcu Tipleri

- **Genel Yolcu**: Standart ücretlendirme
- **Öğrenci**: İndirimli tarife
- **Yaşlı**: Özel tarife ve avantajlar

### 💳 Ödeme Seçenekleri

- Nakit
- Kredi Kartı
- KentKart (Akıllı Kart)
- Otomatik bakiye kontrolü ve uyarılar

### ⭐ Favori Rotalar

- Sık kullanılan rotaları kaydetme
- Hızlı erişim ve paylaşım
- Link kopyalama ve sosyal medya paylaşımı

![Duraklar ve Hatlar](screenshots/Ekran%20görüntüsü%202026-02-11%20022858.png)

## 🛠️ Teknolojiler

### Backend

- **ASP.NET Core 8.0**: Modern, yüksek performanslı web framework
- **C# 12**: Son nesil programlama dili özellikleri
- **RESTful API**: Temiz ve ölçeklenebilir API mimarisi
- **Dependency Injection**: Modüler ve test edilebilir kod yapısı
- **Dijkstra Algoritması**: Optimum rota bulma için grafik algoritması

### Frontend

- **Vanilla JavaScript**: Framework bağımlılığı olmayan, performanslı kod
- **Google Maps JavaScript API**: Harita ve yönlendirme servisleri
- **Modern CSS3**: Flexbox, Grid, Custom Properties
- **Google Fonts (Inter)**: Profesyonel ve okunabilir tipografi
- **Responsive Design**: Mobil-öncelikli tasarım yaklaşımı

### Özellikler

- **Veri Yönetimi**: JSON tabanlı şehir verisi (duraklar, hatlar, taksi bilgileri)
- **Algoritma**: Grafik tabanlı rota bulma ve optimizasyon
- **Önbellekleme**: Hızlı veri erişimi için akıllı önbellekleme
- **Hata Yönetimi**: Kapsamlı hata yönetimi ve logging

## 📋 Kurulum

### Gereksinimler

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Google Maps API Anahtarı ([buradan alın](https://developers.google.com/maps/documentation/javascript/get-api-key))
- Modern web tarayıcı (Chrome, Firefox, Safari, Edge)

### Adımlar

1. **Projeyi klonlayın**

```bash
git clone https://github.com/ozdemirCeng/IzmitTransportationSystem2web.git
cd IzmitTransportationSystem2web
```

2. **Google Maps API Anahtarını ayarlayın**

`appsettings.json` dosyasını açın ve API anahtarınızı ekleyin:

```json
{
  "GoogleMaps": {
    "ApiKey": "YOUR_GOOGLE_MAPS_API_KEY_HERE"
  }
}
```

`wwwroot/index.html` dosyasında da güncelleyin:

```html
<script src="https://maps.googleapis.com/maps/api/js?key=YOUR_GOOGLE_MAPS_API_KEY&libraries=geometry"></script>
```

3. **Uygulamayı çalıştırın**

```bash
dotnet restore
dotnet build
dotnet run
```

4. **Tarayıcıda açın**

```
http://localhost:5057
```

## 🚀 Kullanım

### Rota Planlama

1. **Başlangıç ve Hedef Seçimi**:
   - Haritaya tıklayarak nokta seçin (önce başlangıç, sonra hedef)
   - veya koordinatları manuel olarak girin
   - veya "Duraklar" sekmesinden durak seçin

2. **Tercihlerinizi Belirleyin**:
   - Yolcu tipi: Genel / Öğrenci / Yaşlı
   - Ödeme yöntemi bakiyeleri
   - Sıralama: En hızlı / En ucuz / Dengeli
   - Filtreler: Taksi dahil et / Yalnız yürüme göster

3. **"Rota Bul"** butonuna tıklayın

4. **Sonuçları İnceleyin**:
   - En iyi rota mavi kutuda gösterilir
   - Alternatif rotalar altta listelenir
   - Haritada rota çizilir ve duraklar gösterilir

### Favoriler

1. Rota planladıktan sonra favori adı girin
2. "Kaydet" butonuna tıklayın
3. "Favoriler" sekmesinden istediğiniz zaman erişin

### Paylaşım

- "Link kopyala" ile rotayı URL olarak paylaşın
- "Paylaş" butonu ile doğrudan mobil paylaşım menüsünü açın

## 📁 Proje Yapısı

```
IzmitTransportationSystem2web/
│
├── Controllers/
│   ├── TransportationController.cs    # Ana API controller
│   └── WeatherForecastController.cs
│
├── Models/                             # Veri modelleri
│   ├── Bus.cs
│   ├── CityData.cs
│   ├── JourneyRequest.cs
│   ├── JourneyRoute.cs
│   ├── Passenger.cs
│   ├── PaymentMethod.cs
│   ├── RouteSegment.cs
│   ├── Stop.cs
│   ├── Taxi.cs
│   ├── Tram.cs
│   └── Vehicle.cs
│
├── Services/                           # İş mantığı servisleri
│   ├── RoutePlannerService.cs         # Rota planlama algoritması
│   └── TransportationDataService.cs   # Veri yönetimi
│
├── wwwroot/                            # Frontend dosyaları
│   ├── index.html                     # Ana sayfa
│   ├── site.css                       # Stil dosyası
│   └── screenshots/                   # Ekran görüntüleri
│
├── veri.json                          # Şehir verileri
├── appsettings.json                   # Yapılandırma
├── Program.cs                         # Uygulama giriş noktası
└── Startup.cs                         # Servis yapılandırması
```

## 🎯 API Endpoint

### POST /api/transportation/planjourney

**Request Body:**

```json
{
  "startLatitude": 40.7654,
  "startLongitude": 29.9403,
  "destinationLatitude": 40.7441,
  "destinationLongitude": 29.91,
  "passengerType": "General",
  "payment": {
    "cashAmount": 100,
    "creditCardLimit": 200,
    "kentKartBalance": 50
  }
}
```

**Response:**

```json
{
  "nearestStartStop": "41 Burda AVM (Bus)",
  "distanceToStartStop": 2.10,
  "nearestEndStop": "Yahya Kaptan (Tram)",
  "distanceFromEndStop": 0.72,
  "optimalRoute": {
    "segments": [...],
    "totalDistance": 5.1,
    "totalDuration": 30,
    "totalFare": 41.5,
    "routeType": "Taxi + Bus"
  },
  "alternativeRoutes": [...],
  "stopLocations": {...}
}
```

## 🎨 Tasarım Özellikleri

- **Renk Paleti**: Google Material Design ilkelerine uygun
  - Mavi (#4285F4): Otobüs rotaları
  - Yeşil (#34A853): Yürüme ve başlangıç işaretleri
  - Kırmızı (#EA4335): Hedef işaretleri
  - Mor (#9334E6): Tramvay rotaları
  - Sarı (#F9AB00): Taksi rotaları

- **Tipografi**: Inter font ailesi (400, 500, 600, 700)
- **Boşluklar**: 4px grid sistemi
- **Border Radius**: 6px - 12px arası yuvarlatılmış köşeler
- **Gölgeler**: Çok katmanlı, yumuşak gölge sistemi

## 🔒 Güvenlik

- API anahtarları environment variable olarak saklanmalıdır
- HTTPS kullanımı önerilir
- CORS politikaları yapılandırılmıştır
- Input validasyonu yapılmaktadır

## 🌐 Tarayıcı Desteği

- Chrome/Edge 90+
- Firefox 88+
- Safari 14+
- Opera 76+
- Mobil tarayıcılar (iOS Safari, Chrome Mobile)

## 📱 Mobil Uyumluluk

- Responsive grid sistem
- Touch-friendly arayüz elemanları
- Mobil için optimize edilmiş harita kontrolleri
- Alt panel tasarımı (mobilde)
- Swipe ve pinch-to-zoom desteği

## 🤝 Katkıda Bulunma

1. Bu repo'yu fork edin
2. Feature branch oluşturun (`git checkout -b feature/GelistirmeAdi`)
3. Değişikliklerinizi commit edin (`git commit -m 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/GelistirmeAdi`)
5. Pull Request oluşturun

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## 👨‍💻 Geliştirici

**Ömer Faruk Özdemir**

- 📧 Email: dev.omer.ozdemir@gmail.com
- 📱 Telefon: 0533 448 64 24
- 🌐 GitHub: [@ozdemirCeng](https://github.com/ozdemirCeng)

## 📞 İletişim

Sorularınız, önerileriniz veya hata bildirimleriniz için:

- Email: dev.omer.ozdemir@gmail.com
- Telefon: 0533 448 64 24

---

⭐ Projeyi beğendiyseniz yıldız vermeyi unutmayın!

🚀 İyi kodlamalar!
