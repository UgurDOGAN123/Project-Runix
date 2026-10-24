using System;
using System.Threading;
using UnityEngine;

namespace Runix.TurnControlSystem
{
    /// <summary>
    /// Akış: BattleStart -> [her oyuncu için: PlayTurn -> (RequestEndTurn bekle) -> TurnDamage -> (DamageResolver bekle)]
    ///       -> TurnEnd -> tekrar. EndBattle() çağrılınca BattleEnd ile biter.
    /// Gereksinim: Unity 6 / 2023.2+ (Awaitable, AwaitableCompletionSource).
    /// </summary>
    public class TurnControlManager : MonoBehaviour
    {
        public static TurnControlManager instance;

        [SerializeField] int playerCount = 2;
        [SerializeField] bool autoStart = true;
        [SerializeField] float startDelay = 0.5f;      // BattleStart öncesi
        [SerializeField] float turnEndDelay = 0.5f;    // TurnEnd sonrası
        [SerializeField] float damageFallbackDelay = 1f; // DamageResolver yoksa beklenecek süre

        [SerializeField] TurnEnum currentStage = TurnEnum.None;
        [SerializeField] int currentPlayer;

        /// <summary>Aşama değiştiğinde (stage, player) ile haber verir.</summary>
        public event Action<TurnEnum, int> NewTurn;

        /// <summary>
        /// Hasar sistemi bunu doldurur. TurnDamage eventinden sonra manager,
        /// hesap + animasyon bitene kadar bu görevi bekler. Boşsa sabit süre bekler.
        /// </summary>
        public Func<int, Awaitable> DamageResolver;

        public TurnEnum CurrentStage => currentStage;
        public int CurrentPlayer => currentPlayer;
        public int PlayerCount => playerCount;
        public bool IsBattleRunning { get; private set; }

        AwaitableCompletionSource endTurnSignal;
        bool battleOver;

        #region Unity

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            if (instance == this) instance = null;

            // Bekleyen sinyali iptal et, döngü temiz çıksın
            var s = endTurnSignal;
            endTurnSignal = null;
            s?.SetCanceled();
        }
        private void OnEnable()
        {
            if (autoStart) StartBattle();
        }

        #endregion

        #region Public API

        public async void StartBattle()
        {
            print(0);
            if (IsBattleRunning) return;
            print(1);
            if (playerCount <= 0)
            {
                Debug.LogError("[TurnControlManager] playerCount 0'dan büyük olmalı.", this);
                return;
            }

            try
            {
                await RunBattle(destroyCancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Obje yok edildi, normal
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        /// <summary>
        /// Oyuncu "turu bitir" dediğinde veya AI kart seçimini bitirdiğinde çağrılır.
        /// Sırası olmayan oyuncunun çağrısı ve çift çağrı yok sayılır.
        /// </summary>
        public void RequestEndTurn(int player)
        {
            if (!IsBattleRunning || currentStage != TurnEnum.PlayTurn) return;
            if (player != currentPlayer) return;

            var s = endTurnSignal;
            endTurnSignal = null;
            s?.SetResult();
        }

        /// <summary>Savaşı bitirir. Oyuncu beklenirken çağrılsa da döngü takılmaz.</summary>
        public void EndBattle()
        {
            battleOver = true;

            var s = endTurnSignal;
            endTurnSignal = null;
            s?.SetResult();
        }

        #endregion

        #region Battle Loop

        async Awaitable RunBattle(CancellationToken ct)
        {
            IsBattleRunning = true;
            battleOver = false;
            currentPlayer = 0;

            print(2);
            // Dinleyicilerin abone olması için bir frame bekle
            await Awaitable.NextFrameAsync(ct);
            if (startDelay > 0f) await Awaitable.WaitForSecondsAsync(startDelay, ct);

            SetStage(TurnEnum.BattleStart, 0);

            while (!battleOver)
            {
                for (int p = 0; p < playerCount && !battleOver; p++)
                {
                    currentPlayer = p;

                    // ÖNCE sinyal, SONRA event: AI event içinde anında bitirse bile kaçmaz
                    endTurnSignal = new AwaitableCompletionSource();
                    var signal = endTurnSignal;

                    SetStage(TurnEnum.PlayTurn, p);

                    await signal.Awaitable;
                    if (battleOver) break;

                    SetStage(TurnEnum.TurnDamage, p);
                    if (DamageResolver != null)
                        await DamageResolver(p);
                    else
                        await Awaitable.WaitForSecondsAsync(damageFallbackDelay, ct);
                }

                if (battleOver) break;

                currentPlayer = 0;
                SetStage(TurnEnum.TurnEnd, 0);
                await Awaitable.WaitForSecondsAsync(turnEndDelay, ct);
            }

            SetStage(TurnEnum.BattleEnd, currentPlayer);
            IsBattleRunning = false;
        }

        void SetStage(TurnEnum stage, int player)
        {
            currentStage = stage;
            NewTurn?.Invoke(stage, player);
        }

        #endregion
    }
}