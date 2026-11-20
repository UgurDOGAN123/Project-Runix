# Oyuncu Karakteri, Deste ve Yetenek

Bir savaşçı üç şeyden oluşur: **deste(ler)**, **yetenek** ve **sayılar** (can, savunma, el limitleri). Oyuncu için bunlar `PlayerLoadout` içinde (`DeskManager.Player`), düşman için `EnemySO` asset'inde tutulur. İkisi de `ICharacterLoadout` arayüzünü uygular, bu yüzden oyunun geri kalanı onları ayırt etmez. Düşman için: [03-Yeni-Dusman-Eklemek.md](03-Yeni-Dusman-Eklemek.md).

## A) Yeni deste: kod gerekmez
1. Project penceresinde sağ tık → **Create → SO → DeckSO**.
2. Alanları doldurun:

| Alan | Açıklama |
|---|---|
| Display Name / Description / Icon | Seçim ekranında görünecek ad, açıklama ve ikon |
| **Kind** | `Element`: ana deste (Ateş, Su...). `Special`: yanına eklenen özel deste (örn. Stun kartları) |
| **Element** | Element destesinde **zorunlu** (`None` olamaz). Yeteneğin kullanacağı element de budur |
| Healable | Bu deste seçilince iyileştirmesi geçerli olan elementler |
| Passive Defense | Bu deste seçilince elemente göre sabit hasar azaltma (yüzde, 0-100) |
| Cards | Destedeki kartlar. **Boş bırakmayın**, aynı kartı birkaç kez ekleyebilirsiniz |

3. Eksik alan varsa Console'da sarı uyarı çıkar.

**Birleşim kuralı:** Oyuncu iki deste seçtiyse (element + özel) kartlar birleşir, `Healable` listeleri birleşir, pasif savunmalar elemente göre **toplanır**.

## B) Yeni yetenek
### Mevcut türden (kod yok)
**Create → SO → Ability → TurnElementDamage** (yanma) ya da **StunAbility** (sersemletme):

| Yetenek | Alanlar |
|---|---|
| TurnElementDamage | `Ability Chance` (olasılık), `Burn Turn` (tur), `Burn Damage` (tur başına hasar). Yanma elementi, sahibinin **Element Deck**'inin elementidir |
| StunAbility | `Stun Chance`, `Stun Turns`, `Stun Blocked Slots` (99 = tüm slotlar). Rakip zaten sersemse yeniden uygulanmaz |

Her yetenekte **Display Name** ve **Description** doldurun (boşsa uyarı çıkar, içerik testi kırılır).

### Yeni davranış (kod)
`Ability/` klasörüne `AbilitySO`'dan türeyen bir sınıf yazın. `AbilityRunner` (sahnede oyuncu ve düşman için birer tane) `UseAbility`'yi sahibinin **her PlayTurn başında** çağırır. Sahibi tamamen sersemlemişse çağrılmaz.

```csharp
[CreateAssetMenu(fileName = "MyAbility", menuName = "SO/Ability/MyAbility")]
public class MyAbility : AbilitySO
{
    [Range(0f, 1f)][SerializeField] float chance = 0.3f;

    public override void UseAbility(AbilityContext ctx)
    {
        if (ctx.Opponent == null) return;
        if (Random.value < chance)
            ctx.Opponent.Add(new StunEffect(1));      // rakibe etki
        // ctx.Own.Add(...)                            // kendine etki
    }
}
```
- `ctx.OwnerId`: yeteneği kullanan (`PlayerIds`), `ctx.Own`: onun `EffectManager`'ı, `ctx.Opponent`: rakibin.
- Sahibinin elementi gerekirse: `Managers.DeskManager.instance.GetCharacterSO(ctx.OwnerId).GetPrimaryElement()`.
- Yeni etki türü için: [04-Yeni-Efekt-Eklemek.md](04-Yeni-Efekt-Eklemek.md). Örnekler: `Ability/TurnElementDamage.cs`, `Ability/StunAbility.cs`.
- **Asset'in durumunu yetenek sınıfında tutmayın** (çalışma anı verisi SO'ya yazılmaz).

## C) Oyuncunun kombinasyonu: karakter seçim ekranı
Oyuncu kombinasyonunu `CharacterBuildScene`'de seçer. **Başlat** (`StartButton`) seçimlerden `PlayerLoadout` oluşturup `DeskManager.Player`'a yazar. Yani `DeskManager`'daki Player alanını Inspector'dan doldurmanız gerekmez, her yeni oyunda üzerine yazılır.

**Seçim ekranına yeni deste, yetenek ya da harita eklemek:** Mevcut bir seçim butonunu (örn. `Element Button` prefab'ı) kopyalayın ve butonun üzerindeki `DeckSelectButton` / `AbilitySelectButton` / `MapSelectButton` bileşeninde **Data** alanına yeni asset'i atayın. Buton ikonu ve açıklamayı asset'ten okur, üstüne gelince açıklamayı gösterir. Kopyalanan butonun `OnClick` bağlantısı (`StartButton.GetElementSO` vb.) kopyayla birlikte gelir.

Can ve el limitleri `CharacterBuildScene`'deki **StartButton** üzerindedir:

| Alan | Açıklama |
|---|---|
| Hp Values | `Max Hp`, `Max Defense` (her elementte birikebilecek azami savunma, 0 olmasın) |
| Deck Limits | `Max Hand Card Count` (eldeki azami kart), `Start Card Count` (savaş başında çekilen), `Every Turn Add Card Count` (tur sonu çekilen) |

Bu değerlerden biri 0 ya da altındaysa **Başlat çalışmaz** (şimdilik hata mesajı vermez). Test: oyunu başlatın, doğru kartlar çekiliyor mu, yetenek tetikleniyor mu, can ve savunma değerleri beklediğiniz gibi mi?

## Dikkat
- Çalışma anı verisini (kalan can, deste kopyası) **SO'ya yazmayın**. Savaş içi durum `HpManager` ve `DeckManager`'dadır.
- Serileştirilen alanın adını değiştirirseniz `[FormerlySerializedAs("eskiAd")]` ekleyin ve asset'leri kaydedin (File → Save Project). Alanı bir `struct` içine taşırsanız (örn. `HpValues`) eski değer **otomatik taşınmaz**, elle yeniden girin.
