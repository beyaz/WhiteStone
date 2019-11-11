﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    using System.Linq;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class DeleteMethodTemplate : DeleteMethodTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("/// <summary>\r\n///     Deletes only one record from \'");
            
            #line 8 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableInfo.SchemaName));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 8 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableInfo.TableName));
            
            #line default
            #line hidden
            this.Write("\' by using \'");
            
            #line 8 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(string.Join(" and ", deleteInfo.SqlParameters.Select(x => x.ColumnName.AsMethodParameter()))));
            
            #line default
            #line hidden
            this.Write("\'\r\n/// </summary>\r\npublic GenericResponse<int> Delete(");
            
            #line 10 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(string.Join(", ", deleteInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"))));
            
            #line default
            #line hidden
            this.Write(")\r\n{\r\n\tvar returnObject = InitializeGenericResponse<int>(\"");
            
            #line 12 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(businessClassNamespace));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 12 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(className));
            
            #line default
            #line hidden
            this.Write(".Delete\");\r\n\r\n\tconst string sql = @\"");
            
            #line 14 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(deleteInfo.Sql));
            
            #line default
            #line hidden
            this.Write("\";\r\n\t\r\n\tvar command = this.CreateCommand(sql);\r\n\t");
            
            #line 17 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
 if (deleteInfo.SqlParameters.Any()){ 
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 19 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
 foreach (var columnInfo in deleteInfo.SqlParameters) {
            
            #line default
            #line hidden
            this.Write("\tDBLayer.AddInParameter(command, \"@");
            
            #line 20 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(columnInfo.ColumnName));
            
            #line default
            #line hidden
            this.Write("\", SqlDbType.");
            
            #line 20 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(columnInfo.SqlDbType));
            
            #line default
            #line hidden
            this.Write(", ");
            
            #line 20 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(columnInfo.ColumnName.AsMethodParameter()));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 21 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("\t");
            
            #line 22 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write(@"
	var response = DBLayer.ExecuteNonQuery(command);
    if (!response.Success)
    {
        returnObject.Results.AddRange(response.Results);
        return returnObject;
    }

    returnObject.Value = response.Value;

    return returnObject;
}");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "D:\github\WhiteStone\BOA.EntityGeneration\BOACardDatabaseSchemaToDllExporting\ClassWriters\DeleteMethodTemplate.tt"

private global::BOA.EntityGeneration.ScriptModel.DeleteInfo _deleteInfoField;

/// <summary>
/// Access the deleteInfo parameter of the template.
/// </summary>
private global::BOA.EntityGeneration.ScriptModel.DeleteInfo deleteInfo
{
    get
    {
        return this._deleteInfoField;
    }
}

private global::BOA.EntityGeneration.DbModel.Interfaces.ITableInfo _tableInfoField;

/// <summary>
/// Access the tableInfo parameter of the template.
/// </summary>
private global::BOA.EntityGeneration.DbModel.Interfaces.ITableInfo tableInfo
{
    get
    {
        return this._tableInfoField;
    }
}

private string _businessClassNamespaceField;

/// <summary>
/// Access the businessClassNamespace parameter of the template.
/// </summary>
private string businessClassNamespace
{
    get
    {
        return this._businessClassNamespaceField;
    }
}

private string _classNameField;

/// <summary>
/// Access the className parameter of the template.
/// </summary>
private string className
{
    get
    {
        return this._classNameField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool deleteInfoValueAcquired = false;
if (this.Session.ContainsKey("deleteInfo"))
{
    this._deleteInfoField = ((global::BOA.EntityGeneration.ScriptModel.DeleteInfo)(this.Session["deleteInfo"]));
    deleteInfoValueAcquired = true;
}
if ((deleteInfoValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("deleteInfo");
    if ((data != null))
    {
        this._deleteInfoField = ((global::BOA.EntityGeneration.ScriptModel.DeleteInfo)(data));
    }
}
bool tableInfoValueAcquired = false;
if (this.Session.ContainsKey("tableInfo"))
{
    this._tableInfoField = ((global::BOA.EntityGeneration.DbModel.Interfaces.ITableInfo)(this.Session["tableInfo"]));
    tableInfoValueAcquired = true;
}
if ((tableInfoValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("tableInfo");
    if ((data != null))
    {
        this._tableInfoField = ((global::BOA.EntityGeneration.DbModel.Interfaces.ITableInfo)(data));
    }
}
bool businessClassNamespaceValueAcquired = false;
if (this.Session.ContainsKey("businessClassNamespace"))
{
    this._businessClassNamespaceField = ((string)(this.Session["businessClassNamespace"]));
    businessClassNamespaceValueAcquired = true;
}
if ((businessClassNamespaceValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("businessClassNamespace");
    if ((data != null))
    {
        this._businessClassNamespaceField = ((string)(data));
    }
}
bool classNameValueAcquired = false;
if (this.Session.ContainsKey("className"))
{
    this._classNameField = ((string)(this.Session["className"]));
    classNameValueAcquired = true;
}
if ((classNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("className");
    if ((data != null))
    {
        this._classNameField = ((string)(data));
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
    public class DeleteMethodTemplateBase
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