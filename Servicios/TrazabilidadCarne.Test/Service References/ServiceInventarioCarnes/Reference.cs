﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.18444
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrazabilidadCarne.Test.ServiceInventarioCarnes {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EntidadItemCarniceria", Namespace="http://schemas.datacontract.org/2004/07/TrazabilidadCarne.Services")]
    [System.SerializableAttribute()]
    public partial class EntidadItemCarniceria : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FineLineNbrField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ItemField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ItemDescripcionField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FineLineNbr {
            get {
                return this.FineLineNbrField;
            }
            set {
                if ((object.ReferenceEquals(this.FineLineNbrField, value) != true)) {
                    this.FineLineNbrField = value;
                    this.RaisePropertyChanged("FineLineNbr");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Item {
            get {
                return this.ItemField;
            }
            set {
                if ((object.ReferenceEquals(this.ItemField, value) != true)) {
                    this.ItemField = value;
                    this.RaisePropertyChanged("Item");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ItemDescripcion {
            get {
                return this.ItemDescripcionField;
            }
            set {
                if ((object.ReferenceEquals(this.ItemDescripcionField, value) != true)) {
                    this.ItemDescripcionField = value;
                    this.RaisePropertyChanged("ItemDescripcion");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EntidadMotivoInventario", Namespace="http://schemas.datacontract.org/2004/07/TrazabilidadCarne.Services")]
    [System.SerializableAttribute()]
    public partial class EntidadMotivoInventario : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescripcionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdMotivoInventarioField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Descripcion {
            get {
                return this.DescripcionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescripcionField, value) != true)) {
                    this.DescripcionField = value;
                    this.RaisePropertyChanged("Descripcion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int IdMotivoInventario {
            get {
                return this.IdMotivoInventarioField;
            }
            set {
                if ((this.IdMotivoInventarioField.Equals(value) != true)) {
                    this.IdMotivoInventarioField = value;
                    this.RaisePropertyChanged("IdMotivoInventario");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResultadoProceso", Namespace="http://schemas.datacontract.org/2004/07/TrazabilidadCarne.Services")]
    [System.SerializableAttribute()]
    public partial class ResultadoProceso : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int EstadoResutadoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal ItemField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int EstadoResutado {
            get {
                return this.EstadoResutadoField;
            }
            set {
                if ((this.EstadoResutadoField.Equals(value) != true)) {
                    this.EstadoResutadoField = value;
                    this.RaisePropertyChanged("EstadoResutado");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Item {
            get {
                return this.ItemField;
            }
            set {
                if ((this.ItemField.Equals(value) != true)) {
                    this.ItemField = value;
                    this.RaisePropertyChanged("Item");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceInventarioCarnes.IInventarioCarnes")]
    public interface IInventarioCarnes {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInventarioCarnes/ObtieneItemCaniceria", ReplyAction="http://tempuri.org/IInventarioCarnes/ObtieneItemCaniceriaResponse")]
        TrazabilidadCarne.Test.ServiceInventarioCarnes.EntidadItemCarniceria[] ObtieneItemCaniceria();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInventarioCarnes/ObtieneMotivoInventario", ReplyAction="http://tempuri.org/IInventarioCarnes/ObtieneMotivoInventarioResponse")]
        TrazabilidadCarne.Test.ServiceInventarioCarnes.EntidadMotivoInventario[] ObtieneMotivoInventario();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInventarioCarnes/IngresoItem", ReplyAction="http://tempuri.org/IInventarioCarnes/IngresoItemResponse")]
        TrazabilidadCarne.Test.ServiceInventarioCarnes.ResultadoProceso IngresoItem(int itemPadre, int store, int item, string fechaInventario, string fechaLectura, int origenFrigorifico, decimal certificadoEmbarque, int fechaFaena, decimal pesoNeto, decimal pesoBruto, int cajas, string responsable, int idMotivonventario);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IInventarioCarnesChannel : TrazabilidadCarne.Test.ServiceInventarioCarnes.IInventarioCarnes, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class InventarioCarnesClient : System.ServiceModel.ClientBase<TrazabilidadCarne.Test.ServiceInventarioCarnes.IInventarioCarnes>, TrazabilidadCarne.Test.ServiceInventarioCarnes.IInventarioCarnes {
        
        public InventarioCarnesClient() {
        }
        
        public InventarioCarnesClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public InventarioCarnesClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InventarioCarnesClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InventarioCarnesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public TrazabilidadCarne.Test.ServiceInventarioCarnes.EntidadItemCarniceria[] ObtieneItemCaniceria() {
            return base.Channel.ObtieneItemCaniceria();
        }
        
        public TrazabilidadCarne.Test.ServiceInventarioCarnes.EntidadMotivoInventario[] ObtieneMotivoInventario() {
            return base.Channel.ObtieneMotivoInventario();
        }
        
        public TrazabilidadCarne.Test.ServiceInventarioCarnes.ResultadoProceso IngresoItem(int itemPadre, int store, int item, string fechaInventario, string fechaLectura, int origenFrigorifico, decimal certificadoEmbarque, int fechaFaena, decimal pesoNeto, decimal pesoBruto, int cajas, string responsable, int idMotivonventario) {
            return base.Channel.IngresoItem(itemPadre, store, item, fechaInventario, fechaLectura, origenFrigorifico, certificadoEmbarque, fechaFaena, pesoNeto, pesoBruto, cajas, responsable, idMotivonventario);
        }
    }
}
