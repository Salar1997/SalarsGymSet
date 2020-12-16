using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalarsGymSet.Models
{
    public class PrivateTrainerCard
    {
        [StringLength(16, MinimumLength = 16)]
        [Display(Name = "Private trainer Id")]
        public string PrivateTrainerId { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Private trainer name")]
        public string PrivateTrainerName { get; set; }
    }
}
