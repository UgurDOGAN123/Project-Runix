# Sık Yapılan Hatalar

| Belirti | Olası sebep | Çözüm |
|---|---|---|
| Kartın efekti hiç görünmüyor | `Destroy Time = 0` | 1-2 yapın |
| `animPrefab null` / `animator null` hatası | Alanlar boş | Kart asset'inde doldurun |
| Efekt prefab'ında `NullReferenceException` | Prefab'da SpriteRenderer ya da Animator yok | Prefab'a ekleyin |
| Düşman istediğim kartları oynamıyor | Desenin istediği türden yeterli kart yok, ya da önündeki desen eşleşti | Decks'i ve desen sırasını kontrol edin, [03](03-Yeni-Dusman-Eklemek.md) |
| Düşman bir aşamada hep aynı davranıyor | Health Tiers büyükten küçüğe sıralı değil | Sıralayın (Console'da sarı uyarı çıkar) |
| Karakter değerleri 0 geldi (Max Defense, pasif savunma) | Alan yeniden adlandırıldı, `[FormerlySerializedAs]` yok | Ekleyin, asset'i kaydedin |
| Yeni enum değeri eklenince kartlar/desenler kaydı | Enum'un **ortasına** eklendi | En sona ekleyin |
| Haritada düşman masası çıkmıyor | `DeskManager.Desk Enemies` bağlı değil ya da liste boş | Atayın, her listeye düşman ekleyin |
| `DeskEnemiesSO` kırmızı uyarısı | 4 liste yok, boss listesi yanlış, min >= max | Uyarı metnini okuyun |
| Max Desk kadar masa çıkmıyor | `Random.Range` üst sınırı **dahil değil** | İstenen sayıdan 1 fazla yazın |
| Oyuncu karakteri boş / deste çıkmıyor | `DeskManager.Player` atanmamış ya da `Decks` boş | Atayın |
| İyileştirme işe yaramıyor | Elementi karakterin `Healable` listesinde yok | Ekleyin, test edin |
| Yeni etki tur sonunda silinmiyor | `OnTurnEnd`'i override edip süreyi azaltmayı unuttunuz | `base.OnTurnEnd(ctx)` çağırın |
| Efekt hiç çalışmıyor | `EffectManager`'da `Hp` ya da `Owner Id` boş/yanlış | Sahnede doldurun |
| Kodlama bozuk (Türkçe harfler `?`) | Dosya UTF-8 değil | Visual Studio: Save with Encoding → UTF-8 |
| Editor'de çalışan şey build'de bozuk | ScriptableObject'e çalışma anı verisi yazılmış | Veriyi SO'dan çıkarın |

## Değişiklik yaparken
- Önce `git pull`, sonra çalış, küçük ve tek konulu commit'ler.
- Yeniden adlandırmaları (alan, sınıf, klasör) ayrı bir commit'te yapın ve Unity **içinden** (`.meta` dosyaları birlikte taşınsın).
- Commit'ten önce Test Runner'ı çalıştırın.
