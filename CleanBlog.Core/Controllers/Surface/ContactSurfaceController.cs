using CleanBlog.Core.ViewModels;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Core.Logging;
using CleanBlog.Core.Services;

namespace CleanBlog.Core.Controllers.Surface
{
    public class ContactSurfaceController : SurfaceController
    {
        private readonly ISmtpService _smtpService;

        public ContactSurfaceController(ISmtpService smtpService)
        {
            _smtpService = smtpService;
        }

        [HttpGet]
        public ActionResult RenderForm()
        {

            var vm = new ContactViewModel
            {
                ContactFormId = CurrentPage.Id
            };

            return PartialView("~/Views/Partials/Contact/contact-form.cshtml", vm);

        }

        [HttpPost]
        public ActionResult RenderForm(ContactViewModel model)
        {
            return PartialView("~/Views/Partials/Contact/contact-form.cshtml", model);
        }

        public ActionResult SubmitForm(ContactViewModel model)
        {
            var success = false;

            if( ModelState.IsValid)
            {
                success = _smtpService.SendEmail(model);
            }

            var contactPage = UmbracoContext.Content.GetById(false, model.ContactFormId);

            var successMessage = contactPage.Value<IHtmlString>("successMessage");
            var errorMessage = contactPage.Value<IHtmlString>("errorMessage");

            return PartialView("~/Views/Partials/Contact/result.cshtml", success ? successMessage : errorMessage);

        }

    }
}
