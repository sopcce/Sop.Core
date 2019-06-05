using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sop.FileServer.Helper.GhostScriptSharp
{
    /// <summary>
    /// Native page sizes
    /// </summary>
    /// <remarks>
    /// Missing 11x17 as enums can't start with a number, and I can't be bothered
    /// to add in logic to handle it - if you need it, do it yourself.
    /// </remarks>
    public enum GhostscriptPageSizes
    {
        UNDEFINED,
        ledger,
        legal,
        letter,
        lettersmall,
        archE,
        archD,
        archC,
        archB,
        archA,
        a0,
        a1,
        a2,
        a3,
        a4,
        a4small,
        a5,
        a6,
        a7,
        a8,
        a9,
        a10,
        isob0,
        isob1,
        isob2,
        isob3,
        isob4,
        isob5,
        isob6,
        c0,
        c1,
        c2,
        c3,
        c4,
        c5,
        c6,
        jisb0,
        jisb1,
        jisb2,
        jisb3,
        jisb4,
        jisb5,
        jisb6,
        b0,
        b1,
        b2,
        b3,
        b4,
        b5,
        flsa,
        flse,
        halfletter
    }
}
