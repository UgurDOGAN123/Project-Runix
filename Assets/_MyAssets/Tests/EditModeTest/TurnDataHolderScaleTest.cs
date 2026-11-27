using NUnit.Framework;

namespace Runix.Tests
{
    public class TurnDataHolderScaleTest
    {
        [Test]
        public void ScaleAttack_TumElementleriOlcekler()
        {
            var h = new TurnDataHolder();
            h.AddAttack(Element.Fire, 100);
            h.AddAttack(Element.Water, 50);

            h.ScaleAttack(0.7f);

            Assert.AreEqual(70, h.AttackValues[Element.Fire]);
            Assert.AreEqual(35, h.AttackValues[Element.Water]);
        }

        [Test]
        public void ScaleAttack_Bir_DegistirmezSavunmayaDokunmaz()
        {
            var h = new TurnDataHolder();
            h.AddAttack(Element.Fire, 100);
            h.AddDefense(Element.Fire, 20);

            h.ScaleAttack(1f);

            Assert.AreEqual(100, h.AttackValues[Element.Fire]);
            Assert.AreEqual(20, h.DefenseValues[Element.Fire]);
        }

        [Test]
        public void ScaleAttack_Sifir_SaldiriyiSifirlar()
        {
            var h = new TurnDataHolder();
            h.AddAttack(Element.Fire, 100);
            h.ScaleAttack(0f);
            Assert.AreEqual(0, h.AttackValues[Element.Fire]);
        }
    }
}
