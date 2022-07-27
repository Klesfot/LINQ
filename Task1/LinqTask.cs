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

            return products.Select(
                p => new Linq7CategoryGroup
                {
                    Category = p.Category,
                    UnitsInStockGroup = new[]
                    {
                        new Linq7UnitsInStockGroup
                        {
                            UnitsInStock = p.UnitsInStock,
                            Prices = products.Where(product => product.Category == p.Category && product.UnitsInStock == p.UnitsInStock).Select(pr => pr.UnitPrice).ToArray()
                        }
                    }
                });
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
            IEnumerable<Product> products,
            decimal cheap,
            decimal middle,
            decimal expensive
        )
        {
            var result =
                products
                    .Where(p => p.UnitPrice <= cheap)
                    .GroupBy(p => p.UnitPrice <= cheap)
                    .Select(p =>
                        (cheap, products
                            .Where(p => p.UnitPrice <= cheap)));

            result = result.Union(products
                .Where(p => p.UnitPrice > cheap && p.UnitPrice <= middle)
                    .GroupBy(p => p.UnitPrice > cheap && p.UnitPrice <= middle)
                    .Select(r =>
                        (middle, products
                            .Where(p => p.UnitPrice > cheap && p.UnitPrice <= middle))));

            result = result.Union(
                products
                    .Where(p => p.UnitPrice > middle && p.UnitPrice <= expensive)
                    .GroupBy(p => p.UnitPrice > middle && p.UnitPrice <= expensive)
                    .Select(r =>
                        (expensive, products
                            .Where(p => p.UnitPrice > middle && p.UnitPrice <= expensive))));

            return result;
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(
            IEnumerable<Customer> customers
        )
        {
            /* todo example

            ("Berlin", 2023, 3),
            ("Mexico D.F.", 680, 2),
            ("London", 690, 1),
            ("Warszawa", 1, 0),
            ("Sao Paulo", 0, 0),
            ("USA", 0, 0)
            
            have

            ("Berlin", 674, 3),
            ("Mexico D.F.", 296, 3),
            ("Mexico D.F.", 576, 2),
            ("London", 690, 1),
            ("Mexico D.F.", 0, 2),
            ("London", 0, 0),
            ("Warszawa", 2, 1),
            ("Warszawa", 0, 0),
            ("Sao Paulo", 0, 0),
            ("USA", 0, 0)

             */

            var result = customers
                .Select(c => (c.City,
                    (int)c.Orders
                        .Where(o => o.OrderDate != null)
                        .GroupBy(o => customers.Select(c => c.City))
                        .Select(g => (g, (int)c.Orders.DefaultIfEmpty().Average(o => o?.Total ?? default(int)))).Average(o => o.Item2),
                    c.Orders.Length));
            return result;
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            var result = string.Empty;
            var supplierCountryNames = suppliers.Select(s => s.Country).Distinct().OrderBy(s => s.Length).ThenBy(s => s);
            
            return supplierCountryNames.Aggregate(result, (current, countryName) => current + countryName);
        }
    }
}