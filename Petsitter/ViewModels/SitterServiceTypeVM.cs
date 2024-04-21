using Petsitter.Models;
using System.ComponentModel;
using FoolProof.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Petsitter.ViewModels

{
    public class SitterServiceTypeVM
    {
       
        public int sitterID { get; set; }
        public int serviceName { get; set; }
        


    }
}
