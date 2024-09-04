using System.IO;
using Dk.Digst.Digital.Post.Memolib.Model;

namespace SenderSystem.UtilityLibrary.Memos.Services.Parser
{
    public interface IMemoService
    {
        MessageHeader ParseMemoHeader(Stream memoFile);
    }
}