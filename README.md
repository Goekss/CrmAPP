# 🏢 Enterprise CRM & Projektmanagement System (C# / ASP.NET)

<p align="center">
  <a href="#-deutsch-version">🇩🇪 <b>Deutsch</b></a> &nbsp;|&nbsp; 
  <a href="#-türkçe-surum">🇹🇷 <b>Türkçe</b></a>
</p>

[![C#](https://img.shields.io/badge/Language-C%23-239120?logo=csharp&logoColor=white)](#)
[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-512BD4?logo=dotnet&logoColor=white)](#)
[![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-5.3-blue?logo=dotnet&logoColor=white)](#)
[![ASP.NET Web API](https://img.shields.io/badge/ASP.NET%20Web%20API-2.0-blueviolet?logo=dotnet&logoColor=white)](#)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6.5.1%20(Code--First)-success?logo=dotnet&logoColor=white)](#)
[![Microsoft SQL Server](https://img.shields.io/badge/MS%20SQL%20Server-Database-CC292B?logo=microsoftsqlserver&logoColor=white)](#)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.2.3-7952B3?logo=bootstrap&logoColor=white)](#)
[![jQuery](https://img.shields.io/badge/jQuery-3.7.0-0769AD?logo=jquery&logoColor=white)](#)
[![Two-Factor Authentication](https://img.shields.io/badge/Security-2FA%20%2B%20Lockout-critical?logo=auth0&logoColor=white)](#)
[![ClosedXML](https://img.shields.io/badge/Reporting-ClosedXML%20(Excel)-brightgreen)](#)

---

# 🇩🇪 Deutsch Version

## 📌 Über das Projekt (Unternehmensinterne Management-Plattform & IHK-Abschlussprojekt)

> 📋 **Kompakt-Übersicht (Executive Summary):**  
> **Eigenverantwortliche Konzeption und Umsetzung einer internen CRM- & Projektmanagement-Plattform zur Konsolidierung verteilter Datensysteme in einer zentralen Lösung.**  
> * **4+ Kernmodule:** Kundenverwaltung, Projektsteuerung, Berichtswesen/Dokumentenablage, IT-Support (inkl. Personalverwaltung & Management-Dashboard)  
> * **Tech-Stack:** C#, .NET Framework 4.7.2, ASP.NET MVC 5, RESTful Web API, Entity Framework 6 (Code-First), MS SQL Server, JavaScript/jQuery, Bootstrap 5  
> * **Mehrwert:** Skalierbare, wartungsfreundliche und kosteneffiziente Inhouse-Alternative zu teuren Enterprise-Lizenzen bei maximalem Datenschutz (2FA & Lockout-Schutz).

Dieses Projekt ist eine vollständige, unternehmensnahe **CRM- und Projektmanagement-Webanwendung**, die primär als **interne, zentrale Verwaltungs- und Management-Plattform (Zentrale Administrationsplattform)** für mein damaliges Ausbildungsunternehmen konzipiert und zeitgleich als **offizielles IHK-Abschluss- und Prüfungsprojekt** (Fachinformatiker für Anwendungsentwicklung) mit großem Engagement von Grund auf entwickelt wurde.

Das System kombiniert eine klassische, robuste **ASP.NET MVC Web-Architektur** mit modernen **RESTful Web APIs**, um betriebliche Kernprozesse (Kundenbeziehungen, Projektsteuerung, Personalressourcen, Dokumentenablage und Auswertungen) in einer einzigen, geschützten Unternehmensumgebung effizient, audit-sicher und benutzerfreundlich abzubilden.

---

## 🎯 Was macht dieses System & welcher Nutzen entsteht für das Unternehmen?

### ⚙️ Was macht das System?
* **Zentralisierung aller Unternehmensdaten (Single Source of Truth)**: Beseitigt isolierte Datensilos (dezentrale Excel-Tabellen, E-Mail-Notizen) und führt Kunden-, Projekt-, Personal- und Dokumentendaten in einer strukturierten SQL-Datenbank zusammen.
* **Transparente Projektsteuerung**: Ermöglicht die Zuweisung von mehreren Mitarbeitern pro Projekt, Prioritätseinstufung (`High`, `Middle`, `Low`), Terminüberwachung und Abschlusskontrolle.
* **Sichere Dokumentenablage**: Ersetzt unstrukturierte Netzlaufwerke durch eine projektspezifische Dateiverwaltung (PDF/Office bis 50MB) mit serverseitiger MIME-Prüfung.
* **Echtzeit-Kennzahlen (Management-Dashboard)**: Verschafft der Geschäftsführung und Teamleitern jederzeit einen visuellen Überblick über offene, laufende und fertiggestellte Projekte via ApexCharts.
* **Automatisierter Excel-Export**: Generiert mit einem Klick geschäftsrelevante Auswertungen über ClosedXML ohne manuelle Übertragungsfehler.

### 💼 Welchen betrieblichen & wirtschaftlichen Mehrwert hat es dem Unternehmen gebracht?
1. **⏱️ Erhebliche Zeitersparnis & Prozesseffizienz**: 
   - Das Suchen von Kundenansprechpartnern, Kontakthistorien und Projektständen wurde von durchschnittlich mehreren Minuten auf wenige Sekunden reduziert.
2. **🔒 Höchste Datensicherheit & DSGVO-Konformität**: 
   - Schutz sensibler Kundendaten durch **Zwei-Faktor-Authentifizierung (2FA via E-Mail)**, automatische Kontensperre bei Brute-Force-Angriffen und rollenbasierte Zugriffsberechtigungen (`RBAC`).
3. **📉 Minimierung von Fehlerquellen & Doppelarbeiten**: 
   - Verhindert Missverständnisse bei Kundenabsprachen durch zentral hinterlegte Ansprechpartner und transparente Team-Projektzuweisungen.
4. **💰 Kostenreduktion (Unabhängigkeit von Drittanbietern)**: 
   - Wegfall teurer monatlicher Lizenzkosten für externe CRM-SaaS-Lösungen durch eine maßgeschneiderte, intern wartbare Eigenentwicklung.
5. **🚀 Zukunftsfähigkeit & Skalierbarkeit**: 
   - Dank der integrierten **REST-Web-API** ist die Plattform jederzeit für mobile Apps oder moderne Frontend-Technologien (z. B. React.js) ohne Backend-Umbau erweiterbar.

---

## 🏗️ Software-Architektur & Entwurfsmuster

Die Anwendung folgt bewährten Software-Engineering-Prinzipien und trennt Verantwortlichkeiten strikt nach Best Practices:

* **MVC-Architektur (Model-View-Controller)**: Saubere Entkopplung von Datenmodell, Anwendungslogik und Benutzeroberfläche.
* **ViewModel-Muster (MVVM im Web)**: Strikt typisierte ViewModels (`ViewModels/`) für Validierung (`Data Annotations`), Datenschutz und Vermeidung von Overposting.
* **Code-First ORM mit Entity Framework 6**: Datenbankmodellierung direkt über C#-Entitätsklassen inklusive automatisierter Migrationen.
* **Hybride Backend-Architektur**: 
  - **MVC + Razor View Engine**: Server-seitiges Rendering für das intuitive Management-Dashboard.
  - **ASP.NET Web API 2 mit CORS**: REST-Endpunkte für moderne Frontend-Clients (z. B. React.js) und Drittsysteme.
* **Role-Based Access Control (RBAC)**: Eigener `RoleProvider` (`UserRoller`) zur rollenbasierten Zugriffskontrolle von Aktionen und Views.

---

## 🛠️ Verwendeter Tech-Stack

Projektaufbau und Abhängigkeiten wurden im Quellcode verifiziert:

### Backend & Frameworks
| Technologie | Version | Einsatzbereich |
| :--- | :--- | :--- |
| **.NET Framework** | `4.7.2` | Basis-Laufzeitumgebung |
| **C#** | `Latest` | Backend-Programmiersprache |
| **ASP.NET MVC** | `5.3.0` | Controller, Routing, Filter & Razor Views |
| **ASP.NET Web API** | `5.3.0` | RESTful API Endpoints (`api/kunden`, `api/projekte`) |
| **Entity Framework** | `6.5.1` | ORM (Code-First), LINQ-to-Entities & Migrations |
| **Microsoft SQL Server** | T-SQL | Relationale Datenbank mit relationaler Integrität |
| **MailKit & MimeKit** | `2.15.0` | Asynchroner SMTP-Mailversand für 2FA |
| **ClosedXML / EPPlus** | `0.105.0` / `8.2.1` | Dynamischer Excel-Export für Kunden, Personal & Projekte |

### Frontend & UI / UX (C# Razor & Responsive Design)
| Technologie | Beschreibung |
| :--- | :--- |
| **Razor View Engine (.cshtml)** | Server-seitig gerenderte, dynamische C#-Templates mit modularer Komponenten-Struktur |
| **Bootstrap 5.2.3** | Responsives CSS-Grid-System, flexible Layouts & moderne UI-Komponenten |
| **Custom Styling & SCSS** | Eigenständig entwickelte Stylesheets, anstelle von Utility-First Frameworks (wie Tailwind CSS) bewusst auf wartbare, semantische SCSS/CSS-Klassen und Bootstrap 5 Enterprise-Komponenten gesetzt |
| **JavaScript (ES6+) & jQuery 3.7.0** | Client-seitige Interaktivität, dynamische DOM-Manipulation und AJAX-Aufrufe |
| **jQuery Validation & Unobtrusive** | Lückenlose Validierung auf Client- und Serverebene |
| **ApexCharts & Chart.js & Highcharts** | Interaktive Dashboard-Graphen & 3D-Diagramme zur Geschäftsdaten-Visualisierung |
| **Select2** | Durchsuchbare, benutzerfreundliche Dropdown-Auswahlfelder |
| **WhatsApp Chat Widget** | Schnelle Support- und Kommunikationsanbindung |

---

## 🛡️ Sicherheitskonzept (Security Architecture)

Für den IHK-Kontext wurde ein besonderer Schwerpunkt auf praxisnahe Sicherheitsmechanismen gelegt:

1. **Zwei-Faktor-Authentifizierung (2FA via E-Mail)**:
   - Nach erfolgreicher Passwortprüfung generiert das System einen kryptografisch sicheren 6-stelligen Einmalcode.
   - Der Code wird via `MailKit` im HTML-Design versendet und besitzt ein striktes Ablaufintervall von **5 Minuten**.
2. **Brute-Force-Schutz & Account-Lockout**:
   - Nach **3 aufeinanderfolgenden Fehleingaben** wird das Benutzerkonto automatisch gesperrt (`IsLockedOut = true`, `LockoutTime`).
   - Der Benutzer erhält Rückmeldung über die verbleibenden Anmeldeversuche.
3. **Passwort-Hashing**:
   - Passwörter werden vor der Persistierung sicher gehasht (SHA-256), um Klartextspeicherung auszuschließen.
4. **FormsAuthentication & Custom RoleProvider**:
   - Rollenverwaltung (`Admin`, `Mitarbeiter`, `IT-Support`) über die Klasse `UserRoller`, abgesichert durch `[Authorize(Roles = "...")]`.
5. **CSRF & XSS Schutz**:
   - Konsequenter Einsatz von `@Html.AntiForgeryToken()` in allen POST-Formularen und HTML-Encoding durch Razor.

---

## 🚀 Kernmodule & Funktionsumfang

```
CrmAPP
├── 📊 Dashboard (Echtzeit-KPIs, Projektstatistiken, Status-Verteilungen)
├── 🏢 Kundenverwaltung (CRM, Unternehmensdaten, Logos, Ansprechpartner)
├── 👥 Personalverwaltung (Mitarbeiterkartei, Rollen, Abteilungen, Fotos)
├── 📂 Projektmanagement (Zuweisungen, Deadlines, Prioritäten: High/Mid/Low)
├── 📄 Dokumenten- & Berichtswesen (Upload von PDF/Office bis 50MB, Volltextfilter)
├── 💬 Live-Support & Kommunikation (IT-Support Übersicht, WhatsApp Widget)
└── 🔌 REST Web API (Schnittstellen für React Frontend & externe APIs)
```

### 1. Kundenverwaltung (KundenDatens)
- Vollständige Stammdatenpflege inklusive Firmenlogo-Upload.
- **1:n-Beziehung**: Beliebig viele **Ansprechpartner** (`AnsprechpartnerList`) pro Kunde mit Kontaktdaten und Filialangabe.
- **Excel-Export**: Ein-Klick-Export aller Kundendatensätze via ClosedXML.

### 2. Projektmanagement (PersonalProjekte)
- Planung und Überwachung von IT- und Unternehmensprojekten.
- Zuweisung von Kunden sowie **mehreren Projektmitarbeitern** (`n:m`-Zuordnung).
- Priorisierung (`high`, `middle`, `low`), Statusüberwachung und Abschluss-Workflow (`Fertigstellen`).
- Tabellarischer Excel-Export aller Projektdaten.

### 3. Personalverwaltung (PersonalAngaben)
- Mitarbeiter-Lifecycle, Profilbilder, Kontaktangaben und Abteilungszugehörigkeit.
- Rollenvergabe für Sicherheits- und Zugriffsebenen.

### 4. Berichtswesen & Dokumentenablage (Berichte)
- Zentraler Speicherort für projektrelevante Dokumente (`PDF`, `DOC`, `DOCX`, `XLS`, `XLSX`).
- Dateigrößenbeschränkung bis **50 MB** mit serverseitiger MIME-Type-Prüfung.
- Dynamische Filterung nach Kunde, Projekt und Freitextsuche.

### 5. Management-Dashboard
- Visualisierung des Projektfortschritts über **ApexCharts** und **Chart.js**.
- Live-Zähler für laufende vs. abgeschlossene Projekte sowie Prioritätsverteilungen.

---

## 🔌 RESTful Web API & React-Vorbereitung

Neben den klassischen Razor-Views stellt die Anwendung standardisierte Web-API-Endpunkte bereit, die für den Einsatz mit einem **React-Frontend** vorkonfiguriert sind (CORS aktiviert):

| Methode | Endpunkt | Beschreibung |
| :--- | :--- | :--- |
| `GET` | `/api/kunden` | Liefert alle Kundendaten im JSON-Format |
| `GET` | `/api/kunden/{id}` | Detailansicht eines spezifischen Kunden |
| `GET` | `/api/kunden/personal` | Mitarbeiterliste inklusive vollständiger Bild-URLs |
| `GET` | `/api/projekte` | Vollständige Projektliste mit verknüpften Kunden, Mitarbeitern & Ansprechpartnern |
| `GET` | `/api/projekte/{id}` | Detaillierte Projektinformationen nach ID |
| `GET` | `/api/kunden/weather/{city}` | Drittanbieter-Integration: Live-Wetterdaten über **OpenWeatherMap API** |

---

## ⚙️ Installation & Inbetriebnahme

### Voraussetzungen
* **Visual Studio 2019 / 2022** (mit Workload *ASP.NET und Webentwicklung*)
* **.NET Framework 4.7.2 SDK**
* **Microsoft SQL Server** (LocalDB oder Express)
* **IIS Express** (in Visual Studio integriert)

### Schritt-für-Schritt Einrichtung

1. **Repository klonen**:
   ```bash
   git clone https://github.com/IhrBenutzername/CrmAPP.git
   ```

2. **Projektmappe öffnen**:
   - `CrmAPP.sln` in Visual Studio öffnen.

3. **Verbindungszeichenfolge (Connection String) anpassen**:
   - In `Web.config` den Eintrag `Verbindung` an Ihre SQL Server Instanz anpassen:
   ```xml
   <connectionStrings>
     <add name="Verbindung" 
          connectionString="Data Source=IHRE_INSTANZ\SQLEXPRESS;Initial Catalog=CRMAPP;Integrated Security=SSPI" 
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

4. **NuGet-Pakete wiederherstellen**:
   - Rechtsklick auf die Solution -> **NuGet-Pakete wiederherstellen** (Restore NuGet Packages).

5. **Datenbank über EF-Migrationen initialisieren**:
   - In der Visual Studio Konsole (**Package Manager Console**):
   ```powershell
   Update-Database
   ```

6. **Anwendung starten**:
   - Mit `F5` oder `Strg + F5` im Browser starten (Standard: IIS Express).

---

## 💡 Fazit & Fachliche Erkenntnisse (IHK-Reflexion)

Im Rahmen dieses Abschlussprojekts wurden wesentliche Aspekte moderner Softwareentwicklung praxisnah vertieft:
- Konzeption und Modellierung relationaler Datenstrukturen mit **Entity Framework Code-First**.
- Implementierung eines mehrstufigen **Sicherheits- und Authentifizierungsmodells (2FA, Lockout, RBAC)**.
- Bereitstellung einer sauberen Schnittstelle (**RESTful API**) zur Trennung von Backend-Geschäftslogik und modernen Client-Frameworks (**React.js**).
- Entwicklung ergonomischer Benutzeroberflächen mit **C# Razor Views**, **Bootstrap 5**, individuellem **CSS/SCSS** und dynamischen Visualisierungsbibliotheken.

---
---

# 🇹🇷 Türkçe Sürüm

## 📌 Proje Hakkında (Şirket İçi Merkezi Yönetim Platformu & IHK Bitirme Projesi)

> 📋 **Kısa Proje Özeti (Executive Summary):**  
> **Şirket içi dağınık veri sistemlerini tek bir merkezde toplayan kurumsal CRM ve Proje Yönetim Platformunun uçtan uca mimarisi ve geliştirilmesi.**  
> * **4+ Temel Modül:** Müşteri Yönetimi, Proje Yönetimi, Raporlama/Doküman Arşivi, Canlı Destek (ek olarak Personel Yönetimi & Yönetici Dashboard'u)  
> * **Teknoloji Yığını:** C#, .NET Framework 4.7.2, ASP.NET MVC 5, RESTful Web API, Entity Framework 6 (Code-First), MS SQL Server, JavaScript/jQuery, Bootstrap 5  
> * **Sağlanan Katma Değer:** Yüksek maliyetli kurumsal yazılımlara karşı ölçeklenebilir, bakımı kolay, sıfır lisans maliyetli ve üst düzey güvenlikli (2FA & Hesap Kilitleme) yerel şirket içi alternatif.

Bu proje; çalıştığım şirketin tüm operasyonel süreçlerini tek bir merkezden organize etmek amacıyla **şirket içi (intern) merkezi bir yönetim platformu (central management platform)** olarak tasarlanmış ve aynı zamanda Almanya **IHK (Fachinformatiker für Anwendungsentwicklung)** bitirme ve sınav projem olarak büyük bir emekle sıfırdan hayata geçirilmiştir.

Uygulama; güçlü ve güvenilir **ASP.NET MVC** mimarisini modern **RESTful Web API** altyapısıyla birleştirerek müşteri ilişkileri, proje yönetimi, ekip koordinasyonu, kurumsal doküman arşivleme ve iş zekası analizlerini şirketin iç kullanımına özel, güvenli ve pratik tek bir platform altında toplamaktadır.

---

## 🎯 Bu Sistem Ne Yapıyor ve Şirkete Ne Kazandırdı?

### ⚙️ Bu Proje Ne Yapıyor?
* **Tüm Veriyi Tek Merkezde Toplar (Single Source of Truth)**: Şirket içinde dağınık halde bulunan Excel tabloları, e-posta notları ve masaüstü dosyalarını ortadan kaldırarak; müşteri, personel, proje ve doküman bilgilerini ilişkisel bir SQL veritabanında toplar.
* **Uçtan Uca Proje Takibi**: Projelere birden fazla çalışan (ekip) ve müşteri atamayı, önceliklendirmeyi (`High`, `Middle`, `Low`), teslim tarihlerini ve tamamlama durumlarını canlı takip eder.
* **Güvenli Dosya ve Rapor Arşivi**: Proje ve müşterilere ait teknik şartnameleri, sözleşmeleri ve raporları (50MB'a kadar PDF, Word, Excel) MIME tipi kontrolüyle güvenle saklar.
* **Gerçek Zamanlı Yönetici Özeti (Dashboard)**: Şirket yönetimine ve departman şeflerine devam eden işlerin durumunu interaktif grafiklerle (ApexCharts/Chart.js) anlık olarak sunar.
* **Otomatik Excel Raporlama**: Tek tıkla tüm müşteri ve proje verilerini ClosedXML ile kurumsal Excel formatında dışa aktarır.

### 💼 Şirkete Ne Kazandırdı? (İş Değeri & Somut Faydalar)
1. **⏱️ Ciddi Zaman Tasarrufu ve Operasyonel Hız**:
   - Personelin dağınık Excel dosyalarında müşteri veya proje detayı arama süresini dakikalardan birkaç saniyeye indirdi; günlük operasyonel verimliliği belirgin şekilde artırdı.
2. **🔒 Üst Düzey Veri Güvenliği ve KVKK/GDPR Uyumluluğu**:
   - Hassas müşteri ve personel verileri e-postalarda dolaşmak yerine; **2FA (E-posta ile İki Faktörlü Doğrulama)**, 3 hatalı girişte hesap kilitleme ve rol bazlı erişim denetimi (`RBAC`) ile koruma altına alındı.
3. **📉 Hata Oranlarının ve Mükerrer İşlerin Önlenmesi**:
   - Müşteriye kimin baktığı, projede kimlerin görevli olduğu şeffaflaştı; ekipler arası iletişim kopuklukları ve mükerrer iş yapma riski sıfırlandı.
4. **💰 Maliyet Avantajı (Sıfır Lisans Maliyeti)**:
   - Aylık kullanıcı başına yüksek döviz maliyeti çıkaran harici yabancı CRM yazılımlarına olan bağımlılığı ortadan kaldırdı; şirkete özel, sıfır lisans bedelli yerel bir sistem sağladı.
5. **🚀 Geleceğe Hazır Mimari (Skalabilite)**:
   - Entegre edilen **RESTful Web API** sayesinde, backend koduna dokunmadan gelecekte sisteme React tabanlı web arayüzleri veya mobil uygulamalar bağlama esnekliği kazandırdı.

---

## 🏗️ Yazılım Mimarisi ve Tasarım Desenleri

Proje, kurumsal yazılım geliştirme standartlarına uygun, sürdürülebilir ve modüler bir mimari üzerinde inşa edilmiştir:

* **MVC (Model-View-Controller) Deseni**: İş mantığı, veritabanı modelleri ve arayüz katmanları birbirinden tamamen ayrılmıştır.
* **ViewModel (Web MVVM) Yaklaşımı**: `ViewModels/` katmanında bulunan özel sınıflar sayesinde, varlık modelleri (Entities) doğrudan arayüze maruz bırakılmaz; `Data Annotations` ile eksiksiz veri doğrulaması ve Overposting koruması sağlanır.
* **Code-First ORM (Entity Framework 6)**: Veritabanı tabloları ve ilişkileri C# sınıfları üzerinden modellenmiş, veritabanı şeması otomatik `Migrations` mekanizması ile yönetilmiştir.
* **Hibrit Backend Mimarisi**:
  - **MVC + Razor View Engine**: Yönetim platformu için sunucu taraflı derlenen, hızlı ve SEO dostu dinamik `.cshtml` sayfaları.
  - **ASP.NET Web API 2 (CORS Destekli)**: React.js gibi modern Single Page Application (SPA) frontend'ler veya mobil istemciler için JSON tabanlı RESTful servisler.
* **Rol Tabanlı Yetkilendirme (RBAC)**: Özel olarak geliştirilen `RoleProvider` (`UserRoller`) sınıfı sayesinde, kullanıcı yetkilerine göre (`Admin`, `Mitarbeiter`, `IT-Support`) sayfa ve aksiyon bazlı erişim denetimi.

---

## 🛠️ Kullanılan Teknoloji Yığını (Tech Stack)

Projenin tüm bağımlılıkları kaynak kod üzerinden doğrulanmıştır:

### Backend ve Çerçeveler
| Teknoloji | Versiyon | Görev / Kullanım Alanı |
| :--- | :--- | :--- |
| **.NET Framework** | `4.7.2` | Ana çalışma zamanı (Runtime) |
| **C#** | `Latest` | Backend programlama dili |
| **ASP.NET MVC** | `5.3.0` | Controller, Routing, Filter mekanizmaları ve Razor görünümleri |
| **ASP.NET Web API** | `5.3.0` | Harici ve frontend entegrasyonları için RESTful servisler |
| **Entity Framework** | `6.5.1` | ORM (Code-First), LINQ sorguları ve Migration yönetimi |
| **Microsoft SQL Server** | T-SQL | İlişkisel veritabanı (RDBMS) |
| **MailKit & MimeKit** | `2.15.0` | 2FA onay kodları için asenkron SMTP e-posta servisi |
| **ClosedXML / EPPlus** | `0.105.0` / `8.2.1` | Müşteri, personel ve projeler için dinamik Excel dışa aktarımı |

### Frontend & UI / UX (C# Razor ve Responsive Tasarım)
| Teknoloji | Açıklama |
| :--- | :--- |
| **Razor View Engine (.cshtml)** | Sunucu taraflı derlenen dinamik C# şablonları ve modüler bileşen yapısı |
| **Bootstrap 5.2.3** | Mobil uyumlu (Responsive) grid sistemi ve modern UI bileşenleri |
| **Custom Styling & SCSS** | Utility-first yaklaşımlar (Tailwind CSS gibi) yerine; kurumsal, bakımı kolay, semantik CSS/SCSS sınıfları ve özgün dashboard tasarımı |
| **JavaScript (ES6+) & jQuery 3.7.0** | İstemci taraflı etkileşimler, DOM manipülasyonu ve asenkron AJAX istekleri |
| **jQuery Validation & Unobtrusive** | İstemci ve sunucu tarafında çift katmanlı güvenli form doğrulama |
| **ApexCharts, Chart.js & Highcharts** | Dashboard KPI metrikleri, pasta/çubuk grafikler ve 3D görselleştirmeler |
| **Select2** | Hızlı arama ve filtreleme özelliğine sahip gelişmiş seçim kutuları |
| **WhatsApp Chat Widget** | Müşteriler ve ekip için anlık hızlı mesajlaşma desteği |

---

## 🛡️ Güvenlik Mimarisi (Security Architecture)

IHK bitirme projesinde özellikle kurumsal güvenlik standartlarına öncelik verilmiştir:

1. **İki Faktörlü Doğrulama (2FA - E-posta ile)**:
   - Başarılı kullanıcı adı ve şifre girişinin ardından sistem, arka planda kriptografik olarak güvenli 6 haneli tek kullanımlık kod üretir.
   - Kod, `MailKit` kütüphanesiyle şık bir HTML e-posta olarak gönderilir ve **5 dakikalık katı bir geçerlilik süresi** bulunur.
2. **Brute-Force Koruması ve Hesap Kilitleme (Account Lockout)**:
   - Üst üste **3 hatalı parola denemesinde** ilgili hesap otomatik olarak kilitlenir (`IsLockedOut = true`, `LockoutTime`).
   - Kullanıcıya her denemede kalan hak sayısı şeffaf olarak gösterilir.
3. **Parola Güvenliği (Hashing)**:
   - Parolalar veritabanında kesinlikle açık metin (plaintext) olarak saklanmaz; güvenli hash algoritması (SHA-256) ile şifrelenir.
4. **FormsAuthentication & Özel RoleProvider**:
   - `UserRoller` sınıfı üzerinden rol bazlı yetkilendirme uygulanır; yetkisiz erişimler `[Authorize(Roles = "...")]` nitelikleriyle engellenir.
5. **CSRF & XSS Koruması**:
   - Tüm POST formlarında `@Html.AntiForgeryToken()` doğrulaması ve Razor motorunun otomatik HTML kodlama (Encoding) mekanizması kullanılır.

---

## 🚀 Temel Modüller ve Yetenekler

```
CrmAPP
├── 📊 Yönetici Paneli (Dashboard - Gerçek zamanlı grafikler, proje durumları)
├── 🏢 Müşteri Yönetimi (CRM, firma bilgileri, logolar, çoklu yetkili kişiler)
├── 👥 Personel Yönetimi (Çalışan kartları, roller, departmanlar, fotoğraflar)
├── 📂 Proje Yönetimi (Ekip atama, teslim tarihleri, öncelik seviyeleri)
├── 📄 Doküman & Rapor Yönetimi (50MB'a kadar PDF/Word/Excel arşivi ve filtreleme)
├── 💬 Canlı Destek & İletişim (IT Destek listesi, WhatsApp entegrasyonu)
└── 🔌 REST Web API (React.js frontend ve harici servisler için hazır uç noktalar)
```

### 1. Müşteri Yönetimi (KundenDatens)
- Firma profili oluşturma, kurumsal logo yükleme ve iletişim bilgileri yönetimi.
- **1:N İlişki**: Bir müşteriye bağlı birden fazla **Yetkili Kişi (Ansprechpartner)** tanımlayabilme (şube, telefon, e-posta).
- **Excel Dışa Aktarımı**: Tüm müşteri portföyünü ClosedXML ile anında Excel tablosuna dönüştürme.

### 2. Proje Yönetimi (PersonalProjekte)
- Şirket içi ve müşteri bazlı projelerin uçtan uca planlanması ve takibi.
- Bir projeye hem müşteri hem de **birden fazla personel (ekip)** atayabilme (`n:m` ilişki).
- Öncelik derecelendirmesi (`high`, `middle`, `low`), durum takibi ve tamamlama (`Fertigstellen`) kontrolü.
- Proje listesini tek tıkla Excel formatında raporlama.

### 3. Personel Yönetimi (PersonalAngaben)
- Çalışan profilleri, biyografi, profil fotoğrafları, departman ve unvan yönetimi.
- Sistem erişim rolleri tanımlama ve yetki sınırlandırması.

### 4. Raporlama ve Doküman Arşivi (Berichte)
- Projelere ve müşterilere özel dosya arşivleme (`PDF`, `DOC`, `DOCX`, `XLS`, `XLSX`).
- **50 MB** dosya boyutu sınırı ve sunucu taraflı katı MIME-type doğrulaması.
- Müşteri, proje ve metin bazlı gelişmiş filtreleme ve indirme altyapısı.

### 5. Dashboard ve İş Analitiği
- **ApexCharts** ve **Chart.js** ile desteklenen görsel iş zekası paneli.
- Tamamlanan, devam eden ve geciken projelerin canlı sayaçları ve öncelik dağılım grafikleri.

---

## 🔌 RESTful Web API & React Entegrasyonu

MVC arayüzüne ek olarak sistem, **React.js** gibi modern frontend kütüphaneleriyle haberleşebilecek standart REST API uç noktaları sunar (CORS yapılandırması hazırdır):

| HTTP Metodu | Endpoint | Açıklama |
| :--- | :--- | :--- |
| `GET` | `/api/kunden` | Tüm müşteri listesini JSON formatında döner |
| `GET` | `/api/kunden/{id}` | Belirtilen ID'ye sahip müşterinin detay bilgilerini döner |
| `GET` | `/api/kunden/personal` | Personel listesini tam görsel URL'leri ile birlikte sunar |
| `GET` | `/api/projekte` | Müşteri, personel ve yetkili detaylarıyla tam proje listesi |
| `GET` | `/api/projekte/{id}` | Seçilen projenin ilişkisel tüm ayrıntılarını döner |
| `GET` | `/api/kunden/weather/{city}` | Üçüncü taraf servis: **OpenWeatherMap API** üzerinden canlı hava durumu verisi |

---

## ⚙️ Kurulum ve Çalıştırma Rehberi

### Gereksinimler
* **Visual Studio 2019 veya 2022** (*ASP.NET ve web geliştirme* iş yükü kurulu olmalıdır)
* **.NET Framework 4.7.2 SDK**
* **Microsoft SQL Server** (LocalDB, Express veya tam sürüm)
* **IIS Express** (Visual Studio ile birlikte gelir)

### Adım Adım Kurulum

1. **Depoyu Klonlayın**:
   ```bash
   git clone https://github.com/KullaniciAdiniz/CrmAPP.git
   ```

2. **Çözümü (Solution) Açın**:
   - `CrmAPP.sln` dosyasını Visual Studio ile açın.

3. **Veritabanı Bağlantı Dizesini (Connection String) Düzenleyin**:
   - `Web.config` dosyasındaki `Verbindung` adlı bağlantı dizesini kendi SQL Server sunucunuza göre güncelleyin:
   ```xml
   <connectionStrings>
     <add name="Verbindung" 
          connectionString="Data Source=SUNUCU_ADINIZ\SQLEXPRESS;Initial Catalog=CRMAPP;Integrated Security=SSPI" 
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

4. **NuGet Paketlerini Geri Yükleyin**:
   - Çözüme sağ tıklayın -> **NuGet Paketlerini Geri Yükle** (Restore NuGet Packages).

5. **Veritabanını EF Migrations ile Oluşturun**:
   - Visual Studio içinde **Paket Yöneticisi Konsolu**'nu (Package Manager Console) açın ve çalıştırın:
   ```powershell
   Update-Database
   ```

6. **Projeyi Başlatın**:
   - `F5` veya `Ctrl + F5` tuşlarına basarak projeyi tarayıcınızda başlatın.

---

## 💡 Sonuç ve Mesleki Kazanımlar (IHK Değerlendirmesi)

Bu bitirme projesi sürecinde kurumsal bir yazılımın yaşam döngüsüne dair önemli kazanımlar elde edilmiştir:
- **Entity Framework Code-First** ile ilişkisel veritabanı modellemesi ve migration yönetimi.
- Çok katmanlı **güvenlik ve kimlik doğrulama mimarisi (2FA, Brute-Force Kilitleme, RBAC)** kurgulanması.
- Backend iş mantığını modern istemcilere (**React.js**) açan standart bir **RESTful Web API** tasarımı.
- **C# Razor Views**, **Bootstrap 5**, özel **CSS/SCSS** ve dinamik grafik kütüphaneleriyle kullanıcı dostu, ergonomik arayüz geliştirme deneyimi.
