#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class VerificationTool : EditorWindow
{
    private List<Card> player1Cards = new List<Card>();
    private List<Card> player2Cards = new List<Card>();
    private string resultMessage = "";

    [MenuItem("Tools/VerificationTool")]
    public static void ShowWindow()
    {
        GetWindow<VerificationTool>("VerificationTool");
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("플레이어 1 카드", EditorStyles.boldLabel);
        DrawCardInputs(player1Cards);
        if (player1Cards.Count < 3)
        {
            if (GUILayout.Button("카드 추가 (플레이어 1)"))
                player1Cards.Add(new Card());
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);

        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("플레이어 2 카드", EditorStyles.boldLabel);
        DrawCardInputs(player2Cards);
        if (player2Cards.Count < 3)
        {
            if (GUILayout.Button("카드 추가 (플레이어 2)"))
                player2Cards.Add(new Card());
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("점령 조건 테스트"))
        {
            resultMessage = TestOccupation();
        }
        if (GUILayout.Button("입력 초기화"))
        {
            player1Cards.Clear();
            player2Cards.Clear();
            resultMessage = "";
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (!string.IsNullOrEmpty(resultMessage))
        {
            EditorGUILayout.HelpBox(resultMessage, MessageType.Info);
        }
    }

    private void DrawCardInputs(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            GUILayout.BeginHorizontal();
            cards[i].Color = (ECardColor)EditorGUILayout.EnumPopup(cards[i].Color, GUILayout.Width(80));
            cards[i].CardNumber = EditorGUILayout.IntSlider(cards[i].CardNumber, 1, 9, GUILayout.Width(200));
            if (GUILayout.Button("제거", GUILayout.Width(50)))
            {
                cards.RemoveAt(i);
                break;
            }
            GUILayout.EndHorizontal();
        }
    }

    private string TestOccupation()
    {
        if (player1Cards.Count == 3 && player2Cards.Count == 3)
        {
            var rank1 = EvaluateHand(player1Cards);
            var rank2 = EvaluateHand(player2Cards);

            if (rank1 > rank2)
                return $"플레이어 1이 경계석을 점령합니다. (족보: {rank1} vs {rank2})";
            else if (rank2 > rank1)
                return $"플레이어 2가 경계석을 점령합니다. (족보: {rank1} vs {rank2})";
            else
                return $"무승부입니다. (족보: {rank1} vs {rank2})";
        }
        else if (player1Cards.Count == 3 && player2Cards.Count < 3)
        {
            var allCards = GetAllPossibleCards();
            var used = player1Cards.Concat(player2Cards).ToList();
            var unused = allCards.Where(c => !ContainsCard(used, c)).ToList();

            bool provenWin = IsProvenWin(player1Cards, player2Cards, unused);

            return provenWin
                ? "플레이어 1이 어떤 경우에도 경계석을 점령할 수 있습니다! (명확한 승리)"
                : "아직 명확한 승리가 아닙니다. 상대가 이길 수 있는 경우가 존재합니다.";
        }
        else
        {
            return "각 플레이어의 카드가 3장이어야 하거나, 플레이어 1만 3장일 때만 명확한 승리 판정이 가능합니다.";
        }
    }

    // 이하 기존 EvaluateHand, ContainsCard, GetAllPossibleCards, IsProvenWin, GetCombinations 메서드 동일
    // ...

    // 족보 판정 (간단 버전)
    private enum HandRank
    {
        StraightFlush = 6,
        ThreeOfAKind = 5,
        Straight = 4,
        Flush = 3,
        Pair = 2,
        HighCard = 1
    }

    private HandRank EvaluateHand(List<Card> cards)
    {
        var numbers = cards.Select(c => c.CardNumber).OrderBy(n => n).ToArray();
        bool isFlush = cards.All(c => c.Color == cards[0].Color);
        bool isStraight = numbers.Length == 3 && numbers[2] - numbers[0] == 2 && numbers.Distinct().Count() == 3;
        bool isThree = cards.GroupBy(c => c.CardNumber).Any(g => g.Count() == 3);
        bool isPair = cards.GroupBy(c => c.CardNumber).Any(g => g.Count() == 2);

        if (isFlush && isStraight) return HandRank.StraightFlush;
        if (isThree) return HandRank.ThreeOfAKind;
        if (isStraight) return HandRank.Straight;
        if (isFlush) return HandRank.Flush;
        if (isPair) return HandRank.Pair;
        return HandRank.HighCard;
    }

    // 카드 동등성 비교 (색상+숫자)
    private static bool ContainsCard(List<Card> list, Card card)
    {
        return list.Any(c => c.CardNumber == card.CardNumber && c.Color == card.Color);
    }

    // 모든 가능한 카드 생성 (색상 6종, 숫자 1~9)
    private List<Card> GetAllPossibleCards()
    {
        var all = new List<Card>();
        foreach (ECardColor color in System.Enum.GetValues(typeof(ECardColor)))
        {
            for (int num = 1; num <= 9; num++)
            {
                all.Add(new Card { CardNumber = num, Color = color });
            }
        }
        return all;
    }

    // 명확한 승리 판정
    private bool IsProvenWin(List<Card> myCards, List<Card> opponentCards, List<Card> unusedCards)
    {
        int needed = 3 - opponentCards.Count;
        var possibleHands = GetCombinations(unusedCards, needed);

        foreach (var oppAdd in possibleHands)
        {
            var fullOpp = new List<Card>(opponentCards);
            fullOpp.AddRange(oppAdd);

            var myRank = EvaluateHand(myCards);
            var oppRank = EvaluateHand(fullOpp);

            if (oppRank > myRank)
                return false;
            if (oppRank == myRank)
            {
                // 숫자 비교 (내림차순)
                var myNums = myCards.Select(c => c.CardNumber).OrderByDescending(n => n).ToArray();
                var oppNums = fullOpp.Select(c => c.CardNumber).OrderByDescending(n => n).ToArray();
                for (int i = 0; i < 3; i++)
                {
                    if (oppNums[i] > myNums[i])
                        return false;
                    if (myNums[i] > oppNums[i])
                        break;
                }
            }
        }
        return true;
    }

    // 조합 구하기
    private IEnumerable<List<Card>> GetCombinations(List<Card> list, int count)
    {
        if (count == 0)
        {
            yield return new List<Card>();
            yield break;
        }
        for (int i = 0; i < list.Count; i++)
        {
            var head = list[i];
            var rest = list.Skip(i + 1).ToList();
            foreach (var tail in GetCombinations(rest, count - 1))
            {
                tail.Insert(0, head);
                yield return tail;
            }
        }
    }
}
#endif
