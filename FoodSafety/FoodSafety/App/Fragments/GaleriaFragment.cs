using System;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Support.V4.App;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using FoodSafety.Common.Utils;

namespace FoodSafety
{
    /// <summary>
    /// Galeria fragment.
    /// </summary>
    public class GaleriaFragment : DialogFragment, View.IOnClickListener
    {
        ImageView fotoImg, galeriaImg,photoView;
        Button buttonCancel, buttonAceptar;
        LinearLayout llGalleryFragment;
        TextView tittleBarDialog;
        AlphaAnimation buttonAnimation = new AlphaAnimation(1F, 0.4F);

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(StyleNoTitle, 0);
            // Create your fragment here
        }

        public override void OnResume()
        {
            base.OnResume();
        }

        /// <summary>
        /// Ons the create view.
        /// </summary>
        /// <returns>The create view.</returns>
        /// <param name="inflater">Inflater.</param>
        /// <param name="container">Container.</param>
        /// <param name="savedInstanceState">Saved instance state.</param>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            ViewGroup v = (ViewGroup)inflater.Inflate(Resource.Layout.GalleryFragment, container, false);
            //llGalleryFragment = (LinearLayout)v.FindViewById(Resource.Id.llGalleryFragment);
            photoView = (ImageView)v.FindViewById(Resource.Id.photoView);
            fotoImg = (ImageView)v.FindViewById(Resource.Id.fotoBtn);
            galeriaImg = (ImageView)v.FindViewById(Resource.Id.galeryBtn);
            tittleBarDialog = (TextView)v.FindViewById(Resource.Id.tittleBarDialog);
            buttonCancel = (Button)v.FindViewById(Resource.Id.buttonCancelFoto);
            buttonAceptar = (Button)v.FindViewById(Resource.Id.buttonSendFoto);
            fotoImg.SetOnClickListener(this);
            galeriaImg.SetOnClickListener(this);
            try
            {
                CreateDirectoryForPictures();
                Intent intent = new Intent(MediaStore.ActionImageCapture);
                AppUtils._file = new Java.IO.File(AppUtils._dir, System.String.Format("etiquetaPhoto_{0}.jpg", Guid.NewGuid()));
                intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(AppUtils._file));
                StartActivityForResult(intent, 1);
            }
            catch (Java.Lang.SecurityException)
            {
                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(Activity);
                builder.SetTitle("Trazabilidad de carnes");
                builder.SetIcon(Resource.Mipmap.ic_trazabilidad);
                builder.SetCancelable(false);
                builder.SetMessage("Estimado usuario, por favor ir hasta ajustes del tablet y en permisos de la aplicación habilitar los permisos de ESPACIO DE ALMACENAMIENTO y CÁMARA.");
                builder.SetPositiveButton("Aceptar", delegate { });
                builder.Show();
            }


            /*buttonAceptar.Animation = buttonAnimation;
            buttonAceptar.Click += delegate {
                Intent intentAgregarProductoActivity = new Intent(this.Activity, typeof(AgregarProductoActivity));
                StartActivity(intentAgregarProductoActivity);
                Dismiss();
            };

           

            buttonCancel.Animation = buttonAnimation;
            buttonCancel.Click += delegate {
                DataManager.imagePath = null;
                DataManager.imageData = null;
                DataManager.thumbnail = null;
                Dismiss();
            };

            if(DataManager.imagePath!=null){
                try{
                    if (DataManager.imageData != null)
                    {
                        photoView.SetImageBitmap(DataManager.thumbnail);
                    }
                }catch(Exception e){
                    
                }
            }*/

            return v;
        }

        /// <summary>
        /// Ons the start.
        /// </summary>
        public override void OnStart()
        {
            if (Dialog == null)
            {
                return;
            }

            Dialog.Window.SetWindowAnimations(Resource.Style.DlgAnimationMap);
            base.OnStart();
        }


        /// <summary>
        /// Ons the dismiss.
        /// </summary>
        /// <param name="dialog">Dialog.</param>
        public override void OnDismiss(IDialogInterface dialog)
        {         
            base.OnDismiss(dialog);
            DataManager.imagePath = null;
            DataManager.imageData = null;
            DataManager.imageFrom = 0;
            DataManager.thumbnail = null;
        }

        /// <summary>
        /// Ons the click.
        /// </summary>
        /// <param name="v">V.</param>
        public void OnClick(View v)
        {
           /* v.StartAnimation(buttonAnimation);
            if (v.ToString().Contains("galery"))
            {
                var imageIntent = new Intent();
                imageIntent.SetType("image/*");
                imageIntent.SetAction(Intent.ActionGetContent);
                var parentExist = FragmentManager.Fragments.OfType<GaleriaFragment>();
                if (parentExist.Any())
                {
                    parentExist.ElementAt(0).StartActivityForResult(Intent.CreateChooser(imageIntent, "Selecciona foto"), 0);
                }
            }

                
                if (v.ToString().Contains("foto"))
                {
                    CreateDirectoryForPictures();
                    Intent intent = new Intent(MediaStore.ActionImageCapture);              
                    AppUtils._file = new Java.IO.File(AppUtils._dir, System.String.Format("etiquetaPhoto_{0}.jpg", Guid.NewGuid()));
                    intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(AppUtils._file));
                    StartActivityForResult(intent, 1);
                }        */        
        }

        /// <summary>
        /// Ons the activity result.
        /// </summary>
        /// <param name="requestCode">Request code.</param>
        /// <param name="resultCode">Result code.</param>
        /// <param name="data">Data.</param>
        public override void OnActivityResult(int requestCode, int resultCode, Intent data)//async #todo
        {
            base.OnActivityResult(requestCode, resultCode, data);
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = 110;

            if(resultCode == -1){
                if (requestCode == 1){

                    Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    Android.Net.Uri contentUri = Android.Net.Uri.FromFile(AppUtils._file);
                    mediaScanIntent.SetData(contentUri);
                    //var parentExist = FragmentManager.Fragments.OfType<GaleriaFragment>();
                    //parentExist.ElementAt(0).Context.SendBroadcast(mediaScanIntent);

                    DataManager.imagePath = AppUtils._file.Path;
                    DataManager.imageData = contentUri;
                    DataManager.imageFrom = 2;
                    //buttonAceptar.Visibility = ViewStates.Visible;
                    //tittleBarDialog.Text = "Perfecto! ya estas listo para continuar";
                    try
                    {
                        AppUtils.bitmap = AppUtils._file.Path.LoadAndResizeBitmap(width, height);

                        if (AppUtils.bitmap != null)
                        {
                            DataManager.thumbnail = AppUtils.bitmap;
                            photoView.SetImageBitmap(AppUtils.bitmap);
                            AppUtils.bitmap = null;
                            GC.Collect();
                        }
                    }
                    catch (Exception e){}
                    Intent intentAgregarProductoActivity = new Intent(this.Activity, typeof(AgregarProductoActivity));
                    StartActivity(intentAgregarProductoActivity);
                }
            }else{
                Dismiss();
            }
        }
        
        /// <summary>
        /// Creates the directory for pictures.
        /// </summary>
        private void CreateDirectoryForPictures()
        {
            AppUtils._dir = new Java.IO.File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "TrazabilidadDeCarnes");
            if (!AppUtils._dir.Exists())
            {
                AppUtils._dir.Mkdirs();
            }
        }


    }
}