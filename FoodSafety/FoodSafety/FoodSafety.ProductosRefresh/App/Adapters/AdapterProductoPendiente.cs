using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Utils;

namespace FoodSafety.ProductosRefresh.Adapters
{
    public class AdapterProductoPendiente : RecyclerView.Adapter
    {
        List<ProductoRefreshPendiente> list;
        List<ProductoRefreshPendiente> originList;
        List<object> lproducto;
        public event EventHandler<List<Object>> itemClick;
        AppCompatActivity appCompatActivity;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TrazabilidadCarnes.Adapters.AdapterSucursal"/> class.
        /// </summary>
        /// <param name="lista">Lista.</param>
        /// <param name="appCompatActivity">Fragment.</param>
        public AdapterProductoPendiente(List<ProductoRefreshPendiente> lista, AppCompatActivity appCompatActivity)
        {
            list = lista;
            originList = lista;
            this.appCompatActivity = appCompatActivity;
        }

        /// <summary>
        /// Gets the item count.
        /// </summary>
        /// <value>The item count.</value>
        public override int ItemCount
        {
            get
            {
                return list.Count;
            }
        }

        /// <summary>
        /// Sucursal view holder.
        /// </summary>
        public class ProductoPendienteViewHolder : RecyclerView.ViewHolder
        {

            public View mView;
            public TextView lblTiempoRestante { get; private set; }
            public TextView lblNombreProductoPendiente { get; private set; }
            public TextView lblInfoProductoPendiente { get; private set; }
            public LinearLayout fila_producto_pendiente { get; private set; }
            public LinearLayout llFondoTiempoRestante { get; private set; }
            public ImageView imgTiempoRestanteOk { get; private set; }

            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="T:TrazabilidadCarnes.Adapters.AdapterSucursal.SucursalViewHolder"/> class.
            /// </summary>
            /// <param name="view">View.</param>
            /// <param name="listener">Listener.</param>
            public ProductoPendienteViewHolder(View view, Action<List<Object>> listener) : base(view)
            {
                imgTiempoRestanteOk = view.FindViewById<ImageView>(Resource.Id.imgTiempoRestanteOk);
                llFondoTiempoRestante = view.FindViewById<LinearLayout>(Resource.Id.llFondoTiempoRestante);
                lblTiempoRestante = view.FindViewById<TextView>(Resource.Id.lblTiempoRestante);
                lblNombreProductoPendiente = view.FindViewById<TextView>(Resource.Id.lblNombreProductoPendiente);
                lblInfoProductoPendiente = view.FindViewById<TextView>(Resource.Id.lblInfoProductoPendiente);
                fila_producto_pendiente = view.FindViewById<LinearLayout>(Resource.Id.fila_producto_pendiente);
            }
        }

        /// <summary>
        /// Restas the tiempo.
        /// </summary>
        /// <returns>The tiempo.</returns>
        /// <param name="sumaHrs">Suma hrs.</param>
        public string restaTiempo(string sumaHrs)
        {
            string horaActual = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
            DateTime hora1Act = Convert.ToDateTime(horaActual);
            DateTime sumHrs = Convert.ToDateTime(sumaHrs);
            TimeSpan resta = sumHrs - hora1Act;
            return resta.Hours + "," + resta.Minutes;
        }

