using System;
using System.Collections.Generic;
using System.IO;
using Dk.Digst.Digital.Post.Memolib.Model;
using Dk.Digst.Digital.Post.Memolib.Parser;
using Dk.Digst.Digital.Post.Memolib.Util;
using Dk.Digst.Digital.Post.Memolib.Visitor;
using Microsoft.Extensions.Logging;

namespace SenderSystem.UtilityLibrary.Memos.Services.Parser
{
    public class ParseMemoService : IMemoService
    {
        private readonly ILogger logger;

        public ParseMemoService(ILogger<ParseMemoService> logger)
        {
            this.logger = logger;
        }

        public MessageHeader ParseMemoHeader(Stream memoFile)
        {
            MessageHeader memoMessageHeader = null;

            IMeMoParser parser = MeMoParserFactory.CreateParser(memoFile, true);
            IMeMoStreamProcessorVisitor<MessageHeader> messageHeaderVisitor =
                MeMoStreamVisitorFactory.CreateProcessor<MessageHeader>();

            try
            {
                parser.Traverse(new List<IMeMoStreamVisitor> { messageHeaderVisitor });
                Optional<MessageHeader> messageHeaderOptional = messageHeaderVisitor.GetSingleResult();
                if (messageHeaderOptional.IsPresent())
                {
                    memoMessageHeader = messageHeaderOptional.Get();
                }
            }
            catch (Exception e)
            {
                logger.LogError($"MeMo is invalid and cannot be parsed: '{e}'");
            }

            return memoMessageHeader;
        }
    }
}