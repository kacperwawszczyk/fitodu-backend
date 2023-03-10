using System.ComponentModel;

namespace Fitodu.Core.Enums
{
    public enum PricingPlan
    {
        [Description("Professional")]
        Trial,

        [Description("Basic")]
        BasicMonthly,

        [Description("Basic")]
        BasicYearly,

        [Description("Plus")]
        PlusMonthly,

        [Description("Plus")]
        PlusYearly,

        [Description("Professional")]
        ProfessionalMonthly,

        [Description("Professional")]
        ProfessionalYearly
    }
}
