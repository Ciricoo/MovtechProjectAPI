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
    }
}
