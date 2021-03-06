﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using WhyNotEarth.Meredith.Data.Entity;
using WhyNotEarth.Meredith.Data.Entity.Models;
using WhyNotEarth.Meredith.Email;
using WhyNotEarth.Meredith.Volkswagen;

namespace WhyNotEarth.Meredith.Jobs.Volkswagen
{
    public class MemoJob
    {
        private readonly MeredithDbContext _dbContext;
        private readonly SendGridService _sendGridService;

        public MemoJob(MeredithDbContext dbContext, SendGridService sendGridService)
        {
            _dbContext = dbContext;
            _sendGridService = sendGridService;
        }

        public async Task SendAsync(int memoId)
        {
            var memo = await _dbContext.Memos.FirstOrDefaultAsync(item => item.Id == memoId);
            var company = await _dbContext.Companies.FirstOrDefaultAsync(item => item.Name == VolkswagenCompany.Slug);

            var templateData = new Dictionary<string, object>
            {
                {"subject", memo.Subject},
                {"date", memo.Date},
                {"to", memo.To},
                // To handle new lines correctly we are replacing them with <br> and use {{{description}}}
                // so we have to html encode here
                {"description", Regex.Replace(HttpUtility.HtmlEncode(memo.Description), @"\r\n?|\n", "<br>")}
            };

            var recipients = await _dbContext.EmailRecipients
                .Where(item => item.MemoId == memoId && item.Status == EmailStatus.ReadyToSend)
                .ToListAsync();

            foreach (var batch in recipients.Batch(SendGridService.BatchSize))
            {
                await _sendGridService.SendEmail(company.Id, recipients, templateData, nameof(EmailRecipient.MemoId),
                    memo.Id.ToString());

                foreach (var recipient in batch)
                {
                    recipient.Status = EmailStatus.Sent;
                }

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}