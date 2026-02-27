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
| **Dil Motoru:** | Google Translate <br>
| **Entity Framework Core** | Database Provider & Mapping | <br>
| **JavaScript / AJAX** | Dinamik Bilet Modalları & Filtreleme | <br>
| **SweetAlert2** | Kullanıcı Bildirimleri & Onay Dialogları | <br>
| **Bootstrap 5** | Responsive ve Modern Arayüz Tasarımı | <br>


## 📸 Uygulama Arayüz Analizi (Görsel Rehber) <br>

### 🏠 1. Ana Sayfa ve Karşılama Alanı (Hero Section) <br>
* **Arama Motoru:** Kullanıcıların istedikleri lokasyonu ve tur süresini hızlıca filtreleyebileceği interaktif bir search bar alanı. <br>
* **Görsel Tasarım:** Modern, sade ve beyaz ağırlıklı tasarım ile kullanıcıyı yormayan bir deneyim. <br>
* **Hızlı Rezervasyon:** Sağ üst köşede konumlandırılan dikkat çekici "Make a Reservation" butonu ile dönüşüm odaklı yapı. <br>

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

## 🛠️ Teknik Özellikler ve Notlar <br>
* **UI/UX:** Bootstrap 5 ve özel CSS ile responsive (mobil uyumlu) tasarım. <br>
* **Veri Akışı:** ASP.NET Core MVC mimarisi ile temiz ve sürdürülebilir kod yapısı. <br>
* **Dinamik Menü:** Sidebar ve Header alanlarında yetkilendirmeye göre değişen (Customer/Admin) içerik yönetimi. <br>
<img width="1915" height="905" alt="anasayfa1" src="https://github.com/user-attachments/assets/f6078f90-1fb9-4e20-8b03-5fc520ba2dc6" />
<img width="1893" height="914" alt="anasayfa2" src="https://github.com/user-attachments/assets/75e37524-39bb-4bc7-a847-3a46638fe38d" />
<img width="1914" height="908" alt="anasayfa3" src="https://github.com/user-attachments/assets/abbe54a7-9d82-489d-8bdc-64e2d4ddcfc3" />
<img width="1915" height="909" alt="anasayfa4" src="https://github.com/user-attachments/assets/354b771e-8144-4f16-ac0b-69a66e87a7eb" />


## 🏔️ Gelişmiş Tur Sayfaları ve Keşif Ekosistemi <br>

Kullanıcıların hayallerindeki tatili keşfetmeleri için tasarlanan bu bölüm, modern web teknolojileri ile yapay zekanın harmanlandığı, yüksek etkileşimli ve güvenli bir deneyim sunar. <br>

### 🔍 1. Akıllı Arama ve Katmanlı Filtreleme Mimarisi <br>
Sistem, binlerce seçenek arasında kullanıcıyı en doğru rotaya yönlendirmek için kompleks filtreleme algoritmaları kullanır: <br>
* **Dinamik Kategori Navigasyonu:** Turlar; Kamp, Doğa, Plaj gibi tematik gruplara ayrılmıştır. `ViewComponent` mimarisi sayesinde bu kategoriler asenkron olarak filtrelenebilir. <br>
* **AJAX Destekli Hızlı Arama:** Lokasyon, tur tipi ve süre (Gün/Gece) bilgilerine göre sayfa yenilenmeden sonuç döndüren akıllı arama çubuğu. <br>
* **Bürokratik Filtreleme (Vize Durumu):** Kullanıcılar seyahat planlarını "Vize İsteyen" veya "Vize İstemeyen" turlar olarak ayırarak, pasaport ve vize süreçlerine göre rota seçebilirler. <br>
* **Rota Bazlı Keşif:** Tüm tur rotalarının tek bir panelde görselleştirilmesiyle, coğrafi odaklı keşif imkanı sağlanmıştır. <br>

### 🛡️ 2. Güvenli Etkileşim ve Admin Onay Mekanizması <br>
Sistem, veri kalitesini ve güvenliği en üst düzeyde tutmak için çift aşamalı bir kontrol mekanizması kullanır: <br>
* **Moderasyon Sistemi (Admin Approval):** Kullanıcılar tarafından yapılan tüm **yorumlar** ve **rezervasyon talepleri** doğrudan yayına alınmaz; önce Admin paneline gönderilir. İçerik Admin tarafından onaylandıktan sonra sistemde görünür hale gelir. <br>
* **Yetkilendirilmiş Etkileşim:** Sistemin güvenliğini korumak adına; **Giriş yapmayan kullanıcılar yorum yapamaz ve rezervasyon oluşturamaz.** Bu kısıtlama, Identity kütüphanesi ve `[Authorize]` korumasıyla yönetilir. <br>
* **Sosyal Kanıt (Social Proof):** Onaylanmış kullanıcı yorumları ve puanlamaları, şeffaf bir bilgi akışı sağlayarak potansiyel misafirler için güven inşa eder. <br>

### 🎬 3. Zengin Medya ve AI (Yapay Zeka) Entegrasyonu <br>
Tur detay sayfaları, dijital dünyanın en modern görsel araçlarıyla donatılmıştır: <br>
* **AI Destekli Haritalar:** Rotaları ve konaklama noktalarını gösteren yüksek çözünürlüklü harita görselelleri, **Nano Banana (Yapay Zeka)** modeli kullanılarak projeye özel üretilmiştir. <br>
* **Multimedya Deneyimi:** Her turun atmosferini yansıtan tanıtım videoları ve dinamik fotoğraf galerileri ile kullanıcıya tur öncesi dijital bir simülasyon sunulur. <br>
* **Rehber Blogları:** Profesyonel rehberlerin rotalar hakkındaki özel blog yazıları, kullanıcılara rehber deneyimine göre tur seçme şansı tanır. <br>

