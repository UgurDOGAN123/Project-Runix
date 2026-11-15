using NUnit.Framework;
using Runix.Effect;

namespace Runix.Tests
{
    public class TurnDataHolderTests
    {
        private TurnDataHolder _holder;

        [SetUp]
        public void SetUp()
        {
            // Her testten önce temiz bir TurnDataHolder nesnesi oluþturulur
            _holder = new TurnDataHolder();
        }

        [Test]
        public void AddAttack_AyniElementtenIkiKart_HasariToplamaliVeCarpaniIkiYapmali()
        {
            // 1. Eylem (Act): Ateþ elementinden 10 ve 15 deðerinde iki saldýrý kartý oynanýr
            _holder.AddAttack(Element.Fire, 10);
            _holder.AddAttack(Element.Fire, 15);

            // 2. Doðrulama (Assert): Toplam saldýrý 25, Ateþ çarpaný 2 olmalý
            Assert.AreEqual(25, _holder.AttackValues[Element.Fire], "Saldýrý deðerleri doðru toplanmadý.");
            Assert.AreEqual(2, _holder.MultiplierValues[Element.Fire], "Ayný elementten 2 kart oynandýðýnda çarpan 2 olmalý.");
        }

        [Test]
        public void ElementalCombo_SaldiriSavunmaVeIyilestirme_AyniElementCarpaniniArtirmali()
        {
            // 1. Eylem (Act): Ayný turda Ateþ saldýrý, Ateþ savunma ve Ateþ iyileþtirme oynanýr
            _holder.AddAttack(Element.Fire, 12);
            _holder.AddDefense(Element.Fire, 8);
            _holder.AddHeal(Element.Fire, 5);

            // 2. Doðrulama (Assert): Elemental kombo mekaniði gereði çarpan 3 olmalý
            Assert.AreEqual(12, _holder.AttackValues[Element.Fire]);
            Assert.AreEqual(8, _holder.DefenseValues[Element.Fire]);
            Assert.AreEqual(5, _holder.HealValues[Element.Fire]);
            Assert.AreEqual(3, _holder.MultiplierValues[Element.Fire], "Elemental kombo mekaniði tüm kart tiplerinde çarpaný artýrmalý.");
        }

        [Test]
        public void AllClear_TurSonunda_TumVerileriSifirlamali()
        {
            // 1. Hazýrlýk & Eylem (Arrange & Act): Verileri doldurup AllClear çaðýrýyoruz
            _holder.AddAttack(Element.Ice, 20);
            _holder.AddDefense(Element.Water, 10);
            _holder.AddHeal(Element.Nature, 15);
            _holder.AddEffect(new StunEffect(1, 2));

            _holder.AllClear();

            // 2. Doðrulama (Assert): Tüm sözlükler ve efekt listesi boþalmalý
            Assert.AreEqual(0, _holder.AttackValues.Count, "AttackValues temizlenmedi.");
            Assert.AreEqual(0, _holder.DefenseValues.Count, "DefenseValues temizlenmedi.");
            Assert.AreEqual(0, _holder.HealValues.Count, "HealValues temizlenmedi.");
            Assert.AreEqual(0, _holder.MultiplierValues.Count, "MultiplierValues temizlenmedi.");
            Assert.AreEqual(0, _holder.PendingEffects.Count, "PendingEffects temizlenmedi.");
        }
    }
}