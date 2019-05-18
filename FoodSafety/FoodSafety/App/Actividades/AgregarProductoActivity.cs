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
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Com.Bumptech.Glide;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Services.Delegate;
using FoodSafety.Common.Utils;
using FoodSafety.Properties;
using FoodSafety.Services;
using FR.Ganfra.Materialspinner;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Android.App.DatePickerDialog;

namespace FoodSafety
{
    /// <summary>
    /// Agregar producto activity.
    /// </summary>
    [Activity(Label = "Agregar Producto", Theme = "@style/ThemeNoActionBarAzul", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenLayout)]
    public class AgregarProductoActivity : AppCompatActivity, IOnDateSetListener, FR.Ganfra.Materialspinner.MaterialSpinner.IOnItemSelectedListener, View.IOnKeyListener
    {
        List<SucursalTienda> dataSucursal = DataManager.RealmInstance.All<SucursalTienda>().ToList();
        List<EntidadProcedencia> dataProcedencia = DataManager.RealmInstance.All<EntidadProcedencia>().ToList();
        List<EntidadFrigorifico> dataFrigorifico = DataManager.RealmInstance.All<EntidadFrigorifico>().ToList();
        List<EntidadItem> dataProducto = DataManager.RealmInstance.All<EntidadItem>().ToList();
        MaterialSpinner spiProcedenciaProducto;
        MaterialSpinner spiFrigorificoProducto;
        MaterialSpinner spiItemsProductos;
        private ArrayAdapter<String> adapterProcedencia;
        private ArrayAdapter<String> adapterFrigorifico;
        private ArrayAdapter<String> adapterProductos;
        private const int DATE_DIALOG = 1;
        int year = DateTime.Now.Year;
        int mes = DateTime.Now.Month;
        int dia = DateTime.Now.Day;
        byte[] bitmapData;
        Button btnCancelarProducto;
        Button btnBuscar;
        EditText txtCodigoBlanza;
        EditText txtFechaFaenaProducto;
        EditText txtPesoNetoProducto;
        Fuente fuente;
        ImageButton btnLogout;
        ImageView photoView;
        ImageView btnVolver;
        ImageView imgLoadingBtnAgregaPruducto;
        LinearLayout llBtnAgregarProducto;
        LinearLayout linearLayoutPanelFormularioIngresoProducto;
        LinearLayout linearLayoutBtnIrAMapasDeTienda;
        LinearLayout linearLayoutBtnCambiarUser;
        LinearLayout llAgregaProductoActivity;
        LinearLayout llAgregaProductoActivityHorizontal;
        string[] itemProductoSelecciona = { "Item" };
        string[] itemFrigorificoSelecciona = { "Frigorifico" };
        String nombreSucursal;
        String tipoSucursal;
        String idSucursal;
        String itemBalanza;
        String codigoBalanza;
        String compressImage;
        String encodeImage;
        TextInputLayout txtInputPesoNetoProducto;
        TextView lblBtnAgregarProducto;
        TextView lblCodigoBalanza;
        TextView lblNombreActivity;
        TextView lblNombreTienda;
        TextView lblTipoTienda;
        TextView txtInputFechaFaenaProducto;
        Typeface fontRobotoRegular;
        Typeface fontRobotoMedium;
        Typeface fontRobotoBold;
        Typeface fontMyriadProLightCondensed;
        string fechaFaena = string.Empty;

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            string[] itemsProcedencia = (from c in dataProcedencia select c.NombreProcedencia).ToArray();
            string[] itemsFrigorifico = (from f in dataFrigorifico select f.NombreFrigorifico).ToArray();
            string[] itemsProducto = (from p in dataProducto select p.ItemDescripcion).ToArray();
            if (WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation0 || WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation180)
            {
                SetContentView(Resource.Layout.agregar_producto_activity);
            }
            else
            {
                SetContentView(Resource.Layout.agregar_producto_horizontal_activity);
            }
            photoView = FindViewById<ImageView>(Resource.Id.photoView);
            adapterProcedencia = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, itemsProcedencia);
            adapterProcedencia.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            InitSpinnerHintAndFloatingLabel();
            txtInputPesoNetoProducto = FindViewById<TextInputLayout>(Resource.Id.txtInputPesoNetoProducto);
            llAgregaProductoActivity = FindViewById<LinearLayout>(Resource.Id.llAgregaProductoActivity);
            llAgregaProductoActivityHorizontal = FindViewById<LinearLayout>(Resource.Id.llAgregaProductoActivityHorizontal);
            imgLoadingBtnAgregaPruducto = FindViewById<ImageView>(Resource.Id.imgLoadingBtnAgregaPruducto);
            lblBtnAgregarProducto = FindViewById<TextView>(Resource.Id.lblBtnAgregarProducto);
            llBtnAgregarProducto = FindViewById<LinearLayout>(Resource.Id.llBtnAgregarProducto);
            linearLayoutBtnCambiarUser = FindViewById<LinearLayout>(Resource.Id.linearLayoutBtnCambiarUser);
            linearLayoutBtnCambiarUser.Visibility = ViewStates.Gone;
            llBtnAgregarProducto.Click += BtnAgregarProducto_Click;
            btnLogout = FindViewById<ImageButton>(Resource.Id.btnLogout);
            btnLogout.Visibility = ViewStates.Invisible;
            txtInputFechaFaenaProducto = FindViewById<TextView>(Resource.Id.txtInputFechaFaenaProducto);
            btnVolver = FindViewById<ImageView>(Resource.Id.btnVolver);
            btnVolver.Click += BtnVolver_Click;
            linearLayoutPanelFormularioIngresoProducto = FindViewById<LinearLayout>(Resource.Id.linearLayoutPanelFormularioIngresoProducto);
            linearLayoutBtnIrAMapasDeTienda = FindViewById<LinearLayout>(Resource.Id.linearLayoutBtnIrAMapasDeTienda);
            linearLayoutBtnIrAMapasDeTienda.Click += LinearLayoutBtnIrAMapasDeTienda_Click;
            lblCodigoBalanza = FindViewById<TextView>(Resource.Id.lblCodigoBalanza);
            lblNombreActivity = FindViewById<TextView>(Resource.Id.lblNombreActivity);
            lblNombreActivity.Text = "Agregar Producto";
            lblNombreTienda = FindViewById<TextView>(Resource.Id.lblNombreTienda);
            lblTipoTienda = FindViewById<TextView>(Resource.Id.lblTipoTienda);
            nombreSucursal = dataSucursal.FirstOrDefault().name.ToString();
            tipoSucursal = dataSucursal.FirstOrDefault().format.ToString();
            lblNombreTienda.Text = nombreSucursal;
            lblTipoTienda.Text = tipoSucursal;
            Glide.With(this).Load(Resource.Drawable.cargando).Into(imgLoadingBtnAgregaPruducto);
            btnBuscar = FindViewById<Button>(Resource.Id.btnBuscar);
            btnBuscar.Click += BtnBuscar_Click;
            btnCancelarProducto = FindViewById<Button>(Resource.Id.btnCancelarProducto);
            btnCancelarProducto.Click += BtnCancelarProducto_Click;
            txtCodigoBlanza = FindViewById<EditText>(Resource.Id.txtCodigoBlanza);
            txtFechaFaenaProducto = FindViewById<EditText>(Resource.Id.txtFechaFaenaProducto);
            txtPesoNetoProducto = FindViewById<EditText>(Resource.Id.txtPesoNetoProducto);
            txtFechaFaenaProducto.Touch += TxtFechaFaenaProducto_Click;
            nombreSucursal = dataSucursal.FirstOrDefault().name.ToString();
            tipoSucursal = dataSucursal.FirstOrDefault().format.ToString();
            idSucursal = dataSucursal.FirstOrDefault().id.ToString();
            fuente = new Fuente(this);
            fontRobotoRegular = fuente.fuenteRobotoRegular();
            fontRobotoMedium = fuente.fuenteRobotoMedium();
            fontRobotoBold = fuente.fuenteRobotoBold();
            fontMyriadProLightCondensed = fuente.fuenteRobotoBold();
            tipografiasEnTextView(lblBtnAgregarProducto, lblCodigoBalanza);
            tipografiasEnBotones(btnBuscar, btnCancelarProducto);
            tipografiasEnEditText(txtCodigoBlanza, txtFechaFaenaProducto, txtPesoNetoProducto);
            spiProcedenciaProducto.OnItemSelectedListener = this;
            txtCodigoBlanza.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
            txtPesoNetoProducto.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
            ocultarTeclado(txtCodigoBlanza);
            setImageEtiqueta();
            //txtPesoNetoProducto.SetOnKeyListener(this);
        }

        /// <summary>
        /// Sets the image etiqueta.
        /// </summary>
        void setImageEtiqueta()
        {
            if (DataManager.imagePath != null)
            {
                try
                {
                    if (DataManager.imageData != null)
                    {
                        photoView.SetImageBitmap(DataManager.thumbnail);
                    }
                    else
                    {
                        photoView.SetImageResource(Resource.Drawable.ic_photo);
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        /// <summary>
        /// Ons the resume.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            if (!string.IsNullOrEmpty(txtFechaFaenaProducto.Text))
            {
                txtInputFechaFaenaProducto.Visibility = ViewStates.Visible;
            }
            if (DataManager.estadoBusquedaProducto.Equals("1"))
            {
                linearLayoutPanelFormularioIngresoProducto.Visibility = ViewStates.Visible;
                photoView.Visibility = ViewStates.Gone;
                lblCodigoBalanza.Text = "Código de balanza " + txtCodigoBlanza.Text;
            }
            else
            {
                linearLayoutPanelFormularioIngresoProducto.Visibility = ViewStates.Invisible;
                photoView.Visibility = ViewStates.Visible;
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
        /// Ocultars the teclado.
        /// </summary>
        /// <param name="editText">Edit text.</param>
        public void ocultarTeclado(EditText editText)
        {
            InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(editText.WindowToken, 0);
        }

        /// <summary>
        /// Limpiars the campos.
        /// </summary>
        void limpiarCampos()
        {
            if (!spiItemsProductos.SelectedItem.ToString().Equals("Item"))
            {
                spiItemsProductos.SetSelection(0);
            }
            if (!spiProcedenciaProducto.SelectedItem.ToString().Equals("Procedencia"))
            {
                spiProcedenciaProducto.SetSelection(0);
            }
            if (!string.IsNullOrEmpty(txtFechaFaenaProducto.Text))
            {
                txtFechaFaenaProducto.Text = string.Empty;
            }
            if (!string.IsNullOrEmpty(txtPesoNetoProducto.Text))
            {
                txtPesoNetoProducto.Text = string.Empty;
            }
        }

        /// <summary>
        /// Inits the spinner hint and floating label.
        /// </summary>
        public void InitSpinnerHintAndFloatingLabelProducto()
        {
            spiItemsProductos = FindViewById<MaterialSpinner>(Resource.Id.spiItemsProductos);
            spiItemsProductos.Adapter = adapterProductos;
            spiItemsProductos.SetPaddingSafe(0, 0, 0, 0);
        }

        /// <summary>
        /// Inits the spinner hint and floating label.
        /// </summary>
        public void InitSpinnerHintAndFloatingLabel()
        {
            spiProcedenciaProducto = FindViewById<MaterialSpinner>(Resource.Id.spiProcedenciaProducto);
            spiProcedenciaProducto.Adapter = adapterProcedencia;
            spiProcedenciaProducto.SetPaddingSafe(0, 0, 0, 0);

            spiFrigorificoProducto = FindViewById<MaterialSpinner>(Resource.Id.spiFrigorificoProducto);
            spiFrigorificoProducto.SetPaddingSafe(0, 0, 0, 0);
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
        /// Buttons the volver click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void BtnVolver_Click(object sender, EventArgs e)
        {
            Intent intentAgregarProductoActivity = new Intent(this, typeof(HomeAgregaProductoActivity));
            StartActivity(intentAgregarProductoActivity);
            DataManager.estadoBusquedaProducto = string.Empty;
        }

        /// <summary>
        /// Buttons the buscar click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            ocultarTeclado(txtCodigoBlanza);
            if (!string.IsNullOrEmpty(txtCodigoBlanza.Text))
            {
                var codigoBalanzaBD = DataManager.RealmInstance.All<EntidadItem>().Where(g => g.CodigoBalanza == txtCodigoBlanza.Text).FirstOrDefault();

                if (codigoBalanzaBD != null)
                {
                    itemBalanza = codigoBalanzaBD.Item;
                    lblCodigoBalanza.Text = "Código de balanza " + txtCodigoBlanza.Text;
                    var codigo = txtCodigoBlanza.Text;
                    string[] itemsProducto = (from f in dataProducto where f.CodigoBalanza == codigo select f.Item + " - " + f.ItemDescripcion).ToArray();

                    if (itemsProducto != null)
                    {
                        DataManager.dataProductosItem = itemsProducto;
                    }
                    else
                    {
                        DataManager.dataProductosItem = itemProductoSelecciona;
                    }

                    adapterProductos = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, itemsProducto);
                    adapterProductos.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    spiItemsProductos.Adapter = adapterProductos;
                    linearLayoutPanelFormularioIngresoProducto.Visibility = ViewStates.Visible;
                    photoView.Visibility = ViewStates.Gone;
                    DataManager.estadoBusquedaProducto = "1";
                }
                else
                {
                    if (linearLayoutPanelFormularioIngresoProducto.Visibility == ViewStates.Visible)
                    {
                        linearLayoutPanelFormularioIngresoProducto.Visibility = ViewStates.Invisible;
                        photoView.Visibility = ViewStates.Visible;
                    }
                    limpiarCampos();
                    DataManager.estadoBusquedaProducto = string.Empty;
                    Toast.MakeText(ApplicationContext, "Código no existe en el registro local del tablet", ToastLength.Short).Show();
                    ///hacer rellamada al servicio, delete a la bd local y re insercion de los productos.   
                }
            }
            else
            {
                if (linearLayoutPanelFormularioIngresoProducto.Visibility == ViewStates.Visible)
                {
                    linearLayoutPanelFormularioIngresoProducto.Visibility = ViewStates.Invisible;
                    photoView.Visibility = ViewStates.Visible;
                }
                limpiarCampos();
                DataManager.estadoBusquedaProducto = string.Empty;
                Toast.MakeText(ApplicationContext, "Ingresa un código de balanza válido", ToastLength.Short).Show();
            }
        }

        /// <summary>
        /// Tipografiases the en text view.
        /// </summary>
        /// <param name="lblCodigoBalanza">Lbl codigo balanza.</param>
        public void tipografiasEnTextView(TextView lblBtnAgregarProducto, TextView lblCodigoBalanza)
        {
            lblBtnAgregarProducto.Typeface = fontRobotoMedium;
            lblCodigoBalanza.Typeface = fontRobotoBold;
        }

        /// <summary>
        /// Tipografiases the en edit text.
        /// </summary>
        /// <param name="txtCodigoBlanza">Text codigo blanza.</param>
        /// <param name="txtFechaFaenaProducto">Text fecha faena producto.</param>
        /// <param name="txtPesoNetoProducto">Text peso neto producto.</param>
        public void tipografiasEnEditText(EditText txtCodigoBlanza, EditText txtFechaFaenaProducto, EditText txtPesoNetoProducto)
        {
            txtCodigoBlanza.Typeface = fontRobotoRegular;
            txtFechaFaenaProducto.Typeface = fontRobotoRegular;
            txtPesoNetoProducto.Typeface = fontRobotoRegular;
        }

        /// <summary>
        /// Tipografiases the en botones.
        /// </summary>
        /// <param name="btnAgregarProducto">Button agregar producto.</param>
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
        private void TxtFechaFaenaProducto_Click(object sender, EventArgs e)
        {
            ShowDialog(DATE_DIALOG);
        }

        /// <summary>
        /// Buttons the agregar producto click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private async void BtnAgregarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                estadoBloqueado(false);
                imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Visible;
                lblBtnAgregarProducto.Visibility = ViewStates.Gone;
              
                    if (!spiItemsProductos.SelectedItem.ToString().Equals("Item") &&
                    !spiProcedenciaProducto.SelectedItem.ToString().Equals("Procedencia") &&
                    !spiFrigorificoProducto.SelectedItem.ToString().Equals("Frigorifico") &&
                    !string.IsNullOrEmpty(txtFechaFaenaProducto.Text) &&
                    !string.IsNullOrEmpty(txtPesoNetoProducto.Text) && txtPesoNetoProducto.Text != "." && Convert.ToDouble(txtPesoNetoProducto.Text) > 0)
                    {
                        if (Convert.ToDecimal(txtPesoNetoProducto.Text, CultureInfo.InvariantCulture) <= 100)
                        {
                            var pesoKG = txtPesoNetoProducto.Text.Replace(",", ".").Trim();
                            var nombreFrigorifico = spiFrigorificoProducto.SelectedItem.ToString();
                            var nombreProcedencia = spiProcedenciaProducto.SelectedItem.ToString();
                            var nombreProducto = spiItemsProductos.SelectedItem.ToString().Trim();
                            var nombreProductoClean = nombreProducto.Split('-')[1].Trim();
                            var codItem = nombreProducto.Split('-')[0].Trim();
                            var procedenciaOrigen = dataProcedencia.Where(x => x.NombreProcedencia == nombreProcedencia).FirstOrDefault().CodigoProcedencia;
                            var codFrigorifico = dataFrigorifico.Where(x => x.NombreFrigorifico == nombreFrigorifico).FirstOrDefault().CodigoFrigorifico;
                            var fechaLectura = String.Format("{0:yyyyMMdd HH:mm:ss}", DateTime.Now);
                            var fechaInventario = String.Format("{0:yyyyMMdd}", DateTime.Now);
                            var pesoNetoConvert = Convert.ToInt32(Convert.ToDecimal(pesoKG, CultureInfo.InvariantCulture) * 100);
                            if (DataManager.imageData != null)
                            {
                                Bitmap newImg = MediaStore.Images.Media.GetBitmap(ContentResolver, DataManager.imageData);

                                var streamImg = new MemoryStream();
                                newImg.Compress(Bitmap.CompressFormat.Jpeg, 30, streamImg);//calidad al 30%
                                bitmapData = streamImg.ToArray();
                                encodeImage = Convert.ToBase64String(bitmapData);// Base64.EncodeToString(bitmapData, Base64.Default);
                                compressImage = CompressImageString.CompressString(encodeImage);
                            }

                            byte[] imagen = bitmapData;
                            IngresaProcesaBarrasTabletRequest item = new IngresaProcesaBarrasTabletRequest
                            {
                                Item = codItem,
                                Local = idSucursal,
                                Procedencia = procedenciaOrigen,
                                OrigenFrigorifico = codFrigorifico,
                                CertificadoEmbarque = Constantes.ITEM_CERTIFICADO_EMBARQUE,
                                FechaFaena = fechaFaena,
                                PesaNeto = pesoNetoConvert.ToString(),
                                PesaBruto = Constantes.PESO_BRUTO,
                                Responsable = DataManager.nombreUsuario,
                                Image = imagen
                            };
                            string postData = JsonConvert.SerializeObject(item);
                            var itemInventario = await ServiceDelegate.Instance.IngresoItemImage(JObject.Parse(postData));
                            if (itemInventario.Success)
                            {
                                try
                                {
                                    var codResponse = (int)itemInventario.Response;
                                    if (codResponse == 2)
                                    {
                                        imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Gone;
                                        lblBtnAgregarProducto.Visibility = ViewStates.Visible;
                                        Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                                        builder.SetTitle("Trazabilidad de carnes");
                                        builder.SetIcon(Resource.Mipmap.ic_trazabilidad);
                                        builder.SetCancelable(false);
                                        builder.SetMessage("No existe item padre, para " + item.Item + " - " + nombreProductoClean);
                                        builder.SetPositiveButton("Aceptar", delegate {
                                            Intent intentAlHomeAgregaProductoActivity = new Intent(this, typeof(HomeAgregaProductoActivity));
                                            StartActivity(intentAlHomeAgregaProductoActivity);
                                        });
                                        builder.Show();
                                        DataManager.estadoBusquedaProducto = string.Empty;
                                        AnalyticService.TrackAnalytics("Agregar producto app", new Dictionary<string, string> {
                                            { "Category", "Codigo retorno success false" },
                                            { "Action", "codigo 500 no se ingresa producto ya que no existe item padre para item "+item.Item +" con codigo balanza "+ txtCodigoBlanza.Text}
                                        });
                                    }
                                    else
                                    {
                                        imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Gone;
                                        lblBtnAgregarProducto.Visibility = ViewStates.Visible;
                                        DataManager.estadoProductoAgregado = true;
                                        DataManager.nombreProductoAgregado = nombreProductoClean;
                                        DataManager.kgProductoAgregado = pesoKG;
                                        Intent intentAlHomeAgregaProductoActivity = new Intent(this, typeof(HomeAgregaProductoActivity));
                                        StartActivity(intentAlHomeAgregaProductoActivity);
                                        DataManager.estadoBusquedaProducto = string.Empty;
                                        AnalyticService.TrackAnalytics("Agregar producto app", new Dictionary<string, string> {
                                            { "Category", "Codigo retorno success true" },
                                            { "Action", "producto " + nombreProducto + " agregado, con " + pesoNetoConvert + "kilos. En el servicio"}
                                        });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.Info("BtnAgregarProducto_Click", ex.Message);
                                    imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Gone;
                                    lblBtnAgregarProducto.Visibility = ViewStates.Visible;
                                    DataManager.estadoProductoAgregado = true;
                                    DataManager.nombreProductoAgregado = nombreProductoClean;
                                    DataManager.kgProductoAgregado = pesoKG;
                                    Intent intentAlHomeAgregaProductoActivity = new Intent(this, typeof(HomeAgregaProductoActivity));
                                    StartActivity(intentAlHomeAgregaProductoActivity);
                                    DataManager.estadoBusquedaProducto = string.Empty;
                                    AnalyticService.TrackAnalytics("Agregar producto app", new Dictionary<string, string> {
                                        { "Category", "Codigo retorno success true" },
                                        { "Action", "producto " + nombreProducto + " agregado, con " + pesoNetoConvert + "kilos. En el servicio"}
                                    });
                                }
                            }
                            else
                            {
                                    DataManager.RealmInstance.Write(() =>
                                    {
                                        DataManager.RealmInstance.Add(item);
                                    });
                                    imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Gone;
                                    lblBtnAgregarProducto.Visibility = ViewStates.Visible;
                                    DataManager.estadoProductoAgregado = false;
                                    Intent intentAlHomeAgregaProductoActivity = new Intent(this, typeof(HomeAgregaProductoActivity));
                                    StartActivity(intentAlHomeAgregaProductoActivity);
                                    DataManager.estadoBusquedaProducto = string.Empty;

                                    AnalyticService.TrackAnalytics("Agregar producto app", new Dictionary<string, string> {
                                        { "Category", "Codigo retorno success true" },
                                        { "Action", "producto " + nombreProductoClean + " agregado, con " + pesoKG + "kilos. En el bd local"}
                                    });
                            }
                        }
                        else
                        {
                            imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Gone;
                            lblBtnAgregarProducto.Visibility = ViewStates.Visible;
                            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                            builder.SetTitle("Trazabilidad de carnes");
                            builder.SetCancelable(false);
                            builder.SetIcon(Resource.Mipmap.ic_trazabilidad);
                            builder.SetMessage("El peso neto del producto, debe ser menor o igual a 100 kg");
                            builder.SetPositiveButton("Aceptar", delegate {
                                estadoBloqueado(true);
                            });
                            builder.Show();
                        }
                    }
                    else
                    {
                        imgLoadingBtnAgregaPruducto.Visibility = ViewStates.Gone;
                        lblBtnAgregarProducto.Visibility = ViewStates.Visible;
                        Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                        builder.SetTitle("Trazabilidad de carnes");
                        builder.SetCancelable(false);
                        builder.SetIcon(Resource.Mipmap.ic_trazabilidad);
                        builder.SetMessage("Debes ingresar la información requerida en todos los campos");
                        builder.SetPositiveButton("Aceptar", delegate {
                            estadoBloqueado(true);
                        });
                        builder.Show();
                    }
                
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        /// <summary>
        /// Estados the bloqueado.
        /// </summary>
        /// <param name="estado">If set to <c>true</c> estado.</param>
        public void estadoBloqueado(bool estado)
        {
            llBtnAgregarProducto.Clickable = estado;
            llBtnAgregarProducto.Enabled = estado;
            txtCodigoBlanza.Clickable = estado;
            txtCodigoBlanza.Enabled = estado;
            btnBuscar.Clickable = estado;
            btnBuscar.Enabled = estado;
            spiItemsProductos.Clickable = estado;
            spiItemsProductos.Enabled = estado;
            spiFrigorificoProducto.Clickable = estado;
            spiFrigorificoProducto.Enabled = estado;
            spiProcedenciaProducto.Clickable = estado;
            spiProcedenciaProducto.Enabled = estado;
            txtFechaFaenaProducto.Clickable = estado;
            txtFechaFaenaProducto.Enabled = estado;
            txtPesoNetoProducto.Clickable = estado;
            txtPesoNetoProducto.Enabled = estado;
            linearLayoutBtnIrAMapasDeTienda.Clickable = estado;
            linearLayoutBtnIrAMapasDeTienda.Enabled = estado;
            btnVolver.Clickable = estado;
            btnVolver.Enabled = estado;
            if (!estado)
            {
                txtInputPesoNetoProducto.SetHintTextAppearance(Resource.Style.TextAppearance_App_TextInputLayoutBlock);
                //txtPesoNetoProducto.SetHintTextColor(Color.ParseColor("#cdcdcd"));
                txtPesoNetoProducto.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittextbloqueado);
                txtPesoNetoProducto.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
                txtInputFechaFaenaProducto.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
                txtFechaFaenaProducto.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittextbloqueado);
                txtFechaFaenaProducto.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.edittexthintbloqueado));
                //btnBuscar.SetBackgroundColor(Resources.GetColor(Resource.Color.botonBuscarBloqueado));
            }
            else
            {
                txtInputPesoNetoProducto.SetHintTextAppearance(Resource.Style.TextAppearance_App_TextInputLayout);
                //txtPesoNetoProducto.SetHintTextColor(Color.ParseColor("#444444"));
                txtPesoNetoProducto.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.negro);
                txtPesoNetoProducto.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.negro));
                txtInputFechaFaenaProducto.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.negro));
                txtFechaFaenaProducto.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.negro);
                txtFechaFaenaProducto.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.negro));
            }
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
                case DATE_DIALOG:
                    var datePicker = new DatePickerDialog(this, this, year, mes, dia);
                    datePicker.DatePicker.MaxDate = FormatUtils.SetMaxDate(1);
                    datePicker.DatePicker.MinDate = FormatUtils.SetMinDate();
                    return datePicker;
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
            fechaCalendarioAndroid(year, month, dayOfMonth);
        }

        void fechaCalendarioAndroid(int yearParameter, int month, int dayOfMonth)
        {
            string mesString = string.Empty;
            this.mes = month;
            this.dia = dayOfMonth;
            var diaFormat = String.Format("{0:dd}", dayOfMonth.ToString());
            var mesFormat = String.Format("{0:MM}", (month + 1).ToString());

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
            diaFormat = diaFormat.Length == 1 ? "0" + diaFormat : diaFormat;
            fechaFaena = yearParameter.ToString().Substring(2) + mesString + diaFormat;
            txtFechaFaenaProducto.Text = diaFormat + "/" + mesString + "/" + yearParameter;
            txtInputFechaFaenaProducto.Visibility = ViewStates.Visible;
        }

        /// <summary>
        /// Ons the back pressed.
        /// </summary>
        public override void OnBackPressed()
        {
            Intent intentAgregarProductoActivity = new Intent(this, typeof(HomeAgregaProductoActivity));
            StartActivity(intentAgregarProductoActivity);
            DataManager.estadoBusquedaProducto = string.Empty;
        }

        /// <summary>
        /// Spis the procedencia producto item click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void SpiProcedenciaProducto_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Console.WriteLine(sender.ToString());
        }

        /// <summary>
        /// Ons the item selected.
        /// </summary>
        /// <param name="parent">Parent.</param>
        /// <param name="view">View.</param>
        /// <param name="position">Position.</param>
        /// <param name="id">Identifier.</param>
        public void OnItemSelected(AdapterView parent, View view, int position, long id)
        {
            if (position != -1)
            {
                var cod = dataProcedencia[position].CodigoProcedencia;
                string[] itemsFrigorifico = (from f in dataFrigorifico where f.CodigoProcedencia == cod select f.NombreFrigorifico).ToArray();
                adapterFrigorifico = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, itemsFrigorifico);
                adapterFrigorifico.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spiFrigorificoProducto.Adapter = adapterFrigorifico;
            }
            else
            {
                adapterFrigorifico = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, itemFrigorificoSelecciona);
                adapterFrigorifico.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spiFrigorificoProducto.Adapter = adapterFrigorifico;
            }
        }

        /// <summary>
        /// Ons the nothing selected.
        /// </summary>
        /// <param name="parent">Parent.</param>
        public void OnNothingSelected(AdapterView parent)
        {
            return;
        }

        /// <summary>
        /// Ons the key.
        /// </summary>
        /// <returns><c>true</c>, if key was oned, <c>false</c> otherwise.</returns>
        /// <param name="v">V.</param>
        /// <param name="keyCode">Key code.</param>
        /// <param name="e">E.</param>
        public bool OnKey(View v, [GeneratedEnum] Keycode keyCode, KeyEvent e)
        {

            return true;
        }
    }
}