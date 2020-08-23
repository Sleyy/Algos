using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyRelations
{
    class Program
    {
        static void Main(string[] args)
        {
            var companies = new List<Company>()
            {
                new Company(){Name = "A"},
                new Company(){Name = "B"},
                new Company(){Name = "C"},
                new Company(){Name = "D"},
                new Company(){Name = "E"},
                new Company(){Name = "F"}
            };

            // Parent - Daughter
            var companyRelations = new Dictionary<string, string>()
            {
                {"A", "B" },
                {"C", "D" },
                {"E", "F" },
            };

            // Assign each company it's parent from the list of companies
            // Important that it is the same object
            foreach (var company in companyRelations)
            {
                companies.FirstOrDefault(c => c.Name.Equals(company.Value)).Parent = companies.FirstOrDefault(c => c.Name.Equals(company.Key));
            }

            // Parent - Daughter
            var companiesToCheck = new Dictionary<string, string>()
            {
                {"A", "B" },
                {"B", "C" },
                {"C", "D" },
                {"D", "E" },
                {"E", "F" }
            };

            Console.WriteLine("Attempting Iterative algorithm: ");
            CheckForMatchesByIterating(companies, companiesToCheck);

            Console.WriteLine("Attempting Recursive algorithm: ");
            CheckForMatchesByRecursion(companies, companiesToCheck);
        }

        private static void CheckForMatchesByIterating(List<Company> companies, Dictionary<string, string> companiesToCheck)
        {
            string parent;
            string daughter;
            foreach (var company in companiesToCheck)
            {
                parent = company.Key;
                daughter = company.Value;

                if (companies.Any(c => c.Name == daughter && (c.Parent != null && c.Parent.Name == parent)))
                {
                    Console.WriteLine($"{parent} - {daughter} - Yes.");
                }
                else
                {
                    Console.WriteLine($"{parent} - {daughter} - No.");
                }
            }
        }

        private static void CheckForMatchesByRecursion(List<Company> companies, Dictionary<string, string> companiesToCheck, int index = 0)
        {
            if(index == companiesToCheck.Count)
            {
                return;
            }

            var companyKVP = companiesToCheck.Skip(index).Take(1).FirstOrDefault();

            string parent = companyKVP.Key;
            string daughter = companyKVP.Value;

            if (companies.Any(c => c.Name == daughter && (c.Parent != null && c.Parent.Name == parent)))
            {
                Console.WriteLine($"{parent} - {daughter} - Yes.");
            }
            else
            {
                Console.WriteLine($"{parent} - {daughter} - No.");
            }

            CheckForMatchesByRecursion(companies, companiesToCheck, ++index);
        }
    }
}
