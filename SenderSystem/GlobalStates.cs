using Digst.DigitalPost.Systems.RestPush.Sender.Configuration;
using SenderSystem.UtilityLibrary.Memos.Configuration;
using SenderSystem.UtilityLibrary.Memos.Models;
using SenderSystem.UtilityLibrary.Receipts.Configuration;
using System.Text.Json;

namespace SenderSystem
{
    public static class GlobalStates
    {
        static GlobalStates()
        {
            memoConfiguration = new MemoConfiguration();
        }

        public static MemoConfiguration memoConfiguration { get; set; }

    }
}
