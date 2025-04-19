using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.UI;
using UnityEngine;

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

        [field: SerializeField] public DialogRunner DialogRunner { get; private set; }

        [field: SerializeField] public HUD HUD { get; private set; }

        [field: SerializeField] public Language CurrentLanguage = Language.Ru;

        private int currentDialog = 0;
        private Dictionary<int, Func<Player, IEnumerator>> _dialogs;

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
            if (other.TryGetComponent<Player>(out var player) && _dialogs.TryGetValue(currentDialog, out var dlg))
            {
                StartCoroutine(dlg(player));
            }
        }

        private IEnumerator Introduction(Player player)
        {
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

            currentDialog = 1;
        }

        private IEnumerator UpgradingTheDetector(Player player)
        {
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
            currentDialog = 2;
        }

        private IEnumerator TestTheDetector(Player player)
        {
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Отлично, теперь проверим детектор в деле.",
                        "Я спрятал тренировочный артефакт — найди его с помощью детектора.",
                        "Чем сильнее сигнал, тем ближе он. Нажми R для режима «раскрытие».",
                        "Наведи луч — и ты увидишь его!"
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Great, now let's test the detector.",
                        "I've hidden a training artifact—find it with the detector.",
                        "The stronger the signal, the closer it is. Press R to reveal.",
                        "Aim the beam—and there you go!"
                    }
                }
            });
            currentDialog = 3;
        }

        private IEnumerator ArtifactHunting(Player player)
        {
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
            currentDialog = 4;
        }

        private IEnumerator FirstSell(Player player)
        {
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Выглядит неплохо! Принеси свои находки к этой стойке.",
                        "Вот твой первый заработок — молодец! Настоящий охотник за артефактами.",
                        "На первых трех уровнях много не заработаешь. Для апгрейда нужно 100 камней.",
                        "Сделай пару заходов, чтобы накопить их."
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Looking good! Bring your finds to this counter.",
                        "Here's your first earnings—well done! A real artifact hunter now.",
                        "You won't earn much on the first three floors. To upgrade you need 100 stones.",
                        "Make a couple more runs to collect them."
                    }
                }
            });
            currentDialog = 5;
        }

        private IEnumerator UpgradeOxygen(Player player)
        {
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
            currentDialog = 6;
        }

        private IEnumerator GoDeeper(Player player)
        {
            yield return RunSequence(new Dictionary<Language, string[]>
            {
                {
                    Language.Ru, new[]
                    {
                        "Поздравляю с апгрейдом! На глубине артефакты редче, но ценнее.",
                        "Следи за ловушками — они могут появиться внезапно."
                    }
                },
                {
                    Language.En, new[]
                    {
                        "Congrats on the upgrade! Deeper floors have rarer but pricier artifacts.",
                        "Watch for traps—they can appear unexpectedly."
                    }
                }
            });
            currentDialog = 7;
        }

        private IEnumerator UniqueArtifacts(Player player)
        {
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
            currentDialog = 8;
        }

        private IEnumerator SellTheCat(Player player)
        {
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
            currentDialog = 9;
        }

        private IEnumerator FinalGoal(Player player)
        {
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
            currentDialog = 10;
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