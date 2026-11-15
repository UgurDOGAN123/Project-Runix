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
- **Yakma** yeteneği belli bir olasılıkla rakibi yakar: birkaç tur boyunca her tur hasar verir. Yanma elementi, seçtiğin element destesinden gelir.

## Öne çıkan teknik özellikler
- **Tur sistemi:** `async/await` (Unity `Awaitable`) ile yazılmış durum makinesi (`BattleStart → PlayTurn → TurnDamage → TurnEnd → BattleEnd`). İptal desteği ve hata yönetimi var. Diğer sistemler tur olayına abone olur, tur yöneticisi onları tanımaz.
- **Uyarlanabilir düşman yapay zekası:** Düşman, canının yüzdesine göre öncelik sıralı kart desenlerinden birini seçer. Desenler ve profiller ScriptableObject olduğu için kod yazmadan düzenlenir.
- **Efekt sistemi:** `IEffect` arayüzü ile her tur işleyen ve süresini kendi takip eden durum etkileri (sersemletme, elemental yanma). Yeni bir etki eklemek yeni bir sınıf yazmak kadar kolaydır.
- **Seçilebilir deste ve yetenek:** Oyuncu bir **element destesi**, yanına isteğe bağlı bir **özel deste** ve bir **yetenek** seçer. Kartlar, iyileşebilen elementler ve pasif savunma seçilen desteden gelir. Oyuncu ve düşman aynı arayüzü (`ICharaterLoadout`) kullanır, bu yüzden savaş sistemleri kimin olduğunu bilmez.
- **Yetenekler:** Her `AbilitySO`, sahibinin turunun başında otomatik çalışır. Örnekler: **Yakma** (olasılıkla rakibi birkaç tur yakar), **Sersemletme** (Stun Boss'un oyuncunun slotlarını kilitlemesi).
- **Veri odaklı içerik:** Kartlar, desteler, yetenekler, düşmanlar, desenler ve harita sütunları ScriptableObject olarak tanımlanır. Asset'lerde `OnValidate` ile kural kontrolü yapılır.
- **Hasar hattı:** Kartlar hasarı doğrudan uygulamaz. Tur verisi toplanır, sonra çarpan, savunma ve pasif savunma hesabıyla uygulanır.
- **Testler:** Unity Test Framework ile EditMode (düşman karar mantığı, hasar hesabı, deste mantığı, tur verisi, asset içerik doğrulaması) ve PlayMode (tur sistemi) testleri (`Assets/_MyAssets/Tests`). Hasar ve deste mantığı Unity'den bağımsız sınıflara ayrıldığı için doğrudan test edilir.

## Geliştirici rehberleri
Yeni içerik eklemek için adım adım rehberler:
- [Genel bakış](Docs/00-Genel-Bakis.md)
- [Yeni kart eklemek](Docs/01-Yeni-Kart-Eklemek.md)
- [Oyuncu karakteri, deste ve yetenek](Docs/02-Yeni-Karakter-Eklemek.md)
- [Yeni düşman, boss ve desen eklemek](Docs/03-Yeni-Dusman-Eklemek.md)
- [Yeni efekt eklemek](Docs/04-Yeni-Efekt-Eklemek.md)
- [Sık yapılan hatalar](Docs/05-Sik-Yapilan-Hatalar.md)

## Proje yapısı
```
Assets/_MyAssets
├── Scripts
│   ├── Core/                  Enum'lar, ortak tipler, tur olayı bağlayıcısı
│   ├── Managers/              Tur kontrolü, deste, hasar, efektler, harita durumu
│   ├── Card/                  Kart verisi, kart türleri ve desteler (DeckSO)
│   ├── Ability/               Yetenekler (yanma, sersemletme)
│   ├── Characters/            Oyuncu loadout'u ve savaşçı arayüzü
│   ├── Enemy/                 Düşman ve boss tanımları
│   ├── EnemyPatterns/         Düşman karar mantığı
│   ├── PlayerInteractivity/   Sürükle-bırak ve slotlar
│   └── UI/                    Savaş ve harita arayüzü
├── SO/                        Kart, deste, yetenek, düşman ve desen asset'leri
├── Tests/                     Birim testleri
└── Scenes/                    Oyun sahnesi
```

## Çalıştırma
- **Unity sürümü:** 6000.3.3f1 (Unity 6)
- Projeyi Unity Hub'dan açın, `Assets/_MyAssets/Scenes/SampleScene` sahnesini çalıştırın.
- Testler: **Window → General → Test Runner → EditMode ve PlayMode → Run All**

## Durum ve yol haritası
Oyun oynanabilir bir prototip: 8 element destesi ve bir özel deste, iki yetenek, 1 normal düşman ve 1 boss, 4 sütunlu harita. Deste ve yetenek şimdilik Inspector'dan seçiliyor.

- [ ] Oyuncu için deste ve yetenek **seçim ekranı**
- [ ] Daha fazla düşman, boss, yetenek ve özel deste
- [ ] Zehir, rejenerasyon gibi yeni efektler
- [ ] Ses efektleri ve müzik
- [ ] Ana menü, ayarlar ve ilk kullanıcı için ipuçları
- [ ] Ödül havuzu (şu an ödül, oyuncunun destesinden rastgele kartlardır)
- [ ] Kayıt sistemi

## Geliştirici
**Uğur Doğan** · Unity / C# geliştirici
[GitHub](https://github.com/UgurDOGAN123)
