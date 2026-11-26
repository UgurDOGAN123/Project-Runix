using NUnit.Framework;
using Runix.Character;

namespace Runix.Tests
{
    public class AbilityCooldownTests
    {
        AbilityCooldown cooldown;

        [SetUp]
        public void SetUp()
        {
            cooldown = new AbilityCooldown();
            cooldown.Reset();
        }

        [Test]
        public void IlkTur_HazirdirVeKalanSifirdir()
        {
            cooldown.StartTurn();

            Assert.IsTrue(cooldown.IsReady);
            Assert.AreEqual(0, cooldown.Remaining);
        }

        [Test]
        public void Trigger_KullanildigiTurdaKalanSureCooldownKadardir()
        {
            cooldown.StartTurn();
            cooldown.Trigger(3);

            Assert.IsFalse(cooldown.IsReady);
            Assert.AreEqual(3, cooldown.Remaining);
        }

        [Test]
        public void Trigger3_UcuncuTurSonraTekrarHazirOlur()
        {
            cooldown.StartTurn();   // tur 1: kullanıldı
            cooldown.Trigger(3);

            cooldown.StartTurn();   // tur 2
            Assert.IsFalse(cooldown.IsReady, "tur 2");
            Assert.AreEqual(2, cooldown.Remaining, "tur 2 kalan");

            cooldown.StartTurn();   // tur 3
            Assert.IsFalse(cooldown.IsReady, "tur 3");
            Assert.AreEqual(1, cooldown.Remaining, "tur 3 kalan");

            cooldown.StartTurn();   // tur 4
            Assert.IsTrue(cooldown.IsReady, "tur 4");
            Assert.AreEqual(0, cooldown.Remaining, "tur 4 kalan");
        }

        [Test]
        public void Trigger1_SonrakiTurHazirOlur()
        {
            cooldown.StartTurn();
            cooldown.Trigger(1);

            cooldown.StartTurn();

            Assert.IsTrue(cooldown.IsReady);
        }

        [Test]
        public void Trigger0_BeklemeYoktur()
        {
            cooldown.StartTurn();
            cooldown.Trigger(0);

            Assert.IsTrue(cooldown.IsReady);
        }

        [Test]
        public void NegatifSure_SifirGibiDavranir()
        {
            cooldown.StartTurn();
            cooldown.Trigger(-5);

            Assert.IsTrue(cooldown.IsReady);
            Assert.AreEqual(0, cooldown.Remaining);
        }

        [Test]
        public void Reset_BeklemeyiTemizler()
        {
            cooldown.StartTurn();
            cooldown.Trigger(5);

            cooldown.Reset();
            cooldown.StartTurn();

            Assert.IsTrue(cooldown.IsReady);
            Assert.AreEqual(0, cooldown.Remaining);
        }
    }
}
