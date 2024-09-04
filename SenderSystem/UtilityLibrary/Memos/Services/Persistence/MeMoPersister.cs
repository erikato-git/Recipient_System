using System.IO;
using Dk.Digst.Digital.Post.Memolib.Container;
using Dk.Digst.Digital.Post.Memolib.Factory;
using Dk.Digst.Digital.Post.Memolib.Model;
using Dk.Digst.Digital.Post.Memolib.Writer;
using Microsoft.Extensions.Logging;
using SenderSystem;
using SenderSystem.UtilityLibrary.Memos.Configuration;
using File = System.IO.File;

namespace SenderSystem.UtilityLibrary.Memos.Services.Persistence
{
    public class MeMoPersister : IMeMoPersister
    {
        private readonly ILogger logger;

        public readonly MemoConfiguration MemoConfiguration;

        public MeMoPersister(ILogger<MeMoPersister> logger)
        {
            //MemoConfiguration = memoConfiguration;
            MemoConfiguration = GlobalStates.memoConfiguration;
            this.logger = logger;
        }

        public FileStream OpenFile(string fileName)
        {
            DirectoryInfo directory = Directory.CreateDirectory(MemoConfiguration.MemoDirectoryName);
            return File.Open($"{directory.Name}/{fileName}", FileMode.Open);
        }

        public FileStream CreateNewFile(string fileName)
        {
            DirectoryInfo directory = Directory.CreateDirectory(MemoConfiguration.MemoDirectoryName);
            FileStream file = File.Create($"{directory.FullName}/{fileName}");
            logger.LogInformation("Creating new file at {}", file.Name);
            return file;
        }

        public FileStream SaveMemo(Message message)
        {
            FileStream memoFile = CreateNewFile(GetFileName(message.MessageHeader.messageUUID));
            IMeMoStreamWriter writer = MeMoWriterFactory.CreateWriter(memoFile);
            writer.Write(message);
            memoFile.Position = 0;

            return memoFile;
        }

        public void WriteTarEntry(MeMoContainerWriter writer, Message message)
        {
            writer.WriteEntry(GetFileName(message.MessageHeader.messageUUID), message);
        }

        private string GetFileName(string messageUuid)
        {
            return $"{messageUuid}.xml";
        }
    }
}