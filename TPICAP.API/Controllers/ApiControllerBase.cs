using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TPICAP.API.Controllers
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private HttpRequest request;

        public new HttpRequest Request
        {
            get
            {
                if (this.request == null)
                {
                    return base.Request;
                }
                return this.request;
            }
            set
            {
                this.request = value;
            }
        }

        protected void ValidateId(int urlId, int entityId)
        {
            if (urlId != entityId)
            {
                throw new Exceptions.InvalidRequestException();
            }
        }
    }
}
