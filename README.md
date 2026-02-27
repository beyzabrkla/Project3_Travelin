# 🌍 Travelin - Modern Tour & Reservation Management System 

Travelin, gezginlerin hayallerindeki turları keşfetmesini ve kolayca rezervasyon yapmasını sağlayan, dinamik, güvenli ve kullanıcı dostu bir tur yönetim platformudur. <br>

## 🚀 Öne Çıkan Özellikler 

### 🎫 Dijital Bilet Sistemi <br>
* **Anlık Bilet Oluşturma:** Rezervasyon tamamlandığında kullanıcıya özel, detaylı dijital bilet üretilir. <br>
* **QR Kod Entegrasyonu:** Her bilet, güvenlik ve hızlı kontrol için benzersiz bir QR kod ve PNR numarası içerir. <br>
* **Dinamik Fiyatlandırma:** Kişi sayısına göre anlık toplam tutar hesaplama ve bilet üzerinde detaylı gösterim. <br>
* **Rehber Bağlantısı:** Her rezervasyon, tur rehberinin bilgileri ve görseli ile dinamik olarak ilişkilendirilir. <br>

### 🔐 Gelişmiş Kullanıcı Yönetimi <br>
* **Identity & Role Based Authorization:** Admin ve Customer rolleri ile yetkilendirilmiş erişim kontrolü. <br>
* **Gelişmiş Profil Paneli:** Kullanıcıların aktif/pasif durum takibi, sosyal medya bağlantıları ve üyelik detayları. <br>
* **Güvenli Giriş:** Modern login/register süreçleri ve şifreleme altyapısı. <br>

### 🛠 Teknik Mimari & Veri Yönetimi <br>
* **NoSQL Veritabanı:** Projenin tüm veri yükü yüksek performanslı **MongoDB** üzerinde taşınmaktadır. <br>
* **Asenkron Programlama:** Tüm veritabanı işlemleri (CRUD) UI bloklamasını önlemek için `async/await` yapısıyla kurgulanmıştır. <br>
* **Cross-Reference Data:** Tur, Rehber ve Rezervasyon tabloları arasında kod seviyesinde kurulan ilişkilerle veri tutarlılığı sağlanmıştır. <br>

## 🛠 Kullanılan Teknolojiler <br>

| Teknoloji | Kullanım Alanı | <br>
| **ASP.NET Core 8.0** | Ana Framework & MVC Mimarisi | <br>
| **MongoDB** | NoSQL Veritabanı Yönetimi | <br>
| **Identity Library** | Kimlik Doğrulama ve Yetkilendirme | <br>
| **Dil Motoru:** | Google Translate API <br>
| **Entity Framework Core** | Database Provider & Mapping | <br>
| **JavaScript / AJAX** | Dinamik Bilet Modalları & Filtreleme | <br>
| **SweetAlert2** | Kullanıcı Bildirimleri & Onay Dialogları | <br>
| **Bootstrap 5** | Responsive ve Modern Arayüz Tasarımı | <br>


## 📸 Uygulama Arayüz Analizi (Görsel Rehber) <br>

### 🏠 1. Ana Sayfa ve Karşılama Alanı (Hero Section) <br>
* **Arama Motoru:** Kullanıcıların istedikleri lokasyonu ve tur süresini hızlıca filtreleyebileceği interaktif bir search bar alanı. <br>
* **Görsel Tasarım:** Modern, sade ve beyaz ağırlıklı tasarım ile kullanıcıyı yormayan bir deneyim. <br>
* **Hızlı Rezervasyon:** Sağ üst köşede konumlandırılan dikkat çekici "Make a Reservation" butonu ile dönüşüm odaklı yapı. <br>

<img width="1893" height="914" alt="anasayfa2" src="https://github.com/user-attachments/assets/75e37524-39bb-4bc7-a847-3a46638fe38d" />
<img width="1915" height="905" alt="anasayfa1" src="https://github.com/user-attachments/assets/f6078f90-1fb9-4e20-8b03-5fc520ba2dc6" />
<img width="1914" height="908" alt="anasayfa3" src="https://github.com/user-attachments/assets/abbe54a7-9d82-489d-8bdc-64e2d4ddcfc3" />
<img width="1915" height="909" alt="anasayfa4" src="https://github.com/user-attachments/assets/354b771e-8144-4f16-ac0b-69a66e87a7eb" />


### 🧭 2. Tur Kategorileri ve Planlama <br>
* **Dinamik Kategori Kartları:** Kamp, Doğa Yürüyüşü, Plaj Turları gibi tur türleri özel ikonlarla sınıflandırılmıştır. <br>
* **3 Adımda Kusursuz Tur:** Kullanıcıya sürecin nasıl işlediğini (İsteğini Belirt, Rotanı Paylaş, Tercihlerini Ayarla) anlatan bilgi panelleri. <br>
* **Güven Endeksi:** 20 yıllık deneyim, 530+ tur paketi ve 850+ mutlu müşteri gibi verilerle desteklenen istatistik bölümü. <br>

### 📋 3. Kullanıcı Yönetimi ve Güvenlik (Admin Panel) <br>
* **Kullanıcı Listeleme:** Sistemde kayıtlı kullanıcıların rolleri (Admin/Customer) ve e-posta adresleri ile listelendiği yönetim arayüzü. <br>
* **Anlık Durum Kontrolü:** "Active" veya "Passive" durumları üzerinden kullanıcı erişim yönetimi. <br>
* **Eylem Menüsü:** Kullanıcıyı sistemden silme, profilini görüntüleme veya yetki seviyesini güncelleme fonksiyonları. <br>

### 🗄️ 4. Veri Katmanı ve MongoDB Altyapısı <br>
* **NoSQL Veri Yönetimi:** Klasik tablo yapısı yerine esnek döküman bazlı MongoDB koleksiyonları kullanılmaktadır. <br>
* **Sorgu Performansı:** MongoDB Shell üzerinden yapılan `db["Users"].find()` gibi komutlarla veritabanı seviyesinde anlık veri manipülasyonu ve izleme. <br>
* **Yüksek Ölçeklenebilirlik:** NoSQL yapısı sayesinde büyük veriler altında bile yüksek performanslı çalışma kapasitesi. <br>

### 📱 5. Bilgi Alanı ve Footer (Alt Bilgi) <br>
* **Zengin Footer İçeriği:** Tur fotoğrafları, hızlı bağlantılar ve kurumsal bilgilerle donatılmış geniş alt panel. <br>
* **Bülten Aboneliği:** Kullanıcıların yeni turlardan haberdar olmasını sağlayan "Newsletter" entegrasyonu. <br>
* **Global Ödeme Desteği:** Visa, Mastercard gibi ödeme yöntemleri ikonları ile profesyonel güven algısı. <br>

---

## 🛠️ Teknik Özellikler ve Notlar <br>

* **UI/UX:** Bootstrap 5 ve özel CSS ile responsive (mobil uyumlu) tasarım. <br>
* **Veri Akışı:** ASP.NET Core MVC mimarisi ile temiz ve sürdürülebilir kod yapısı. <br>
* **Dinamik Menü:** Sidebar ve Header alanlarında yetkilendirmeye göre değişen (Customer/Admin) içerik yönetimi. <br>

---
