using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(c => c.Orders.Sum(o => o.Total) > limit);
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            return customers.Select(c => (c, suppliers.Where(s => s.Country == c.Country && s.City == c.City)));
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            return customers.Select(c => (c, suppliers.Where(s => s.Country == c.Country && s.City == c.City)));
        }

        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(c => c.Orders.Sum(o => o.Total) > limit && c.Orders.Length > 0);
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(
            IEnumerable<Customer> customers
        )
        {
            return customers.Select(
                c => (c, c.Orders.Select(o => o.OrderDate).FirstOrDefault(d => d == c.Orders.Min(o => o.OrderDate))))
                .Where(t => t.c.Orders.Length != 0);
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(
            IEnumerable<Customer> customers
        )
        {
            return customers.Select(
                    c => (c, c.Orders.Select(o => o.OrderDate).FirstOrDefault(d => d == c.Orders.Min(o => o.OrderDate))))
                .Where(t => t.c.Orders.Length != 0)
                .OrderBy(t => t.c.Orders.FirstOrDefault().OrderDate.Year)
                .ThenBy(t => t.c.Orders.FirstOrDefault().OrderDate.Month)
                .ThenByDescending(t => t.c.Orders.FirstOrDefault().Total)
                .ThenBy(t => t.c.CompanyName);
        }

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            Regex postalRegex = new Regex("^\\d+$");
            Regex phoneRegex = new Regex("\\(([^\\)]+)\\)");
            return customers.Select(c => c).Where(c =>
                !postalRegex.IsMatch(c.PostalCode) || string.IsNullOrEmpty(c.Region) || !phoneRegex.IsMatch(c.Phone));
        }

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {
            /* example of Linq7result

             category - Beverages
	            UnitsInStock - 39
		            price - 18.0000
		            price - 19.0000
	            UnitsInStock - 17
		            price - 18.0000
		            price - 19.0000
             */
            //var result = products
            //    .GroupBy(p => new { p.Category, p.UnitsInStock, price = p.UnitPrice })
            //    .OrderBy(p => p.Key.price);

            //return result;
            throw new NotImplementedException();
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
            IEnumerable<Product> products,
            decimal cheap,
            decimal middle,
            decimal expensive
        )
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(
            IEnumerable<Customer> customers
        )
        {
            throw new NotImplementedException();
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            throw new NotImplementedException();
        }
    }
}