using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Elmah;

namespace TrazabilidadCarne.Services
{
    public class ErrorHandler : IErrorHandler
    {
        #region IErrorHandler Members
        public bool HandleError(Exception error)
        {
            return false;
        }
        public void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            if (error == null)
                return;
            ErrorLog.GetDefault(null).Log(new Elmah.Error(error)); 
        }
        #endregion
    }


}