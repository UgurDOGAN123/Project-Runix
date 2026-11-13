# Yeni Efekt Eklemek (yanma, zehir, stun...)

Efekt, birkaç tur süren bir durumdur. Sahibinin her turunda işlenir, süresini kendisi takip eder, süresi bitince otomatik silinir.

## Parçalar
| Dosya | Görevi |
|---|---|
| `IEffect` | Arayüz: `RemainingTurns`, `OnTurnStart`, `OnTurnEnd` |
| `Effect` | Taban sınıf: kalan turu tutar, tur sonunda bir azaltır |
| `EffectManager` | Oyuncu ve düşman için birer tane (sahnede). Efekt listesini tutar ve tur olayını dinler |
| `EffectContext` | Efektin sahibine ulaşma yolu (`Hp`, `OwnerId`) |

## Efektin hayat döngüsü (`EffectManager`)
- `BattleStart` / `BattleEnd`: liste temizlenir.
- **Sahibinin** `PlayTurn`'ü: tüm efektlerin `OnTurnStart`'ı çağrılır.
- **Sahibinin** `TurnDamage`'ı: tüm efektlerin `OnTurnEnd`'i çağrılır (süre bir azalır), süresi 0 olanlar silinir.

## Yeni efekt yazmak
```csharp
public class MyEffect : Effect
{
    readonly int amount;
    public MyEffect(int turns, int amount) : base(turns) { this.amount = amount; }

    public override void OnTurnStart(EffectContext ctx)
    {
        ctx.Hp.TakeDirectDamage(amount, Element.Fire);   // örnek: savunmayı delen, pasif savunmaya tabi hasar
    }
}
```
Örnekler: `Effects/ElementEffect.cs` (yanma), `Effects/StunEffect.cs`.

> Efekt sınıfları **normal C# sınıfıdır, ScriptableObject değildir**. Kalan tur çalışma anı verisidir, asset'e yazılmamalıdır. Ayarlar (süre, hasar) kart ya da karakter asset'inden okunur, efekt savaşta `new` ile üretilir.

## Efekti nasıl uygularsınız?
| Kaynak | Kod | Kime gider |
|---|---|---|
| **Kart** | `turnDataHolder.AddEffect(new MyEffect(...))` (`UseCard` içinde) | **Rakibe**, tur sonunda `TurnDataManager` aktarır |
| **Karakter yeteneği** | `ctx.Opponent.Add(...)` ya da `ctx.Own.Add(...)` (`UseAbility` içinde) | Rakibe / kendine, hemen |

Kendi kendine efekt veren **kart** için şu an hazır bir yol yok (kart efektleri rakibe gider). Gerekirse `TurnDataHolder`'a ayrı bir "kendine" listesi eklenip `TurnDataManager.SendToDatas` içinde kendi `EffectManager`'ına aktarılabilir. Alternatif: karakter yeteneği kullanın.

## Stun efekti (örnek)
`StunEffect(turns, blockedSlots)`: sahibinin sıradaki turunda `blockedSlots` kadar slotu kilitler (`StunEffect.AllSlots` = hepsi). Kilidi `StunController` uygular. Tüm slotlar kilitliyse "Sersemledin" paneli çıkar ve tur otomatik geçer. Düşman kilitli slotları atlar, tamamen sersemse kart oynamaz.

## Test
Efekti ekledikten sonra Console'da `EffectManager`'ın debug loglarına bakın (`[Effect:0] eklendi ...`). Süre bitince `... etki bitti` yazar.

## Kontrol listesi
- [ ] `EffectManager`'ların `Owner Id` (oyuncu 0, düşman 1) ve `Hp` alanları dolu
- [ ] Efekt hiçbir olaya abone olmuyor, `static` listede tutulmuyor (bellekte kalmasın)
- [ ] Hasar veren efektlerde ölüm kontrolü (`TakeDirectDamage` içinde var) ve can barı güncellemesi çalışıyor
