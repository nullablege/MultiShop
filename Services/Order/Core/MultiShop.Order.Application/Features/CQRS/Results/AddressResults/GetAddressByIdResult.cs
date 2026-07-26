using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.CQRS.Results.AddressResults
{
    public class GetAddressByIdResult
    {
        public int AddressId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone {  get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Detail1 {  get; set; } = string.Empty;
        public string Detail2 { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }
}
