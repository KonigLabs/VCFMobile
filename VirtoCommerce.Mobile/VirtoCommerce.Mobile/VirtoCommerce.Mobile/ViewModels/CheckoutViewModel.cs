﻿using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Mobile.Model;
using VirtoCommerce.Mobile.Services;

namespace VirtoCommerce.Mobile.ViewModels
{
    public class CheckoutViewModel : MvxViewModel
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private MvxCommand _createOrderCommand;
        private bool _canCreateOrder;

        public bool CanCreateOrder
        {
            set { _canCreateOrder = value; RaisePropertyChanged(); }
            get { return _canCreateOrder; }
        }

        public MvxCommand CreateOrderCommand
        {
            get
            {
                return _createOrderCommand ?? (_createOrderCommand = new MvxCommand(() =>
                {
                    _orderService.CreateOreder();
                    ShowViewModel<ThanksViewModel>();
                },
                () => CanCreateOrder = PaymentMethods.Any(x => x.IsSelect)));
            }
        }
        public Cart Cart { set; get; }
        public ICollection<PaymnetMethodViewModel> PaymentMethods { set; get; }

        public CheckoutViewModel(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
            Cart = _cartService.GetCart();
            PaymentMethods = _orderService.PaymentMethods().Select(x => new PaymnetMethodViewModel(x, CreateOrderCommand)).ToArray();
        }
    }
}
