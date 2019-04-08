using FFImageLoading.Forms;
using IDO_Client.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IDO_Client.Controls
{
    class NoteCell : ViewCell
    {

        public CachedImage cachedImage { get; set; }
        public NoteCell() : base()
        {
            cachedImage = new CachedImage();
            View = cachedImage;
        }

        protected override void OnBindingContextChanged()
        {

            base.OnBindingContextChanged();
            cachedImage.Source = null;
            var item = BindingContext as Data;

            if (item == null)
            {
                return;
            }
            cachedImage.Source  = item.Image;

        }
    }
}
