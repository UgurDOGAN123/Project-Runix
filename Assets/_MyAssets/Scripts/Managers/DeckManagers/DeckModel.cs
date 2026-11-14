using System;
using System.Collections.Generic;

namespace Runix.Managers
{
    /// <summary>
    /// Deste mantığı (çekme destesi, çöp, el). Unity'ye bağlı değil, bu yüzden test edilebilir.
    /// Listeler dışarıdan verilir: DeckManager'daki Inspector alanları aynen görünmeye devam eder.
    /// </summary>
    public class DeckModel<T>
    {
        readonly List<T> drawPile;
        readonly List<T> trash;
        readonly List<T> hand;
        readonly Random rng;

        public DeckModel(List<T> drawPile, List<T> trash, List<T> hand, Random rng = null)
        {
            this.drawPile = drawPile;
            this.trash = trash;
            this.hand = hand;
            this.rng = rng ?? new Random();
        }

        public int DrawCount => drawPile.Count;
        public int TrashCount => trash.Count;
        public int HandCount => hand.Count;
        public int TotalCount => drawPile.Count + trash.Count + hand.Count;

        /// <summary>Çekme destesi boş ama çöpte kart var: karıştırıp geri koymak gerekir.</summary>
        public bool NeedsReshuffle => drawPile.Count == 0 && trash.Count > 0;

        public bool IsHandFull(int maxHand) => hand.Count >= maxHand;

        /// <summary>Savaş başı: her şeyi sıfırlar, kartları çekme destesine koyup karıştırır.</summary>
        public void StartBattle(IEnumerable<T> startCards)
        {
            drawPile.Clear();
            trash.Clear();
            hand.Clear();
            drawPile.AddRange(startCards);
            Shuffle(drawPile);
        }

        /// <summary>Çöpteki kartları çekme destesine taşır, karıştırır, çöpü boşaltır.</summary>
        public void Reshuffle()
        {
            drawPile.AddRange(trash);
            trash.Clear();
            Shuffle(drawPile);
        }

        /// <summary>Çekme destesinin başından bir kart alıp ele ekler. Deste boşsa false.</summary>
        public bool TryDraw(out T card)
        {
            if (drawPile.Count == 0)
            {
                card = default;
                return false;
            }
            card = drawPile[0];
            drawPile.RemoveAt(0);
            hand.Add(card);
            return true;
        }

        public void Discard(T card) => trash.Add(card);

        public bool RemoveFromHand(T card) => hand.Remove(card);

        /// <summary>Kartı ele geri koyar. El doluysa eklemez ve false döner.</summary>
        public bool ReturnToHand(T card, int maxHand)
        {
            if (hand.Count >= maxHand) return false;
            hand.Add(card);
            return true;
        }

        // Fisher-Yates
        void Shuffle(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
