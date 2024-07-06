using ProjectManagment.Areas.Identity.Data;
using ProjectManagment.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagment.DTOs.Requests
{
    public class CreateCommentReq
    {
        public string Content { get; set; }
    }
}
