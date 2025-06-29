using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("�������� ��")]
    [SerializeField] 
    private int _stoneCount = 9;

    private List<List<Card>> _player1Stones;
    private List<List<Card>> _player2Stones;
    private List<Card> _deck;

    protected override void Awake()
    {
        base.Awake();
        InitializeGame();
    }

    protected override void Start()
    {
        base.Start();
        Application.targetFrameRate = 60;
    }

    private void InitializeGame()
    {
        _player1Stones = new List<List<Card>>();
        _player2Stones = new List<List<Card>>();
        for (int i = 0; i < _stoneCount; i++)
        {
            _player1Stones.Add(new List<Card>());
            _player2Stones.Add(new List<Card>());
        }
        _deck = GetAllPossibleCards();
    }

    // ��輮�� ī�� ��ġ
    public bool PlaceCard(int player, int stoneIndex, Card card)
    {
        if (!ContainsCard(_deck, card)) 
            return false;

        if (player == 1 && _player1Stones[stoneIndex].Count < 3)
            _player1Stones[stoneIndex].Add(card);
        else if (player == 2 && _player2Stones[stoneIndex].Count < 3)
            _player2Stones[stoneIndex].Add(card);
        else
            return false;

        _deck.RemoveAll(c => c.CardNumber == card.CardNumber && c.Color == card.Color);
        return true;
    }

    // ��輮 ���� ����
    // 1: �÷��̾�1 ����, -1: �÷��̾�2 ����, 0: ������/���º�
    public int JudgeStone(int stoneIndex)
    {
        var p1 = _player1Stones[stoneIndex];
        var p2 = _player2Stones[stoneIndex];

        if (p1.Count == 3 && p2.Count == 3)
        {
            return CompareHands(p1, p2);
        }
        else if (p1.Count == 3 && p2.Count < 3)
        {
            var unused = GetUnusedCards();
            bool provenWin = IsProvenWin(p1, p2, unused);
            return provenWin ? 1 : 0;
        }
        else if (p2.Count == 3 && p1.Count < 3)
        {
            var unused = GetUnusedCards();
            bool provenWin = IsProvenWin(p2, p1, unused);
            return provenWin ? -1 : 0;
        }
        return 0;
    }

    // ��ü �̻�� ī�� ��ȯ
    public List<Card> GetUnusedCards()
    {
        var used = _player1Stones.SelectMany(x => x).Concat(_player2Stones.SelectMany(x => x)).ToList();
        return GetAllPossibleCards().Where(c => !ContainsCard(used, c)).ToList();
    }

    // ��� ī�� ���� (���� 6��, ���� 1~9)
    private List<Card> GetAllPossibleCards()
    {
        var all = new List<Card>();
        foreach (ECardColor color in System.Enum.GetValues(typeof(ECardColor)))
        {
            for (int num = 1; num <= 9; num++)
            {
                all.Add(new Card(num, color));
            }
        }
        return all;
    }

    // ī�� ��� �� (����+����)
    private static bool ContainsCard(List<Card> list, Card card)
    {
        return list.Any(c => c.CardNumber == card.CardNumber && c.Color == card.Color);
    }

    // ���� ����
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

        if (isFlush && isStraight) 
            return HandRank.StraightFlush;
        if (isThree) 
            return HandRank.ThreeOfAKind;
        if (isStraight) 
            return HandRank.Straight;
        if (isFlush) 
            return HandRank.Flush;
        if (isPair) 
            return HandRank.Pair;

        return HandRank.HighCard;
    }

    // ���� ��
    private int CompareHands(List<Card> hand1, List<Card> hand2)
    {
        var rank1 = EvaluateHand(hand1);
        var rank2 = EvaluateHand(hand2);

        if (rank1 > rank2) 
            return 1;
        if (rank2 > rank1) 
            return -1;

        var sorted1 = hand1.Select(c => c.CardNumber).OrderByDescending(n => n).ToArray();
        var sorted2 = hand2.Select(c => c.CardNumber).OrderByDescending(n => n).ToArray();
        for (int i = 0; i < 3; i++)
        {
            if (sorted1[i] > sorted2[i]) 
                return 1;
            if (sorted2[i] > sorted1[i])
                return -1;
        }
        return 0;
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

            int result = CompareHands(myCards, fullOpp);

            if (result <= 0)
                return false;
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
