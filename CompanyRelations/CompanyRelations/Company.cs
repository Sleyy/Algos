using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CompanyRelations
{
    public class Company
    {
        public Company()
        {
            this.Parents = new Collection<Company>();
        }

        public string Name { get; set; }
        public ICollection<Company> Parents { get; set; }
        public bool Checked { get; set; }
    }
}
