using NewDayChallenge.Data;
using NewDayChallenge.Domain;
using System.Linq;

namespace NewDayChallenge.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;

        public CandidateService(ICandidateRepository candidateRepository) => _candidateRepository = candidateRepository;

        public void Add(Candidate candidate)
        {
            candidate.Skills = candidate.Skills.Select(x => x.ToLowerInvariant()).ToArray();
            
            _candidateRepository.Add(candidate);
        }

        public Candidate Search(string[] skills)
        {
            skills = skills.Select(x => x.ToLowerInvariant()).ToArray();

            var result = _candidateRepository.Search(skills);

            return result.MatchCount > 0 ? result.Candidate : null;
        }
    }
}
