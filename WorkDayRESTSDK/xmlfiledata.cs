using System;
using System.Collections.Generic;
using System.Text;

namespace WorkDayRESTSDK
{
    class xmlfiledata
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:com.workday.report/CR_Pay_Active_Deductions_Outbound")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:com.workday.report/CR_Pay_Active_Deductions_Outbound", IsNullable = false)]
        public partial class Report_Data
        {

            private Report_DataReport_Entry[] report_EntryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Report_Entry")]
            public Report_DataReport_Entry[] Report_Entry
            {
                get
                {
                    return this.report_EntryField;
                }
                set
                {
                    this.report_EntryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:com.workday.report/CR_Pay_Active_Deductions_Outbound")]
        public partial class Report_DataReport_Entry
        {

            private object[] itemsField;

            private ItemsChoiceType[] itemsElementNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Active_Status", typeof(byte))]
            [System.Xml.Serialization.XmlElementAttribute("Email", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("Employee_ID", typeof(uint))]
            [System.Xml.Serialization.XmlElementAttribute("First_Name", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("Hire_Date", typeof(System.DateTime), DataType = "date")]
            [System.Xml.Serialization.XmlElementAttribute("Last_Name", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("Location_ID", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("Location_Name", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("On_Leave", typeof(byte))]
            [System.Xml.Serialization.XmlElementAttribute("Pay_Rate_Type", typeof(Report_DataReport_EntryPay_Rate_Type))]
            [System.Xml.Serialization.XmlElementAttribute("Phone", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("Position", typeof(Report_DataReport_EntryPosition))]
            [System.Xml.Serialization.XmlElementAttribute("Terminated", typeof(byte))]
            [System.Xml.Serialization.XmlElementAttribute("Termination_Date", typeof(System.DateTime), DataType = "date")]
            [System.Xml.Serialization.XmlElementAttribute("Total_Base_Pay_-_Amount", typeof(decimal))]
            [System.Xml.Serialization.XmlElementAttribute("Total_Base_Pay_-_Frequency", typeof(Report_DataReport_EntryTotal_Base_Pay__Frequency))]
            [System.Xml.Serialization.XmlElementAttribute("Total_Base_Pay_Annualized_-_Amount", typeof(decimal))]
            [System.Xml.Serialization.XmlElementAttribute("Work_State", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("primaryWorkEmail", typeof(string))]
            [System.Xml.Serialization.XmlElementAttribute("primaryWorkPhone", typeof(string))]
            [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
            public object[] Items
            {
                get
                {
                    return this.itemsField;
                }
                set
                {
                    this.itemsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public ItemsChoiceType[] ItemsElementName
            {
                get
                {
                    return this.itemsElementNameField;
                }
                set
                {
                    this.itemsElementNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:com.workday.report/CR_Pay_Active_Deductions_Outbound")]
        public partial class Report_DataReport_EntryPay_Rate_Type
        {

            private Report_DataReport_EntryPay_Rate_TypeID[] idField;

            private string descriptorField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ID")]
            public Report_DataReport_EntryPay_Rate_TypeID[] ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
            public string Descriptor
            {
                get
                {
                    return this.descriptorField;
                }
                set
                {
                    this.descriptorField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:com.workday.report/CR_Pay_Active_Deductions_Outbound")]
        public partial class Report_DataReport_EntryPay_Rate_TypeID
        {

            private string typeField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
            public string type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:com.workday.report/CR_Pay_Active_Deductions_Outbound")]
        public partial class Report_DataReport_EntryPosition
        {

            private Report_DataReport_EntryPositionID idField;

            private string descriptorField;

            /// <remarks/>
            public Report_DataReport_EntryPositionID ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
            public string Descriptor
            {
                get
                {
                    return this.descriptorField;
                }
                set
                {
                    this.descriptorField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:com.workday.report/CR_Pay_Active_Deductions_Outbound")]
        public partial class Report_DataReport_EntryPositionID
        {

            private string typeField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
            public string type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:com.workday.report/CR_Pay_Active_Deductions_Outbound")]
        public partial class Report_DataReport_EntryTotal_Base_Pay__Frequency
        {

            private Report_DataReport_EntryTotal_Base_Pay__FrequencyID[] idField;

            private string descriptorField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ID")]
            public Report_DataReport_EntryTotal_Base_Pay__FrequencyID[] ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
            public string Descriptor
            {
                get
                {
                    return this.descriptorField;
                }
                set
                {
                    this.descriptorField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:com.workday.report/CR_Pay_Active_Deductions_Outbound")]
        public partial class Report_DataReport_EntryTotal_Base_Pay__FrequencyID
        {

            private string typeField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
            public string type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:com.workday.report/CR_Pay_Active_Deductions_Outbound", IncludeInSchema = false)]
        public enum ItemsChoiceType
        {

            /// <remarks/>
            Active_Status,

            /// <remarks/>
            Email,

            /// <remarks/>
            Employee_ID,

            /// <remarks/>
            First_Name,

            /// <remarks/>
            Hire_Date,

            /// <remarks/>
            Last_Name,

            /// <remarks/>
            Location_ID,

            /// <remarks/>
            Location_Name,

            /// <remarks/>
            On_Leave,

            /// <remarks/>
            Pay_Rate_Type,

            /// <remarks/>
            Phone,

            /// <remarks/>
            Position,

            /// <remarks/>
            Terminated,

            /// <remarks/>
            Termination_Date,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("Total_Base_Pay_-_Amount")]
            Total_Base_Pay__Amount,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("Total_Base_Pay_-_Frequency")]
            Total_Base_Pay__Frequency,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("Total_Base_Pay_Annualized_-_Amount")]
            Total_Base_Pay_Annualized__Amount,

            /// <remarks/>
            Work_State,

            /// <remarks/>
            primaryWorkEmail,

            /// <remarks/>
            primaryWorkPhone,
        }


    }
}
