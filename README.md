# Project Runix

Unity ile geliştirdiğim, tur tabanlı bir kart savaşı oyunu. Kartları slotlara yerleştirip rakibinle sırayla savaşıyor, her zaferden sonra desteni güçlendiriyor ve haritanın sonunda bir boss ile karşılaşıyorsun.

<!-- Ekran görüntüsü ve video ekledikten sonra yorum işaretlerini kaldır:
![Savaş ekranı](Docs/images/battle.png)
▶️ [Oynanış videosu](BAGLANTI) · [Oynanabilir sürüm](BAGLANTI)
-->

## Nasıl oynanır
1. Elindeki kartlardan istediklerini ekrandaki **4 slota** sürükleyip bırak. Slottaki kartı tıklayarak geri alabilirsin.
2. **Turu Bitir** butonuna bas. Kartlar sırayla uygulanır: saldırı, savunma ve iyileştirme.
3. Rakip de aynı slotları kendi sırasında kullanır.
4. Rakibin canını bitirirsen destene bir kart eklersin ve haritada sıradaki savaşa geçersin. Harita 4 sütundur, sonuncusu boss'tur.

**Kart türleri:** Saldırı · Savunma · İyileştirme · Özel (şimdilik Sersemletme)
**8 element:** Fiziksel, Ateş, Su, Rüzgâr, Toprak, Doğa, Yıldırım, Buz.

- Aynı elementten art arda oynanan kartlar **çarpan** kazanır.
- Düşmanlar elementlere göre **pasif savunmaya** sahip olabilir.
- **Sersemletme** rakibin sonraki turunda slotlarını kilitler. Tüm slotlar kilitlenirse tur kendiliğinden geçer.
- Ateş Şövalyesi (oyuncu karakteri) belli bir olasılıkla rakibi **yakar**: birkaç tur boyunca her tur hasar verir.

## Öne çıkan teknik özellikler
- **Tur sistemi:** `async/await` (Unity `Awaitable`) ile yazılmış durum makinesi (`BattleStart → PlayTurn → TurnDamage → TurnEnd → BattleEnd`). İptal desteği ve hata yönetimi var. Diğer sistemler tur olayına abone olur, tur yöneticisi onları tanımaz.
- **Uyarlanabilir düşman yapay zekası:** Düşman, canının yüzdesine göre öncelik sıralı kart desenlerinden birini seçer. Desenler ve profiller ScriptableObject olduğu için kod yazmadan düzenlenir.
- **Efekt sistemi:** `IEffect` arayüzü ile her tur işleyen ve süresini kendi takip eden durum etkileri (sersemletme, elemental yanma). Yeni bir etki eklemek yeni bir sınıf yazmak kadar kolaydır.
- **Karakter yetenekleri:** Oyuncu ve düşman karakterlerinin özel yeteneği `CharacterSO.UseAbility` ile her tur otomatik çalışır. Örnek: **Stun Boss** her turunda belli bir olasılıkla oyuncunun slotlarını kilitler.
- **Veri odaklı içerik:** Kartlar, karakterler, düşmanlar, desenler ve harita sütunları ScriptableObject olarak tanımlanır. Asset'lerde `OnValidate` ile kural kontrolü yapılır.
- **Hasar hattı:** Kartlar hasarı doğrudan uygulamaz. Tur verisi toplanır, sonra çarpan, savunma ve pasif savunma hesabıyla uygulanır.
- **Birim testleri:** Unity Test Framework ile düşman karar mantığı, tur verisi ve efektler için testler (`Assets/_MyAssets/Tests`). Assembly Definition ile ayrı derlenir.

## Geliştirici rehberleri
Yeni içerik eklemek için adım adım rehberler:
- [Genel bakış](Docs/00-Genel-Bakis.md)
- [Yeni kart eklemek](Docs/01-Yeni-Kart-Eklemek.md)
- [Yeni karakter ve yetenek eklemek](Docs/02-Yeni-Karakter-Eklemek.md)
- [Yeni düşman, boss ve desen eklemek](Docs/03-Yeni-Dusman-Eklemek.md)
- [Yeni efekt eklemek](Docs/04-Yeni-Efekt-Eklemek.md)
- [Sık yapılan hatalar](Docs/05-Sik-Yapilan-Hatalar.md)

## Proje yapısı
```
Assets/_MyAssets
├── Scripts
│   ├── Core/                  Enum'lar, ortak tipler, tur olayı bağlayıcısı
│   ├── Managers/              Tur kontrolü, deste, hasar, efektler, harita durumu
│   ├── Card/                  Kart verisi ve kart türleri
│   ├── Characters/            Karakter verisi ve yetenek arayüzü
│   ├── Enemy/                 Düşman ve boss tanımları
│   ├── EnemyPatterns/         Düşman karar mantığı
│   ├── PlayerInteractivity/   Sürükle-bırak ve slotlar
│   └── UI/                    Savaş ve harita arayüzü
├── SO/                        Kart, karakter, düşman ve desen asset'leri
├── Tests/                     Birim testleri
└── Scenes/                    Oyun sahnesi
```

## Çalıştırma
- **Unity sürümü:** 6000.3.3f1 (Unity 6)
- Projeyi Unity Hub'dan açın, `Assets/_MyAssets/Scenes/SampleScene` sahnesini çalıştırın.
- Testler: **Window → General → Test Runner → EditMode → Run All**

## Durum ve yol haritası
Oyun oynanabilir bir prototip: 8 elementte saldırı, savunma ve iyileştirme kartları, 1 oyuncu karakteri, 1 normal düşman ve 1 boss, 4 sütunlu harita.

- [ ] Daha fazla düşman, boss ve kart (özellikle özel kartlar)
- [ ] Zehir, rejenerasyon gibi yeni efektler
- [ ] Ses efektleri ve müzik
- [ ] Ana menü, ayarlar ve ilk kullanıcı için ipuçları
- [ ] Ödül havuzu (şu an ödül, oyuncunun destesinden rastgele kartlardır)
- [ ] Kayıt sistemi

## Geliştirici
**Uğur Doğan** · Unity / C# geliştirici
[GitHub](https://github.com/UgurDOGAN123)
