using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyRelations
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initial company list
            var companies = new List<Company>()
            {
                new Company(){Name = "A"},
                new Company(){Name = "B"},
                new Company(){Name = "C"},
                new Company(){Name = "D"},
                new Company(){Name = "E"},
                new Company(){Name = "F"}
            };

            // Parent - Daughter Mappings
            var companyRelations = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("A", "B" ),
                //new KeyValuePair<string, string>("B", "C" ),
                new KeyValuePair<string, string>("C", "D" ),
                new KeyValuePair<string, string>("E", "F" ),
            };

            // Assign each company it's parent from the list of companies
            MapCompanies(companies, companyRelations);
            
            // Company list to check if related
            // Parent - Daughter
            var companiesToCheck = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string,string>("A", "B" ),
                new KeyValuePair<string,string>("A", "C" ),
                new KeyValuePair<string,string>("B", "C" ),
                new KeyValuePair<string,string>("C", "D" ),
                new KeyValuePair<string,string>("E", "F" )
            };

            CheckForMatches(companies, companiesToCheck);
        }

        private static void MapCompanies(List<Company> companies, List<KeyValuePair<string, string>> companyRelations)
        {
            Company daughterCompany;
            Company parentCompany;
            foreach (var company in companyRelations)
            {
                daughterCompany = companies.FirstOrDefault(c => c.Name.Equals(company.Value));
                parentCompany = companies.FirstOrDefault(c => c.Name.Equals(company.Key));

                // Both companies Exist
                if (daughterCompany == null || parentCompany == null)
                {
                    return;
                }

                daughterCompany.Parent = parentCompany;
            }
        }

        private static void CheckForMatches(List<Company> companies, List<KeyValuePair<string, string>> companiesToCheck)
        {
            string parentCompanyName;
            string daughterCompanyName;
            foreach (var company in companiesToCheck)
            {
                parentCompanyName = company.Key;
                daughterCompanyName = company.Value;

                var startCompany = companies.FirstOrDefault(c => c.Name.Equals(daughterCompanyName));

                if (startCompany == null)
                {
                    Console.WriteLine($"Company: {daughterCompanyName} Not Found!");
                    continue;
                }

                // Iterative Algorithm Check
                bool companyIsRelated = IterativeMatches(startCompany, parentCompanyName);

                if (companyIsRelated)
                {
                    Console.WriteLine($"Iterative: {parentCompanyName} - {daughterCompanyName} - Yes.");
                }
                else
                {
                    Console.WriteLine($"Iterative: {parentCompanyName} - {daughterCompanyName} - No.");
                }

                // Recursive Algorithm Check
                companyIsRelated = RecursiveMatches(startCompany, parentCompanyName);

                if (companyIsRelated)
                {
                    Console.WriteLine($"Recursive: {parentCompanyName} - {daughterCompanyName} - Yes.");
                }
                else
                {
                    Console.WriteLine($"Recursive: {parentCompanyName} - {daughterCompanyName} - No.");
                }

                Console.WriteLine();
            }
        }

        private static bool IterativeMatches(Company company, string parentCompanyName)
        {
            while(company.Parent != null)
            {
                if (company.Parent.Name.Equals(parentCompanyName))
                {
                    return true;
                }

                company = company.Parent;
            }

            return false;
        }

        private static bool RecursiveMatches(Company company, string parentCompanyName)
        {
            if(company == null || company.Parent == null)
            {
                return false;
            }

            if (company.Parent.Name.Equals(parentCompanyName))
            {
                return true;
            }

            return RecursiveMatches(company.Parent, parentCompanyName);
        }
    }
}
