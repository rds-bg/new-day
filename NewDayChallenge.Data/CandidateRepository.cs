using NewDayChallenge.Domain;
using System.Collections.Concurrent;
using System.Linq;

namespace NewDayChallenge.Data
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ConcurrentBag<Candidate> candidatesDb = new();

        public void Add(Candidate candidate) => candidatesDb.Add(candidate);

        public (Candidate Candidate, int MatchCount) Search(string[] skills)
        {
            var query = candidatesDb.Select(
                x => new { Candidate = x, 
                    MatchCount = x.Skills.Intersect(skills).Count() })
                .DefaultIfEmpty();

            var maxCount = query?.Max(x => x?.MatchCount);

            var candidate = query?.FirstOrDefault(x => x?.MatchCount == maxCount);

            return (candidate?.Candidate, candidate?.MatchCount ?? 0);
        }
    }
}
