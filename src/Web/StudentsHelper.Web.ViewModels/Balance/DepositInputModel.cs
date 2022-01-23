namespace StudentsHelper.Web.ViewModels.Balance
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using StudentsHelper.Common;

    public class DepositInputModel
    {
        [Display(Name = "Пари за курсове - лв")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        [Range(
                ValidationConstants.MinDepositAmount,
                ValidationConstants.MaxDepositAmount,
                ErrorMessage = "Стойноста трябва да е поне {1} и не повече от {2} лв.")]
        public int DepositRequestMoneyAmount { get; set; }
    }
}
