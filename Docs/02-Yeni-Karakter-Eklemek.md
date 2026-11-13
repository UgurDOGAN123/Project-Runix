# Yeni Karakter Eklemek

Karakterler `CharacterSO`'dan türeyen ScriptableObject'lerdir (`Characters/CharacterSO.cs`). Düşmanlar da karakterdir (`EnemySO : CharacterSO`), ortak alanlar aynıdır.

## A) Mevcut türden yeni bir karakter (aynı yetenek, farklı sayılar): kod gerekmez
1. `SO/Characters` klasöründe sağ tık → **Create → SO → Character → FireKnight**.
2. Alanları doldurun:

| Alan | Açıklama |
|---|---|
| Character Name / Description | Ad ve açıklama |
| Max Hand Card Count | Elde tutulabilecek en fazla kart |
| Start Card Count | Savaş başında çekilen kart |
| Every Turn Add Card Count | Her tur sonunda çekilen kart |
| Max Hp | Azami can |
| Max Defense | Her elementte birikebilecek azami savunma |
| Healable | İyileştirmesi geçerli elementler |
| Passive Defense | Elemente göre sabit hasar azaltma (yüzde, 0-100) |
| Decks | Karakterin destesi (kart asset'leri). **Boş bırakmayın** |
| Yeteneğe özel alanlar (FireKnight: Ability Chance, Burn Turn, Burn Damage, Element) | Yetenek ayarları |

3. **Oyuncu olarak kullanmak için:** Sahnede `DeskManager` nesnesini seçin, **Player** alanına karakteri sürükleyin.
4. Test: Oyunu başlatın. Deste doğru çekiliyor mu, can ve savunma değerleri beklediğiniz gibi mi?

## B) Yeni yetenekli karakter: kod gerekir
Yetenek `UseAbility` içinde yazılır. `AbilityRunner` (sahnede oyuncu ve düşman için birer tane) bunu sahibinin **her PlayTurn başında** çağırır. Sahibi tamamen sersemlemişse çağrılmaz.

```csharp
[CreateAssetMenu(fileName = "MyHero", menuName = "SO/Character/MyHero")]
public class MyHero : CharacterSO
{
    [Range(0f, 1f)][SerializeField] float chance = 0.3f;

    public override void UseAbility(AbilityContext ctx)
    {
        if (ctx.Opponent == null) return;
        if (Random.value < chance)
            ctx.Opponent.Add(new StunEffect(1));       // rakibe etki
        // ctx.Own.Add(...)                             // kendine etki
    }
}
```
- `ctx.Own`: yeteneği kullananın `EffectManager`'ı, `ctx.Opponent`: rakibin.
- Yeni etki türü için: [04-Yeni-Efekt-Eklemek.md](04-Yeni-Efekt-Eklemek.md).
- Örnekler: `Characters/FireKnight.cs` (yanma), `Enemy/StunBoss.cs` (stun).

## Dikkat
- Karakter SO'larında **çalışma anı verisi tutmayın** (kalan can, deste kopyası...). SO'nun kendisi değişir, Editor'de kalıcı olur.
- Alan adı değiştirirseniz `[FormerlySerializedAs("eskiAd")]` ekleyin ve tüm karakter asset'lerini kaydedin (File → Save Project).
