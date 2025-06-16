using Stripe;
using Stripe.Checkout;
using AeroLinea.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AeroLinea.Services
{
    public class StripeService
    {
        private readonly string _stripeSecretKey;

        public StripeService()
        {
            _stripeSecretKey = "sk_test_51Rah0SRqc17EghR3aCm7XcWJgty0uHHgqcGKtgs4mYrv1uYOYX5xAoz7RxrK6RcsQPzJtDD40ofmVoghglfhGxyg00XwABagjr";
            StripeConfiguration.ApiKey = _stripeSecretKey;
        }

        public async Task<string> CrearSesionPago(PagoModel pago)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(pago.Monto * 100),
                            Currency = "pen",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = pago.Descripcion,
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = $"http://localhost:5185/Home/ConfirmacionPago?session_id={{CHECKOUT_SESSION_ID}}&idReserva={pago.IdReserva}",
                CancelUrl = "http://localhost:5185/Home/CancelacionPago",
                CustomerEmail = pago.EmailCliente,
                Locale = "es",
                PaymentMethodOptions = new SessionPaymentMethodOptionsOptions
                {
                    Card = new SessionPaymentMethodOptionsCardOptions
                    {
                        RequestThreeDSecure = "automatic"
                    }
                }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);
            return session.Url;
        }

        public async Task<bool> VerificarPago(string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);
            return session.PaymentStatus == "paid";
        }
    }
} 