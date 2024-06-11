using Newtonsoft.Json;
using Questao2.Responses;
using Questao2.Util;

public class Program
{
    private static readonly string BASE_URL = "https://jsonmock.hackerrank.com/api/football_matches";

    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    private static int getTotalScoredGoals(string team, int year)
    {

        int scoredGoalsAsHome = getScoredGoals(team, year, "team1");
        int scoredGoalsAsVisitant = getScoredGoals(team, year, "team2");

        return scoredGoalsAsHome + scoredGoalsAsVisitant;
    }

    private static int getScoredGoals(string team, int year, string typeTeam)
    {
        string url = $"{BASE_URL}?{typeTeam}={team}&year={year}";

        MatchInfoResponse info = ApiConector.GetRequest<MatchInfoResponse>(url).Result;

        int totalPages = info.TotalPages;

        int totalScoredGoals = info.Data.Sum(itemInfo => getTeamGoals(typeTeam, itemInfo));

        for (int i = 1; i < totalPages; i++)
        {
            info = ApiConector.GetRequest<MatchInfoResponse>($"{url}&page={i + 1}").Result;

            int totalPageGoals = info.Data.Sum(itemInfo => getTeamGoals(typeTeam, itemInfo));

            totalScoredGoals = totalScoredGoals + totalPageGoals;
        }

        return totalScoredGoals;
    }

    private static int getTeamGoals(string teamType, TeamInfoResponse info)
    {
        return teamType == "team1" ? info.Team1goals : info.Team2goals;
    }

}