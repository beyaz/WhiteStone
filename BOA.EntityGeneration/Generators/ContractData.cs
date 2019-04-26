using System;
using System.Collections.Generic;

namespace BOA.EntityGeneration.Generators
{
    [Serializable]
    public class ContractData:GeneratorData
    {
        public IReadOnlyList<string> ContractInterfaces { get; set; }
    }
}