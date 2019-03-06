﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;


using DotNetKit.Misc.Disposables;
using DotNetKit.Windows.Media;

namespace DotNetKit.Windows.Controls
{
    /// <summary>
    /// AutoCompleteComboBox.xaml の相互作用ロジック
    /// </summary>
    public partial class AutoCompleteComboBox 
    {
        
        readonly SerialDisposable disposable = new SerialDisposable();

        TextBox editableTextBoxCache;
        public TextBox EditableTextBox
        {
            get
            {
                if (editableTextBoxCache == null)
                {
                    
                    const string name = "PART_EditableTextBox";
                    editableTextBoxCache = (TextBox)VisualTreeModule.FindChild(this, name);
                }
                return editableTextBoxCache;
            }
        }

        /// <summary>
        /// Gets text to match with the query from an item.
        /// Never null.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string TextFromItem(object item)
        {
            if (item == null) return "";

            var d = new DependencyVariable<string>();
            d.SetBinding(item, TextSearch.GetTextPath(this));
            return d.Value ?? "";
        }

        #region Setting
        static readonly DependencyProperty settingProperty =
            DependencyProperty.Register(
                "Setting",
                typeof(AutoCompleteComboBoxSetting),
                typeof(AutoCompleteComboBox)
            );

        public static DependencyProperty SettingProperty
        {
            get { return settingProperty; }
        }

        public AutoCompleteComboBoxSetting Setting
        {
            get { return (AutoCompleteComboBoxSetting)GetValue(SettingProperty); }
            set { SetValue(SettingProperty, value); }
        }

        AutoCompleteComboBoxSetting SettingOrDefault
        {
            get { return Setting ?? AutoCompleteComboBoxSetting.Default; }
        }
        #endregion

        #region OnTextChanged
        long revisionId;
        string previousText;

        struct TextBoxStatePreserver
            : IDisposable
        {
            readonly TextBox textBox;
            readonly int selectionStart;
            readonly int selectionLength;
            readonly string text;

            public void Dispose()
            {
                textBox.Text = text;
                textBox.Select(selectionStart, selectionLength);
            }

            public TextBoxStatePreserver(TextBox textBox)
            {
                this.textBox = textBox;
                selectionStart = textBox.SelectionStart;
                selectionLength = textBox.SelectionLength;
                text = textBox.Text;
            }
        }

        static int CountWithMax<X>(IEnumerable<X> xs, Func<X, bool> predicate, int maxCount)
        {
            var count = 0;
            foreach (var x in xs)
            {
                if (predicate(x))
                {
                    count++;
                    if (count > maxCount) return count;
                }
            }
            return count;
        }

        void Unselect()
        {
            var textBox = EditableTextBox;
            textBox.Select(textBox.SelectionStart + textBox.SelectionLength, 0);
        }

        void UpdateFilter(Func<object, bool> filter)
        {
            using (new TextBoxStatePreserver(EditableTextBox))
            using (Items.DeferRefresh())
            {
                // Can empty the text box. I don't why.
                Items.Filter = item => filter(item);
            }
        }

        void OpenDropDown(Func<object, bool> filter)
        {
            UpdateFilter(filter);
            IsDropDownOpen = true;
            Unselect();
        }

        void OpenDropDown()
        {
            var setting = SettingOrDefault;
            var filter = setting.GetFilter(Text, TextFromItem);
            OpenDropDown(filter);
        }

        void UpdateSuggestionList()
        {
            var text = Text;

            if (text == previousText) return;
            previousText = text;

            if (string.IsNullOrEmpty(text))
            {
                IsDropDownOpen = false;
                SelectedItem = null;

                using (Items.DeferRefresh())
                {
                    Items.Filter = null;
                }
            }
            else if (SelectedItem != null && TextFromItem(SelectedItem) == text)
            {
                // It seems the user selected an item.
                // Do nothing.
            }
            else
            {
                using (new TextBoxStatePreserver(EditableTextBox))
                {
                    SelectedItem = null;
                }

                var setting = SettingOrDefault;
                var filter = setting.GetFilter(text, TextFromItem);
                var maxCount = setting.MaxSuggestionCount;
                var count = CountWithMax(ItemsSource.Cast<object>(), filter, maxCount);

                if (count > maxCount) return;

                OpenDropDown(filter);
            }
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChanged();
        }
        void OnTextChanged()
        {
            var id      = unchecked(++revisionId);
            var setting = SettingOrDefault;

            if (setting.Delay <= TimeSpan.Zero)
            {
                UpdateSuggestionList();
                return;
            }

            disposable.Content =
                new Timer(
                          state =>
                          {
                              Dispatcher.InvokeAsync(() =>
                              {
                                  if (revisionId != id) return;
                                  UpdateSuggestionList();
                              });
                          },
                          null,
                          setting.Delay,
                          Timeout.InfiniteTimeSpan
                         );
        }
        #endregion

        void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.Space)
            //{
            //    OpenDropDown();
            //    e.Handled = true;
            //}

            OnTextChanged();

        }

        public AutoCompleteComboBox()
        {
            InitializeComponent();

            // AddHandler(TextBoxBase.TextChangedEvent,new TextChangedEventHandler(OnTextChanged));
        }
    }
}
