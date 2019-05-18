using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views.InputMethods;
using Android.Widget;
using FoodSafety.Common.Utils;
using FoodSafety.ProductosRefresh.Services;

namespace FoodSafety.ProductosRefresh
{
    /// <summary>
    /// Ingreso usuario responsable activity.
    /// </summary>
    [Activity(Label = "Formulario ingreso", Theme = "@style/ThemeNoActionBarAzul")]
    public class IngresoUsuarioResponsableActivity : Activity
    {
        static Dialog customDialog = null;

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ingreso_usuarioresponsable_dialog);
        }

        /// <summary>
        /// Views the formulario user.
        /// </summary>
        /// <param name="activity">Activity.</param>
        public static void viewFormularioUser(Activity activity)
        {
            customDialog = new Dialog(activity, Resource.Style.Theme_Dialog_Translucent);
            var ha = (HomeAgregaProductoActivity)activity;
            customDialog.SetCancelable(false);
            customDialog.SetContentView(Resource.Layout.ingreso_usuarioresponsable_dialog);
            EditText txtNombreUsuario = customDialog.FindViewById<EditText>(Resource.Id.txtUser);
            TextView lblMensajeError = customDialog.FindViewById<TextView>(Resource.Id.lblMensajeError);
            TextView lblBtnIngresarUser = customDialog.FindViewById<TextView>(Resource.Id.lblBtnIngresarUser);
            LinearLayout llBtnIngresarUser = customDialog.FindViewById<LinearLayout>(Resource.Id.llBtnIngresarUser);

            if (!string.IsNullOrEmpty(DataManager.nombreUsuario))
            {
                txtNombreUsuario.Text = DataManager.nombreUsuario;
                llBtnIngresarUser.Enabled = true;
                llBtnIngresarUser.SetBackgroundResource(Resource.Drawable.cssBotonAgregarProducto);
            }
            else
            {
                txtNombreUsuario.Text = string.Empty;
                llBtnIngresarUser.Enabled = false;
                llBtnIngresarUser.SetBackgroundResource(Resource.Drawable.cssBotonBloqueado);
            }

            txtNombreUsuario.TextChanged += delegate
            {
                if (txtNombreUsuario.Text.Trim().Length >= 5 || txtNombreUsuario.Text.Trim().Length == 0)
                {
                    lblMensajeError.Text = string.Empty;
                }
                if (!string.IsNullOrEmpty(txtNombreUsuario.Text))
                {
                    llBtnIngresarUser.Enabled = true;
                    llBtnIngresarUser.SetBackgroundResource(Resource.Drawable.cssBotonAgregarProducto);
                }
                else
                {
                    llBtnIngresarUser.Enabled = false;
                    llBtnIngresarUser.SetBackgroundResource(Resource.Drawable.cssBotonBloqueado);
                }

            };

            llBtnIngresarUser.Click += delegate
            {
                if (txtNombreUsuario.Text.Trim().Length >= 5)
                {
                    DataManager.nombreUsuario = txtNombreUsuario.Text.Trim();
                    ha.nombreUsuarioToolbar();
                    customDialog.Dismiss();
                    AnalyticService.TrackAnalytics("Ingreso Usuario", new Dictionary<string, string> {
                        { "Category", "Usuario " + DataManager.nombreUsuario + " agregado, el dia " + DateTime.Now },
                        { "Action", "click llBtnIngresarUser"}
                    });
                }
                else
                {
                    lblMensajeError.Text = "Error al ingresar los datos";
                    AnalyticService.TrackAnalytics("Error ngreso Usuario", new Dictionary<string, string> {
                        { "Category", "Usuario " + txtNombreUsuario.Text + " agregado, el dia " + DateTime.Now },
                        { "Action", "click llBtnIngresarUser"}
                    });
                }
            };
            txtNombreUsuario.ImeOptions = ImeAction.Done;
            customDialog.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            customDialog.Show();
        }
    }
}