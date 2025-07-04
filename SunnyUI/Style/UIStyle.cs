﻿
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Exoplanet.UI
{
    public interface IStyleInterface
    {
        UIStyle Style { get; set; }

        string Version { get; }

        string TagString { get; set; }

        void SetStyleColor(UIBaseStyle uiColor);

        //void SetStyle(UIStyle style);
        void SetInheritedStyle(UIStyle style);

        void SetDPIScale();
    }

    /// <summary>
    /// 主题样式
    /// </summary>
    public enum UIStyle
    {
        /// <summary>
        /// 继承的全局主题
        /// </summary>
        [Description("继承的全局主题")]
        Inherited = -1,

        /// <summary>
        /// 自定义
        /// </summary>
        [Description("Custom")]
        Custom = 0,

        /// <summary>
        /// 蓝
        /// </summary>
        [Description("Blue")]
        Blue = 1,

        /// <summary>
        /// 绿
        /// </summary>
        [Description("Green")]
        Green = 2,

        /// <summary>
        /// 橙
        /// </summary>
        [Description("Orange")]
        Orange = 3,

        /// <summary>
        /// 红
        /// </summary>
        [Description("Red")]
        Red = 4,

        /// <summary>
        /// 灰
        /// </summary>
        [Description("Gray")]
        Gray = 5,

        /// <summary>
        /// 紫
        /// </summary>
        [Description("Purple")]
        Purple = 6,

        /// <summary>
        /// LayuiGreen
        /// </summary>
        [Description("LayuiGreen")]
        LayuiGreen = 7,

        /// <summary>
        /// LayuiRed
        /// </summary>
        [Description("LayuiRed")]
        LayuiRed = 8,

        /// <summary>
        /// LayuiOrange
        /// </summary>
        [Description("LayuiOrange")]
        LayuiOrange = 9,

        /// <summary>
        /// 深蓝
        /// </summary>
        [Description("DarkBlue")]
        DarkBlue = 101,

        /// <summary>
        /// 黑
        /// </summary>
        [Description("Black")]
        Black = 102,

        /// <summary>
        /// 多彩的
        /// </summary>
        [Description("Colorful")]
        Colorful = 999
    }

    /// <summary>
    /// 主题颜色
    /// </summary>
    public static class UIColor
    {
        /// <summary>
        /// 蓝
        /// </summary>
        public static readonly Color Blue = Color.FromArgb(80, 160, 255);

        /// <summary>
        /// 绿
        /// </summary>
        public static readonly Color Green = Color.FromArgb(110, 190, 40);

        /// <summary>
        /// 红
        /// </summary>
        public static readonly Color Red = Color.FromArgb(230, 80, 80);

        /// <summary>
        /// 灰
        /// </summary>
        public static readonly Color Gray = Color.FromArgb(140, 140, 140);

        /// <summary>
        /// 橙
        /// </summary>
        public static readonly Color Orange = Color.FromArgb(220, 155, 40);

        /// <summary>
        /// LayuiGreen
        /// </summary>
        public static readonly Color LayuiGreen = Color.FromArgb(0, 150, 136);

        /// <summary>
        /// LayuiRed
        /// </summary>
        public static readonly Color LayuiRed = Color.FromArgb(255, 87, 34);

        /// <summary>
        /// LayuiOrange
        /// </summary>
        public static readonly Color LayuiOrange = Color.FromArgb(255, 184, 0);

        /// <summary>
        /// LayuiCyan
        /// </summary>
        public static readonly Color LayuiCyan = Color.FromArgb(46, 57, 79);

        /// <summary>
        /// LayuiCyan
        /// </summary>
        public static readonly Color LayuiBlue = Color.FromArgb(69, 149, 255);

        /// <summary>
        /// LayuiCyan
        /// </summary>
        public static readonly Color LayuiBlack = Color.FromArgb(52, 55, 66);

        /// <summary>
        /// 深蓝
        /// </summary>
        public static readonly Color DarkBlue = Color.FromArgb(14, 30, 63);

        /// <summary>
        /// 白
        /// </summary>
        public static readonly Color White = Color.White;

        /// <summary>
        /// 黑
        /// </summary>
        public static readonly Color Black = Color.Black;

        /// <summary>
        /// 紫
        /// </summary>
        public static readonly Color Purple = Color.FromArgb(102, 58, 183);

        /// <summary>
        /// 浅紫
        /// </summary>
        public static readonly Color LightPurple = Color.FromArgb(250, 238, 255);

        /// <summary>
        /// 透明
        /// </summary>
        public static readonly Color Transparent = Color.Transparent;

        /// <summary>
        /// 浅蓝
        /// </summary>
        public static readonly Color LightBlue = Color.FromArgb(235, 243, 255);

        /// <summary>
        /// 浅绿
        /// </summary>
        public static readonly Color LightGreen = Color.FromArgb(239, 248, 232);

        /// <summary>
        /// 浅红
        /// </summary>
        public static readonly Color LightRed = Color.FromArgb(251, 238, 238);

        /// <summary>
        /// 浅灰
        /// </summary>
        public static readonly Color LightGray = Color.FromArgb(242, 242, 244);

        /// <summary>
        /// 浅橙
        /// </summary>
        public static readonly Color LightOrange = Color.FromArgb(251, 245, 233);

        /// <summary>
        /// 中蓝
        /// </summary>
        public static readonly Color RegularBlue = Color.FromArgb(216, 233, 255);

        /// <summary>
        /// 中绿
        /// </summary>
        public static readonly Color RegularGreen = Color.FromArgb(224, 242, 210);

        /// <summary>
        /// 中红
        /// </summary>
        public static readonly Color RegularRed = Color.FromArgb(248, 222, 222);

        /// <summary>
        /// 中灰
        /// </summary>
        public static readonly Color RegularGray = Color.FromArgb(230, 230, 232);

        /// <summary>
        /// 中橙
        /// </summary>
        public static readonly Color RegularOrange = Color.FromArgb(247, 234, 210);
    }

    /// <summary>
    /// 不可用颜色
    /// </summary>
    public static class UIDisableColor
    {
        /// <summary>
        /// 填充色
        /// </summary>
        public static readonly Color Fill = UIFontColor.Plain;

        /// <summary>
        /// 字体色
        /// </summary>
        public static readonly Color Fore = UIFontColor.Regular;
    }

    /// <summary>
    /// 字体颜色
    /// </summary>
    public static class UIFontColor
    {
        /// <summary>
        /// 主要颜色
        /// </summary>
        public static readonly Color Primary = Color.FromArgb(48, 48, 48);

        /// <summary>
        /// 正常颜色
        /// </summary>
        public static readonly Color Regular = Color.FromArgb(96, 96, 96);

        /// <summary>
        /// 次要颜色
        /// </summary>
        public static readonly Color Secondary = Color.FromArgb(144, 144, 144);

        /// <summary>
        /// 其他颜色
        /// </summary>
        public static readonly Color Plain = Color.Silver;

        /// <summary>
        /// 白色
        /// </summary>
        public static readonly Color White = Color.FromArgb(248, 248, 248);
    }

    /// <summary>
    /// 边框颜色
    /// </summary>
    public static class UIRectColorColor
    {
        /// <summary>
        /// 主要颜色
        /// </summary>
        public static readonly Color Primary = Color.FromArgb(0xDC, 0xDF, 0xE6);

        /// <summary>
        /// 正常颜色
        /// </summary>
        public static readonly Color Regular = Color.FromArgb(0xE4, 0xE7, 0xED);

        /// <summary>
        /// 次要颜色
        /// </summary>
        public static readonly Color Secondary = Color.FromArgb(0xEB, 0xEE, 0xF5);

        /// <summary>
        /// 其他颜色
        /// </summary>
        public static readonly Color Plain = Color.FromArgb(0xF2, 0xF6, 0xFC);
    }

    /// <summary>
    /// 背景色
    /// </summary>
    public static class UIBackgroundColor
    {
        /// <summary>
        /// 白
        /// </summary>
        public static readonly Color White = UIColor.White;

        /// <summary>
        /// 黑
        /// </summary>
        public static readonly Color Black = UIColor.Black;

        /// <summary>
        /// 透明色
        /// </summary>
        public static readonly Color Transparent = Color.Transparent;
    }

    public static class UIStyleHelper
    {
        /// <summary>
        /// 主题的调色板
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static UIBaseStyle Colors(this UIStyle style)
        {
            return UIStyles.GetStyleColor(style);
        }

        public static bool IsCustom(this UIStyle style)
        {
            return style.Equals(UIStyle.Custom);
        }

        public static bool IsValid(this UIStyle style)
        {
            return (int)style > 0;
        }

        public static void SetChildUIStyle(Control ctrl, UIStyle style)
        {
            List<Control> controls = ctrl.GetUIStyleControls("IStyleInterface");
            foreach (var control in controls)
            {
                if (control is IStyleInterface item && item.Style == UIStyle.Inherited)
                {
                    if (item is UIPage uipage && uipage.Parent is TabPage tabpage)
                    {
                        TabControl tabControl = tabpage.Parent as TabControl;
                        if (tabControl.SelectedTab == tabpage)
                        {
                            item.SetInheritedStyle(style);
                        }
                    }
                    else
                    {
                        item.SetInheritedStyle(style);
                    }
                }
            }

            FieldInfo[] fieldInfo = ctrl.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var info in fieldInfo)
            {
                if (info.FieldType.Name == "UIContextMenuStrip")
                {
                    UIContextMenuStrip context = (UIContextMenuStrip)info.GetValue(ctrl);
                    if (context != null && context.Style == UIStyle.Inherited)
                    {
                        context.SetInheritedStyle(style);
                    }
                }
            }
        }

        public static void SetChildCustomStyle(this Control ctrl, UIStyle style)
        {
            List<Control> controls = ctrl.GetUIStyleControls("IStyleInterface");
            foreach (var control in controls)
            {
                if (control is IStyleInterface item)
                {
                    if (item is UIPage uipage && uipage.Parent is TabPage tabpage)
                    {
                        TabControl tabControl = tabpage.Parent as TabControl;
                        if (tabControl.SelectedTab == tabpage)
                        {
                            item.SetStyleColor(style.Colors());
                            item.Style = UIStyle.Custom;
                        }
                    }
                    else
                    {
                        item.SetStyleColor(style.Colors());
                        item.Style = UIStyle.Custom;
                    }
                }
            }

            FieldInfo[] fieldInfo = ctrl.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var info in fieldInfo)
            {
                if (info.FieldType.Name == "UIContextMenuStrip")
                {
                    UIContextMenuStrip item = (UIContextMenuStrip)info.GetValue(ctrl);
                    if (item != null)
                    {
                        item.SetStyleColor(style.Colors());
                        item.Style = UIStyle.Custom;
                    }
                }
            }
        }

        /// <summary>
        /// 查找包含接口名称的控件列表
        /// </summary>
        /// <param name="ctrl">容器</param>
        /// <param name="interfaceName">接口名称</param>
        /// <returns>控件列表</returns>
        public static List<Control> GetUIStyleControls(this Control ctrl, string interfaceName)
        {
            List<Control> values = new List<Control>();

            foreach (Control obj in ctrl.Controls)
            {
                if (obj.GetType().GetInterface(interfaceName) != null)
                {
                    values.Add(obj);
                }

                if (obj is UIPage) continue;
                //if (obj is UITableLayoutPanel) continue;
                //if (obj is UIFlowLayoutPanel) continue;
                //if (obj is UIUserControl) continue;
                //if (obj is TableLayoutPanel) continue;

                if (obj.Controls.Count > 0)
                {
                    values.AddRange(obj.GetUIStyleControls(interfaceName));
                }
            }

            return values;
        }
    }
}