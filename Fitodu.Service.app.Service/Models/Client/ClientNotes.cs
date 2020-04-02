using Fitodu.Service.Models.PrivateNote;
using Fitodu.Service.Models.PublicNote;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Service.Models.Client
{
    public class ClientNotes
    {
        public PublicNoteOutput PublicNote { get; set; }
        public PrivateNoteOutput PrivateNote { get; set; }
    }
}
