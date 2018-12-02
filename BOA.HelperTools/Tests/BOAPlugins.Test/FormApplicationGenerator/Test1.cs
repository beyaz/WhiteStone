using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOA.Common.Helpers;
using BOAPlugins.FormApplicationGenerator.Templates;
using BOAPlugins.FormApplicationGenerator.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOAPlugins.FormApplicationGenerator
{
    [TestClass]
    public  class ListFormOrchestrationFileTest
    {
        [TestMethod]
        public void TemplateDebug()
        {
            
            var data = new ListFormTsxCodeGeneratorData
            {
                RequestClassLocation  = "BOA.Types.CardGeneral.DebitCard.dll -> BOA.Types.CardGeneral.DebitCard.CourierIncomingFileListFormRequest",
                SearchFields = new List<BField>
                {
                    new BAccountComponent("Data.AccountNumber"),
                    new BInputMask("Data.CardNumber"){ IsCreditCard = true}
                }
            };

            ListFormTsxCodeGenerator.Generate(data);

            

        }
    }
}
