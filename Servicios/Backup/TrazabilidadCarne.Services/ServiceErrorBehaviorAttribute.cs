using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;

namespace TrazabilidadCarne.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceErrorBehaviorAttribute : Attribute, IServiceBehavior
    {
        Type errorHandlerType;
        public ServiceErrorBehaviorAttribute(Type errorHandlerType)
        {
            this.errorHandlerType = errorHandlerType;
        }
        #region IServiceBehavior Members
        public void AddBindingParameters(ServiceDescription serviceDescription,
           System.ServiceModel.ServiceHostBase serviceHostBase,
           System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints,
           System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            IErrorHandler errorHandler;
            errorHandler = (IErrorHandler)Activator.CreateInstance(errorHandlerType);
            foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher cd = cdb as ChannelDispatcher;
                cd.ErrorHandlers.Add(errorHandler);
            }
        }
        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }
        #endregion
    }


}