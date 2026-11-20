# Testler

Testler `Assets/_MyAssets/Tests` altındadır ve **Window → General → Test Runner** ile çalışır.

## İki mod
Hangi moda ait olduğunu **asmdef** belirler, klasör ya da dosya adı değil.

| | EditMode (`Tests.asmdef`) | PlayMode (`PlayModeTests.asmdef`) |
|---|---|---|
| Çalışma | Oyun çalıştırılmadan, çok hızlı | Play moda girilerek |
| Uygun olan | Saf mantık: `DamageCalculator`, `DeckModel`, efektler, loadout | `MonoBehaviour`, `Awake/OnEnable`, kare beklemek |
| İçerik | Karar mantığı, hasar, deste, loadout, efekt, tur verisi, içerik doğrulaması | Tur sistemi, `EffectManager`, `HpManager` |

Önce EditMode'a yazmayı deneyin, yaşam döngüsü ya da zaman gerekiyorsa PlayMode'a taşıyın. Test sayısı yaklaşık 110 (EditMode ~78, PlayMode ~34).

## Çalıştırma
Test Runner → **EditMode** sekmesi → Run All, ardından **PlayMode** sekmesi → Run All. Commit'ten önce ikisi de yeşil olmalı.

## Kullanılan teknikler
- **Saf sınıflara ayırma:** Hasar (`DamageCalculator`) ve deste (`DeckModel`) mantığı Unity'den bağımsızdır, doğrudan test edilir.
- **Arayüzle taklit (fake):** `EffectContext.Hp` bir `IDirectDamageable`'dır. Testte `FakeHpManager` ile hasar doğrudan ölçülür. Moq/NSubstitute Unity'de IL2CPP'de çalışmadığı için elle yazılmış fake kullanılır.
- **Birleşim noktası (seam):** `EffectManager.TurnCalculator` korumalı bir metottur. Test alt sınıfı onu çağırarak tur olayına bağımlı kalmadan akışı sürer.
- **Özel alanlar:** `[SerializeField]` alanlar yansıma (reflection) ile atanır. `MonoBehaviour`'u önce pasif `GameObject`'e ekleyip alanları ayarlayın, sonra `SetActive(true)` ile `Awake`'i çalıştırın.
- **AAA düzeni:** Arrange (hazırla), Act (çalıştır), Assert (doğrula). `Assert.AreEqual(beklenen, gerçek)` sırasına dikkat.

## Yeni test yazarken
1. Bir davranış, bir test. Adı davranışı söylesin (`TakeDirectDamage_OlduktenSonra_...`).
2. Sınır durumlarını deneyin: sıfır, boş liste, `null`, tam sınır değeri.
3. Önce testi yazıp kırmızıyı görün, sonra kodu düzeltip yeşile çevirin.
4. `TearDown`'da oluşturduğunuz `GameObject`/`ScriptableObject`'leri `DestroyImmediate` ile temizleyin (singleton'lar sıfırlansın).
5. Hata mesajı hangi adımda kırıldığını söylesin (`"1. TurnDamage: hp"`).
