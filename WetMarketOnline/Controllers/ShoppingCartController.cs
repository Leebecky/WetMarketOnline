using EWM.HelperClass;
using EWM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWM.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult ShoppingCart()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }
            MstCustomer user = (MstCustomer)Session["Account"];
            //TxnShoppingCart cart = TxnShoppingCart.GetTxnShoppingCartItems(user.CustomerId);

            return View();
        }

        //? ShoppingCart Items
        public ActionResult ShoppingCartItems()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }
            MstCustomer user = (MstCustomer)Session["Account"];
            TxnShoppingCart cart = TxnShoppingCart.GetTxnShoppingCartItems(user.CustomerId);

            return PartialView(cart.GetCartItems());
        }

        //? AJAX - Calculate Item Order Total
        public ActionResult CalculateCartTotal()
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return RedirectToAction("Login", "Account"); }
            MstCustomer user = (MstCustomer)Session["Account"];
            TxnShoppingCart cart = TxnShoppingCart.GetTxnShoppingCartItems(user.CustomerId);
            decimal totalPrice = 0;

            List<TxnShoppingCart> cartItems = cart.GetCartItems();

            foreach (var item in cartItems)
            {
                MstProduct product = item.GetProductItem();
                totalPrice += (product.Price.Value * item.Quantity);
            }

            Math.Round(totalPrice, 2);

            return Json(totalPrice);
        }

        //? AJAX - Add items to shopping cart
        public ActionResult AddToShoppingCart(string productId, string quantity)
        {
            // get item id > update quantity
            // update nav header

            // Validating that customer is logged in
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return Json("Please login first!"); }
            MstCustomer user = (MstCustomer)Session["Account"];

            TxnShoppingCart cart = TxnShoppingCart.GetTxnShoppingCartItems(user.CustomerId);

            TxnShoppingCart cartItem = cart.GetCartItems().Find(l => l.ProductId == productId);
            cartItem.Quantity = int.Parse(quantity);
            if (cartItem != null)
            {
                cartItem.UpdateTxnShoppingCartItem();
            }
            else
            {
                return Json("Item not found");
            }
            //Session["ShoppingCart"] = cart.RetrieveCartItemsFromDb().Count;

            return Json("Ok");
        }

        //? AJAX - Add items to shopping cart
        public ActionResult SubtractFromShoppingCart(string productId, string quantity)
        {
            // get item id > update quantity
            // update nav header

            // Validating that customer is logged in
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return Json("Please login first!"); }
            MstCustomer user = (MstCustomer)Session["Account"];

            TxnShoppingCart cart = TxnShoppingCart.GetTxnShoppingCartItems(user.CustomerId);

            TxnShoppingCart cartItem = cart.GetCartItems().Find(l => l.ProductId == productId);
            cartItem.Quantity = int.Parse(quantity);


            //Session["ShoppingCart"] = cart.RetrieveCartItemsFromDb().Count;
            if (cartItem != null)
            {
                cartItem.UpdateTxnShoppingCartItem();
            }
            else
            {
                return Json("Item not found");
            }

            return Json("Ok");
        }

        //? AJAX - Add items to shopping cart
        public ActionResult DeleteFromShoppingCart(string productId)
        {
            // get item id > update quantity
            // update nav header

            // Validating that customer is logged in
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return Json("Please login first!"); }
            MstCustomer user = (MstCustomer)Session["Account"];

            TxnShoppingCart cart = TxnShoppingCart.GetTxnShoppingCartItems(user.CustomerId);

            TxnShoppingCart cartItem = cart.GetCartItems().Find(l => l.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.DeleteTxnShoppingCartItem();
            }
            else
            {
                return Json("Item not found");
            }

            Session["ShoppingCart"] = cart.RetrieveCartItemsFromDb().Count;

            return Json("Ok");
        }

        //? AJAX - Verify Promotion Code
        public ActionResult CheckPromotionCode(string promoCode)
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return Json("Please login first!"); }
            MstCustomer user = (MstCustomer)Session["Account"];

            MstPromotion promo = new MstPromotion();
            promo.PromotionCode = promoCode;
            promo.Status = "Active";

            int valid = promo.CheckMstPromotion();

            if (valid == 1)
            {
                MstPromotion promotion = promo.SelectMstPromotion("All")[0];
                TxnOrderHdr order = new TxnOrderHdr();
                order.CustomerId = user.CustomerId;
                order.PromotionId = promotion.PromotionId;
                List<TxnOrderHdr> promoUsageCheck = order.SelectTxnOrderHdr("All");

                if (promoUsageCheck.Count > 0)
                {
                    return Json("This promotion has already been used!");
                }

                return Json(promotion);
            }
            else
            {
                return Json("No promotion with that code found!");
            }
        }

        //? Create Order
        [HttpPost]
        public ActionResult CreateOrder(string promotionId = "", string discount = "", string cartTotal = "", string shippingFee = "")
        {
            if (!GeneralBLL.VerifyAccessRight(Session["AccountType"], "Customer")) { return Json("Please login first!"); }
            MstCustomer user = (MstCustomer)Session["Account"];

            // Order Header
            TxnOrderHdr orderHdr = new TxnOrderHdr();
            orderHdr.OrderHdrId = Guid.NewGuid().ToString();
            orderHdr.CustomerId = user.CustomerId;
            orderHdr.PromotionId = promotionId;
            orderHdr.OrderDate = DateTime.Now;
            orderHdr.ShippingFee = decimal.Parse(shippingFee);
            orderHdr.Discount = decimal.Parse(discount);
            orderHdr.TotalPrice = decimal.Parse(cartTotal);

            orderHdr.CreateTxnOrderHdr(user.Username);

            // Order Details
            TxnShoppingCart cart = TxnShoppingCart.GetTxnShoppingCartItems(user.CustomerId);
            List<TxnShoppingCart> cartItems = cart.GetCartItems();
            foreach (var item in cartItems)
            {
                MstProduct product = item.GetProductItem();

                TxnOrderDtl dtl = new TxnOrderDtl();
                dtl.OrderHdrId = orderHdr.OrderHdrId;
                dtl.ProductId = product.ProductId;
                dtl.Quantity = item.Quantity;
                dtl.Price = product.Price.Value;

                dtl.CreateTxnOrderDtl(user.Username);
            }

            // Clear shopping cart
            for (int i = 0; i < cartItems.Count; i++)
            {
                cartItems[i].PurchaseTxnShoppingCartItem();
            }

            Session["ShoppingCart"] = 0;

            // TODO Redirect to Order Summary page
            return RedirectToAction("MstCustomer", "MstCustomer");
        }
    }
}