### 🛠️ 4. Teknik Altyapı ve Veri Yönetimi <br>
* **NoSQL Esnekliği:** Tüm veriler **MongoDB** üzerinde döküman bazlı saklanır. Onay bekleyen yorumların ve rezervasyonların durum takibi (Status: Pending/Approved) MongoDB'nin esnek sorgulama yetenekleri ile hızlıca yönetilir. <br>
* **Modüler Tasarım (ViewComponent):** Tur listeleri, kategori filtreleri ve rehber blogları bağımsız bileşenler olarak geliştirilmiştir. <br>
* **Responsive Grid Sistemi:** Bootstrap 5 altyapısı sayesinde tüm tur sayfaları; masaüstü, tablet ve mobil cihazlarda kusursuz bir görüntüleme sunar. <br>
* **Plan Your Trip (3 Adım Mantığı):** Kullanıcı deneyimini basitleştirmek adına "İsteğini Belirt", "Rotanı Paylaş" ve "Tercihlerini Ayarla" şeklinde kurgulanan interaktif planlama paneli. <br>

<img width="1915" height="911" alt="tur1" src="https://github.com/user-attachments/assets/549d0b4f-1de3-4c9a-89b1-beadd94613e3" />
<img width="1916" height="909" alt="tur2" src="https://github.com/user-attachments/assets/e7050e11-6518-4d34-8132-9618640ad3e9" />
<img width="1916" height="909" alt="tur3" src="https://github.com/user-attachments/assets/37cd0548-6b2c-4565-afeb-d2b629ef039e" />
<img width="1912" height="903" alt="tur4" src="https://github.com/user-attachments/assets/299ed289-dac5-43dd-87af-a295497984b8" />
<img width="1915" height="912" alt="tur5" src="https://github.com/user-attachments/assets/21698564-3569-46dc-90db-ecff0463761f" />
<img width="1918" height="913" alt="tur6" src="https://github.com/user-attachments/assets/d8721e57-d168-46f0-812a-1a6ee37fbff6" />
<img width="1917" height="913" alt="tur7" src="https://github.com/user-attachments/assets/c5c90bab-257e-4260-8cf2-6680df61d805" />
<img width="1038" height="623" alt="tur8" src="https://github.com/user-attachments/assets/9eeaa6c3-60c9-4063-b633-d1551a68aafc" />
<img width="859" height="480" alt="tur9" src="https://github.com/user-attachments/assets/d8a79917-2263-4ebf-9836-f8a7aa2ca611" />
<img width="1346" height="649" alt="tur10" src="https://github.com/user-attachments/assets/28e77e3a-c109-4690-9c14-ed68a0307204" />
<img width="1907" height="808" alt="tur11" src="https://github.com/user-attachments/assets/8f00b226-2f0e-4da2-9203-7f4043fc76cf" />

## 🔐 Kullanıcı Yönetimi ve Yetkilendirme Hiyerarşisi <br>
Travelin, güvenli bir ekosistem sağlamak adına ASP.NET Core Identity altyapısını kullanarak gelişmiş bir rol tabanlı yetkilendirme (Role-Based Access Control) sistemi sunar. <br>

### 👥 1. Otomatik Rol Atama (Default Customer) <br>
* **Hızlı Üyelik:** Kayıt olan her yeni kullanıcı, sistem tarafından otomatik olarak **"Customer" (Müşteri)** rolü ile sınıflandırılır. <br>
* **Anında Erişim:** Müşteriler giriş yaptıkları andan itibaren turları inceleyebilir, yorum yapabilir ve rezervasyon talebi oluşturabilirler. <br>

### 🛡️ 2. Admin Kontrol ve Rehber Atama <br>
* **Merkezi Yönetim:** Sadece Admin yetkisine sahip kullanıcılar tarafından erişilebilen özel yönetim paneli. <br>
* **Görev Dağılımı:** Admin, sistemdeki mevcut üyeler arasından uygun kişilere **"Guide" (Rehber)** rolü atayabilir. Bu sayede rehberler, kendi sorumluluklarındaki turlar için blog yazma ve içerik yönetme yetkisi kazanır. <br>
* **Kullanıcı Modülasyonu:** Admin, kullanıcıların aktif/pasif durumlarını değiştirebilir, yetki seviyelerini güncelleyebilir veya hesapları yönetebilir. <br>

### 🔑 3. Güvenli Giriş ve Yetki Kontrolü <br>
* **Admin Login:** Yönetim paneline erişim, özel bir admin giriş sayfası ve `[Authorize(Roles = "Admin")]` korumasıyla sağlanır. <br>
* **Müşteri Login:** Müşteriler kendilerine özel panel üzerinden rezervasyon geçmişlerini ve onaylanmış biletlerini görüntüleyebilirler. <br>
* **Kimlik Doğrulama:** Şifreleme (Password Hashing) ve Cookie bazlı kimlik doğrulama ile kullanıcı verileri uçtan uca korunur. <br>
<img width="772" height="450" alt="giriş1" src="https://github.com/user-attachments/assets/00ae53af-3874-4478-a98f-1abe8a8805ac" />
<img width="827" height="388" alt="giriş2" src="https://github.com/user-attachments/assets/c6c31b4c-038b-4ff4-b155-d6f35454b776" />


