﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.ClassWriters
{
    using System.Linq;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class BusinessClassTemplate : BusinessClassTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("/// <summary>\r\n///     Data access part of \'");
            
            #line 6 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.Name));
            
            #line default
            #line hidden
            this.Write("\' sql.\r\n/// </summary>\r\npublic sealed class ");
            
            #line 8 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.BusinessClassName));
            
            #line default
            #line hidden
            this.Write(" : ObjectHelper\r\n{\r\n\t/// <summary>\r\n\t///     Data access part of \'");
            
            #line 11 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.Name));
            
            #line default
            #line hidden
            this.Write("\' sql.\r\n\t/// </summary>\r\n\tpublic ");
            
            #line 13 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.BusinessClassName));
            
            #line default
            #line hidden
            this.Write("(ExecutionDataContext context) : base(context) {}\r\n\r\n\t/// <summary>\r\n\t///     Dat" +
                    "a access part of \'");
            
            #line 16 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.Name));
            
            #line default
            #line hidden
            this.Write("\' sql.\r\n\t/// </summary>\r\n");
            
            #line 18 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
if (data.SqlResultIsCollection)
            
            #line default
            #line hidden
            
            #line 19 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
{
            
            #line default
            #line hidden
            this.Write("\tpublic GenericResponse<List<");
            
            #line 20 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.ResultContractName));
            
            #line default
            #line hidden
            this.Write(">> Execute(");
            
            #line 20 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.ParameterContractName));
            
            #line default
            #line hidden
            this.Write(" request)\r\n\t{\r\n\t\tvar returnObject = InitializeGenericResponse<List<");
            
            #line 22 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.ResultContractName));
            
            #line default
            #line hidden
            this.Write(">>(\"");
            
            #line 22 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(key));
            
            #line default
            #line hidden
            this.Write("\");\r\n\t\r\n\t\tconst string sql = @\"\r\n\t\t\t");
            
            #line 25 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.Sql));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\";\r\n\t\r\n\t\tvar command = DBLayer.GetDBCommand(Databases.BOACard, sql, null, Com" +
                    "mandType.Text);\r\n\t\t");
            
            #line 29 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
if (data.Parameters.Any()){
            
            #line default
            #line hidden
            this.Write("\t\t\r\n");
            
            #line 31 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
foreach (var item in data.Parameters){
            
            #line default
            #line hidden
            this.Write("\t\tDBLayer.AddInParameter(command, \"@");
            
            #line 32 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Name));
            
            #line default
            #line hidden
            this.Write("\", SqlDbType.");
            
            #line 32 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.SqlDbTypeName));
            
            #line default
            #line hidden
            this.Write(", request.");
            
            #line 32 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ValueAccessPathForAddInParameter));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 33 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("\t\t");
            
            #line 34 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("\t\r\n\t\tvar response = DBLayer.ExecuteReader(command);\r\n\t\tif (!response.Success)\r\n\t\t" +
                    "{\r\n\t\t    returnObject.Results.AddRange(response.Results);\r\n\t\t    return returnOb" +
                    "ject;\r\n\t\t}\t\t\r\n\t\tvar reader = response.Value;\r\n\t\r\n\t\tvar listOfDataContract = new " +
                    "List<");
            
            #line 44 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.ResultContractName));
            
            #line default
            #line hidden
            this.Write(">();\t\t\r\n\t\t\r\n\t\twhile (reader.Read())\r\n\t\t{\r\n\t\t\tvar dataContract = new ");
            
            #line 48 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.ResultContractName));
            
            #line default
            #line hidden
            this.Write("();\r\n\r\n\t\t\tReadContract(reader, dataContract);\r\n\r\n\t\t\tlistOfDataContract.Add(dataCo" +
                    "ntract);\r\n\t\t}\t\t\r\n\t\t\r\n\t\treader.Close();\r\n\r\n\t\treturnObject.Value = listOfDataContr" +
                    "act;\r\n\r\n\t\treturn returnObject;\r\n\t}\r\n");
            
            #line 61 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
}
            
            #line default
            #line hidden
            
            #line 62 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
else
            
            #line default
            #line hidden
            
            #line 63 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
{
            
            #line default
            #line hidden
            this.Write("\tpublic GenericResponse<");
            
            #line 64 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.ResultContractName));
            
            #line default
            #line hidden
            this.Write("> Execute(");
            
            #line 64 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.ParameterContractName));
            
            #line default
            #line hidden
            this.Write(" request)\r\n\t{\r\n\t\tvar returnObject = InitializeGenericResponse<");
            
            #line 66 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.ResultContractName));
            
            #line default
            #line hidden
            this.Write(">(\"");
            
            #line 66 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(key));
            
            #line default
            #line hidden
            this.Write("\");\r\n\t\r\n\t\tconst string sql = @\"\r\n\t\t\t");
            
            #line 69 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.Sql));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\";\r\n\t\r\n\t\tvar command = DBLayer.GetDBCommand(Databases.BOACard, sql, null, Com" +
                    "mandType.Text);\r\n\t\t");
            
            #line 73 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
if (data.Parameters.Any())
            
            #line default
            #line hidden
            this.Write("\t\t");
            
            #line 74 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
{
            
            #line default
            #line hidden
            this.Write("            \r\n");
            
            #line 76 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
foreach (var item in data.Parameters){
            
            #line default
            #line hidden
            this.Write("\t\tDBLayer.AddInParameter(command, \"@");
            
            #line 77 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Name));
            
            #line default
            #line hidden
            this.Write("\", SqlDbType.");
            
            #line 77 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.SqlDbTypeName));
            
            #line default
            #line hidden
            this.Write(", request.");
            
            #line 77 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ValueAccessPathForAddInParameter));
            
            #line default
            #line hidden
            this.Write(");\r\n            ");
            
            #line 78 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
}
            
            #line default
            #line hidden
            
            #line 79 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("\t\r\n\t\tvar response = DBLayer.ExecuteReader(command);\r\n\t\tif (!response.Success)\r\n\t\t" +
                    "{\r\n\t\t    returnObject.Results.AddRange(response.Results);\r\n\t\t    return returnOb" +
                    "ject;\r\n\t\t}\t\t\r\n\t\tvar reader = response.Value;\r\n\t\r\n\t\t");
            
            #line 89 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.ResultContractName));
            
            #line default
            #line hidden
            this.Write(" dataContract = null;\r\n\t\t\r\n\t\twhile (reader.Read())\r\n\t\t{\r\n\t\t\tdataContract = new ");
            
            #line 93 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.ResultContractName));
            
            #line default
            #line hidden
            this.Write("();\r\n\r\n\t\t\tReadContract(reader, dataContract);\r\n\r\n\t\t\tbreak;\r\n\t\t}\t\t\r\n\t\t\r\n\t\treader.C" +
                    "lose();\r\n\r\n\t\treturnObject.Value = dataContract;\r\n\r\n\t\treturn returnObject;\r\n\t}\r\n");
            
            #line 106 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("\r\n\t/// <summary>\r\n\t///     Maps reader columns to contract for \'");
            
            #line 109 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.Name));
            
            #line default
            #line hidden
            this.Write("\' sql.\r\n\t/// </summary>\r\n\tstatic void ReadContract(IDataRecord reader, ");
            
            #line 111 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(data.ResultContractName));
            
            #line default
            #line hidden
            this.Write(" contract)\r\n\t{\r\n");
            
            #line 113 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
foreach (var item in data.ResultColumns){
            
            #line default
            #line hidden
            this.Write("\t\tcontract.");
            
            #line 114 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.NameInDotnet));
            
            #line default
            #line hidden
            this.Write(" = SQLDBHelper.");
            
            #line 114 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.SqlReaderMethod));
            
            #line default
            #line hidden
            this.Write("(reader[\"");
            
            #line 114 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Name));
            
            #line default
            #line hidden
            this.Write("\"]);\r\n");
            
            #line 115 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("\t}\r\n}");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardCustomSqlIntoProjectInjection\ClassWriters\BusinessClassTemplate.tt"

private global::BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.CustomSqlInfo _dataField;

/// <summary>
/// Access the data parameter of the template.
/// </summary>
private global::BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.CustomSqlInfo data
{
    get
    {
        return this._dataField;
    }
}

private string _keyField;

/// <summary>
/// Access the key parameter of the template.
/// </summary>
private string key
{
    get
    {
        return this._keyField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool dataValueAcquired = false;
if (this.Session.ContainsKey("data"))
{
    this._dataField = ((global::BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.CustomSqlInfo)(this.Session["data"]));
    dataValueAcquired = true;
}
if ((dataValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("data");
    if ((data != null))
    {
        this._dataField = ((global::BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.CustomSqlInfo)(data));
    }
}
bool keyValueAcquired = false;
if (this.Session.ContainsKey("key"))
{
    this._keyField = ((string)(this.Session["key"]));
    keyValueAcquired = true;
}
if ((keyValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("key");
    if ((data != null))
    {
        this._keyField = ((string)(data));
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public class BusinessClassTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
