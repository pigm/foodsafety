using System;
using System.Collections.Generic;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FoodSafety;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Utils;

namespace TrazabilidadCarnes.Adapters
{
    /// <summary>
    /// Adapter sucursal.
    /// </summary>
    public class AdapterSucursal : RecyclerView.Adapter
    {
        List<Sucursal> list;
        List<Sucursal> originList;
        List<object> lNombreLatLong;
        public event EventHandler<List<Object>> itemClick;
        FragmentSucursales fragment;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TrazabilidadCarnes.Adapters.AdapterSucursal"/> class.
        /// </summary>
        /// <param name="lista">Lista.</param>
        /// <param name="fragment">Fragment.</param>
        public AdapterSucursal(List<Sucursal> lista, FragmentSucursales fragment)
        {
            list = lista;
            originList = lista;
            this.fragment = fragment;
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
        public class SucursalViewHolder : RecyclerView.ViewHolder
        {

            public View mView;
            public TextView mNombre { get; private set; }
            public TextView mDistancia { get; private set; }
            public ImageView mImage { get; set; }
            public LinearLayout row { get; private set; }

            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="T:TrazabilidadCarnes.Adapters.AdapterSucursal.SucursalViewHolder"/> class.
            /// </summary>
            /// <param name="view">View.</param>
            /// <param name="listener">Listener.</param>
            public SucursalViewHolder(View view, Action<List<Object>> listener) : base(view)
            {
                mNombre = view.FindViewById<TextView>(Resource.Id.nombre_sucursal);
                mDistancia = view.FindViewById<TextView>(Resource.Id.distancia_sucursal);
                mImage = view.FindViewById<ImageView>(Resource.Id.image_sucursal);
                row = view.FindViewById<LinearLayout>(Resource.Id.fila_sucursal);
            }
        }

        /// <summary>
        /// Ons the bind view holder.
        /// </summary>
        /// <param name="holder">Holder.</param>
        /// <param name="position">Position.</param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            SucursalViewHolder miholder = holder as SucursalViewHolder;
            LinearLayout view = miholder.row;
            ImageView icon = (ImageView)view.FindViewById(Resource.Id.image_sucursal);
            Sucursal sucursal = list[position];
            icon.SetImageResource(Resource.Drawable.IconoTienda);
            miholder.mImage = icon;
            miholder.mNombre.Text = sucursal.name;
            miholder.mDistancia.Text = sucursal.location.address;//Math.Round(sucursal.UserDistance, 1) + " Km";
        }

        /// <summary>
        /// Ons the create view holder.
        /// </summary>
        /// <returns>The create view holder.</returns>
        /// <param name="parent">Parent.</param>
        /// <param name="viewType">View type.</param>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View card = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.row_sucursal, parent, false);
            SucursalViewHolder viewholder = new SucursalViewHolder(card, onClick);

            viewholder.row.Click += (sender, e) =>
            {
                Sucursal sucursal = list[viewholder.AdapterPosition];
                lNombreLatLong = new List<object>();
                lNombreLatLong.Add(sucursal.name);
                lNombreLatLong.Add(new LatLng(sucursal.Latitude, sucursal.Longitude));
                lNombreLatLong.Add(sucursal.location.address);
                this.fragment.Activity.FinishActivity(100);
                string id = sucursal.id+string.Empty;
                DataManager.sucursalSeleccionada = sucursal;
                itemClick(sender, lNombreLatLong);
            };
            return viewholder;
        }



        /// <summary>
        /// Ons the click.
        /// </summary>
        /// <param name="locationName">Location name.</param>
        void onClick(List<Object> locationName)
        {
            Intent intentAHomeAgregaProducto = new Intent(fragment.Activity, typeof(HomeAgregaProductoActivity));
            fragment.Activity.StartActivity(intentAHomeAgregaProducto);
        }


        /// <summary>
        /// Refreshs the adapter.
        /// </summary>
        /// <param name="l">L.</param>
        /// <param name="map">Map.</param>
        public void refreshAdapter(List<Sucursal> l, GoogleMap map)
        {
            list = new List<Sucursal>();
            list = SortSucursales(l);
            originList = SortSucursales(l);
            LatLng location = new LatLng(originList[0].Latitude, originList[1].Longitude);
            map.MoveCamera(CameraUpdateFactory.NewLatLng(location));
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(15);
            builder.Bearing(0);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            map.MoveCamera(cameraUpdate);
            map.MyLocationEnabled = true;
            NotifyDataSetChanged();
        }

        /// <summary>
        /// Sorts the sucursales.
        /// </summary>
        /// <returns>The sucursales.</returns>
        /// <param name="sSucursal">S sucursal.</param>
        private List<Sucursal> SortSucursales(List<Sucursal> sSucursal)
        {
            sSucursal.Sort((x, y) =>
            {
                if (x.HasUserDistance && !y.HasUserDistance)
                {
                    return -1;
                }
                else if (!x.HasUserDistance && y.HasUserDistance)
                {
                    return 1;
                }
                else if (!x.HasUserDistance && !y.HasUserDistance)
                {
                    return 0;
                }

                if (x.UserDistance < y.UserDistance)
                    return -1;
                else if (Math.Abs(x.UserDistance - y.UserDistance) <= Double.Epsilon)
                    return 0;
                else
                    return 1;
            });
            return sSucursal;
        }
    }
}