# Yeni Düşman Eklemek

Düşman (`EnemySO`) oyuncu gibi bir savaşçıdır (`ICharaterLoadout`): destesi, yeteneği, canı ve el limitleri vardır. Üstüne üç alan ekler: **Enemy Type**, **Pattern** (düşmanın davranış profili), **Enemy Icon** (haritada görünen resim). Deste ve yetenek asset'leri için: [02-Yeni-Karakter-Eklemek.md](02-Yeni-Karakter-Eklemek.md).

## 1. Düşman asset'ini oluşturun
`SO/Enemy/<Ad>` klasöründe sağ tık → **Create → SO → Enemy → Enemy**. (Boss için de aynı sınıf kullanılır, farkı **Enemy Type** ve yetenektir. `StunBoss` sınıfı ek bir davranış içermez.)
- **Character Name / Description**
- **Deck Limits:** `Max Hand Card Count`, `Start Card Count`, `Every Turn Add Card Count`. Üçü de 0'dan büyük olmalı.
- **Hp Values:** `Max Hp`, `Max Defense`. Hiçbiri 0 olmamalı.
- **Element Deck:** düşmanın ana destesi (`DeckSO`, Kind = Element). Düşmanın `Healable` ve `Passive Defense` değerleri de bu desteden gelir.
- **Special Deck:** isteğe bağlı ikinci deste.
- **Ability:** `AbilitySO` asset'i (örn. boss için `StunAbility`). Boş bırakırsanız Console'da kırmızı hata çıkar ve içerik testi kırılır.
- **Enemy Type:** normal düşman için `Enemy`, boss için `Boss`.
- **Enemy Icon:** harita masasında görünen sprite.
- **Pattern:** 2. adımdaki profil.

## 2. Davranış profilini (Pattern) hazırlayın
Profil: **Create → SO → Enemy → Adaptive Profile** (`AdaptiveEnemyProfileSO`).
- **Max Play Slots:** düşmanın bir turda oynayabileceği en fazla kart (genelde 4, boss için daha fazla olabilir).
- **Health Tiers:** can aşamaları. **En yüksek candan en düşüğe doğru** sıralayın (örn. 0.7, 0.3, 0). Yanlış sıralarsa Console'da sarı uyarı çıkar.
- Her aşamanın **Prioritized Patterns** listesi, öncelik sırasıyla desenleri içerir.

Desen: **Create → SO → Enemy → EnemyReq** (`EnemyCardPatternSO`). Her satır bir kart türü ve adet:
`Attack x2 + Defense x2` gibi.

**Karar mantığı** (`EnemyDecisionResolver`):
1. Düşmanın can yüzdesine uyan **ilk aşama** seçilir.
2. O aşamanın desenleri **sırayla** denenir. Eldeki kartlarla **tam karşılanan ilk desen** oynanır.
3. Hiçbiri karşılanmazsa eldeki **ilk kartlar** (Max Play Slots'a kadar) oynanır.

Bu yüzden:
- İstediğiniz deseni listede **önce** koyun. Önündeki desen karşılanırsa o seçilir.
- Düşmanın destelerinde desenlerin istediği türden yeterli kart olsun. Aksi halde desen hep elenir.
- Desenin Card Type'ı kartın Card Type'ı ile eşleşir (kartın sınıfı değil enum değeri).

## 3. Düşmanı bir masaya ekleyin
`SO/Desk/DeskEnemiesSO` asset'ini açın. **Enemy Lists** tam **4 liste** içermelidir:
- **Liste 0, 1, 2:** normal düşmanlar (`Enemy Type = Enemy`)
- **Liste 3:** boss'lar (`Enemy Type = Boss`)

Düşmanı ilgili listenin **Enemies** alanına ekleyin. Her listede ayrıca:
- **Min Desk / Max Desk:** o sütunda kaç düşman masası çıkacağı. **Max Desk dahil değildir**, istediğiniz en büyük sayıdan 1 fazla yazın (örn. en fazla 3 masa → 4).
- Min, Max'tan küçük olmalı.

`DeskManager` nesnesinin **Desk Enemies** alanı bu asset'e bağlı olmalı (sahnede bir kez).

Asset üzerinde kural ihlali varsa Console'da kırmızı uyarı çıkar (4 liste yok, boss listesinde normal düşman var, vs.).

## 4. Test
Hızlı test için düşmanı **Liste 0'a tek başına** koyun, Min Desk 1 / Max Desk 2 yapın, oyunu başlatın ve masayı seçin.
- Düşman beklenen kartları oynuyor mu? (Console'a elini loglatabilirsiniz.)
- Can aşamasına göre davranış değişiyor mu? (canını düşürerek deneyin)
- Boss ise Liste 3'te mi?

## Boss'a özel yetenek
Boss'un **Ability** alanına bir yetenek asset'i atayın (örn. `SO/Ability/StunAbility`: her turunda olasılıkla oyuncunun slotlarını kilitler). Yeni davranış için `AbilitySO`'dan türeyen sınıf yazın, bkz. [02-Yeni-Karakter-Eklemek.md](02-Yeni-Karakter-Eklemek.md).

## Kontrol listesi
- [ ] Element Deck dolu ve desenlerin istediği türde kartlar var
- [ ] Pattern atanmış, Health Tiers büyükten küçüğe sıralı
- [ ] Enemy Type doğru (Boss → Liste 3)
- [ ] Enemy Icon atanmış (yoksa masada resim boş görünür)
- [ ] Hp Values ve Deck Limits 0 değil
- [ ] Ability atanmış
