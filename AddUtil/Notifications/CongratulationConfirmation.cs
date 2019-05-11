using AddUtil.Models;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddUtil.Notifications
{
    public class CongratulationConfirmation : Confirmation
    {
        public CongratulationModel Congratulation { get; set; }
    }
}
