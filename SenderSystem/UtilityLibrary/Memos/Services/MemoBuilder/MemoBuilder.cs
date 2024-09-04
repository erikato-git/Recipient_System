using System;
using System.IO;
using Dk.Digst.Digital.Post.Memolib.Builder;
using Dk.Digst.Digital.Post.Memolib.Container;
using Dk.Digst.Digital.Post.Memolib.Model;
using SenderSystem;
using SenderSystem.UtilityLibrary.Memos.Configuration;
using SenderSystem.UtilityLibrary.Memos.Services.Persistence;
using File = Dk.Digst.Digital.Post.Memolib.Model.File;

namespace SenderSystem.UtilityLibrary.Memos.Services.MemoBuilder
{
    public class MemoBuilder : IMemoBuilder
    {
        public readonly MemoConfiguration memoConfiguration;

        public readonly IMeMoPersister memoPersister;

        public MemoBuilder(IMeMoPersister memoPersister)
        {
            //this.memoConfiguration = memoConfiguration;
            memoConfiguration = GlobalStates.memoConfiguration;
            this.memoPersister = memoPersister;
        }

        public Message CreateMemo(Guid memoUuid)
        {
            Message message = BuildMemo(memoUuid);

            memoPersister.SaveMemo(message).Close();

            return message;
        }

        public FileStream CreateMemoFile(Guid memoUuid)
        {
            Message message = BuildMemo(memoUuid);

            return memoPersister.SaveMemo(message);
        }

        public FileStream CreateMemoTar()
        {
            string fileName = Guid.NewGuid() + ".tar.lzma";
            FileStream fileStream = memoPersister.CreateNewFile(fileName);

            MeMoContainerWriter writer = new MeMoContainerWriter(new MeMoContainerOutputStream(fileStream));
            for (int i = 0; i < memoConfiguration.NumberOfMemos; i++)
            {
                Message message = BuildMemo(Guid.NewGuid());
                memoPersister.WriteTarEntry(writer, message);
            }

            writer.Dispose();
            fileStream.Dispose();
            return memoPersister.OpenFile(fileName);
        }

        private Message BuildMemo(Guid memoUuid)
        {
            return MessageBuilder.NewBuilder()
                .MessageHeader(
                    MessageHeaderBuilder.NewBuilder()
                        .MessageType(memoMessageType.DIGITALPOST)
                        .MessageUUID(memoUuid)
                        .Sender(BuildSender())
                        .Recipient(BuildRecipient())
                        .Notification(memoConfiguration.Header.Notification).Label(memoConfiguration.Header.Label)
                        .Mandatory(memoConfiguration.Header.Mandatory)
                        .LegalNotification(memoConfiguration.Header.LegalNotification).Build())
                .MessageBody(BuildMessageBody())
                .Build();
        }

        private MessageBody BuildMessageBody()
        {
            return MessageBodyBuilder.NewBuilder()
                .CreatedDateTime(DateTime.UtcNow.ToLocalTime())
                .MainDocument(BuildMainDocumentation())
                .Build();
        }

        private Sender BuildSender()
        {
            return SenderBuilder.NewBuilder()
                .SenderId(memoConfiguration.Header.Sender.Id)
                .IdType(memoConfiguration.Header.Sender.IdType)
                .Label(memoConfiguration.Header.Sender.Label)
                .Build();
        }

        private Recipient BuildRecipient()
        {
            return RecipientBuilder.NewBuilder()
                .RecipientID(memoConfiguration.Header.Recipient.Id)
                .IdType(memoConfiguration.Header.Recipient.IdType)
                .Build();
        }

        private MainDocument BuildMainDocumentation()
        {
            return MainDocumentBuilder.NewBuilder()
                .DocumentID(memoConfiguration.Body.MainDocument.Id)
                .Label(memoConfiguration.Body.MainDocument.Label)
                .AddFile(BuildFile())
                .Build();
        }

        private File BuildFile()
        {
            return FileBuilder.NewBuilder()
                .Language(memoConfiguration.Body.MainDocument.Language)
                .EncodingFormat(memoConfiguration.Body.MainDocument.EncodingFormat)
                .Filename(memoConfiguration.Body.MainDocument.FileName)
                .Content(FileContentBuilder.NewBuilder()
                    .Base64Content(memoConfiguration.Body.MainDocument.Base64Content).Build())
                .Build();
        }
    }
}