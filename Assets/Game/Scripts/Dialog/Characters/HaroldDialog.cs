using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Artifacts;
using Game.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Dialog.Characters
{
    [RequireComponent(typeof(Collider2D))]
    public class HaroldDialog : MonoBehaviour
    {
        public enum Language
        {
            Ru,
            En
        }

        [field: SerializeField]
        public DialogRunner DialogRunner { get; private set; }

        [field: SerializeField]
        public HUD HUD { get; private set; }

        [field: SerializeField]
        public Language CurrentLanguage { get; private set; } = Language.En;

        [field: SerializeField]
        public DetectorUpgrader DetectorUpgrader { get; private set; }

        [field: SerializeField]
        public ArtifactSpawner ArtifactSpawner { get; private set; }

        [field: SerializeField]
        public ArtifactsDatabase ArtifactsDatabase { get; private set; }

        [field: SerializeField]
        public ArtifactContainer ArtifactContainer { get; private set; }

        [field: SerializeField]
        public PlayerCamera PlayerCamera { get; private set; }


        private int _currentDialog = 0;
        private Dictionary<int, Func<Player, IEnumerator>> _dialogs;
        private Goal _currentGoal = null;

        private void Awake()
        {
            _dialogs = new Dictionary<int, Func<Player, IEnumerator>>
            {
                { 0, Introduction },
                { 1, UpgradingTheDetector },
                { 2, TestTheDetector },
                { 3, ArtifactHunting },
                { 4, FirstSell },
                { 5, UpgradeOxygen },
                { 6, GoDeeper },
                { 7, UniqueArtifacts },
                { 8, SellTheCat },
                { 9, FinalGoal }
            };
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                if (!_currentGoal?.Achieved ?? false)
                {
                    return;
                }

                if (_dialogs.TryGetValue(_currentDialog, out var dlg))
                {
                    StartCoroutine(dlg(player));
                }
            }
        }

        private IEnumerator Introduction(Player player)
        {
            player.DisableInput();
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "А вот и ты... Должник, верно?",
                        "Хм. Что ж, уважаю, что ты выбрал честный труд вместо бегства.",
                        "Сначала покажу, как тут всё устроено..."
                    }
                },
                {
                    Language.En, new[]
                    {
                        "There you are... A debtor, right?",
                        "Hm. I respect that you chose honest work over running.",
                        "First, let me show you how things work around here..."
                    }
                }
            });

            HUD.OxygenCanvasUI.gameObject.SetActive(true);
            HUD.InventoryCanvasUI.gameObject.SetActive(true);
            HUD.DetectorCanvasUI.gameObject.SetActive(true);

            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Вот тебе сломанный детектор, маленький баллон кислорода и пара карманов. Не потеряй.",
                        "Чтобы починить его, нужны блестящие камни. Их тут полно.",
                        "Собери десять камней — хватит на ремонт.",
                        "Давай, спускайся вниз. Я буду ждать здесь.",
                        "Ах да — если не собрал десять, ничего страшного, просто попробуй ещё раз."
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Take this broken detector, a small oxygen tank, and two pockets. Don't lose them.",
                        "To repair it, you'll need shiny stones. There are plenty underground.",
                        "Gather ten stones—that's enough for the repair.",
                        "Go on, head down. I'll be waiting here.",
                        "Oh, and if you didn't gather ten, no worries—just try again."
                    }
                }
            });
            player.EnableInput();
            _currentGoal = new Goal(
                text: "Collect 10 shine stones",
                condition: () => player.Inventory.Money > 10);
            _currentDialog = 1;
        }

        private IEnumerator UpgradingTheDetector(Player player)
        {
            player.DisableInput();
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Помни: уровни генерируются заново при каждом заходе, так что камни и артефакты всегда есть.",
                        "Подойди к стойке, заплати за ремонт детектора и вернись ко мне."
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Remember: levels regenerate each run, so there are always stones and artifacts.",
                        "Approach the station, pay for the detector repair, and come back to me."
                    }
                }
            });
            DetectorUpgrader.HighlightEnable();
            player.EnableInput();
            _currentGoal = new Goal(
                text: "Upgrade the detector",
                condition: () => player.Detector.UpgradeLevel > 0);
            _currentDialog = 2;
        }

        private IEnumerator TestTheDetector(Player player)
        {
            player.DisableInput();
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Отлично, теперь давай опробуем детектор в действии.",
                        "Я спрятал перед тобой тренировочный артефакт — найди его при помощи детектора.",
                        "Чем сильнее сигнал, тем ближе артефакт. Попробуй включить режим «раскрытие» (R).",
                        "Наведи луч на место — и вот он, готов к подбору!"
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Great, now let’s test the detector in action.",
                        "I’ve hidden a training artifact here — locate it using the detector.",
                        "The stronger the signal, the closer the artifact. Try switching to “reveal” mode (R).",
                        "Aim the beam there — and there it is, ready to pick up!"
                    }
                }
            });

            var firefly = ArtifactsDatabase.AllArtifacts.First(x => x.ArtifactId == ArtifactId.Firefly);
            var spawnerArtifact = ArtifactSpawner.DropArtifact(new Vector2(-8.62f, -3.04f), new ArtifactData(firefly));
            spawnerArtifact.Reveal();
            player.EnableInput();
            yield return new WaitUntil(() => player.Inventory.CollectedArtifacts.Contains(spawnerArtifact.Data));

            player.DisableInput();
            PlayerCamera.SetTarget(transform);

            var artifactSold = false;
            UnityAction onArtifactSold = () => artifactSold = true;
            ArtifactContainer.OnArtifactSold.AddListener(onArtifactSold);
            ArtifactContainer.EnableHighlight();

            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Хорошая работа, бери и приноси сюда на стойку для продажи артефактов.",
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Good job — grab it and bring it back here to the selling rack for artifacts.",
                    }
                }
            });
            PlayerCamera.SetTarget(player.transform);
            player.EnableInput();
            yield return new WaitUntil(() => artifactSold);
            ArtifactContainer.OnArtifactSold.RemoveListener(onArtifactSold);

            player.DisableInput();
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Славно, теперь ты примерно понимаешь, как происходит поиск и продажа артефактов.",
                        "Подойди ко мне как будешь готов продолжить."
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Well done, now you have a general idea of how artifact hunting and selling works.",
                        "Come back to me when you will be ready."
                    }
                }
            });
            player.EnableInput();

            _currentDialog = 3;
        }

        private IEnumerator ArtifactHunting(Player player)
        {
            player.DisableInput();
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Теперь по-настоящему: найди артефакт 'Груз' — он как чугунная гиря.",
                        "Будь осторожен: кислород на глубине уходит быстрее.",
                        "Не опускайся ниже третьего этажа — воздуха не хватит."
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Now for real: find the 'Cargo' artifact—it looks like an iron weight.",
                        "Be careful: oxygen drains faster at depth.",
                        "Don't go below the third floor—you won't have enough air."
                    }
                }
            });

            player.EnableInput();
            _currentGoal = new Goal(
                text: "Bring LightWeight artifact",
                condition: () => player.Inventory.CollectedArtifacts.Any(x => x.ArtifactId == ArtifactId.LightWeight));
            _currentDialog = 4;
        }

        private IEnumerator FirstSell(Player player)
        {
            player.DisableInput();
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Выглядит неплохо! Принеси свои находки к стойке.",
                        "Вот твой первый заработок — молодец! Настоящий охотник за артефактами.",
                        "На первых трех уровнях много не заработаешь. Для апгрейда нужно 100 камней.",
                        "Сделай пару заходов, чтобы накопить их."
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Looking good! Bring your finds to the selling rack.",
                        "Here's your first earnings—well done! A real artifact hunter now.",
                        "You won't earn much on the first three floors. To upgrade you need 100 stones.",
                        "Make a couple more runs to collect them."
                    }
                }
            });
            player.EnableInput();
            _currentGoal = new Goal(
                text: "Collect 100 shiny stones",
                condition: () => player.Inventory.Money >= 100);
            _currentDialog = 5;
        }

        private IEnumerator UpgradeOxygen(Player player)
        {
            player.DisableInput();
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Доступно улучшение баллона: потрать заработанное и увеличь запас кислорода.",
                        "Теперь можно добраться до 4–6 этажей."
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Tank upgrade available: spend your coins to boost oxygen.",
                        "Now you can reach floors 4–6."
                    }
                }
            });
            player.EnableInput();
            _currentGoal = new Goal(
                text: "Upgrade oxygen balloon",
                condition: () => player.Oxygen.UpgradeLevel > 0);
            _currentDialog = 6;
        }

        private IEnumerator GoDeeper(Player player)
        {
            player.DisableInput();
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Поздравляю с апгрейдом! На глубине артефакты редче, но ценнее.",
                        "Следи за ловушками — они могут появиться внезапно.",
                        "Подойди ко мне как будешь готов продолжить."
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Congrats on the upgrade! Deeper floors have rarer but pricier artifacts.",
                        "Watch for traps—they can appear unexpectedly.",
                        "Come back to me when you will be ready."
                    }
                }
            });
            player.EnableInput();
            _currentDialog = 7;
        }

        private IEnumerator UniqueArtifacts(Player player)
        {
            player.DisableInput();
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Есть специальное задание: найти Кота‑оценщика.",
                        "Он показывает цену артефактов, но занимает слот.",
                        "Реши пространственную ловушку — кислород не уходит."
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Special task: find the Cat Appraiser.",
                        "He shows artifact prices but uses a slot.",
                        "Solve the spatial trap—oxygen won't drain."
                    }
                }
            });
            player.EnableInput();

            _currentGoal = new Goal(
                text: "Find the Cat Appraiser.",
                condition: () => player.Inventory.CollectedArtifacts.Any(x => x.ArtifactId == ArtifactId.CatAppraiser));
            _currentDialog = 8;
        }

        private IEnumerator SellTheCat(Player player)
        {
            player.DisableInput();
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Вот ты и вернулся… Кот с тобой?",
                        "Можешь продать его за 500 монет или оставить."
                    }
                },
                {
                    Language.En, new[]
                    {
                        "You're back... Got the Cat?",
                        "You can sell him for 500 coins or keep him."
                    }
                }
            });
            player.EnableInput();
            _currentDialog = 9;
        }

        private IEnumerator FinalGoal(Player player)
        {
            player.DisableInput();
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Теперь цель — выплатить долг. Доступ к 7–9 этажам открою, когда будешь готов.",
                        "На 9 этаже страж — не пустит, пока долг не погашен.",
                        "Как накопишь нужную сумму, возвращайся ко мне."
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Now the goal is to pay off your debt. I'll unlock floors 7–9 when you're ready.",
                        "On floor 9 stands a guard—he won't let you pass until you pay.",
                        "Once you've got the required amount, come back to me."
                    }
                }
            });
            player.EnableInput();

            _currentGoal = new Goal(
                text: "Pay off the debt",
                condition: () => player.Inventory.Money >= 1000);
            _currentDialog = 10;
        }

        private IEnumerator RunSequence(Dictionary<Language, string[]> textsByLanguage)
        {
            if (!textsByLanguage.TryGetValue(CurrentLanguage, out var lines))
                throw new ArgumentException($"No dialog for language {CurrentLanguage}");

            var nodes = new List<DialogNode>();
            foreach (var text in lines)
                nodes.Add(Create(text));
            yield return DialogRunner.RunDialogSequence(nodes);
        }

        private static DialogNode Create(string text)
        {
            var node = ScriptableObject.CreateInstance<DialogNode>();
            node.SpeakerId = SpeakerId.Harold;
            node.Text = text;
            return node;
        }
    }
}