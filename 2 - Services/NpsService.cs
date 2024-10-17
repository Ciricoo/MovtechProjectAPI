using MovtechProject._3___Domain.CommandHandlers;

namespace MovtechProject._2___Services
{
    public class NpsService
    {
        private readonly NpsCommandHandlers _npsCommandHandlers;

        public NpsService(NpsCommandHandlers npsCommandHandlers)
        {
            _npsCommandHandlers = npsCommandHandlers;
        }

        public async Task<int> NpsScore()
        {
            return await _npsCommandHandlers.NpsScore();
        }

        public async Task<List<int>> GetAnswersAccordingNpsGrade()
        {
            return await _npsCommandHandlers.GetAnswersAccordingNpsGrade();
        }

        public async Task<int> GetNpsByGroupId(int idGroup)
        {
            return await _npsCommandHandlers.GetNpsByGroupId(idGroup);
        }
    }
}
