﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.18052
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrazabilidadCarne.Test.TrazabilidadCarnesSericeLocal {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TrazabilidadCarnesSericeLocal.ILocal")]
    public interface ILocal {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocal/ObtieneLocales", ReplyAction="http://tempuri.org/ILocal/ObtieneLocalesResponse")]
        TrazabilidadCarne.Services.EntidadLocal[] ObtieneLocales();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILocalChannel : TrazabilidadCarne.Test.TrazabilidadCarnesSericeLocal.ILocal, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LocalClient : System.ServiceModel.ClientBase<TrazabilidadCarne.Test.TrazabilidadCarnesSericeLocal.ILocal>, TrazabilidadCarne.Test.TrazabilidadCarnesSericeLocal.ILocal {
        
        public LocalClient() {
        }
        
        public LocalClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LocalClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LocalClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LocalClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public TrazabilidadCarne.Services.EntidadLocal[] ObtieneLocales() {
            return base.Channel.ObtieneLocales();
        }
          
        //public TrazabilidadCarne.Services.EntidadItem[] ObtieneItemBalanza()
        //{
        //    return base.Channel.ObtieneLocales();
        //}
    }
}
