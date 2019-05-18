using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Utils;
using FoodSafety.ProductosRefresh.Properties;
using FR.Ganfra.Materialspinner;
using Microsoft.AppCenter.Crashes;
using ZXing.Mobile;
using static Android.App.DatePickerDialog;

namespace FoodSafety.ProductosRefresh
{
    /// <summary>
    /// Agregar producto activity.
    /// </summary>
    [Activity(Label = "Agregar Producto", Theme = "@style/ThemeNoActionBarAzul", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenLayout)]
    public class AgregarProductoActivity : AppCompatActivity, IOnDateSetListener
    {
        List<SucursalTienda> dataSucursal = DataManager.RealmInstance.All<SucursalTienda>().ToList();
        List<EntidadItemRefresh> dataProducto = DataManager.RealmInstance.All<EntidadItemRefresh>().ToList();
        private ArrayAdapter<String> adapterProductos;
        private const int DATE_DIALOG_FECHA_DESCONGELACION = 1;
        private const int DATE_DIALOG_FECHA_ELABORACION = 2;
        int codeFecha;
        int year = DateTime.Now.Year;
        int mes = DateTime.Now.Month;
        int dia = DateTime.Now.Day;
        byte[] bitmapData;
        Button btnCancelarProducto;
        Button btnBuscar;
        EditText txtCodigoProducto;
        EditText txtFechaDescongelacion;
        EditText txtLoteProducto;
        EditText txtFechaElaboracion;
        EditText txtTemperaturaProducto;
        EditText txtComentarioProducto;
        EditText txtFechaVencimientoDescongelacionProducto;
        EditText txtCantidadDeUnidadesProducto;
        Fuente fuente;
        ImageButton btnLogout;
        ImageButton btnScanCodigoBarra;
        ImageView photoView;
        ImageView btnVolver;
        ImageView imgLoadingBtnAgregaPruducto;
        LinearLayout llBtnAgregarProducto;
        LinearLayout linearLayoutPanelFormularioIngresoProducto;
        LinearLayout linearLayoutBtnIrAMapasDeTienda;
        LinearLayout lltxtFechaElaboracion;
        LinearLayout linearLayoutBtnCambiarUser;
        MaterialSpinner spiItemsProductos;
        RadioButton rbtnLote;
        RadioButton rbtnFechaElaboracion;
        RadioGroup radioGroupInfoAdicionalProducto;
        String nombreSucursal;
        String tipoSucursal;
        int idSucursal;
        String itemBalanza;
        String itemNombreProducto;
        String itemNombreDptoProducto;
        String itemDiasPerecibilidad;
        string[] itemProductoSelecciona = { "Item" };
        TextInputLayout txtInputLoteProducto;
        TextInputLayout txtInputCantidadDeUnidadesProducto;
        TextInputLayout txtInputComentarioProducto;
        TextView lblhinttxtFechaElaboracion;
        TextView lblBtnAgregarProducto;
        TextView lblDepartamento;
        TextView lblNombreActivity;
        TextView lblNombreTienda;
        TextView lblTipoTienda;
        TextView lblNombreUsuarioToolbar;
        TextView lblhintDepartamento;
        TextView txthintFechaDescongelacion;
        TextView lblhintFechaVencimientoDescongelacionProducto;
        Typeface fontRobotoRegular;
        Typeface fontRobotoMedium;
        Typeface fontRobotoBold;
        Typeface fontMyriadProLightCondensed;
        string fechaDescong = string.Empty;
        string fechaElabora = string.Empty;
        string temperaturaIdealProducto;
        string tiempoDeCorte;
        IngresoItemRefreshRequest item;
        
        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            string[] itemsProducto = (from p in dataProducto select p.CodigoBalanza).ToArray();
            string[] itemsNombreProducto = (from p in dataProducto select p.DescripcionItem1).ToArray();
            string[] itemsNombreDepartamentoProducto = (from p in dataProducto select p.NombreDepartamento).ToArray();
            if (WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation0 || WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation180)
            {
                SetContentView(Resource.Layout.agregar_producto_activity);
            }
            else
            {
                SetContentView(Resource.Layout.agregar_producto_horizontal_activity);
            }
            photoView = FindViewById<ImageView>(Resource.Id.photoView);
            itemDiasPerecibilidad = "1";
            lblhintDepartamento = FindViewById<TextView>(Resource.Id.lblhintDepartamento);
            lblhintFechaVencimientoDescongelacionProducto = FindViewById<TextView>(Resource.Id.lblhintFechaVencimientoDescongelacionProducto);
            spiItemsProductos = FindViewById<MaterialSpinner>(Resource.Id.spiItemsProductos);          
            lblhinttxtFechaElaboracion = FindViewById<TextView>(Resource.Id.lblhinttxtFechaElaboracion);
            txtInputLoteProducto = FindViewById<TextInputLayout>(Resource.Id.txtInputLoteProducto);
            txtInputCantidadDeUnidadesProducto = FindViewById<TextInputLayout>(Resource.Id.txtInputCantidadDeUnidadesProducto);
            txtInputComentarioProducto = FindViewById<TextInputLayout>(Resource.Id.txtInputComentarioProducto);
            imgLoadingBtnAgregaPruducto = FindViewById<ImageView>(Resource.Id.imgLoadingBtnAgregaPruducto);
            lblBtnAgregarProducto = FindViewById<TextView>(Resource.Id.lblBtnAgregarProducto);
            llBtnAgregarProducto = FindViewById<LinearLayout>(Resource.Id.llBtnAgregarProducto);
            llBtnAgregarProducto.Click += BtnAgregarProducto_Click;
            btnLogout = FindViewById<ImageButton>(Resource.Id.btnLogout);
            btnLogout.Visibility = ViewStates.Invisible;
            btnScanCodigoBarra = FindViewById<ImageButton>(Resource.Id.btnScanCodigoBarra);
            btnScanCodigoBarra.Click += BtnScanCodigoBarra_Click;
            btnVolver = FindViewById<ImageView>(Resource.Id.btnVolver);
            btnVolver.Click += BtnVolver_Click;
            lblNombreUsuarioToolbar = FindViewById<TextView>(Resource.Id.lblNombreUsuarioToolbar);
            linearLayoutPanelFormularioIngresoProducto = FindViewById<LinearLayout>(Resource.Id.linearLayoutPanelFormularioIngresoProducto);
            linearLayoutBtnIrAMapasDeTienda = FindViewById<LinearLayout>(Resource.Id.linearLayoutBtnIrAMapasDeTienda);
            linearLayoutBtnIrAMapasDeTienda.Click += LinearLayoutBtnIrAMapasDeTienda_Click;

