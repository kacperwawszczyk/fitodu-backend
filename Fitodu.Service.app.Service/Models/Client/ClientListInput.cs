using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Service.Models
{
    public class ClientListInput : PaginationInput
    {
        public SortInput Sort { get; set; }
        public string SearchTerm { get; set; }

        // tutaj jakies dodatkowe wlasciwosci jesli potrzebne

        public ClientListInput()
        {
            Sort = new SortInput();
        }
    }
}
