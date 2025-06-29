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
        GUILayout.Label("�÷��̾� 1 ī��", EditorStyles.boldLabel);
        DrawCardInputs(player1Cards);
        if (player1Cards.Count < 3)
        {
            if (GUILayout.Button("ī�� �߰� (�÷��̾� 1)"))
                player1Cards.Add(new Card());
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);

        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("�÷��̾� 2 ī��", EditorStyles.boldLabel);
        DrawCardInputs(player2Cards);
        if (player2Cards.Count < 3)
        {
            if (GUILayout.Button("ī�� �߰� (�÷��̾� 2)"))
                player2Cards.Add(new Card());
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("���� ���� �׽�Ʈ"))
        {
            resultMessage = TestOccupation();
        }
        if (GUILayout.Button("�Է� �ʱ�ȭ"))
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
            if (GUILayout.Button("����", GUILayout.Width(50)))
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
                return $"�÷��̾� 1�� ��輮�� �����մϴ�. (����: {rank1} vs {rank2})";
            else if (rank2 > rank1)
                return $"�÷��̾� 2�� ��輮�� �����մϴ�. (����: {rank1} vs {rank2})";
            else
                return $"���º��Դϴ�. (����: {rank1} vs {rank2})";
        }
        else if (player1Cards.Count == 3 && player2Cards.Count < 3)
        {
            var allCards = GetAllPossibleCards();
            var used = player1Cards.Concat(player2Cards).ToList();
            var unused = allCards.Where(c => !ContainsCard(used, c)).ToList();

            bool provenWin = IsProvenWin(player1Cards, player2Cards, unused);

            return provenWin
                ? "�÷��̾� 1�� � ��쿡�� ��輮�� ������ �� �ֽ��ϴ�! (��Ȯ�� �¸�)"
                : "���� ��Ȯ�� �¸��� �ƴմϴ�. ��밡 �̱� �� �ִ� ��찡 �����մϴ�.";
        }
        else
        {
            return "�� �÷��̾��� ī�尡 3���̾�� �ϰų�, �÷��̾� 1�� 3���� ���� ��Ȯ�� �¸� ������ �����մϴ�.";
        }
    }

    // ���� ���� EvaluateHand, ContainsCard, GetAllPossibleCards, IsProvenWin, GetCombinations �޼��� ����
    // ...

    // ���� ���� (���� ����)
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

    // ī�� ��� �� (����+����)
    private static bool ContainsCard(List<Card> list, Card card)
    {
        return list.Any(c => c.CardNumber == card.CardNumber && c.Color == card.Color);
    }

    // ��� ������ ī�� ���� (���� 6��, ���� 1~9)
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

    // ��Ȯ�� �¸� ����
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
                // ���� �� (��������)
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

    // ���� ���ϱ�
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
