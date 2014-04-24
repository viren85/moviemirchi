
namespace MvcWebRole1.Controllers.api
{
    /// <summary>
    /// get all user who has liked param celeb and send an email to all of them
    /// </summary>
    public class SendMailController : BaseController
    {
        //get api/SendMail?name=[actor, producer, writer etc.]
        protected override string ProcessRequest()
        {
            return string.Empty;
        }
    }
}
