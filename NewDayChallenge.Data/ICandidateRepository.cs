using NewDayChallenge.Domain;

namespace NewDayChallenge.Data
{
    public interface ICandidateRepository
    {
        void Add(Candidate candidate);

        (Candidate Candidate, int MatchCount) Search(string[] skills);
    }
}
