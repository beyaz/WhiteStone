using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;
using Ninject;

namespace BOA.EntityGeneration.Generators
{
    public class ContractDataCreator
    {
        #region Public Properties
        [Inject]
        public GeneratorDataCreator GeneratorDataCreator { get; set; }
        #endregion

        #region Public Methods
        public ContractData Create(GeneratorData generatorData)
        {
            var data = JsonHelper.Deserialize<ContractData>(JsonHelper.Serialize(generatorData));

            var interfaces = new List<string>
            {
                Names.ISupportDmlOperationInsert
            };

            if (data.IsSupportUpdate)
            {
                interfaces.Add(Names.ISupportDmlOperationUpdate);
            }

            if (data.IsSupportUpdate)
            {
                interfaces.Add(Names.ISupportDmlOperationDelete);
            }

            if (data.IsSupportGetAll)
            {
                interfaces.Add(Names.ISupportDmlOperationSelectAll);
            }

            if (data.IsSupportSelectByKey)
            {
                interfaces.Add(Names.ISupportDmlOperationSelectByKey);
            }

            if (data.IsSupportSelectByUniqueIndex)
            {
                interfaces.Add(Names.ISupportDmlOperationSelectByUniqueIndex);
            }

            if (data.IsSupportSelectByIndex)
            {
                interfaces.Add(Names.ISupportDmlOperationSelectByIndex);
            }

            data.ContractInterfaces = interfaces;

            return data;
        }
        #endregion
    }
}