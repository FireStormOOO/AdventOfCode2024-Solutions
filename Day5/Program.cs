
using System.Diagnostics;
using System.Text.RegularExpressions;
using Day5;

int Calculate(string input, bool tallyCorrect=true)
{
    var rules =  Regex.Matches(input, @"(\d{2})\|(\d{2})")
        .Select(match => (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value))).ToList();
    var updates = Regex.Matches(input, @"^(\d+,?)+$", RegexOptions.Multiline)
        .Select(match => match.Groups[0].Value.Split(',').Select(int.Parse).ToList()).ToList();
    Debug.Assert(rules.Count + updates.Count == input.Count(c => c == '\n'));

    int tally = 0;
    foreach (var update in updates)
    {
        Debug.Assert(update.Count % 2 ==1);
        bool correct = true;
        foreach (var rule in rules.Where(r=>update.Contains(r.Item1) && update.Contains(r.Item2))) 
            correct &= update.IndexOf(rule.Item1) < update.IndexOf(rule.Item2);

        if (correct && tallyCorrect)
            tally += update[update.Count >> 1];
        else if(!tallyCorrect && !correct)
        {
            var applicableRules = rules.Where(r => update.Contains(r.Item1) && update.Contains(r.Item2)).ToList();
            for (var rule = applicableRules.FirstOrDefault(r => update.IndexOf(r.Item1) >= update.IndexOf(r.Item2),(0,0));
                 rule != (0,0); rule = applicableRules.FirstOrDefault(r => update.IndexOf(r.Item1) >= update.IndexOf(r.Item2),(0,0)))
            {
                update.Remove(rule.Item2);
                update.Insert(update.IndexOf(rule.Item1) + 1, rule.Item2);
                applicableRules.Remove(rule);
                applicableRules.Add(rule);
            }
            tally += update[update.Count >> 1];
        }
        
    }

    return tally;
}

Debug.Assert(143 ==
    Calculate("47|53\n97|13\n97|61\n97|47\n75|29\n61|13\n75|53\n29|13\n97|29\n53|29\n61|53\n97|53\n61|29\n47|13\n75|47\n97|75\n47|61\n75|61\n47|29\n75|13\n53|13\n\n75,47,61,53,29\n97,61,53,29,13\n75,29,13\n75,97,47,61,53\n61,13,29\n97,13,75,29,47"));
Debug.Assert(123 ==
             Calculate("47|53\n97|13\n97|61\n97|47\n75|29\n61|13\n75|53\n29|13\n97|29\n53|29\n61|53\n97|53\n61|29\n47|13\n75|47\n97|75\n47|61\n75|61\n47|29\n75|13\n53|13\n\n75,47,61,53,29\n97,61,53,29,13\n75,29,13\n75,97,47,61,53\n61,13,29\n97,13,75,29,47", false));

Console.WriteLine($"Sum of correct middle pages is {Calculate(PuzzleInput.Input)}");
Console.WriteLine($"Sum of incorrect middle pages is {Calculate(PuzzleInput.Input,false)}");    