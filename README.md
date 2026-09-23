# 🏥 Fındık Özel Klinik - Hastane Randevu Yönetim Sistemi

Modern ve kullanıcı dostu hastane randevu yönetim sistemi. ASP.NET Core ile geliştirilmiş, responsive tasarım ve kapsamlı yönetim özellikleri içeren web uygulaması.

## 📋 İçindekiler

- [Özellikler](#-özellikler)
- [Teknolojiler](#-teknolojiler)
- [Kurulum](#-kurulum)
- [Kullanım](#-kullanım)
- [Roller ve Yetkiler](#-roller-ve-yetkiler)
- [Ekran Görüntüleri](#-ekran-görüntüleri)
- [Katkıda Bulunma](#-katkıda-bulunma)
- [Lisans](#-lisans)

## ✨ Özellikler

### 👥 Hasta Özellikleri
- **Randevu Alma**: Departman ve doktor seçimi ile kolay randevu oluşturma
- **Hızlı Randevu**: Acil durumlar için hızlı randevu alma sistemi
- **Randevu Yönetimi**: Mevcut randevuları görüntüleme ve iptal etme
- **Profil Yönetimi**: Kişisel bilgileri güncelleme

### 👨‍⚕️ Doktor Özellikleri
- **Randevu Takibi**: Günlük, haftalık randevu programını görüntüleme
- **Hasta Geçmişi**: Hasta bilgilerini ve geçmiş randevuları inceleme
- **Randevu Detayları**: Randevu notları ve hasta bilgilerini güncelleme
- **Program Yönetimi**: Çalışma saatlerini düzenleme

### 🔧 Admin Özellikleri
- **Dashboard**: Kapsamlı istatistikler ve sistem genel bakışı
- **Doktor Yönetimi**: Doktor ekleme, düzenleme, silme işlemleri
- **Bölüm Yönetimi**: Hastane bölümlerini organize etme
- **Kullanıcı Yönetimi**: Tüm kullanıcıları yönetme ve rol atama
- **Randevu Yönetimi**: Tüm randevuları görüntüleme ve düzenleme
- **Sistem Ayarları**: Uygulama yapılandırma ayarları
- **Sistem Logları**: Uygulama loglarını izleme
- **Yedekleme**: Veritabanı yedekleme ve geri yükleme

## 🛠️ Teknolojiler

- **Backend**: ASP.NET Core 9.0
- **Frontend**: HTML5, CSS3, JavaScript (ES6+)
- **UI Framework**: Bootstrap 5
- **Database**: SQL Server / SQLite
- **ORM**: Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Icons**: Bootstrap Icons
- **Animations**: AOS (Animate On Scroll)

## 📦 Kurulum

### Gereksinimler
- .NET 9.0 SDK
- SQL Server (LocalDB yeterli)
- Visual Studio 2022 veya Visual Studio Code

### Adımlar

1. **Projeyi klonlayın**
```bash
git clone https://github.com/kullaniciadi/randevu-kayit.git
cd randevu-kayit
```

2. **Bağımlılıkları yükleyin**
```bash
dotnet restore
```

3. **Veritabanını yapılandırın**
```bash
dotnet ef database update
```

4. **Uygulamayı çalıştırın**
```bash
dotnet run
```

5. **Tarayıcıda açın**
```
https://localhost
```

### İlk Kurulum Verisi

Uygulama ilk çalıştığında otomatik olarak şunları oluşturur:
- Admin kullanıcısı (admin@hospital.com / Admin123!)
- Örnek bölümler ve doktorlar
- Test verisi

## 🚀 Kullanım

### Hasta Olarak Giriş
1. Ana sayfadan "Kayıt Ol" ile hesap oluşturun
2. Email doğrulaması yapın
3. "Randevu Al" sekmesinden randevu oluşturun

### Doktor Olarak Giriş
1. Admin tarafından oluşturulan hesapla giriş yapın
2. "Doktor Paneli"nden randevularınızı görüntüleyin
3. Hasta detaylarını ve notları güncelleyin

### Admin Olarak Giriş
1. admin@hospital.com / Admin123! ile giriş yapın
2. Sol menüden tüm yönetim özelliklerine erişin
3. Sistem ayarlarını yapılandırın

## 👤 Roller ve Yetkiler

| Rol | Yetkiler |
|-----|----------|
| **Hasta** | Randevu alma, görüntüleme, iptal etme, profil düzenleme |
| **Doktor** | Randevu görüntüleme, hasta bilgileri, randevu notları |
| **Admin** | Tüm sistem yönetimi, kullanıcı yönetimi, raporlar |

## 📸 Ekran Görüntüleri

### Ana Sayfa
![Ana Sayfa](screenshots/homepage.png)

### Admin Dashboard
![Admin Dashboard](screenshots/admin-dashboard.png)

### Randevu Alma
![Randevu Alma](screenshots/appointment-booking.png)

## 🔒 Güvenlik Özellikleri

- **ASP.NET Core Identity** ile güvenli kimlik doğrulama
- **Role-based Authorization** ile yetki kontrolü
- **HTTPS** zorunlu bağlantı
- **CSRF** koruması
- **SQL Injection** koruması (Entity Framework)
- **XSS** koruması

## 📱 Responsive Tasarım

- Mobil uyumlu tasarım
- Tablet ve desktop optimizasyonu
- Modern Bootstrap 5 bileşenleri
- Smooth animasyonlar

## 🔧 Yapılandırma

### Veritabanı Bağlantısı
`appsettings.json` dosyasında bağlantı dizesini güncelleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RandevuKayitDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Email Ayarları
SMTP ayarlarını `appsettings.json`'da yapılandırın:

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "your-email@gmail.com",
    "SmtpPassword": "your-app-password"
  }
}
```

## 📊 Veritabanı Şeması

### Ana Tablolar
- **Users** (ApplicationUser) - Kullanıcı bilgileri
- **Departments** - Hastane bölümleri
- **Appointments** (Randevu) - Randevu kayıtları
- **QuickAppointments** - Hızlı randevu kayıtları
- **MedicalHistory** - Hasta geçmişi

## 🚀 Deployment

### IIS'e Deployment
```bash
dotnet publish -c Release
```

### Docker ile Deployment
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY . .
EXPOSE 80
ENTRYPOINT ["dotnet", "randevu_kayit.dll"]
```

## 🤝 Katkıda Bulunma

1. Bu repository'yi fork edin
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some AmazingFeature'`)
4. Branch'inizi push edin (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun

## 📝 Geliştirme Notları

### Code Style
- C# Coding Conventions kullanılmıştır
- Clean Code prensipleri uygulanmıştır
- Repository Pattern kullanılmıştır

### Testing
```bash
dotnet test
```

## 🐛 Bilinen Sorunlar

- [ ] Email gönderimi için SMTP yapılandırması gerekli
- [ ] Veritabanı migration'ları manuel çalıştırılmalı

## 📞 İletişim

- **Developer**: Mevlüt Fındık
- **Email**: mevlut201mevlut@gmail.com
  

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakın.

## 🔄 Güncellemeler

### v1.0.0 (2024-06-07)
- ✅ İlk sürüm yayınlandı
- ✅ Temel randevu yönetimi
- ✅ Admin paneli
- ✅ Responsive tasarım

### Roadmap
- [ ] Mobile app geliştirme
- [ ] API geliştirme
- [ ] Notification sistemi
- [ ] Online ödeme entegrasyonu
- [ ] Video görüşme özelliği

---

⭐ **Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!**

📧 **Sorularınız için issue açabilir veya doğrudan iletişime geçebilirsiniz.**