        /// <summary>
        /// Ons the bind view holder.
        /// </summary>
        /// <param name="holder">Holder.</param>
        /// <param name="position">Position.</param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ProductoPendienteViewHolder miholder = holder as ProductoPendienteViewHolder;
            LinearLayout view = miholder.fila_producto_pendiente;             
            ProductoRefreshPendiente producto = list[position];
            var tiempoRestanteAmostrar = restaTiempo(producto.TiempoRestante);
            var hrs = tiempoRestanteAmostrar.Substring(0, 1);
            var dateTimeNow = DateTime.Now;
            var horaActual = String.Format("{0:HH:mm}", dateTimeNow);          
            var timeRestante = String.Format("{0:HH:mm}", DateTime.Parse(producto.TiempoRestante));
            DateTime dtHoraActual = DateTime.Parse(horaActual);
            DateTime dtTimeRestante = DateTime.Parse(timeRestante);
            if (hrs.Equals("-"))
            {
                hrs = "0";
            }
            int time = int.Parse(hrs);        
            if (producto.DayOfCharge == DateTime.Now.DayOfYear)
            {
                /*if (dtTimeRestante <= dtHoraActual)
                {
                    miholder.llFondoTiempoRestante.SetBackgroundResource(Resource.Drawable.BadgeCircleOK);
                    miholder.lblTiempoRestante.Text = tiempoRestanteAmostrar + "h";
                    miholder.lblTiempoRestante.Visibility = ViewStates.Gone;
                    miholder.imgTiempoRestanteOk.Visibility = ViewStates.Visible;                                 
                }
                else
                {
                    miholder.llFondoTiempoRestante.SetBackgroundResource(Resource.Drawable.BadgeCircle);
                    miholder.lblTiempoRestante.Text = tiempoRestanteAmostrar + "h";
                    miholder.lblTiempoRestante.Visibility = ViewStates.Visible;
                    miholder.imgTiempoRestanteOk.Visibility = ViewStates.Gone;
                }*/
                miholder.llFondoTiempoRestante.SetBackgroundResource(Resource.Drawable.BadgeCirclePendiente);
                miholder.lblTiempoRestante.Text = tiempoRestanteAmostrar + "h";
                miholder.lblTiempoRestante.Visibility = ViewStates.Gone;
                miholder.imgTiempoRestanteOk.Visibility = ViewStates.Visible;

                miholder.lblNombreProductoPendiente.Text = producto.NombreProducto;
                string HoraCreacionFormateada = "";
                if (!producto.HoraCreacion.Contains(":"))
                {
                    string hr = producto.HoraCreacion.Substring(0, 2);
                    string min = producto.HoraCreacion.Substring(2, 2);
                    string seg = producto.HoraCreacion.Substring(4, 2);
                    HoraCreacionFormateada = hr + ":" + min + ":" + seg;
                }
                else
                    HoraCreacionFormateada = producto.HoraCreacion;
                miholder.lblInfoProductoPendiente.Text = "Item " + producto.ItemPadre + ", último registro " + HoraCreacionFormateada + " hrs.";
            }
        }

        /// <summary>
        /// Ons the create view holder.
        /// </summary>
        /// <returns>The create view holder.</returns>
        /// <param name="parent">Parent.</param>
        /// <param name="viewType">View type.</param>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View card = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.row_producto_pendiente, parent, false);
            ProductoPendienteViewHolder viewholder = new ProductoPendienteViewHolder(card, onClick);

            viewholder.fila_producto_pendiente.Click += (sender, e) =>
            {
                ProductoRefreshPendiente prodpendiente = list[viewholder.AdapterPosition];
                lproducto = new List<object>();
                lproducto.Add(prodpendiente.ItemPadre);
                lproducto.Add(prodpendiente.FechaElaboracion);
                lproducto.Add(prodpendiente.LoteProduccion);
                lproducto.Add(prodpendiente.FechaDescongelado);
                lproducto.Add(prodpendiente.IdParametro);
                lproducto.Add(prodpendiente.Temperatura);
                lproducto.Add(prodpendiente.EtiquetaPropia);
                lproducto.Add(prodpendiente.UsuarioCreacion);
                lproducto.Add(prodpendiente.FechaCreacion);
                lproducto.Add(prodpendiente.HoraCreacion);
                lproducto.Add(prodpendiente.Comentario);
                lproducto.Add(prodpendiente.CantidadUnidades);
                lproducto.Add(prodpendiente.Imagen);
                lproducto.Add(prodpendiente.NombreProducto);
                lproducto.Add(prodpendiente.TiempoRestante);

                appCompatActivity.FinishActivity(100);
                itemClick(sender, lproducto);
                DataManager.ProductoPendienteSeleccionado = prodpendiente;
            };
            return viewholder;
        }
         
        /// <summary>
        /// Ons the click.
        /// </summary>
        /// <param name="locationName">Location name.</param>
        void onClick(List<Object> locationName)
        {
            Intent intentAHomeAgregaProducto = new Intent(appCompatActivity, typeof(ProductosPendientesActivity));
            appCompatActivity.StartActivity(intentAHomeAgregaProducto);
        }
    }
}
