using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Common.Helpers.Interface
{
    public interface IViewRenderService
    {
        string RenderToString(string viewName, object model);
    }
}
