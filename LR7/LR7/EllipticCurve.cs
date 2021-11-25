using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LR7
{
    public class EllipticCurve
    {

        public BigInteger p { get; } = BigInteger.Parse("340282366762482138434845932244680310783");


        public BigInteger a { get; } = BigInteger.Parse("340282366762482138434845932244680310780");
        public BigInteger b { get; } = BigInteger.Parse("308990863222245658030922601041482374867");


        public BigIntegerPoint G { get; } = new BigIntegerPoint(
            BigInteger.Parse("29408993404948928992877151431649155974"),
            BigInteger.Parse("275621562871047521857442314737465260675"));


        public BigInteger n { get; } = BigInteger.Parse("340282366762482138443322565580356624661");

    }
}
