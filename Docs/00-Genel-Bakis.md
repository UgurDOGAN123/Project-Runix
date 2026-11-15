# Genel Bakış

Bu klasör, projeye yeni içerik eklerken izlenecek adımları anlatır. Önce bu sayfayı, sonra ihtiyacınız olanı okuyun.

| Ne eklemek istiyorsun? | Rehber |
|---|---|
| Yeni kart (ya da yeni kart türü) | [01-Yeni-Kart-Eklemek.md](01-Yeni-Kart-Eklemek.md) |
| Oyuncu karakteri, yeni deste, yeni yetenek | [02-Yeni-Karakter-Eklemek.md](02-Yeni-Karakter-Eklemek.md) |
| Yeni düşman / boss / düşman deseni | [03-Yeni-Dusman-Eklemek.md](03-Yeni-Dusman-Eklemek.md) |
| Yeni efekt (yanma, zehir, stun gibi) | [04-Yeni-Efekt-Eklemek.md](04-Yeni-Efekt-Eklemek.md) |
| Bir şey çalışmıyor | [05-Sik-Yapilan-Hatalar.md](05-Sik-Yapilan-Hatalar.md) |

## Temel fikir: veri ScriptableObject'te, mantık kodda
Kartlar, desteler, yetenekler, düşmanlar ve düşman desenleri **ScriptableObject (SO)** asset'leridir. Yeni içerik çoğunlukla "doğru SO'yu oluştur, alanları doldur, doğru listeye ekle" demektir. Kod yazmak sadece yeni bir **davranış** gerekiyorsa lazımdır.

Oluşturma menüsü: Project penceresinde sağ tık → **Create → SO → ...**

| Menü yolu | Ne oluşturur |
|---|---|
| SO/Card/Attack, Defense, Heal, Stun | Kart |
| SO/DeckSO | Deste (kart listesi + iyileşme ve pasif savunma). Element ya da özel deste |
| SO/Ability/TurnElementDamage, SO/Ability/StunAbility | Yetenek (yanma, sersemletme) |
| SO/Enemy/Enemy, SO/Enemy/StunBoss | Düşman / boss |
| SO/Enemy/Adaptive Profile | Düşmanın can aşamalı davranış profili |
| SO/Enemy/EnemyReq | Tek bir kart kombinasyonu (desen) |
| SO/DeskEnemiesSO | Masalara (haritadaki savaş sütunlarına) göre düşman listeleri |

## Savaşçı modeli: deste + yetenek + sayılar
Oyuncu da düşman da aynı arayüzü (`ICharaterLoadout`) uygular, bu yüzden deste, can ve yetenek sistemleri kimin olduğunu bilmez, hepsi aynı yoldan okur:

| | Oyuncu | Düşman |
|---|---|---|
| Sınıf | `PlayerLoadout` (`DeskManager.Player`, Inspector'da doldurulur) | `EnemySO` (asset) |
| Desteler | **Element Deck** + **Special Deck** (isteğe bağlı) | Element Deck + Special Deck (isteğe bağlı) |
| Yetenek | `AbilitySO` asset'i | `AbilitySO` asset'i |
| Sayılar | Hp Values, Deck Limits | Hp Values, Deck Limits |

Savaşta kullanılacak kartlar, iyileşebilen elementler ve pasif savunma **tüm desteler birleştirilerek** hesaplanır (`LoadoutExtensions`). Yeteneğin kullandığı element, ilk **Element** türündeki destenin elementidir.

## Savaş akışı (kısa)
Tur durumları (`TurnEnum`, `Core/Enums.cs`):
`BattleStart` (bir kez) → `PlayTurn` (kart oynanır) → `TurnDamage` (hasar hesaplanır ve uygulanır) → `TurnEnd` → başa dön → `BattleEnd` (bir kez).

Oyuncu (`PlayerIds.Player = 0`) ve düşman (`PlayerIds.Enemy = 1`) sırayla oynar. Sistemler tur olayına `MonoNewTurnEventConnector` üzerinden abone olur, tur yöneticisi onları tanımaz.

## Hasar hattı
Kart `UseCard` ile değer üretir → `TurnDataHolder` (element bazında toplar, aynı elementten kart sayısı çarpan olur) → `TurnDataManager` karşı tarafa dağıtır → `HpManager` savunma, pasif savunma ve çarpanla uygular.

## Harita ve run akışı
`DeskManager` (sahnede tek, singleton) çalışan oyun durumunu tutar: `Player` (`PlayerLoadout`), `Enemy` (seçili düşman), `DeskCount`, `deskEnemies`. Diğer sistemler oyuncuya ya da düşmana `GetCharacterSO(id)` ile ulaşır.
`PlayMapController` her sütunda `DeskEnemiesSO.EnemyLists[sütun]` listesinden rastgele düşman masaları üretir. Oyuncu masayı seçince `DeskManager.Enemy` o düşmana ayarlanır.

## Önemli kurallar
- **Enum'lara yeni değer eklerken en sona ekleyin** (`Element`, `CardType`, `TurnEnum`). Asset'ler enum'ları sayı olarak saklar, araya eklerseniz mevcut kartlar ve desenler kayar.
- **Serileştirilen alanı yeniden adlandırırken** `[FormerlySerializedAs("eskiAd")]` ekleyin, yoksa asset'lerdeki değerler sıfırlanır.
- **Çalışma anı verisini SO'ya yazmayın** (asset'in kendisi değişir, Editor'de kalıcı olur).
- Yeni özellikten sonra Test Runner'daki testleri çalıştırın (Window → General → Test Runner → EditMode).
