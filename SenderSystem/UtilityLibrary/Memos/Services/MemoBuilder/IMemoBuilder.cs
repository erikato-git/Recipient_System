using System;
using System.IO;
using Dk.Digst.Digital.Post.Memolib.Model;

namespace SenderSystem.UtilityLibrary.Memos.Services.MemoBuilder
{
    public interface IMemoBuilder
    {
        FileStream CreateMemoFile(Guid memoUuid);

        FileStream CreateMemoTar();

        Message CreateMemo(Guid memoUuid);
    }
}