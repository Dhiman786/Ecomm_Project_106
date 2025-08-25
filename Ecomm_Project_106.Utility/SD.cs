using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_Project_106.Utility
{
    public static class SD
    {
        //CoverType SP
        public const string GetCoverTypes = "SP_GetCoverTypes";
        public const string GetCoverType = "SP_GetCoverType";
        public const string CreateCoverType = "SP_CreateCoverType";
        public const string updatecoverType = "SP_UpdateCoverType";
        public const string DeleteCoverType = "SP_DeleteCoverType";

        //Roles
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "EmployeeUser";
        public const string Role_Company = "CompanyUser";
        public const string Role_Individual = "IndividualUser"; 

        //Session
        public const string Ss_CartSessionCount = "CartCountSession";
         

        public static double GetPriceBasedOnQuantity(double quantity,double price,double price50,double price100)
        {
            if (quantity < 50)
                return price;
            else if (quantity < 100)
                return price50;return price100;
        }
        //order Status
        public const string OrderStatusPending = "Pending";
        public const string OrderStatusApproved = "Approved";
        public const string OrderStatusInProgress = "Processing";
        public const string OrderStatusInShipped = "Shipped";
        public const string OrderStatusCancelled = "Cancelled";
        public const string OrderStatusRefunded = "Refunded";

        //Payment Status
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayPayment = "PaymentStatusDelay";
        public const string PaymentStatusDelayRejected = "Rejected";

    }

}