            lblDepartamento = FindViewById<TextView>(Resource.Id.lblDepartamento);
            lltxtFechaElaboracion = FindViewById<LinearLayout>(Resource.Id.lltxtFechaElaboracion);
            lblNombreActivity = FindViewById<TextView>(Resource.Id.lblNombreActivity);
            lblNombreActivity.Text = "Agregar Producto";
            lblNombreTienda = FindViewById<TextView>(Resource.Id.lblNombreTienda);
            linearLayoutBtnCambiarUser = FindViewById<LinearLayout>(Resource.Id.linearLayoutBtnCambiarUser);
            linearLayoutBtnCambiarUser.Visibility = ViewStates.Gone;
            lblTipoTienda = FindViewById<TextView>(Resource.Id.lblTipoTienda);
            nombreSucursal = dataSucursal.FirstOrDefault().name.ToString();
            tipoSucursal = dataSucursal.FirstOrDefault().format.ToString();
            lblNombreTienda.Text = nombreSucursal;
            lblTipoTienda.Text = tipoSucursal;

            btnBuscar = FindViewById<Button>(Resource.Id.btnBuscar);
            btnBuscar.Click += BtnBuscar_Click;
            btnCancelarProducto = FindViewById<Button>(Resource.Id.btnCancelarProducto);
            btnCancelarProducto.Click += BtnCancelarProducto_Click;
            txtLoteProducto = FindViewById<EditText>(Resource.Id.txtLoteProducto);
            txtFechaElaboracion = FindViewById<EditText>(Resource.Id.txtFechaElaboracion);
            txtFechaElaboracion.Touch += TxtFechaElaboracion_Click;
            txtTemperaturaProducto = FindViewById<EditText>(Resource.Id.txtTemperaturaProducto);
            txtComentarioProducto = FindViewById<EditText>(Resource.Id.txtComentarioProducto);
            txtCodigoProducto = FindViewById<EditText>(Resource.Id.txtCodigoProducto);
            txtCantidadDeUnidadesProducto = FindViewById<EditText>(Resource.Id.txtCantidadDeUnidadesProducto);
            txtFechaDescongelacion = FindViewById<EditText>(Resource.Id.txtFechaDescongelacion);           
            txtFechaDescongelacion.Touch += TxtFechaDescongelacion_Click;
            txtFechaVencimientoDescongelacionProducto = FindViewById<EditText>(Resource.Id.txtFechaVencimientoDescongelacionProducto);
            nombreSucursal = dataSucursal.FirstOrDefault().name.ToString();
            tipoSucursal = dataSucursal.FirstOrDefault().format.ToString();
            idSucursal = dataSucursal.FirstOrDefault().id;
            radioGroupInfoAdicionalProducto = FindViewById<RadioGroup>(Resource.Id.radioGroupInfoAdicionalProducto);
            rbtnLote = FindViewById<RadioButton>(Resource.Id.rbtnLote);
            rbtnFechaElaboracion = FindViewById<RadioButton>(Resource.Id.rbtnFechaElaboracion);
            fuente = new Fuente(this);
            fontRobotoRegular = fuente.fuenteRobotoRegular();
            fontRobotoMedium = fuente.fuenteRobotoMedium();
            fontRobotoBold = fuente.fuenteRobotoBold();
            fontMyriadProLightCondensed = fuente.fuenteRobotoBold();
            tipografiasEnTextView(lblBtnAgregarProducto, lblDepartamento);
            tipografiasEnBotones(btnBuscar, btnCancelarProducto);
            tipografiasEnEditText(txtCodigoProducto, txtFechaDescongelacion);
            txtCodigoProducto.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
            txtLoteProducto.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
            txtTemperaturaProducto.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
            txtComentarioProducto.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
            ocultarTeclado(txtCodigoProducto);
            ocultarTeclado(txtLoteProducto);
            ocultarTeclado(txtTemperaturaProducto);
            ocultarTeclado(txtComentarioProducto);
            setImageEtiqueta();
            if (DataManager.estadoBusquedaProducto.Equals("pendiente"))
            {
                txtInputCantidadDeUnidadesProducto.SetHintTextAppearance(Resource.Style.TextAppearance_App_TextInputLayoutBlock);
                txtInputComentarioProducto.SetHintTextAppearance(Resource.Color.edittexthintbloqueado);
                txtInputLoteProducto.SetHintTextAppearance(Resource.Style.TextAppearance_App_TextInputLayoutBlock);
            }
        }

