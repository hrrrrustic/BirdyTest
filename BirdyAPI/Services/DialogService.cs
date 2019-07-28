﻿using System;
using System.Collections.Generic;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;

namespace BirdyAPI.Services
{
    public class DialogService
    {
        private readonly BirdyContext _context;

        public DialogService(BirdyContext context)
        {
            _context = context;
        }

        public List<DialogPreviewDto> GetDialogsPreview(int userId)
        {
            return _context.DialogUsers
                .Where(k => k.FirstUserID == userId)
                .Union(_context.DialogUsers
                    .Where(k => k.SecondUserID == userId))
                .Select(k => GetDialogPreview(k.DialogID, userId))
                .ToList();
        }

        private DialogPreviewDto GetDialogPreview(Guid dialogId, int currentUserId)
        {
            DialogUser currentDialog = _context.DialogUsers.Find(dialogId);

            Message lastMessage = _context.Messages
                .Where(k => k.ConversationID == dialogId)
                .OrderByDescending(k => k.SendDate)
                .FirstOrDefault();

            return DialogPreviewDto.Create(GetInterlocutorUniqueTag(currentDialog, currentUserId),
                GetUserUniqueTag(currentUserId), lastMessage);
        }

        private string GetUserUniqueTag(int userId)
        {
            return _context.Users.Find(userId).UniqueTag;
        }

        private string GetInterlocutorUniqueTag(DialogUser currentDialog, int currentUserId)
        {
            return currentUserId == currentDialog.FirstUserID ? GetUserUniqueTag(currentDialog.SecondUserID) : GetUserUniqueTag(currentUserId);
        }
        public List<MessageDto> GetDialog(int currentUserId, int interlocutorId, int? offset, int? count)
        {
            DialogUser currentDialog = _context.DialogUsers.SingleOrDefault(k =>
                k.FirstUserID == currentUserId && k.SecondUserID == interlocutorId);

            List<Message> lastMessages = _context.Messages
                .Where(k => k.ConversationID == currentDialog.DialogID)
                .OrderByDescending(k => k.SendDate)
                .Skip(offset ?? 0)
                .Take(count ?? 50)
                .ToList();

            return lastMessages.Select(k => new MessageDto(GetUserUniqueTag(k.AuthorID), k.Text, k.SendDate)).ToList();
        }
    }
}
