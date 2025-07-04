﻿
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Exoplanet.UI
{
    public enum EditType
    {
        Text,
        Integer,
        Double,
        Date,
        DateTime,
        Password,
        Combobox,
        Switch,
        ComboTreeView,
        ComboCheckedListBox,
        ComboDataGridView,
        FileSelect,
        DirSelect
    }

    public class ComboCheckedListBoxItem
    {
        public bool Checked;
        public string Text;

        public ComboCheckedListBoxItem()
        {
        }

        public ComboCheckedListBoxItem(string text, bool isChecked)
        {
            Checked = isChecked;
            Text = text;
        }
    }

    public class EditInfo
    {
        public string DataPropertyName { get; set; }
        public EditType EditType { get; set; }
        public string Text { get; set; }
        public object Value { get; set; }
        public bool CheckEmpty { get; set; }
        public bool Enabled { get; set; }
        public bool HalfWidth { get; set; }
        public object DataSource { get; set; }
        public string DisplayMember { get; set; }
        public string ValueMember { get; set; }
        public int DecimalPlaces { get; set; } = 2;
    }

    public class UIEditOption
    {
        public readonly List<EditInfo> Infos = new List<EditInfo>();
        public readonly ConcurrentDictionary<string, EditInfo> Dictionary = new ConcurrentDictionary<string, EditInfo>();

        public string Text { get; set; } = "Edit";
        public bool AutoLabelWidth { get; set; }
        public int LabelWidth { get; set; } = 180;
        public int ValueWidth { get; set; } = 320;

        public bool ExistsDataPropertyName(string dataPropertyName)
        {
            return Dictionary.ContainsKey(dataPropertyName);
        }

        public void AddText(string dataPropertyName, string text, string value, bool checkEmpty, bool enabled = true)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.Text,
                Text = text,
                Value = value,
                CheckEmpty = checkEmpty,
                Enabled = enabled
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddFileSelect(string dataPropertyName, string text, string filename, bool checkEmpty, string filter = "", string defaultExt = "", bool enabled = true)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.FileSelect,
                Text = text,
                Value = filename,
                CheckEmpty = checkEmpty,
                Enabled = enabled,
                DisplayMember = filter,
                ValueMember = defaultExt
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddDirSelect(string dataPropertyName, string text, string dirname, bool checkEmpty, string desc = "Please select a folder", bool enabled = true)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.DirSelect,
                Text = text,
                Value = dirname,
                CheckEmpty = checkEmpty,
                Enabled = enabled,
                DisplayMember = desc,
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddPassword(string dataPropertyName, string text, string value, bool checkEmpty, bool enabled = true)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.Password,
                Text = text,
                Value = value,
                CheckEmpty = checkEmpty,
                Enabled = enabled
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddDouble(string dataPropertyName, string text, double value, bool enabled = true, bool halfWidth = true)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.Double,
                Text = text,
                Value = value,
                Enabled = enabled,
                HalfWidth = halfWidth
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddDouble(string dataPropertyName, string text, double value, int decimalPlaces, bool enabled = true, bool halfWidth = true)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.Double,
                Text = text,
                Value = value,
                Enabled = enabled,
                HalfWidth = halfWidth,
                DecimalPlaces = decimalPlaces
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddInteger(string dataPropertyName, string text, int value, bool enabled = true, bool halfWidth = true)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.Integer,
                Text = text,
                Value = value,
                Enabled = enabled,
                HalfWidth = halfWidth
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddDate(string dataPropertyName, string text, DateTime value, bool enabled = true, bool halfWidth = true)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.Date,
                Text = text,
                Value = value,
                Enabled = enabled,
                HalfWidth = halfWidth
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddDateTime(string dataPropertyName, string text, DateTime value, bool enabled = true, bool halfWidth = false)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.DateTime,
                Text = text,
                Value = value,
                Enabled = enabled,
                HalfWidth = halfWidth
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddSwitch(string dataPropertyName, string text, bool value, bool enabled = true)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.Switch,
                Text = text,
                Value = value,
                Enabled = enabled,
                HalfWidth = true
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddSwitch(string dataPropertyName, string text, bool value, string activeText, string inActiveText, bool enabled = true)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.Switch,
                Text = text,
                Value = value,
                Enabled = enabled,
                DataSource = new string[2] { activeText, inActiveText },
                HalfWidth = true
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddCombobox(string dataPropertyName, string text, IList dataSource, string displayMember,
            string valueMember, object value, bool enabled = true, bool halfWidth = false)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.Combobox,
                Text = text,
                Value = value,
                Enabled = enabled,
                HalfWidth = halfWidth,
                DataSource = dataSource,
                DisplayMember = displayMember,
                ValueMember = valueMember
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddCombobox(string dataPropertyName, string text, string[] items, int selectedIndex = -1, bool enabled = true, bool halfWidth = false)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.Combobox,
                Text = text,
                Value = selectedIndex,
                Enabled = enabled,
                HalfWidth = halfWidth,
                DataSource = items
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddComboTreeView(string dataPropertyName, string text, TreeNode[] nodes, TreeNode value, bool enabled = true, bool halfWidth = false)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.ComboTreeView,
                Text = text,
                Value = value,
                Enabled = enabled,
                HalfWidth = halfWidth,
                DataSource = nodes
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddComboCheckedListBox(string dataPropertyName, string text, ComboCheckedListBoxItem[] nodes, string value, bool enabled = true, bool halfWidth = false)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.ComboCheckedListBox,
                Text = text,
                Value = value,
                Enabled = enabled,
                HalfWidth = halfWidth,
                DataSource = nodes
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }

        public void AddComboDataGridView(string dataPropertyName, string text, DataTable dataTable, string displayMember,
            string valueMember, int selectedIndex = -1, bool enabled = true, bool halfWidth = false)
        {
            if (Dictionary.ContainsKey(dataPropertyName))
                throw new DuplicateNameException(dataPropertyName + ": already exists");

            EditInfo info = new EditInfo()
            {
                DataPropertyName = dataPropertyName,
                EditType = EditType.ComboDataGridView,
                Text = text,
                Value = selectedIndex,
                DisplayMember = displayMember,
                ValueMember = valueMember,
                Enabled = enabled,
                HalfWidth = halfWidth,
                DataSource = dataTable
            };

            Infos.Add(info);
            Dictionary.TryAdd(info.DataPropertyName, info);
        }
    }

}