        void setImageEtiqueta(){
            if (DataManager.imagePath != null)
            {
                try
                {
                    if (DataManager.imageData != null)
                    {
                        photoView.SetImageBitmap(DataManager.thumbnail);
                    }else{
                        photoView.SetImageResource(Resource.Drawable.ic_photo);
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        /// <summary>
        /// Buttons the scan codigo barra click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void BtnScanCodigoBarra_Click(object sender, EventArgs e)
        {
            MobileBarcodeScanner.Initialize(Application);
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();

            var result = await scanner.Scan();

            if (result != null)
                DataManager.barra = result.Text;
        }

        protected override void OnResume()
        {
            base.OnResume();
            txthintFechaDescongelacion = FindViewById<TextView>(Resource.Id.txthintFechaDescongelacion);
            rbtnLote.Click += delegate {
                txtInputLoteProducto.Visibility = ViewStates.Visible;
                lltxtFechaElaboracion.Visibility = ViewStates.Gone;
            };
            rbtnFechaElaboracion.Click += delegate {
                txtInputLoteProducto.Visibility = ViewStates.Gone;
                lltxtFechaElaboracion.Visibility = ViewStates.Visible;
            };
            if (rbtnLote.Checked)
            {
                txtInputLoteProducto.Visibility = ViewStates.Visible;
                lltxtFechaElaboracion.Visibility = ViewStates.Gone;
            }
            if (rbtnFechaElaboracion.Checked)
            {
                txtInputLoteProducto.Visibility = ViewStates.Gone;
                lltxtFechaElaboracion.Visibility = ViewStates.Visible;
            }
            if (!string.IsNullOrEmpty(DataManager.barra))
            {
                txtCodigoProducto.Text = DataManager.barra;
                consultaProducto(2);
            }
            if (DataManager.estadoBusquedaProducto.Equals("1"))
            {
                linearLayoutPanelFormularioIngresoProducto.Visibility = ViewStates.Visible;
                photoView.Visibility = ViewStates.Gone;
                lblDepartamento.Text = DataManager.refreshDepto;
            }else if(DataManager.estadoBusquedaProducto.Equals("pendiente")){
                linearLayoutPanelFormularioIngresoProducto.Visibility = ViewStates.Visible;
                photoView.Visibility = ViewStates.Gone;
                txtCodigoProducto.Text = DataManager.ProductoPendienteSeleccionado.ItemPadre;
                consultaProducto(1);
                lblDepartamento.Text = DataManager.ProductoPendienteSeleccionado.NombreDpto;
                string fechaDescFormat = formatFecha(DataManager.ProductoPendienteSeleccionado.FechaDescongelado, 1);             
                txtFechaDescongelacion.Text = fechaDescFormat;
                txtFechaVencimientoDescongelacionProducto.Text = DataManager.ProductoPendienteSeleccionado.FechaVencimiento;
                txtCantidadDeUnidadesProducto.Text = DataManager.ProductoPendienteSeleccionado.CantidadUnidades;
                block();       
            }
            else
            {
                linearLayoutPanelFormularioIngresoProducto.Visibility = ViewStates.Invisible;
                photoView.Visibility = ViewStates.Visible;
                lblDepartamento.Text = "";
            }
            if (DataManager.dataProductosItem != null)
            {
                adapterProductos = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, DataManager.dataProductosItem);
                adapterProductos.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            }
            else
            {
                adapterProductos = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, itemProductoSelecciona);
                adapterProductos.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            }
            InitSpinnerHintAndFloatingLabelProducto();
        }

        /// <summary>
        /// Formats the fecha.
        /// </summary>
        /// <returns>The fecha.</returns>
        /// <param name="fecha">Fecha.</param>
        /// <param name="tipoFormato">Tipo formato.</param>
        string formatFecha(string fecha, int tipoFormato)
        {
            string fechaElabFormat = "";
            string timeformat = "";

            if (tipoFormato == 1)
                timeformat = "yyyyMMdd HH:mm:ss";
            else if (tipoFormato == 2)
                timeformat = "yyyyMMdd";
            else
                timeformat = "dd/MM/yyyy";

            DateTime datetimeobject = DateTime.ParseExact(fecha, timeformat, System.Globalization.CultureInfo.InvariantCulture);

            if (tipoFormato == 3)
                 fechaElabFormat = String.Format("{0:yyyyMMdd}", datetimeobject);
            else
                fechaElabFormat = String.Format("{0:dd/MM/yyyy}", datetimeobject);
            return fechaElabFormat;
        }

        void block(){
            if (!string.IsNullOrEmpty(DataManager.ProductoPendienteSeleccionado.LoteProduccion))
            {
                rbtnLote.Checked = true;
                txtInputLoteProducto.Visibility = ViewStates.Visible;
                lltxtFechaElaboracion.Visibility = ViewStates.Gone;
                txtLoteProducto.Text = DataManager.ProductoPendienteSeleccionado.LoteProduccion;
                txtLoteProducto.Clickable = false;
                txtLoteProducto.Enabled = false;
                txtLoteProducto.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittextbloqueado);
                txtLoteProducto.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
                txtLoteProducto.SetHintTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
                txtInputLoteProducto.SetHintTextAppearance(Resource.Style.TextAppearance_App_TextInputLayoutBlock);
            }
            else
            {
                rbtnFechaElaboracion.Checked = true;
                txtInputLoteProducto.Visibility = ViewStates.Gone;
                lltxtFechaElaboracion.Visibility = ViewStates.Visible;
                string fechaElabFormat = formatFecha(DataManager.ProductoPendienteSeleccionado.FechaElaboracion, 2);
                txtFechaElaboracion.Text = fechaElabFormat;
                txtFechaElaboracion.Clickable = false;
                txtFechaElaboracion.Enabled = false;
                txtFechaElaboracion.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittextbloqueado);
                txtFechaElaboracion.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
                lblhinttxtFechaElaboracion.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
            }
            txtTemperaturaProducto.Text = "";
            txtComentarioProducto.Text = DataManager.ProductoPendienteSeleccionado.Comentario;

            txtCodigoProducto.Clickable = false;
            txtCodigoProducto.Enabled = false;
            btnBuscar.Clickable = false;
            btnBuscar.Enabled = false;
            btnScanCodigoBarra.Clickable = false;
            btnScanCodigoBarra.Enabled = false;
            spiItemsProductos.Clickable = false;
            spiItemsProductos.Enabled = false;
            txtFechaDescongelacion.Clickable = false;
            txtFechaDescongelacion.Enabled = false;
            txtFechaVencimientoDescongelacionProducto.Clickable = false;
            txtFechaVencimientoDescongelacionProducto.Enabled = false;
            txtCantidadDeUnidadesProducto.Clickable = false;
            txtCantidadDeUnidadesProducto.Enabled = false;
            rbtnLote.Clickable = false;
            rbtnLote.Enabled = false;
            rbtnFechaElaboracion.Clickable = false;
            rbtnFechaElaboracion.Enabled = false;
            txtComentarioProducto.Clickable = false;
            txtComentarioProducto.Enabled = false;
            rbtnLote.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
            rbtnFechaElaboracion.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
            lblhintDepartamento.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
            lblDepartamento.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
            txthintFechaDescongelacion.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
            lblhintFechaVencimientoDescongelacionProducto.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
            txtCodigoProducto.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittextbloqueado);
            txtCodigoProducto.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
            txtFechaDescongelacion.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittextbloqueado);
            txtFechaDescongelacion.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
            txtFechaVencimientoDescongelacionProducto.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittextbloqueado);
            txtFechaVencimientoDescongelacionProducto.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
            txtInputCantidadDeUnidadesProducto.SetHintTextAppearance(Resource.Style.TextAppearance_App_TextInputLayoutBlock);
            txtCantidadDeUnidadesProducto.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittextbloqueado);
            txtCantidadDeUnidadesProducto.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
            txtCantidadDeUnidadesProducto.SetHintTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
            txtInputComentarioProducto.SetHintTextAppearance(Resource.Color.edittexthintbloqueado);
            txtComentarioProducto.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittextbloqueado);
            txtComentarioProducto.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
            txtComentarioProducto.SetHintTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
        }

        /// <summary>
        /// Ocultars the teclado.
        /// </summary>
        /// <param name="editText">Edit text.</param>
        public void ocultarTeclado(EditText editText)
        {
            InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(editText.WindowToken, 0);
        }

        /// <summary>
        /// Linears the layout button ir AM apas de tienda click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void LinearLayoutBtnIrAMapasDeTienda_Click(object sender, EventArgs e)
        {
            Intent intentAUbicacionTiendaActivity = new Intent(this, typeof(UbicacionTiendaActivity));
            StartActivity(intentAUbicacionTiendaActivity);
            DataManager.estadoVolverAlMapa = "1";
        }

        /// <summary>
        /// Inits the spinner hint and floating label.
        /// </summary>
        public void InitSpinnerHintAndFloatingLabelProducto()
        {
            spiItemsProductos.Adapter = adapterProductos;
            spiItemsProductos.SetPaddingSafe(0, 0, 0, 0);
        }

        /// <summary>
        /// Buttons the volver click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void BtnVolver_Click(object sender, EventArgs e)
        {
            Intent intentAlHome = new Intent(this, typeof(HomeAgregaProductoActivity));
            StartActivity(intentAlHome);
            DataManager.estadoBusquedaProducto = string.Empty;
            DataManager.refreshItem = string.Empty;
            DataManager.refreshDepto = string.Empty;
            DataManager.refreshItemBalanza = string.Empty;
            DataManager.refreshNombreProducto = string.Empty;
        }

        /// <summary>
        /// Buttons the buscar click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            consultaProducto(1);
        }

        /// <summary>
        /// Items the material spinner.
        /// </summary>
        /// <param name="itemSpinner">Item spinner.</param>
        void itemMaterialSpinner(string[] itemSpinner)
        {
            if (itemSpinner != null)
            {
                DataManager.dataProductosItem = itemSpinner;
                spinnerCargado(DataManager.dataProductosItem);
            }
            else
            {
                DataManager.dataProductosItem = itemProductoSelecciona;
                spinnerCargado(DataManager.dataProductosItem);
            }
        }

        /// <summary>
        /// Consultas the producto.
        /// </summary>
        /// <param name="type">Type.</param>
        private void consultaProducto(int type)
        {
            try
            {
                string[] itemsProducto;
                var codigo = txtCodigoProducto.Text;
                ocultarTeclado(txtCodigoProducto);
                if (!string.IsNullOrEmpty(txtCodigoProducto.Text))
                {
                    EntidadItemRefresh codigoBalanzaBD;
                    if (type == 1)
                    {
                        if (txtCodigoProducto.Text.Trim().Length < 6)
                        {
                            codigoBalanzaBD = DataManager.RealmInstance.All<EntidadItemRefresh>().Where(g => g.CodigoBalanza == txtCodigoProducto.Text).FirstOrDefault();
                            itemsProducto = (from f in dataProducto where f.CodigoBalanza == codigo select f.OldNumber + " - " + f.DescripcionItem1).ToArray();
                            itemMaterialSpinner(itemsProducto);

                        }
                        else if (txtCodigoProducto.Text.Trim().Length < 8 && txtCodigoProducto.Text.Trim().Length >= 6)
                        {
                            codigoBalanzaBD = DataManager.RealmInstance.All<EntidadItemRefresh>().Where(g => g.OldNumber == txtCodigoProducto.Text).FirstOrDefault();
                            itemsProducto = (from f in dataProducto where f.OldNumber == codigo select f.OldNumber + " - " + f.DescripcionItem1).ToArray();
                            itemMaterialSpinner(itemsProducto);
                            if (codigoBalanzaBD != null)
                            {
                                spiItemsProductos.SetSelection(1);
                            }
                        }
                        else
                        {
                            var barraSinDigito = calcularBarra(txtCodigoProducto.Text).Length == 13 ?
                                                          calcularBarra(txtCodigoProducto.Text).Substring(0, calcularBarra(txtCodigoProducto.Text).Length - 1) :
                                                          (calcularBarra(txtCodigoProducto.Text).Length == 12 ? ValidadorBarras.ValidadorBarrasRefresh(calcularBarra(txtCodigoProducto.Text)) :
                                                                                                                                        calcularBarra(txtCodigoProducto.Text));

                            codigoBalanzaBD = DataManager.RealmInstance.All<EntidadItemRefresh>().Where(g => g.Barra == calcularBarra(txtCodigoProducto.Text)).FirstOrDefault();
                            codigoBalanzaBD = codigoBalanzaBD ?? DataManager.RealmInstance.All<EntidadItemRefresh>().Where(g => g.Barra == barraSinDigito).FirstOrDefault();
                            itemsProducto = (from f in dataProducto where f.Barra == codigo || f.Barra == barraSinDigito select f.OldNumber + " - " + f.DescripcionItem1).ToArray();
                            itemMaterialSpinner(itemsProducto);

                        }


                    }
                    else
                    {
                        var barraSinDigito = calcularBarra(txtCodigoProducto.Text).Length == 13 ?
                                                          calcularBarra(txtCodigoProducto.Text).Substring(0, calcularBarra(txtCodigoProducto.Text).Length - 1) :
                                                          (calcularBarra(txtCodigoProducto.Text).Length == 12 ? ValidadorBarras.ValidadorBarrasRefresh(calcularBarra(txtCodigoProducto.Text)) :
                                                                                                                                        calcularBarra(txtCodigoProducto.Text));
                        codigoBalanzaBD = DataManager.RealmInstance.All<EntidadItemRefresh>().Where(g => g.Barra == calcularBarra(txtCodigoProducto.Text)).FirstOrDefault();
                        codigoBalanzaBD = codigoBalanzaBD ?? DataManager.RealmInstance.All<EntidadItemRefresh>().Where(g => g.Barra == barraSinDigito).FirstOrDefault();

                        itemsProducto = (from f in dataProducto where f.Barra == codigo || f.Barra == barraSinDigito select f.OldNumber + " - " + f.DescripcionItem1).ToArray();
                        itemMaterialSpinner(itemsProducto);

                    }

                    if (codigoBalanzaBD != null)
                    {
                        temperaturaIdealProducto = codigoBalanzaBD.TemperaturaIdeal;
                        tiempoDeCorte = codigoBalanzaBD.HoraTemperaturaIdeal;
                        itemBalanza = codigoBalanzaBD.ItemNumber;
                        itemNombreProducto = codigoBalanzaBD.DescripcionItem1;
                        itemNombreDptoProducto = codigoBalanzaBD.NombreDepartamento;
                        itemDiasPerecibilidad = codigoBalanzaBD.DiasPericibilidad;
                        lblDepartamento.Text = itemNombreDptoProducto;
                        linearLayoutPanelFormularioIngresoProducto.Visibility = ViewStates.Visible;
                        photoView.Visibility = ViewStates.Gone;
                        if (DataManager.estadoBusquedaProducto.Equals("pendiente"))
                        {
                            DataManager.estadoBusquedaProducto = "pendiente";
                        }
                        else
                        {
                            DataManager.estadoBusquedaProducto = "1";
                        }
                        DataManager.refreshDepto = lblDepartamento.Text;
                        DataManager.refreshItemBalanza = itemBalanza;
                        DataManager.refreshNombreProducto = itemNombreProducto;
                    }
                    else
                    {
                        if (linearLayoutPanelFormularioIngresoProducto.Visibility == ViewStates.Visible)
                        {
                            linearLayoutPanelFormularioIngresoProducto.Visibility = ViewStates.Invisible;
                            photoView.Visibility = ViewStates.Visible;
                        }
                        Toast.MakeText(ApplicationContext, "Código no existe en el registro local del tablet", ToastLength.Short).Show();
                        DataManager.estadoBusquedaProducto = string.Empty;
                        DataManager.refreshItem = string.Empty;
                        DataManager.refreshDepto = string.Empty;
                        DataManager.barra = string.Empty;
                        DataManager.refreshItemBalanza = string.Empty;
                        DataManager.refreshNombreProducto = string.Empty;
                    }
                }
                else
                {
                    if (linearLayoutPanelFormularioIngresoProducto.Visibility == ViewStates.Visible)
                    {
                        linearLayoutPanelFormularioIngresoProducto.Visibility = ViewStates.Invisible;
                        photoView.Visibility = ViewStates.Visible;
                    }
                    Toast.MakeText(ApplicationContext, "Ingresa un código de balanza válido", ToastLength.Short).Show();
                    DataManager.estadoBusquedaProducto = string.Empty;
                    DataManager.refreshItem = string.Empty;
                    DataManager.refreshDepto = string.Empty;
                    DataManager.barra = string.Empty;
                    DataManager.refreshItemBalanza = string.Empty;
                    DataManager.refreshNombreProducto = string.Empty;
                }
            }
            catch(Java.Lang.ArrayIndexOutOfBoundsException ex)
            {
                Console.WriteLine(ex);
            }
        }


        void spinnerCargado(string[] itemsProducto)
        {
            adapterProductos = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, itemsProducto);
            adapterProductos.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spiItemsProductos.Adapter = adapterProductos;
        }

        /// <summary>
        /// Calculars the barra.
        /// </summary>
        /// <returns>The barra.</returns>
        /// <param name="txtCodigoProducto">Text codigo producto.</param>
        private string calcularBarra(string txtCodigoProducto)
        {
            string balanzaDesdeBarra = string.Empty;
            var barra = txtCodigoProducto;
            if (int.Parse(barra.Substring(0, 2)) == 25)
            {
                balanzaDesdeBarra = barra.Substring(0, 7);
            }
            else if (int.Parse(barra.Substring(0, 2)) < 25)
            {
                balanzaDesdeBarra = barra.Substring(0, 7);
            }
            return balanzaDesdeBarra.PadRight(13,'0');
        }

        /// <summary>
        /// Tipografiases the en text view.
        /// </summary>
        /// <param name="lblBtnAgregarProducto">Lbl button agregar producto.</param>
        /// <param name="lblDepartamento">Lbl departamento.</param>
        public void tipografiasEnTextView(TextView lblBtnAgregarProducto, TextView lblDepartamento)
        {
            lblBtnAgregarProducto.Typeface = fontRobotoMedium;
            lblDepartamento.Typeface = fontRobotoBold;
        }

        /// <summary>
        /// Tipografiases the en edit text.
        /// </summary>
        /// <param name="txtCodigoProductoRefresh">Text codigo blanza.</param>
        /// <param name="txtFechaFaenaProducto">Text fecha faena producto.</param>       
        public void tipografiasEnEditText(EditText txtCodigoProductoRefresh, EditText txtFechaFaenaProducto)
        {
            txtCodigoProductoRefresh.Typeface = fontRobotoRegular;
            txtFechaFaenaProducto.Typeface = fontRobotoRegular;
        }

        /// <summary>
        /// Tipografiases the en botones.
        /// </summary>
        /// <param name="btnBuscar">Button buscar.</param>
        /// <param name="btnCancelarProducto">Button cancelar producto.</param>
        public void tipografiasEnBotones(Button btnBuscar, Button btnCancelarProducto)
        {
            btnBuscar.Typeface = fontRobotoMedium;
            btnCancelarProducto.Typeface = fontRobotoMedium;
        }

        /// <summary>
        /// Texts the fecha faena producto click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void TxtFechaDescongelacion_Click(object sender, EventArgs e)
        {
            ocultarTeclado(txtLoteProducto);
            ocultarTeclado(txtCantidadDeUnidadesProducto);
            ocultarTeclado(txtTemperaturaProducto);
            ocultarTeclado(txtComentarioProducto);
            ShowDialog(DATE_DIALOG_FECHA_DESCONGELACION);
        }

        /// <summary>
        /// Texts the fecha elaboracion click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void TxtFechaElaboracion_Click(object sender, EventArgs e)
        {
            ocultarTeclado(txtLoteProducto);
            ocultarTeclado(txtCantidadDeUnidadesProducto);
            ocultarTeclado(txtTemperaturaProducto);
            ocultarTeclado(txtComentarioProducto);
            ShowDialog(DATE_DIALOG_FECHA_ELABORACION);
        }

        /// <summary>
        /// Buttons the agregar producto click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void BtnAgregarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (DataManager.imageData != null)
                {
                    Bitmap newImg = MediaStore.Images.Media.GetBitmap(ContentResolver, DataManager.imageData);

                    var streamImg = new MemoryStream();
                    newImg.Compress(Bitmap.CompressFormat.Jpeg, 30, streamImg);//calidad al 30%
                    bitmapData = streamImg.ToArray();
                }
                var fechaInventario = String.Format("{0:yyyyMMdd}", DateTime.Now);
                var horaInventario = String.Format("{0:HHmmss}", DateTime.Now);
                var horaDescongelado = String.Format("{0:HH:mm:ss}", DateTime.Now);
                imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Visible;
                lblBtnAgregarProducto.Visibility = ViewStates.Gone;
                if (!spiItemsProductos.SelectedItem.ToString().Equals("Item") &&
                    !string.IsNullOrEmpty(txtFechaDescongelacion.Text) &&
                    !string.IsNullOrEmpty(txtCantidadDeUnidadesProducto.Text) && int.Parse(txtCantidadDeUnidadesProducto.Text) >= 1 &&
                    !string.IsNullOrEmpty(txtTemperaturaProducto.Text) && txtTemperaturaProducto.Text != "." &&
                    !string.IsNullOrEmpty(txtComentarioProducto.Text))
                {
                    if (Convert.ToDecimal(txtTemperaturaProducto.Text, CultureInfo.InvariantCulture) <= 100)
                    {
                        byte[] imagen = bitmapData;
                        var nombreProducto = spiItemsProductos.SelectedItem.ToString().Trim();
                        var nombreProductoClean = nombreProducto.Split('-')[1].Trim();
                        var codItem = nombreProducto.Split('-')[0].Trim();
                        DataManager.refreshItem = nombreProducto;
                        if (spiItemsProductos.Count > 1)
                        {
                            var codigoBalanzaBD = DataManager.RealmInstance.All<EntidadItemRefresh>().Where(g => g.OldNumber == codItem).FirstOrDefault();
                            temperaturaIdealProducto = codigoBalanzaBD.TemperaturaIdeal;
                        }

                        if (!string.IsNullOrEmpty(temperaturaIdealProducto) && !temperaturaIdealProducto.Equals("-999"))
                        {
                            if (string.IsNullOrEmpty(DataManager.fechaDescongelacionManager))
                                DataManager.fechaDescongelacionManager = formatFecha(txtFechaDescongelacion.Text, 3);
                            if (rbtnLote.Checked)
                            {
                                if (!string.IsNullOrEmpty(txtLoteProducto.Text))
                                {
                                    item = new IngresoItemRefreshRequest
                                    {
                                        ItemPadre = codItem,
                                        FechaElaboracion = Constantes.FECHA_ELABORACION,
                                        LoteProduccion = txtLoteProducto.Text,
                                        FechaDescongelado = DataManager.fechaDescongelacionManager + " " + horaDescongelado,
                                        IdParametro = Constantes.ID_PARAMETRO,
                                        Temperatura = txtTemperaturaProducto.Text,
                                        EtiquetaPropia = Constantes.ETIQUETA_PROPIA,
                                        UsuarioCreacion = DataManager.nombreUsuario,
                                        FechaCreacion = fechaInventario,
                                        HoraCreacion = horaInventario,
                                        Comentario = txtComentarioProducto.Text,
                                        CantidadUnidades = txtCantidadDeUnidadesProducto.Text,
                                        Imagen = imagen,
                                        Correlativo = 0,
                                        StoreNbr = dataSucursal.FirstOrDefault().id
                                    };
                                    AgregarProductoConfirmacionActivity.dialogoConfirmacionAgregarProducto(this, item
                                                                                                           , DataManager.refreshNombreProducto
                                                                                                           , txtCantidadDeUnidadesProducto.Text
                                                                                                           , codItem, DataManager.refreshDepto
                                                                                                           , txtFechaVencimientoDescongelacionProducto.Text
                                                                                                           , txtFechaDescongelacion.Text
                                                                                                           , temperaturaIdealProducto
                                                                                                           , tiempoDeCorte);
                                    imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Gone;
                                    lblBtnAgregarProducto.Visibility = ViewStates.Visible;
                                }
                                else
                                {
                                    imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Gone;
                                    lblBtnAgregarProducto.Visibility = ViewStates.Visible;
                                    dialogValidator(Constantes.MENSAJE_VALIDACION_GENERICO);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(txtFechaElaboracion.Text))
                                {
                                    if (string.IsNullOrEmpty(DataManager.fechaElaboracionManager))
                                        DataManager.fechaElaboracionManager = txtFechaElaboracion.Text;
                                    item = new IngresoItemRefreshRequest
                                    {
                                        ItemPadre = codItem,
                                        FechaElaboracion = DataManager.fechaElaboracionManager,
                                        LoteProduccion = string.Empty,
                                        FechaDescongelado = DataManager.fechaDescongelacionManager + " " + horaDescongelado,
                                        IdParametro = Constantes.ID_PARAMETRO,
                                        Temperatura = txtTemperaturaProducto.Text,
                                        EtiquetaPropia = Constantes.ETIQUETA_PROPIA,
                                        UsuarioCreacion = DataManager.nombreUsuario,
                                        FechaCreacion = fechaInventario,
                                        HoraCreacion = horaInventario,
                                        Comentario = txtComentarioProducto.Text,
                                        CantidadUnidades = txtCantidadDeUnidadesProducto.Text,
                                        Imagen = imagen,
                                        Correlativo = 0,
                                        StoreNbr = dataSucursal.FirstOrDefault().id
                                    };
                                    AgregarProductoConfirmacionActivity.dialogoConfirmacionAgregarProducto(this
                                                                                                           , item
                                                                                                           , DataManager.refreshNombreProducto
                                                                                                           , txtCantidadDeUnidadesProducto.Text
                                                                                                           , codItem
                                                                                                           , DataManager.refreshDepto
                                                                                                           , txtFechaVencimientoDescongelacionProducto.Text
                                                                                                           , txtFechaDescongelacion.Text
                                                                                                           , temperaturaIdealProducto
                                                                                                           , tiempoDeCorte);
                                    imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Gone;
                                    lblBtnAgregarProducto.Visibility = ViewStates.Visible;
                                }
                                else
                                {
                                    imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Gone;
                                    lblBtnAgregarProducto.Visibility = ViewStates.Visible;
                                    dialogValidator(Constantes.MENSAJE_VALIDACION_GENERICO);
                                }
                            }
                        }
                        else
                        {
                            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                            builder.SetTitle("Productos Refresh");
                            builder.SetIcon(Resource.Mipmap.ic_refresh);
                            builder.SetCancelable(false);
                            builder.SetMessage("El producto no contiene temperatura ideal");
                            builder.SetPositiveButton("Aceptar", delegate {
                                Intent iAHome = new Intent(this, typeof(HomeAgregaProductoActivity));
                                StartActivity(iAHome);
                            });
                            builder.Show();
                        }
                    }
                    else
                    {
                        imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Gone;
                        lblBtnAgregarProducto.Visibility = ViewStates.Visible;
                        dialogValidator(Constantes.MENSAJE_VALIDACION_TEMPERATURA);
                    }
                }
                else
                {
                    imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Gone;
                    lblBtnAgregarProducto.Visibility = ViewStates.Visible;
                    dialogValidator(Constantes.MENSAJE_VALIDACION_GENERICO);
                }
            }
            catch (Java.IO.FileNotFoundException fnfe)
            {
                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                builder.SetTitle("Productos Refresh");
                builder.SetCancelable(false);
                builder.SetIcon(Resource.Mipmap.ic_refresh);
                builder.SetMessage("Estimado usuario, por favor ir hasta ajustes del tablet y en permisos de la aplicación habilitar los permisos de ESPACIO DE ALMACENAMIENTO.");
                builder.SetPositiveButton("Aceptar", delegate {});
                builder.Show();
                Crashes.TrackError(fnfe);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        /// <summary>
        /// Dialogs the validator.
        /// </summary>
        void dialogValidator(string mensaje){
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetTitle("Productos Refresh");
            builder.SetIcon(Resource.Mipmap.ic_refresh);
            builder.SetCancelable(false);
            builder.SetMessage(mensaje);
            builder.SetPositiveButton("Aceptar", delegate {
                llBtnAgregarProducto.Clickable = true;
                llBtnAgregarProducto.Enabled = true;
            });
            builder.Show();
        }

        /// <summary>
        /// Buttons the cancelar producto click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void BtnCancelarProducto_Click(object sender, EventArgs e)
        {
            Intent intentAlHomeAgregaProductoActivity = new Intent(this, typeof(HomeAgregaProductoActivity));
            StartActivity(intentAlHomeAgregaProductoActivity);
            DataManager.estadoBusquedaProducto = string.Empty;
            DataManager.barra = string.Empty;
            DataManager.refreshItem = string.Empty;
            DataManager.refreshDepto = string.Empty;
            DataManager.refreshItemBalanza = string.Empty;
            DataManager.refreshNombreProducto = string.Empty;
        }

        /// <summary>
        /// Ons the create dialog.
        /// </summary>
        /// <returns>The create dialog.</returns>
        /// <param name="id">Identifier.</param>
        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case DATE_DIALOG_FECHA_DESCONGELACION:
                    var datePickerFechaDescongelacion = new DatePickerDialog(this, this, year, mes, dia);
                    datePickerFechaDescongelacion.DatePicker.MaxDate = FormatUtils.SetMaxDate(1);
                    datePickerFechaDescongelacion.DatePicker.MinDate = FormatUtils.SetMinDate();
                    codeFecha = DATE_DIALOG_FECHA_DESCONGELACION;
                    return datePickerFechaDescongelacion;
                case DATE_DIALOG_FECHA_ELABORACION:
                    var datePickerFechaElaboracion = new DatePickerDialog(this, this, year, mes, dia);
                    datePickerFechaElaboracion.DatePicker.MaxDate = FormatUtils.SetMaxDate(1);
                    datePickerFechaElaboracion.DatePicker.MinDate = FormatUtils.SetMinDate();
                    codeFecha = DATE_DIALOG_FECHA_ELABORACION;
                    return datePickerFechaElaboracion;
                default:
                    break;
            }
            return null;
        }

        /// <summary>
        /// Ons the date set.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="year">Year.</param>
        /// <param name="month">Month.</param>
        /// <param name="dayOfMonth">Day of month.</param>
        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            string mesString = string.Empty;
            this.year = year;
            this.mes = month;
            this.dia = dayOfMonth;
            var diaFormat = String.Format("{0:dd}", dayOfMonth.ToString());
            var mesFormat = String.Format("{0:MM}", (month + 1).ToString());
            mesFormat = mesFormat.Length == 1 ? "0" + mesFormat : mesFormat;
            var formatDia = diaFormat.Length == 1 ? "0" + diaFormat : diaFormat;
            if (mes == 0)
            {
                mesString = "01";
            }
            else if (mes == 1)
            {
                mesString = "02";
            }
            else if (mes == 2)
            {
                mesString = "03";
            }
            else if (mes == 3)
            {
                mesString = "04";
            }
            else if (mes == 4)
            {
                mesString = "05";
            }
            else if (mes == 5)
            {
                mesString = "06";
            }
            else if (mes == 6)
            {
                mesString = "07";
            }
            else if (mes == 7)
            {
                mesString = "08";
            }
            else if (mes == 8)
            {
                mesString = "09";
            }
            else if (mes == 9)
            {
                mesString = "10";
            }
            else if (mes == 10)
            {
                mesString = "11";
            }
            else if (mes == 11)
            {
                mesString = "12";
            }

            if (codeFecha == 1)
            {
                txtFechaDescongelacion.Text = formatDia + "/" + mesString + "/" + year;
                string fechaDescongelacionProductoRefresh = year +"/"+ (mes + 1) + "/" + formatDia;
                DateTime odate = Convert.ToDateTime(fechaDescongelacionProductoRefresh);
                DateTime fechaVence = odate.AddDays(int.Parse(itemDiasPerecibilidad));
                var fechaVencimiento = String.Format("{0:dd/MM/yyyy}", fechaVence);

                //var fechaVencimientoEditText = String.Format("{0:yyyyMMdd}", fechaVencimiento.ToString());
                txtFechaVencimientoDescongelacionProducto.Text = fechaVencimiento.ToString();
                fechaDescong = year.ToString() + mesFormat + formatDia;
                DataManager.fechaDescongelacionManager = fechaDescong;
            }
            else if(codeFecha == 2)
            {
                txtFechaElaboracion.Text = formatDia + "/" + mesString + "/" + year;
                fechaElabora = year.ToString() + mesFormat + formatDia;
                DataManager.fechaElaboracionManager = fechaElabora;
            }
        }

        /// <summary>
        /// Ons the back pressed.
        /// </summary>
        public override void OnBackPressed()
        {
            DataManager.estadoBusquedaProducto = string.Empty;
            DataManager.barra = string.Empty;
            DataManager.refreshItem = string.Empty;
            DataManager.refreshDepto = string.Empty;
            DataManager.refreshItemBalanza = string.Empty;
            DataManager.refreshNombreProducto = string.Empty;
            Intent intentAlHome = new Intent(this, typeof(HomeAgregaProductoActivity));
            StartActivity(intentAlHome);
        }

        /// <summary>
        /// Ons the nothing selected.
        /// </summary>
        /// <param name="parent">Parent.</param>
        public void OnNothingSelected(AdapterView parent)
        {
            return;
        }
    }
}