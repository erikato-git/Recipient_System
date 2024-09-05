using SenderSystem.DTOs;

namespace SenderSystem.Services
{
    public interface ISendMemoService
    {
        Task ProcessMemoInput(MemoInputDTO input);
    }
}
