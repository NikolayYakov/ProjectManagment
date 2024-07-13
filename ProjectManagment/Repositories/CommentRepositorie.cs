using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProjectManagment.Data;
using ProjectManagment.DTOs.Requests;

namespace ProjectManagment.Repositories
{
    public class CommentRepositorie
    {
        private ApplicationDbContext dbContext;
        public CommentRepositorie(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Comment> GetComment(Guid commentId)
        {
            return await dbContext.Comments.FirstOrDefaultAsync(x => x.Id == commentId && !x.isDeleted);
        }

        public async Task CreateComment(Comment comment)
        {
            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteComment(Comment comment)
        {
            comment.isDeleted = true;
            dbContext.SaveChanges();
        }

        public async Task UpdateComment(Comment comment)
        {
            dbContext.Comments.Update(comment);
            dbContext.SaveChanges();
        }

        public async Task<IQueryable<Comment>> GetIssueComments(Guid issueId)
        {
            return dbContext.Comments.Where(x => x.IssueId == issueId && !x.isDeleted).Include(x=>x.Author);
        }
    }
}
