using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            }.AsEnumerable();

            // Daughter - Parent Mappings
            var companyRelations = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("A", "B" ),
                new KeyValuePair<string, string>("B", "C" ),
                new KeyValuePair<string, string>("C", "D" ),
                //new KeyValuePair<string, string>("C", "E" ),
                new KeyValuePair<string, string>("E", "F" ),
            }.AsEnumerable();

            // Assign each company it's parent from the list of companies
            MapCompanies(companies, companyRelations);

            // Company list to check if related
            // Company - relatedCompany
            var companiesToCheck = new List<KeyValuePair<string, string>>()
            {
                // Direct Cases
                new KeyValuePair<string,string>("A", "B" ),
                new KeyValuePair<string,string>("B", "C" ),
                new KeyValuePair<string,string>("C", "D" ),
                new KeyValuePair<string,string>("E", "F" ),
                // Indirect Cases
                new KeyValuePair<string,string>("A", "E" ),
                new KeyValuePair<string,string>("A", "F" ),
                new KeyValuePair<string,string>("F", "A" ),
                // Backwards relations
                new KeyValuePair<string,string>("D", "A" ),

            }.AsEnumerable();

            CheckForMatches(companies, companiesToCheck);
        }

        private static void MapCompanies(IEnumerable<Company> companies, IEnumerable<KeyValuePair<string, string>> companyRelations)
        {
            Company daughterCompany;
            Company parentCompany;
            foreach (var company in companyRelations)
            {
                daughterCompany = companies.FirstOrDefault(c => c.Name.Equals(company.Key));
                parentCompany = companies.FirstOrDefault(c => c.Name.Equals(company.Value));

                // Both companies Exist
                if (daughterCompany == null || parentCompany == null)
                {
                    return;
                }

                daughterCompany.Parents.Add(parentCompany);
            }
        }

        private static void CheckForMatches(IEnumerable<Company> companies, IEnumerable<KeyValuePair<string, string>> companiesToCheck)
        {
            string daughterCompanyName;
            string relatedCompanyName;

            bool companiesAreRelatedIteratively = false;
           

            foreach (var company in companiesToCheck)
            {
                daughterCompanyName = company.Key;
                relatedCompanyName = company.Value;

                var startCompany = companies.FirstOrDefault(c => c.Name.Equals(daughterCompanyName));
                var relatedCompany = companies.FirstOrDefault(c => c.Name.Equals(relatedCompanyName));

                if (startCompany == null || relatedCompany == null)
                {
                    Console.WriteLine($"Companies Not Found!");
                    continue;
                }


                Console.WriteLine($"Checking {daughterCompanyName} - {relatedCompanyName} for relations.");

                // Iterative Algorithm Check

                companiesAreRelatedIteratively = IterativeMatches(startCompany, relatedCompany);
                // Double check for backwards relations
                if (!companiesAreRelatedIteratively)
                {
                    companiesAreRelatedIteratively = IterativeMatches(relatedCompany, startCompany);
                }
                Console.WriteLine($"Iterative {(companiesAreRelatedIteratively ? "Yes" : "No")}");


                // Recursive Algorithm Check
                bool companiesAreRelatedRecursively = false;
                RecursiveMatches(startCompany, relatedCompany, ref companiesAreRelatedRecursively);
                // Reset checked flag
                companies.Select(c => { c.Checked = false; return c; }).ToList();
                // Double check for backwards relations
                if (!companiesAreRelatedRecursively)
                {
                    RecursiveMatches(relatedCompany, startCompany, ref companiesAreRelatedRecursively);
                    companies.Select(c => { c.Checked = false; return c; }).ToList();
                }
                Console.WriteLine($"Recursive {(companiesAreRelatedRecursively ? "Yes" : "No")}");

                Console.WriteLine();
            }
        }

        private static bool IterativeMatches(Company startCompany, Company relatedCompany)
        {
            // Using HashSet to avoid endless cycles of iterations
            var parentCompanies = new HashSet<Company>();

            parentCompanies.UnionWith(startCompany.Parents);

            int parentCompaniesCount;
            var currentParentCompany = new Company();
            var alreadyCheckedParents = new Collection<Company>();

            while (parentCompanies.Count > 0)
            {
                // Check for direct relation
                if (parentCompanies.Contains(relatedCompany))
                {
                    return true;
                }

                // Replace parents by their parents
                parentCompaniesCount = parentCompanies.Count;
                for (int i = 0; i < parentCompaniesCount; i++)
                {
                    currentParentCompany = parentCompanies.ElementAt(i);

                    if (currentParentCompany.Parents.Count > 0)
                    {
                        parentCompanies.UnionWith(currentParentCompany.Parents);
                    }

                    // Store the already checked parents
                    alreadyCheckedParents.Add(currentParentCompany);
                }

                // Remove the already checked parents
                parentCompanies.RemoveWhere(pc => alreadyCheckedParents.Contains(pc));
                alreadyCheckedParents.Clear();
            }

            return false;
        }

        private static void RecursiveMatches(Company startCompany, Company relatedCompany, ref bool companiesAreRelated)
        {
            // Check for Direct Relation
            if (startCompany.Parents.Contains(relatedCompany))
            {
                companiesAreRelated = true;
                // End recursion on result found
                return;
            }

            startCompany.Checked = true;

            // Check each parent if their parent has matches
            foreach (var pc in startCompany.Parents.Where(p => !p.Checked))
            {
                RecursiveMatches(pc, relatedCompany, ref companiesAreRelated);
            }
        }
    }
}
