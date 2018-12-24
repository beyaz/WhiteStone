using System;
using System.Collections.Generic;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class BCard : Container
    {
        #region Public Properties
        public List<BField> Fields { get; set; } = new List<BField>();

        public string       Title  { get; set; }
        #endregion

        #region Public Methods
        public override void InsertField(int index, BField field)
        {
            field.Container = this;
            Fields.Insert(index, field);
        }

        public override void RemoveField(BField field)
        {
            if (field.Container != this)
            {
                throw new ArgumentException();
            }

            Fields?.Remove(field);

            field.Container = null;
        }
        #endregion
    }

    [Serializable]
    public abstract class Container
    {
        #region Public Methods
        public abstract void InsertField(int    index, BField field);
        public abstract void RemoveField(BField field);
        #endregion
    }
}