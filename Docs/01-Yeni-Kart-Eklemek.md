# Yeni Kart Eklemek

## A) Mevcut türde kart (Saldırı / Savunma / İyileştirme / Stun): kod gerekmez

1. Project penceresinde `SO/Cards/<Element>` klasörüne sağ tık → **Create → SO → Card → Attack** (ya da Defense / Heal / Stun).
2. Alanları doldurun:

| Alan | Açıklama |
|---|---|
| Card Name / Description | Kartın adı ve açıklaması |
| Background Img, Card Image, Element Img, **Card Type** (Sprite), Card On Run | Kartın görselleri. Dikkat: `CardType` adlı **Sprite** alanı tür ikonudur, aşağıdaki `cardType` enum'u ile karıştırmayın |
| Anim Card Sprite, Anim Prefab, Animator | Kart oynanınca çıkan efekt. Prefab'ın üzerinde **SpriteRenderer ve Animator** bileşeni olmalı |
| Value | Saldırı/savunma/iyileştirme miktarı. **Stun kartında kapatılacak slot sayısı** |
| Element | Kartın elementi (çarpan ve pasif savunma buna göre hesaplanır) |
| **Card Type** (enum) | Kartın sınıfıyla **uyumlu olmalı**: Attack kartı → `Attack`, Defense → `Defense`, Heal → `Heal`, Stun → `Special` |
| Destroy Time | Efekt nesnesinin kaç saniye sonra silineceği. **0 yazmayın**, efekt hiç görünmeden silinir (örnek kartlarda 2) |
| Turns (sadece Stun) | Stun'ın kaç tur süreceği |

3. **Desteye ekleyin:** Kartı ilgili **deste asset'inin** (örn. `SO/Cards/Fire/FireDeck`) **Cards** listesine sürükleyin. Aynı kartı birden fazla kez ekleyebilirsiniz (örn. `StunDeck` 4 Stun kartı içerir). Kart ancak oyuncunun ya da düşmanın seçili destelerinden birinde varsa oyunda çıkar. Yeni deste için: [02-Yeni-Karakter-Eklemek.md](02-Yeni-Karakter-Eklemek.md).
4. **Test:** Oyunu başlatın. Kart elde görünüyor mu, slota konuyor mu, etkisi uygulanıyor mu, can/savunma barı değişiyor mu?

> **Ödül ekranı:** Şu an ayrı bir ödül havuzu yok. `RewardUIManager`, oyuncunun **mevcut destesinden** (`deckClone`) rastgele kartlar gösterir ve seçileni destenin bir kopyasını daha ekler. Yeni bir kartı ödül olarak göstermek için önce oyuncunun seçili destelerinde olması gerekir.

### Düşman neden kartı oynamıyor?
Düşman desenleri kartları **Card Type'a göre** seçer (`EnemyCardPatternSO`). Düşmanın destelerinde o türden yeterli kart yoksa desen eşleşmez ve eldeki ilk kartlar oynanır. Ayrıntı: [03-Yeni-Dusman-Eklemek.md](03-Yeni-Dusman-Eklemek.md).

## B) Yeni kart türü (yeni davranış): kod gerekir

1. `Card/Cards/` altına yeni bir sınıf yazın (örnek: `Stun.cs`):
```csharp
[CreateAssetMenu(fileName = "MyCard", menuName = "SO/Card/MyCard")]
public class MyCard : CardSO
{
    public override void UseCard(TurnDataHolder turnDataHolder, DeckManager deckManagers)
    {
        base.UseCard(turnDataHolder, deckManagers);  // çöpe atma ve animasyon (silmeyin)
        // buraya kartın etkisi
        // örn: turnDataHolder.AddAttack(element, Value);
        // örn: turnDataHolder.AddEffect(new StunEffect(turns, Value));  (etki rakibe gider)
    }
}
```
2. Gerekirse `Core/Enums.cs` içindeki `CardType` enum'unun **sonuna** yeni değer ekleyin (araya değil).
3. Düşman bu kartı seçsin istiyorsanız `EnemyReq` desenlerinde bu `CardType`'ı kullanın.
4. Etki üreten kart ise: [04-Yeni-Efekt-Eklemek.md](04-Yeni-Efekt-Eklemek.md).

## C) Yeni element
`Core/Enums.cs` → `Element` enum'unun **sonuna** ekleyin. Sonra o element için kart asset'leri ve bir **Element** türünde deste (`DeckSO`, **Element** alanı doldurulmuş) oluşturun. Elementin iyileşmesi ve pasif savunması destenin **Healable** ve **Passive Defense** alanlarından gelir.

## Kontrol listesi
- [ ] Anim Prefab'da SpriteRenderer ve Animator var
- [ ] Destroy Time 0'dan büyük
- [ ] Card Type sınıfla uyumlu
- [ ] Kart bir deste asset'inde (`DeckSO.Cards`) var
- [ ] Console'da `animPrefab null` ya da `animator null` hatası yok
