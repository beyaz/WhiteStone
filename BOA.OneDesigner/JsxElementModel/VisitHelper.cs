using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    public class VisitContext
    {
        #region Public Properties
        public List<object> History       { get; set; }
        public int?         Index         { get; set; }
        public object       Instance      { get; set; }
        public PropertyInfo PropertyInfo  { get; set; }
        public object       PropertyValue { get; set; }
        public object       ValueAtIndex  { get; set; }
        #endregion
    }

    public static class VisitHelper
    {
        #region Public Methods
        public static void ConvertToNewComponent(VisitContext context)
        {
            var input = context.ValueAtIndex as BInput;
            if (input?.IsAccountComponent == true)
            {
                var componentInfo = new ComponentInfo
                {
                    Type = new ComponentType
                    {
                        IsAccountComponent = true
                    },
                    SizeInfo                        = input.SizeInfo,
                    LabelTextInfo                   = LabelInfoHelper.CreateNewLabelInfo("Müşteri No, TCKN, VKN"),
                    ValueBindingPath                = input.ValueBindingPath,
                    ValueChangedOrchestrationMethod = input.ValueChangedOrchestrationMethod,
                    IsDisabledBindingPath           = input.IsDisabledBindingPath,
                    IsVisibleBindingPath            = input.IsVisibleBindingPath
                };

                if (context.Index == null)
                {
                    throw new InvalidOperationException();
                }

                ((IList) context.PropertyValue)[context.Index.Value] = componentInfo;
            }

            var label = context.ValueAtIndex as BLabel;
            if (label != null)
            {
                var componentInfo = new ComponentInfo
                {
                    Type = new ComponentType
                    {
                        IsLabel = true
                    },
                    SizeInfo = label.SizeInfo,
                    TextInto = label.TextInto,
                    IsBold   = label.IsBold
                };

                if (context.Index == null)
                {
                    throw new InvalidOperationException();
                }

                ((IList) context.PropertyValue)[context.Index.Value] = componentInfo;
            }
        }

        public static void VisitAllChildren(object instance, Action<VisitContext> on)
        {
            var visitContext = new VisitContext
            {
                History = new List<object>()
            };

            VisitAllChildren(visitContext, instance, on);
        }
        #endregion

        #region Methods
        static void VisitAllChildren(VisitContext context, object instance, Action<VisitContext> on)
        {
            if (instance == null)
            {
                return;
            }

            if (context.History.Contains(instance))
            {
                return;
            }

            context.History.Add(instance);

            foreach (var propertyInfo in instance.GetType().GetProperties().Where(p => p.CanRead))
            {
                var value = propertyInfo.GetValue(instance);

                context.Instance      = instance;
                context.PropertyInfo  = propertyInfo;
                context.PropertyValue = value;

                context.Index        = null;
                context.ValueAtIndex = null;

                on(context);

                if (value == null
                    || value is string
                    || value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is bool)
                {
                    continue;
                }

                var list = value as IList;

                if (list != null)
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        var obj = list[i];

                        context.Index        = i;
                        context.ValueAtIndex = obj;

                        on(context);

                        VisitAllChildren(context, obj, on);

                        i++;
                    }

                    continue;
                }

                VisitAllChildren(context, value, on);
            }
        }
        #endregion
    }
}