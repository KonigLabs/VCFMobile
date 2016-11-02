
using System;
using System.Drawing;
using VirtoCommerce.Mobile.Model;
using Foundation;
using UIKit;

using Xamarin.Themes;

namespace VirtoCommerce.Mobile.iOS.UI.Products
{
	public class ProductCell : UITableViewCell
	{
		private UIView _statusView { get; set; }
		private UIImageView _productImage { get; set; }
		private UILabel _titleLabel { get; set; }
        private UILabel _descriptionLabel { set; get; }
        private UIView _actionsView { set; get; }
        private UILabel _salePriceLable { set; get; }
        private UILabel _listPriceLable { set; get; }
        private UIButton _cartButton { set; get; }
        private Action<Product> _cartTouch { set; get; }
		Product _data;

		public static readonly NSString Key = new NSString ("ProductCell");
		
		public ProductCell(Action<Product> cartTouch) : base (UITableViewCellStyle.Default, Key)
		{
            _cartTouch = cartTouch;
			InitSubviews();
			ApplyStyles();
		}

        void InitSubviews()
        {
            _productImage = new UIImageView(new RectangleF(5, 5, 231, 200));
            BackgroundView = new UIView(new RectangleF(0, 0, 241, 1000))
            {
                BackgroundColor = UIColor.White
            };

            AddSubviews(BackgroundView, _productImage);

            _statusView = new UIView(new RectangleF(0, 213, 241, 71));

            _titleLabel = new UILabel(new RectangleF(8, 5, 224, 22))
            {
                TextColor = UIColor.Black,
                TextAlignment = UITextAlignment.Center,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap
            };
            _descriptionLabel = new UILabel(new RectangleF(8, 30, 224, 22))
            {
                Font = UIFont.FromName("Helvetica Neue", 12),
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.Black,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap
            };

            _statusView.AddSubviews(_titleLabel, _descriptionLabel);
            Add(_statusView);
            _listPriceLable = new UILabel(new RectangleF(8, 5, 50, 30))
            {
                TextColor = UIColor.Gray,
                Font = UIFont.FromName("Helvetica Neue", 20),
                TextAlignment = UITextAlignment.Center
            };
            _salePriceLable = new UILabel(new RectangleF(58, 5, 50, 30))
            {
                TextColor = UIColor.Black,
                Font = UIFont.FromName("Helvetica Neue", 20),
                TextAlignment = UITextAlignment.Center
            };
            _cartButton = new UIButton(new RectangleF(200, 0, 30, 30));
            _cartButton.TouchDown += TouchCart;
            _cartButton.SetImage(UIImage.FromFile("cart.png"), UIControlState.Normal);
            _actionsView = new UIView(new RectangleF(5, 10, 241, 30));
            _actionsView.AddSubviews(_listPriceLable, _salePriceLable, _cartButton);
            Add(_actionsView);
        }

        private void TouchCart(object s, EventArgs e)
        {
            _cartTouch?.Invoke(_data);
        } 

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _cartButton.AllTouchEvents -= TouchCart;
        }

        void ApplyStyles()
		{
			BackgroundColor = UIColor.White;
		}

        public void Bind(Product data)
        {

            _data = data;
            _titleLabel.Text = data.Name;
            _descriptionLabel.Text = data.Description;
            var listPrice = data.Price.FormattedListPrice;
            if (!string.IsNullOrEmpty(listPrice))
            {
                var attrString = new NSMutableAttributedString(listPrice);
                attrString.AddAttribute(
                    UIStringAttributeKey.StrikethroughStyle,
                    NSNumber.FromInt32((int)NSUnderlineStyle.Single),
                    new NSRange(0, attrString.Length));
                _listPriceLable.AttributedText = attrString;
            }
            _salePriceLable.Text =  data.Price.FormattedSalePrice;
        }

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
            var image = UIImage.FromFile($"{_data.Id}.png");
            var scale = 230 / image.Size.Width;
            image = image.Scale(new CoreGraphics.CGSize(image.Size.Width * scale, image.Size.Height * scale));
			var imageFrame = _productImage.Frame;
            
			imageFrame.Size = image.Size;
			_productImage.Frame = imageFrame;
			_productImage.Image = image;
            //
            _descriptionLabel.SizeToFit();
            _statusView.SizeToFit();
			var statusFrame = _statusView.Frame;
			statusFrame.Y = 2 * imageFrame.Y + imageFrame.Height;
            statusFrame.Height = _descriptionLabel.Frame.Height + _titleLabel.Frame.Height + 16;
			_statusView.Frame = Rectangle.Round((RectangleF)statusFrame);
            //
            _listPriceLable.SizeToFit();
            _salePriceLable.SizeToFit();
            var actionFrame = _actionsView.Frame;
            actionFrame.Y = statusFrame.Y + statusFrame.Height + 8;
            _actionsView.Frame = actionFrame;
            var saleFrame = _salePriceLable.Frame;
            saleFrame.X = _listPriceLable.Frame.Width + _listPriceLable.Frame.X + 3;
            _salePriceLable.Frame = saleFrame;
            //

			var cellFrame = Frame;
			cellFrame.Width = 241;
            cellFrame.Height = _productImage.Frame.Y + _productImage.Frame.Height + statusFrame.Height + 15 + actionFrame.Height;
			Frame = Rectangle.Round((RectangleF)cellFrame);

			Superview.Superview.LayoutSubviews();
		}
	}
}

