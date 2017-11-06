using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Songhay.DataAccess.Extensions;
using Songhay.DataAccess.Models;
using Songhay.DataAccess.TextTemplating;
using Songhay.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Songhay.DataAccess.Tests
{
    [TestClass]
    public class OracleTableMetadataTest
    {
        /// <summary>
        /// Gets or sets the test context.
        /// </summary>
        /// <value>The test context.</value>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Initializes the test.
        /// </summary>
        [TestInitialize]
        public void InitializeTest()
        {
            this.TestContext.RemovePreviousTestResults();
        }

        [TestCategory("Integration")]
        [TestMethod]
        [TestProperty("csFileTemplate", @"OracleTableMetadata\{tableName}.cs")]
        [TestProperty("metaJsonFileTemplate", @"OracleTableMetadata\{tableName}.json")]
        [TestProperty("tableName", "OPS_PARTY")]
        public void ShouldGenerateClass()
        {
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 2);

            #region test properties:

            var csFileTemplate = this.TestContext.Properties["csFileTemplate"].ToString();
            var metaJsonFileTemplate = this.TestContext.Properties["metaJsonFileTemplate"].ToString();

            var tableName = this.TestContext.Properties["tableName"].ToString();

            #endregion

            var csFile = csFileTemplate.Replace("{tableName}", tableName.ToCamelCaseFromUnderscores());
            csFile = Path.Combine(projectsFolder, this.GetType().Namespace, csFile);

            var outputJsonFile = metaJsonFileTemplate.Replace("{tableName}", tableName);
            outputJsonFile = Path.Combine(projectsFolder, this.GetType().Namespace, outputJsonFile);
            this.TestContext.ShouldFindFile(outputJsonFile);

            var metadata = JsonConvert.DeserializeObject<IEnumerable<OracleTableMetadata>>(File.ReadAllText(outputJsonFile));
            Assert.IsTrue(metadata.Any(), "The expected table metadata is not here.");

            //var t4 = new OracleEntityGenerator(metadata);
            //var cs = t4.TransformText();
            //File.WriteAllText(csFile, cs);
            //this.TestContext.ShouldFindFile(csFile);
        }
    }
}
