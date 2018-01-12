using System;
using System.Web.UI.WebControls;

namespace WebCode.asp.net.Validation
{
    public partial class TestForValidation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                validationSummary.ValidationGroup = "Submit";
                requireValidator.ValidationGroup = "Submit";


                // btnSave.ValidationGroup = "Submit";//WON'T TRIGGER VALIDATION
                btnSubmit.ValidationGroup = "Submit";

                Operator_Index_Changed(sender, e);
                Type_Index_Changed(sender, e);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            requireValidator.Validate();
            regularExpressionValidator.Validate();
            rangeValidator.Validate();
            TriggerCompareValidation();
            customerValidator.Validate();

            if (Page.IsValid)
            {
                //Only Page is valid then submit     
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // WON'T TRIIGER VALIDATION 
            
        }

        #region customer validation
        protected void ServerValidation(object source, ServerValidateEventArgs args)
        {
            try
            {
                // Test whether the value entered into the text box is even.
                args.IsValid = args.Value.Length <= 5;
            }

            catch (Exception ex)
            {

                args.IsValid = false;

            }

        }

        #endregion

        #region compare validation

        protected void TriggerCompareValidation()
        {
            switch (lstOperator.SelectedIndex)
            {
                case 0:
                    compareValidator.Operator = ValidationCompareOperator.Equal;
                    break;
                case 1:
                    compareValidator.Operator = ValidationCompareOperator.NotEqual;
                    break;
                case 2:
                    compareValidator.Operator = ValidationCompareOperator.GreaterThan;
                    break;
                case 3:
                    compareValidator.Operator = ValidationCompareOperator.GreaterThanEqual;
                    break;
                case 4:
                    compareValidator.Operator = ValidationCompareOperator.LessThan;
                    break;
                case 5:
                    compareValidator.Operator = ValidationCompareOperator.LessThanEqual;
                    break;
                case 6:
                    compareValidator.Operator = ValidationCompareOperator.DataTypeCheck;
                    break;
            }
            switch (lstDataType.SelectedIndex)
            {
                case 0:
                    compareValidator.Type = ValidationDataType.String;
                    break;
                case 1:
                    compareValidator.Type = ValidationDataType.Integer;
                    break;
                case 2:
                    compareValidator.Type = ValidationDataType.Double;
                    break;
                case 3:
                    compareValidator.Type = ValidationDataType.Date;
                    break;
                case 4:
                    compareValidator.Type = ValidationDataType.Currency;
                    break;
            }
            compareValidator.Validate();
        }

        protected void Operator_Index_Changed(object sender, EventArgs e)
        {
            TriggerCompareValidation();
        }

        protected void Type_Index_Changed(object sender, EventArgs e)
        {
            TriggerCompareValidation();
        }
        #endregion


    }
}