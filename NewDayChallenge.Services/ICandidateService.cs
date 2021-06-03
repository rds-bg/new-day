using NewDayChallenge.Domain;

namespace NewDayChallenge.Services
{
    public interface ICandidateService
    {
        Candidate Search(string[] skills);
        void Add(Candidate candidate);
    }
}
