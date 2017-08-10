using ConFin.Common.Web;
using System;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class ContaFinanceiraController: BaseController
    {
        public ActionResult ContaFinanceira()
        {
            try
            {
                return View("ContaFinanceira");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult GetModalCadastroEdicao()
        {
            try
            {
                return View("_ModalCadastroEdicao");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Post()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Put()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Delete()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
