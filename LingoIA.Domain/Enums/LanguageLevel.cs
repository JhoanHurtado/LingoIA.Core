using System.ComponentModel;

namespace LingoIA.Domain.Enums
{
    public enum LanguageLevel
    {
        [Description("Beginner - Puede comprender y usar expresiones básicas.")]
        Beginner,

        [Description("Elementary - Se comunica en situaciones cotidianas con frases cortas.")]
        Elementary,

        [Description("Pre-Intermediate - Puede interactuar en conversaciones breves sobre temas familiares.")]
        PreIntermediate,

        [Description("Intermediate - Mantiene conversaciones sobre temas generales.")]
        Intermediate,

        [Description("Upper-Intermediate - Puede conversar con fluidez en la mayoría de las situaciones.")]
        UpperIntermediate,

        [Description("Advanced - Se expresa con precisión y naturalidad en casi cualquier contexto.")]
        Advanced,

        [Description("Fluent - Habla con fluidez similar a un nativo.")]
        Fluent
    }
}
