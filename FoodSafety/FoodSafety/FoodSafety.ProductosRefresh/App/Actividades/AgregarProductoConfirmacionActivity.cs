using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Services.Delegate;
using FoodSafety.Common.Utils;
using FoodSafety.ProductosRefresh.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FoodSafety.ProductosRefresh
{
    /// <summary>
    /// Agregar producto confirmacion activity.
    /// </summary>
    [Activity(Label = "AgregarProductoConfirmacionActivity")]
    public class AgregarProductoConfirmacionActivity : Activity
    {
        static Dialog customDialog = null;

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.agregar_producto_confirmacion_dialogo);
        }

        /// <summary>
        /// Dialogos the confirmacion agregar producto.
        /// </summary>
        /// <param name="activity">Activity.</param>
        /// <param name="item">Item.</param>
        /// <param name="itemNombreProducto">Item nombre producto.</param>
        /// <param name="txtCantidadDeUnidadesProducto">Text cantidad de unidades producto.</param>
        /// <param name="itemBalanza">Item balanza.</param>
        /// <param name="itemDpto">Item dpto.</param>
        /// <param name="fechaVencDesc">Fecha venc desc.</param>
        /// <param name="txtFechaDescongelacion">Text fecha descongelacion.</param>
        /// <param name="temperaturaIdeal">Temperatura ideal.</param>
        /// <param name="tiempo">Tiempo.</param>
        public static void dialogoConfirmacionAgregarProducto(Activity activity
                                                              ,IngresoItemRefreshRequest item
                                                              ,string itemNombreProducto
                                                              ,string txtCantidadDeUnidadesProducto
                                                              ,string itemBalanza
                                                              ,string itemDpto
                                                              ,string fechaVencDesc
                                                              ,string txtFechaDescongelacion
                                                              ,string temperaturaIdeal
                                                              ,string tiempo)
        {
            customDialog = new Dialog(activity, Resource.Style.Theme_Dialog_Translucent);
            customDialog.SetCancelable(false);
            customDialog.SetContentView(Resource.Layout.agregar_producto_confirmacion_dialogo);
            ImageView imgLoadingBtnAgregaPruductoConfirmacion = customDialog.FindViewById<ImageView>(Resource.Id.imgLoadingBtnAgregaPruductoConfirmacion);
            Glide.With(activity).Load(Resource.Drawable.cargando).Into(imgLoadingBtnAgregaPruductoConfirmacion);
            TextView lblBtnAgregarProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblBtnAgregarProductoConfirmacion);
            ImageView imgDanger = customDialog.FindViewById<ImageView>(Resource.Id.imgDanger);
            TextView lblNombreProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblNombreProductoConfirmacion);
            lblNombreProductoConfirmacion.Text = itemNombreProducto;
            TextView lblItemProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblItemProductoConfirmacion);
            lblItemProductoConfirmacion.Text = itemBalanza;
            TextView lblDepartamentoProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblDepartamentoProductoConfirmacion);
            lblDepartamentoProductoConfirmacion.Text = itemDpto;

            TextView lblFechaDescongelacionProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblFechaDescongelacionProductoConfirmacion);
            lblFechaDescongelacionProductoConfirmacion.Text = txtFechaDescongelacion;
            TextView lblFechaVencimientoDescongelacionProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblFechaVencimientoDescongelacionProductoConfirmacion);
            lblFechaVencimientoDescongelacionProductoConfirmacion.Text = fechaVencDesc;
            TextView lblCantidadDeUnidadesProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblCantidadDeUnidadesProductoConfirmacion);
            lblCantidadDeUnidadesProductoConfirmacion.Text = item.CantidadUnidades;
            TextView lblTemperaturaIdealProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblTemperaturaIdealProductoConfirmacion);
            lblTemperaturaIdealProductoConfirmacion.Text = temperaturaIdeal + " ºC";
            TextView lblHintLoteProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblHintLoteProductoConfirmacion);
            TextView lblLoteProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblLoteProductoConfirmacion);

            TextView lblHintFechaElaboracionProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblHintFechaElaboracionProductoConfirmacion);
            TextView lblFechaElaboracionProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblFechaElaboracionProductoConfirmacion);
            TextView lblHintTemperaturaProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblHintTemperaturaProductoConfirmacion);
            TextView lblTemperaturaProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblTemperaturaProductoConfirmacion);
            TextView lblHintComentarioProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblHintComentarioProductoConfirmacion);
            TextView lblComentarioProductoConfirmacion = customDialog.FindViewById<TextView>(Resource.Id.lblComentarioProductoConfirmacion);
            LinearLayout llMensajeProductoExistente = customDialog.FindViewById<LinearLayout>(Resource.Id.llMensajeProductoExistente);
            if (string.IsNullOrEmpty(item.LoteProduccion))
            {
                lblHintLoteProductoConfirmacion.Visibility = ViewStates.Gone;
                lblLoteProductoConfirmacion.Visibility = ViewStates.Gone;
                lblHintTemperaturaProductoConfirmacion.Visibility = ViewStates.Visible;
                lblTemperaturaProductoConfirmacion.Visibility = ViewStates.Visible;
                lblTemperaturaProductoConfirmacion.Text = item.Temperatura + " ºC";
                lblHintComentarioProductoConfirmacion.Visibility = ViewStates.Visible;
                lblComentarioProductoConfirmacion.Visibility = ViewStates.Visible;
                lblComentarioProductoConfirmacion.Text = item.Comentario; 
                lblHintFechaElaboracionProductoConfirmacion.Visibility = ViewStates.Visible;
                lblFechaElaboracionProductoConfirmacion.Visibility = ViewStates.Visible;
                lblFechaElaboracionProductoConfirmacion.Text = item.FechaElaboracion;
            }else{
                lblHintLoteProductoConfirmacion.Visibility = ViewStates.Visible;
                lblLoteProductoConfirmacion.Visibility = ViewStates.Visible;
                lblLoteProductoConfirmacion.Text = item.LoteProduccion;
                lblHintTemperaturaProductoConfirmacion.Visibility = ViewStates.Visible;
                lblTemperaturaProductoConfirmacion.Visibility = ViewStates.Visible;
                lblTemperaturaProductoConfirmacion.Text = item.Temperatura + " ºC";
                lblHintComentarioProductoConfirmacion.Visibility = ViewStates.Visible;
                lblComentarioProductoConfirmacion.Visibility = ViewStates.Visible;
                lblComentarioProductoConfirmacion.Text = item.Comentario;
                lblHintFechaElaboracionProductoConfirmacion.Visibility = ViewStates.Gone;
                lblFechaElaboracionProductoConfirmacion.Visibility = ViewStates.Gone;
            }
            List<LoteParceladoProducto> existenProductosLoteParceladoExacto = DataManager.RealmInstance.All<LoteParceladoProducto>().Where(w => w.ItemPadre == item.ItemPadre && w.DayOfCharge == DataManager.FechaHoy && w.LoteProduccion == item.LoteProduccion && w.CantidadUnidades == item.CantidadUnidades && w.Comentario == item.Comentario).ToList();
            List<LoteParceladoProducto> existenProductosLoteParcelado = DataManager.RealmInstance.All<LoteParceladoProducto>().Where(w => w.ItemPadre == item.ItemPadre && w.DayOfCharge == DataManager.FechaHoy && w.LoteProduccion == item.LoteProduccion && w.CantidadUnidades == item.CantidadUnidades).ToList();
            List<LoteParceladoProducto> existenProductosLoteParceladoCompleta = DataManager.RealmInstance.All<LoteParceladoProducto>().Where(w => w.ItemPadre == item.ItemPadre && w.DayOfCharge == DataManager.FechaHoy).ToList();
            List<ProductoRefreshPendiente> existenProductosPendientesLote = DataManager.RealmInstance.All<ProductoRefreshPendiente>().Where(w => w.ItemPadre == item.ItemPadre && w.DayOfCharge == DataManager.FechaHoy).ToList();
            if (existenProductosLoteParcelado.Any())
            {
                llMensajeProductoExistente.Visibility = ViewStates.Visible;
            }
            else
            {
                llMensajeProductoExistente.Visibility = ViewStates.Gone;
            }
            //if (!item.Temperatura.Equals(temperaturaIdeal))
            if (Convert.ToDecimal(item.Temperatura, CultureInfo.InvariantCulture) <= Convert.ToDecimal(temperaturaIdeal, CultureInfo.InvariantCulture)
            && !item.Temperatura.Equals(temperaturaIdeal))
            { 
                lblHintTemperaturaProductoConfirmacion.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(activity.ApplicationContext, Resource.Color.colorRed));
                lblTemperaturaProductoConfirmacion.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(activity.ApplicationContext, Resource.Color.colorRed));
                imgDanger.Visibility = ViewStates.Visible;
            }
            else
            {
                lblHintTemperaturaProductoConfirmacion.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(activity.ApplicationContext, Resource.Color.gris));
                lblTemperaturaProductoConfirmacion.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(activity.ApplicationContext, Resource.Color.gris));
                imgDanger.Visibility = ViewStates.Gone;
            }

            Button btnCancelarProductoConfirmacion = customDialog.FindViewById<Button>(Resource.Id.btnCancelarProductoConfirmacion);
            btnCancelarProductoConfirmacion.Click += delegate {
                customDialog.Dismiss();
            };
            LinearLayout llBtnAgregarProductoConfirmacion = customDialog.FindViewById<LinearLayout>(Resource.Id.llBtnAgregarProductoConfirmacion);
            Button btnEsUnNuevoProducto = customDialog.FindViewById<Button>(Resource.Id.btnEsUnNuevoProducto);
            Button btnEsElMismoProducto = customDialog.FindViewById<Button>(Resource.Id.btnEsElMismoProducto);


            if (!existenProductosLoteParcelado.Any())
            {
                llBtnAgregarProductoConfirmacion.Visibility = ViewStates.Visible;
                btnEsUnNuevoProducto.Visibility = ViewStates.Gone;
                btnEsElMismoProducto.Visibility = ViewStates.Gone;
            }
            else
            {
                llBtnAgregarProductoConfirmacion.Visibility = ViewStates.Gone;
                btnEsUnNuevoProducto.Visibility = ViewStates.Visible;
                btnEsElMismoProducto.Visibility = ViewStates.Visible;
            }                 

            llBtnAgregarProductoConfirmacion.Click += async delegate {
                llBtnAgregarProductoConfirmacion.Clickable = false;
                llBtnAgregarProductoConfirmacion.Enabled = false;
                imgLoadingBtnAgregaPruductoConfirmacion.Visibility = ViewStates.Visible;
                lblBtnAgregarProductoConfirmacion.Visibility = ViewStates.Gone;
                if (item.FechaElaboracion.Contains("/"))
                {
                    DateTime dt = DateTime.ParseExact(item.FechaElaboracion, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    var fechaElabService = String.Format("{0:yyyyMMdd}", dt);
                    item.FechaElaboracion = fechaElabService;
                }
                if (!string.IsNullOrEmpty(item.LoteProduccion))
                {
                    if (string.IsNullOrEmpty(DataManager.correlativo.ToString()) || DataManager.correlativo == 0)
                    {
                        if (existenProductosLoteParceladoCompleta.Count >= 1)
                        {
                            item.Correlativo = int.Parse(existenProductosLoteParceladoCompleta.LastOrDefault().Correlativo) + 1;
                        }
                        else
                        {
                            item.Correlativo = 1;
                        }
                    }
                    else
                    {
                        item.Correlativo = DataManager.correlativo;
                    }
                }
                string postData = JsonConvert.SerializeObject(item);
                var itemInventario = await ServiceDelegate.Instance.IngresoItemRefreshImg(JObject.Parse(postData));
                if (itemInventario.Success)
                {
                    imgLoadingBtnAgregaPruductoConfirmacion.Visibility = ViewStates.Gone;
                    lblBtnAgregarProductoConfirmacion.Visibility = ViewStates.Visible;
                    DataManager.estadoProductoAgregado = true;
                    DataManager.nombreProductoAgregado = itemNombreProducto;
                    DataManager.cantidadUnidadesProductoAgregado = "ha sido agregado por " + txtCantidadDeUnidadesProducto + " unidades";
                    DataManager.fechaDescongelacionManager = string.Empty;
                    DataManager.fechaElaboracionManager = string.Empty;
                    Intent intentAlHomeAgregaProductoActivity = new Intent(activity, typeof(HomeAgregaProductoActivity));
                    activity.StartActivity(intentAlHomeAgregaProductoActivity);
                    DataManager.estadoBusquedaProducto = string.Empty;
                    AnalyticService.TrackAnalytics("Agregar producto app", new Dictionary<string, string> {
                        { "Category", "Codigo retorno success true" },
                        { "Action", "producto " + DataManager.refreshNombreProducto + DataManager.cantidadUnidadesProductoAgregado + ". En el servicio"}
                    });                  
                }
                else
                {
                    DataManager.RealmInstance.Write(() =>
                    {
                        DataManager.RealmInstance.Add(item);
                    });
                    imgLoadingBtnAgregaPruductoConfirmacion.Visibility = ViewStates.Gone;
                    lblBtnAgregarProductoConfirmacion.Visibility = ViewStates.Visible;
                    DataManager.estadoProductoAgregado = false;
                    DataManager.fechaDescongelacionManager = string.Empty;
                    DataManager.fechaElaboracionManager = string.Empty;
                    Intent intentAlHomeAgregaProductoActivity = new Intent(activity, typeof(HomeAgregaProductoActivity));
                    activity.StartActivity(intentAlHomeAgregaProductoActivity);
                    DataManager.estadoBusquedaProducto = string.Empty;
                    AnalyticService.TrackAnalytics("Agregar producto app", new Dictionary<string, string> {
                        { "Category", "Codigo retorno success true" },
                        { "Action", "producto " + DataManager.refreshNombreProducto + DataManager.cantidadUnidadesProductoAgregado + ". En el bd local"}
                    });                  
                }
                if (!string.IsNullOrEmpty(item.LoteProduccion))
                {
                    if (string.IsNullOrEmpty(DataManager.correlativo.ToString()) || DataManager.correlativo == 0)
                    {
                        if (existenProductosLoteParceladoCompleta.Count >= 1)
                        {
                            item.Correlativo = int.Parse(existenProductosLoteParceladoCompleta.LastOrDefault().Correlativo) + 1;
                        }
                        else
                        {
                            item.Correlativo = 1;
                        }
                    }
                    else
                    {
                        item.Correlativo = DataManager.correlativo;
                    }
                }
                //*---Validar si temperatura ingresada es igual a la temperatura ideal
                //* ---de no cumplirse, se debe agregar a la bd local de productos pendientes
                //if (!item.Temperatura.Equals(temperaturaIdeal))
                if (Convert.ToDecimal(item.Temperatura, CultureInfo.InvariantCulture) <= Convert.ToDecimal(temperaturaIdeal, CultureInfo.InvariantCulture)
                && !item.Temperatura.Equals(temperaturaIdeal))
                {
                    string hora = item.HoraCreacion;
                    string a = hora.Insert(2, ":");
                    string horaCrea = a.Insert(5, ":");
                    string tiempoHrsRestante = calcularTiempo(horaCrea, tiempo);
                    string tiempoRestanteProd = tiempoHrsRestante.Substring(0, 5);
                    //DataManager.cantidadUnidadesProductoAgregado = "quedó registrado pero deberá esperar aproximadamente hasta las " + tiempoRestanteProd + " horas, Para que llegue a su temperatura ideal de " + temperaturaIdeal + " ºC";
                    DataManager.cantidadUnidadesProductoAgregado = "quedó registrado pero deberá esperar que alcance la Tº ideal, Por favor revise su procedimiento refresh.";
                    ProductoRefreshPendiente p1 = new ProductoRefreshPendiente();
                    p1.ItemPadre = item.ItemPadre;
                    p1.FechaElaboracion = item.FechaElaboracion;
                    p1.LoteProduccion = item.LoteProduccion;
                    p1.FechaDescongelado = item.FechaDescongelado;
                    p1.IdParametro = item.IdParametro;
                    p1.Temperatura = item.Temperatura;
                    p1.EtiquetaPropia = item.EtiquetaPropia;
                    p1.UsuarioCreacion = item.UsuarioCreacion;
                    p1.FechaCreacion = item.FechaCreacion;
                    p1.HoraCreacion = item.HoraCreacion;
                    p1.Comentario = item.Comentario;
                    p1.CantidadUnidades = item.CantidadUnidades;
                    p1.Imagen = item.Imagen;
                    p1.NombreProducto = itemNombreProducto;
                    p1.TiempoRestante = tiempoRestanteProd;
                    p1.NombreDpto = itemDpto;
                    p1.FechaVencimiento = fechaVencDesc;
                    p1.EstadoAlertaNotificacion = false;
                    p1.DayOfCharge = DateTime.Now.DayOfYear;
                    p1.Correlativo = item.Correlativo;
                    List<ProductoRefreshPendiente> lll = new List<ProductoRefreshPendiente>();
                    List<LoteParceladoProducto> lp = new List<LoteParceladoProducto>();
                    var p1Json = JsonConvert.SerializeObject(p1);
                    LoteParceladoProducto prodAddLote = JsonConvert.DeserializeObject<LoteParceladoProducto>(p1Json);
                    lp.Add(prodAddLote);
                    lll.Add(p1);
                    var dataPendientes = DataManager.RealmInstance.All<ProductoRefreshPendiente>();
                    var existenProductosPendientes = DataManager.RealmInstance.All<ProductoRefreshPendiente>().Where(x => x.ItemPadre == p1.ItemPadre);
                    if (!string.IsNullOrEmpty(item.LoteProduccion))
                    {

                        DataManager.RealmInstance.Write(() =>
                        {
                            foreach (LoteParceladoProducto pLp in lp)
                            {
                                DataManager.RealmInstance.Add(pLp);
                            }

                        });
                    }
                    if (existenProductosPendientes.Any())
                    {
                        if (DataManager.isProductoNuevo)
                        {
                            DataManager.RealmInstance.Write(() =>
                            {
                                foreach (ProductoRefreshPendiente prp in lll)
                                {
                                    DataManager.RealmInstance.Add(prp);
                                }
                            });

                        }
                        else
                        {
                            var ppendientes = existenProductosPendientes.Where(x => x.Correlativo == p1.Correlativo && x.ItemPadre == p1.ItemPadre);
                            if (ppendientes != null)
                            {
                                using (var transitemref = DataManager.RealmInstance.BeginWrite())
                                {
                                    DataManager.RealmInstance.RemoveRange<ProductoRefreshPendiente>(ppendientes);
                                    transitemref.Commit();
                                }
                                DataManager.RealmInstance.Write(() =>
                                {
                                    foreach (ProductoRefreshPendiente prp in lll)
                                    {
                                        DataManager.RealmInstance.Add(prp);
                                    }
                                });
                            }
                            else{
                                DataManager.RealmInstance.Write(() =>
                                {
                                    foreach (ProductoRefreshPendiente prp in lll)
                                    {
                                        DataManager.RealmInstance.Add(prp);
                                    }
                                });
                            }
                        }
                    }
                    else
                    {
                        DataManager.RealmInstance.Write(() =>
                        {
                            foreach (ProductoRefreshPendiente prp in lll)
                            {
                                DataManager.RealmInstance.Add(prp);
                            }
                        });
                    }
                }
                else
                {
                    var existenProductosPendientes = DataManager.RealmInstance.All<ProductoRefreshPendiente>().Where(x => x.ItemPadre == item.ItemPadre);
                    if (existenProductosPendientes.ToList().Any())
                    {
                        var productoConsulta = existenProductosLoteParceladoExacto.LastOrDefault();
                        var pendienteEliminar = DataManager.RealmInstance.All<ProductoRefreshPendiente>().Where(x => x.ItemPadre == productoConsulta.ItemPadre 
                        && x.CantidadUnidades == productoConsulta.CantidadUnidades && x.Comentario == productoConsulta.Comentario 
                        && x.DayOfCharge == productoConsulta.DayOfCharge && x.EtiquetaPropia == productoConsulta.EtiquetaPropia 
                        && x.FechaCreacion == productoConsulta.FechaCreacion 
                        && x.FechaVencimiento == productoConsulta.FechaVencimiento && x.LoteProduccion == productoConsulta.LoteProduccion 
                        && x.NombreDpto == productoConsulta.NombreDpto && x.NombreProducto== productoConsulta.NombreProducto).LastOrDefault();

                        if (!string.IsNullOrEmpty(item.LoteProduccion))
                        {
                            var listaExistenProductosPendientesLoteParcelado = DataManager.RealmInstance.All<ProductoRefreshPendiente>().ToList();
                            var existenProductosPendientesLoteParcelado = DataManager.RealmInstance.All<ProductoRefreshPendiente>().Where(x => x.ItemPadre == item.ItemPadre && x.Correlativo == pendienteEliminar.Correlativo);
                            using (var transitemref = DataManager.RealmInstance.BeginWrite())
                            {
                                DataManager.RealmInstance.RemoveRange<ProductoRefreshPendiente>(existenProductosPendientesLoteParcelado);
                                transitemref.Commit();
                            }
                        }
                        else
                        {
                            var existenProductosPendientesFechaElaboracion = DataManager.RealmInstance.All<ProductoRefreshPendiente>().Where(x => x.ItemPadre == item.ItemPadre && x.FechaElaboracion == item.FechaElaboracion);
                            using (var transitemref = DataManager.RealmInstance.BeginWrite())
                            {
                                DataManager.RealmInstance.RemoveRange<ProductoRefreshPendiente>(existenProductosPendientesFechaElaboracion);
                                transitemref.Commit();
                            }
                        }
                        GC.Collect();
                    }
                }
            };

            btnEsElMismoProducto.Click += delegate {
                try
                {
                    DataManager.isProductoNuevo = false;
                    DataManager.correlativo = int.Parse(existenProductosLoteParceladoExacto.LastOrDefault().Correlativo);
                    llBtnAgregarProductoConfirmacion.Visibility = ViewStates.Visible;
                    btnEsUnNuevoProducto.Visibility = ViewStates.Gone;
                    btnEsElMismoProducto.Visibility = ViewStates.Gone;
                }
                catch (System.NullReferenceException ex)
                {
                    Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(activity);
                    builder.SetTitle("Productos Refresh");
                    builder.SetIcon(Resource.Mipmap.ic_refresh);
                    builder.SetCancelable(false);
                    builder.SetMessage("El producto no es el mismo, Por favor corrobora que la información de los campos se la misma (Ítem, Cantidad de unidades, Lote, Comentario)");
                    builder.SetPositiveButton("Aceptar", delegate { customDialog.Dismiss(); });
                    builder.Show();
                }

            };

            btnEsUnNuevoProducto.Click += delegate {
                DataManager.isProductoNuevo = true;
                DataManager.correlativo = int.Parse(existenProductosLoteParceladoCompleta.LastOrDefault().Correlativo)+1;
                llBtnAgregarProductoConfirmacion.Visibility = ViewStates.Visible;
                btnEsUnNuevoProducto.Visibility = ViewStates.Gone;
                btnEsElMismoProducto.Visibility = ViewStates.Gone;
            };

            customDialog.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            customDialog.Show();
        }

        /// <summary>
        /// Calculars the tiempo.
        /// </summary>
        /// <returns>The tiempo.</returns>
        /// <param name="itemHoraCreacion">Item hora creacion.</param>
        /// <param name="tiempo">Tiempo.</param>
        public static string calcularTiempo(string itemHoraCreacion, string tiempo)
        {    
            DateTime hora1 = Convert.ToDateTime(itemHoraCreacion);
            DateTime hora2 = Convert.ToDateTime(tiempo);
            DateTime suma = hora1.AddTicks(hora2.Ticks);
            var sumaHrs = suma.Hour.ToString("00") + ":" + suma.Minute.ToString("00") + ":" + suma.Second.ToString("00");
            return sumaHrs;
        }
    }
}