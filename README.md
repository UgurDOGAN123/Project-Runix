# Project Runix

Unity ile geliştirdiğim, tur tabanlı bir kart savaşı oyunu. Kartları slotlara yerleştirip rakibinle sırayla savaşıyor, her zaferden sonra desteni yeni kartlarla güçlendiriyor ve sonunda bir boss ile karşılaşıyorsun.


## Nasıl oynanır
1. Elindeki kartlardan istediklerini ekrandaki **4 slota** sürükleyip bırak. Slottaki kartı tıklayarak geri alabilirsin.
2. **Turu Bitir** butonuna bas. Kartlar sırayla uygulanır: saldırı, savunma ve iyileştirme.
3. Rakip de aynı slotları kendi sırasında kullanır.
4. Rakibin canını bitirirsen ödül olarak yeni bir kart seçersin ve haritada sıradaki savaşa geçersin.

**Kart türleri:** Saldırı · Savunma · İyileştirme · Özel ( Şimdilik Sadece  Sersemletme (Stun) var)
Aynı elementten art arda oynanan kartlar çarpan kazanır. Sersemletme kartı rakibin sonraki turunda slotlarını kilitler.

## Öne çıkan teknik özellikler
- **Tur sistemi:** `async/await` (Unity `Awaitable`) ile yazılmış durum makinesi (`BattleStart → PlayTurn → TurnDamage → TurnEnd → BattleEnd`). Diğer sistemler tur olayına abone olur, tur yöneticisi onları tanımaz.
- **Uyarlanabilir düşman yapay zekası:** Düşman, canının yüzdesine göre öncelik sıralı kart desenlerinden birini seçer. Desenler ScriptableObject olduğu için kod yazmadan düzenlenir.
- **Efekt sistemi:** `IEffect` arayüzü ile her tur işleyen ve süresini kendi takip eden durum etkileri (örneğin sersemletme). Yeni bir etki eklemek yeni bir sınıf yazmak kadar kolaydır.
- **Karakter yetenekleri:** Boss gibi karakterlerin özel yeteneği, `CharacterSO.UseAbility` üzerinden her tur otomatik çalışır. Örnek: **Stun Boss** her turunda belli bir olasılıkla oyuncunun slotlarını kilitler.
- **Veri odaklı içerik:** Kartlar, karakterler, düşmanlar ve desenler ScriptableObject olarak tanımlanır.
- **Hasar hattı:** Kartlar hasarı doğrudan uygulamaz. Tur verisi toplanır, sonra çarpan ve savunma hesabıyla uygulanır.

## Proje yapısı
```
Assets/_MyAssets/Scripts
├── Managers/        Tur kontrolü, deste, hasar, efektler, yetenekler
├── Card/            Kart verisi ve kart türleri
├── Characters/      Karakter verisi ve yetenek arayüzü
├── Enemy/           Düşman ve boss tanımları
├── EnemyPattens/    Düşman karar mantığı
├── PlayerInteractivity/   Sürükle-bırak ve slotlar
└── UI/              Savaş ve harita arayüzü
```

## Çalıştırma
- **Unity sürümü:** 6000.3.3f1 (Unity 6)
- Projeyi Unity Hub'dan açın, `Assets/_MyAssets/Scenes/SampleScene` sahnesini çalıştırın.

## Yapılacaklar
- [ ] Yanma ve zehir gibi sürekli hasar efektleri
- [ ] Daha fazla karakter ve boss
- [ ] Ses ve 

## Geliştirici
**Uğur Doğan** · Unity / C# geliştirici
[GitHub](https://github.com/UgurDOGAN123)
