using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace OrderService.Model.Entities
{
    [BsonIgnoreExtraElements]
    public class Order
    {
        [BsonConstructor]
        public Order() { }

        [BsonConstructor]
        public Order(string userId,
        string firstName,
        string lastName,
        string address,
        string phoneNumber,
        int totalPrice,
        ICollection<OrderLine> orderLines)
        {
            this.UserId = userId;
            this.OrderLines = orderLines;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.TotalPrice = totalPrice;
        }

        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public DateTime OrderPlaced { get; private set; } = DateTime.Now;
        public bool OrderPaid { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int TotalPrice { get; set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public ICollection<OrderLine> OrderLines { get; set; }

        public void RequestPayment()
        {
            PaymentStatus = PaymentStatus.RequestPayment;
        }

        public void PaymentIsDone()
        {
            OrderPaid = true;
            PaymentStatus = PaymentStatus.isPaid;
        }

    }
}
