using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace General.Framework.Controllers
{
    public abstract class BaseController : Controller
    {
        private AjaxResult _ajaxResult;

        public BaseController()
        {
            this._ajaxResult = new AjaxResult();
        }

        /// <summary>
        /// ajax请求的数据结果
        /// </summary>
        public AjaxResult AjaxData
        {
            get
            {
                return _ajaxResult;
            }
        }
    }
}
