﻿
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Exoplanet.UI
{
    public partial class UIBaseForm : Form, IFrame, IStyleInterface, ITranslate
    {
        public UIBaseForm()
        {
            InitializeComponent();

            this.Register();
            Version = UIGlobal.Version;

            controlBoxForeColor = UIStyles.Blue.FormControlBoxForeColor;
            controlBoxFillHoverColor = UIStyles.Blue.FormControlBoxFillHoverColor;
            controlBoxCloseFillHoverColor = UIStyles.Blue.FormControlBoxCloseFillHoverColor;
            rectColor = UIStyles.Blue.FormRectColor;
            ForeColor = UIStyles.Blue.FormForeColor;
            titleColor = UIStyles.Blue.FormTitleColor;
            titleForeColor = UIStyles.Blue.FormTitleForeColor;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (AutoScaleMode == AutoScaleMode.Font) AutoScaleMode = AutoScaleMode.None;
            if (base.BackColor == SystemColors.Control) base.BackColor = UIStyles.Blue.PageBackColor;

            Render();
            CalcSystemBoxPos();
            SetDPIScale();

            if (AfterShown != null)
            {
                AfterShownTimer = new System.Windows.Forms.Timer();
                AfterShownTimer.Tick += AfterShownTimer_Tick;
                AfterShownTimer.Start();
            }
        }

        private void AfterShownTimer_Tick(object sender, EventArgs e)
        {
            AfterShownTimer.Stop();
            AfterShownTimer.Tick -= AfterShownTimer_Tick;
            AfterShownTimer?.Dispose();
            AfterShownTimer = null;

            AfterShown?.Invoke(this, EventArgs.Empty);
            AfterShown = null;
        }

        private System.Windows.Forms.Timer AfterShownTimer;
        public event EventHandler AfterShown;

        protected UIStyle _style = UIStyle.Inherited;

        /// <summary>
        /// Color theme
        /// </summary>
        [Description("Color Theme"), Category("ExoplanetUI")]
        [DefaultValue(UIStyle.Inherited)]
        public UIStyle Style
        {
            get => _style;
            set => SetStyle(value);
        }

        protected virtual void SetStyle(UIStyle style)
        {
            this.SuspendLayout();

            if (!style.IsCustom())
            {
                SetStyleColor(style.Colors());
                Invalidate();
            }

            _style = style == UIStyle.Inherited ? UIStyle.Inherited : UIStyle.Custom;
            UIStyleChanged?.Invoke(this, new EventArgs());
            this.ResumeLayout();
        }

        public virtual void SetStyleColor(UIBaseStyle uiColor)
        {
            controlBoxForeColor = uiColor.FormControlBoxForeColor;
            controlBoxFillHoverColor = uiColor.FormControlBoxFillHoverColor;
            ControlBoxCloseFillHoverColor = uiColor.FormControlBoxCloseFillHoverColor;
            rectColor = uiColor.FormRectColor;
            ForeColor = uiColor.FormForeColor;
            BackColor = uiColor.FormBackColor;
            titleColor = uiColor.FormTitleColor;
            titleForeColor = uiColor.FormTitleForeColor;
        }

        private float DefaultFontSize = -1;
        private float TitleFontSize = -1;

        public void SetDPIScale()
        {
            if (DesignMode) return;
            if (!UIDPIScale.NeedSetDPIFont()) return;

            if (DefaultFontSize < 0) DefaultFontSize = this.Font.Size;
            if (TitleFontSize < 0) TitleFontSize = this.TitleFont.Size;

            this.SetDPIScaleFont(DefaultFontSize);
            TitleFont = TitleFont.DPIScaleFont(TitleFontSize);
            foreach (var control in this.GetAllDPIScaleControls())
            {
                control.SetDPIScale();
            }
        }

        public event EventHandler UIStyleChanged;

        public void Render()
        {
            if (DesignMode) return;

            if (UIStyles.Style.IsValid())
                SetInheritedStyle(UIStyles.Style);

            if (UIStyles.MultiLanguageSupport)
                Translate();
        }

        public virtual void SetInheritedStyle(UIStyle style)
        {
            if (!DesignMode)
            {
                this.SuspendLayout();
                UIStyleHelper.SetChildUIStyle(this, style);

                if (_style == UIStyle.Inherited && style.IsValid())
                {
                    SetStyleColor(style.Colors());
                    Invalidate();
                    _style = UIStyle.Inherited;
                }

                UIStyleChanged?.Invoke(this, new EventArgs());
                this.ResumeLayout();
            }
        }

        protected bool showDragStretch;

        protected void SetPadding()
        {
            if (showDragStretch)
            {
                Padding = new Padding(Math.Max(Padding.Left, 2), showTitle ? TitleHeight + 1 : 2, Math.Max(Padding.Right, 2), Math.Max(Padding.Bottom, 2));
            }
            else
            {
                Padding = new Padding(0, showTitle ? TitleHeight : 0, 0, 0);
            }
        }

        [Browsable(false)]
        [Description("Show title bar icon"), Category("ExoplanetUI")]
        [DefaultValue(false)]
        public bool ShowTitleIcon { get; set; }

        protected Image IconToImage(Icon icon)
        {
            MemoryStream mStream = new MemoryStream();
            icon.Save(mStream);
            Image image = Image.FromStream(mStream);
            return image;
        }

        private Image iconImage = null;

        [Description("The Titlebar Icon Image Is Used, But The Statusbar Still Uses The Icon Property"), Category("ExoplanetUI")]
        [DefaultValue(null)]
        public Image IconImage
        {
            get => iconImage;
            set
            {
                iconImage = value;
                Invalidate();
            }
        }

        private int iconImageSize = 24;

        [Description("Titlebar Icon Image Size"), Category("ExoplanetUI")]
        [DefaultValue(24)]
        public int IconImageSize
        {
            get => iconImageSize;
            set
            {
                iconImageSize = Math.Max(16, value);
                iconImageSize = Math.Min(titleHeight - 2, iconImageSize);
                Invalidate();
            }
        }

        private StringAlignment textAlignment = StringAlignment.Near;

        [Description("Text Alignment"), Category("ExoplanetUI")]
        public StringAlignment TextAlignment
        {
            get => textAlignment;
            set
            {
                textAlignment = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Override mouse leave event
        /// </summary>
        /// <param name="e">Mouse event parameters</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            InExtendBox = InControlBox = InMaxBox = InMinBox = false;
            Invalidate();
        }

        private bool showFullScreen;

        /// <summary>
        /// Whether to enter maximized mode in full screen
        /// </summary>
        [Description("Whether To Enter Maximized Mode In Full-Screen"), Category("WindowStyle")]
        public bool ShowFullScreen
        {
            get => showFullScreen;
            set
            {
                showFullScreen = value;
                base.MaximumSize = ShowFullScreen ? Screen.PrimaryScreen.Bounds.Size : Screen.PrimaryScreen.WorkingArea.Size;
            }
        }

        /// <summary>
        /// Whether to display the form's control buttons
        /// </summary>
        private bool controlBox = true;

        /// <summary>
        /// Whether to show the form's control buttons
        /// </summary>
        [Description("Whether To Show The Form's Control Buttons"), Category("WindowStyle"), DefaultValue(true)]
        public new bool ControlBox
        {
            get => controlBox;
            set
            {
                controlBox = value;
                if (!controlBox)
                {
                    MinimizeBox = MaximizeBox = false;
                }

                CalcSystemBoxPos();
                Invalidate();
            }
        }

        /// <summary>
        /// Whether to show the form's maximize button
        /// </summary>
        [Description("Whether to show the form's maximize button"), Category("WindowStyle"), DefaultValue(true)]
        public new bool MaximizeBox
        {
            get => maximizeBox;
            set
            {
                maximizeBox = value;
                if (value) minimizeBox = true;
                CalcSystemBoxPos();
                Invalidate();
            }
        }

        /// <summary>
        /// Whether to display the form's maximize button
        /// </summary>
        private bool maximizeBox = true;

        /// <summary>
        /// Whether to display the form's minimize button
        /// </summary>
        private bool minimizeBox = true;

        /// <summary>
        /// Whether to show the form's minimize button
        /// </summary>
        [Description("Whether To Show The Form's Minimize Button"), Category("WindowStyle"), DefaultValue(true)]
        public new bool MinimizeBox
        {
            get => minimizeBox;
            set
            {
                minimizeBox = value;
                if (!value) maximizeBox = false;
                CalcSystemBoxPos();
                Invalidate();
            }
        }

        protected bool InControlBox, InMaxBox, InMinBox, InExtendBox;

        public void RegisterHotKey(Sunny.UI.ModifierKeys modifierKey, Keys key)
        {
            if (hotKeys == null) hotKeys = new ConcurrentDictionary<int, HotKey>();

            int id = HotKey.CalculateID(modifierKey, key);
            if (!hotKeys.ContainsKey(id))
            {
                HotKey newHotkey = new HotKey(modifierKey, key);
                hotKeys.TryAdd(id, newHotkey);
                Win32.User.RegisterHotKey(Handle, id, (int)newHotkey.ModifierKey, (int)newHotkey.Key);
            }
        }

        public void UnRegisterHotKey(Sunny.UI.ModifierKeys modifierKey, Keys key)
        {
            if (hotKeys == null) return;

            int id = HotKey.CalculateID(modifierKey, key);
            if (hotKeys.ContainsKey(id))
            {
                hotKeys.TryRemove(id, out _);
                Win32.User.UnregisterHotKey(Handle, id);
            }
        }

        protected ConcurrentDictionary<int, HotKey> hotKeys;

        /// <summary>
        /// Tag string
        /// </summary>
        [DefaultValue(null)]
        [Description("Gets Or Sets The Object String Containing Data About The Control"), Category("ExoplanetUI")]

        public string TagString
        {
            get; set;
        }

        [DefaultValue(false)]
        [Description("Allow Displaying The Titlebar"), Category("ExoplanetUI")]
        public bool AllowShowTitle
        {
            get => ShowTitle;
            set => ShowTitle = value;
        }

        [Browsable(false)]
        public new bool IsMdiContainer
        {
            get => base.IsMdiContainer;
            set => base.IsMdiContainer = false;
        }

        [Browsable(false)]
        public new bool AutoScroll
        {
            get => base.AutoScroll;
            set => base.AutoScroll = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (CloseAskString.IsValid())
            {
                if (!this.ShowAskDialog(CloseAskString, false))
                {
                    e.Cancel = true;
                }
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            if (MainTabControl != null)
            {
                foreach (var item in MainTabControl.GetControls<UIPage>(true))
                {
                    item.Final();
                    item.Close();
                    item.Dispose();
                }
            }
        }

        [Description("Prompt text when the form is closing; if empty, no prompt is shown"), Category("ExoplanetUI"), DefaultValue(null)]
        public string CloseAskString
        {
            get; set;
        }

        /// <summary>
        /// Control the form dragging through Windows API
        /// </summary>
        public static void MousePressMove(IntPtr handle)
        {
            Win32.User.ReleaseCapture();
            Win32.User.SendMessage(handle, Win32.User.WM_SYSCOMMAND, Win32.User.SC_MOVE + Win32.User.HTCAPTION, 0);
        }

        /// <summary>
        /// Call in the constructor to enable form moving
        /// </summary>
        /// <param name="cs">The cs.</param>
        protected void AddMousePressMove(params Control[] cs)
        {
            foreach (Control ctrl in cs)
            {
                if (ctrl != null && !ctrl.IsDisposed)
                {
                    ctrl.MouseDown += CtrlMouseDown;
                }
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the c control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void CtrlMouseDown(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                return;
            }

            if (sender == this)
            {
                if (FormBorderStyle == FormBorderStyle.None && e.Y <= titleHeight && e.X < ControlBoxLeft)
                {
                    MousePressMove(Handle);
                }
            }
            else
            {
                MousePressMove(Handle);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.HideComboDropDown();
        }

        /// <summary>
        /// Whether to disable Alt+F4
        /// </summary>
        [Description("Whether to disable Alt+F4"), Category("Key")]
        [DefaultValue(false)]
        public bool IsForbidAltF4
        {
            get; set;
        }

        [Description("Use Esc key to close the window"), Category("ExoplanetUI")]
        [DefaultValue(false)]
        public bool EscClose { get; set; } = false;

        /// <summary>
        /// Does the escape.
        /// </summary>
        protected virtual void DoEsc()
        {
            if (EscClose)
                Close();
        }

        protected virtual void DoEnter()
        {
        }

        /// <summary>
        /// Shortcut key
        /// </summary>
        /// <param name="msg">The <see cref="T:System.Windows.Forms.Message" /> passed by reference that represents the Win32 message to process.</param>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
        /// <returns>True if the control processes and uses the keystroke; otherwise, false to allow further processing.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int num = 256;
            int num2 = 260;
            if (msg.Msg == num | msg.Msg == num2)
            {
                if (keyData == (Keys.Alt | Keys.F4) && IsForbidAltF4)
                {
                    //DisableAlt+F4
                    return true;
                }

                if (keyData == Keys.Escape)
                {
                    DoEsc();
                }

                if (keyData == Keys.Enter)
                {
                    DoEnter();
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);   // Other keys are handled by default
        }


        [DefaultValue(true)]
        [Description("Whether clicking the title bar can move the form"), Category("ExoplanetUI")]
        public bool Movable { get; set; } = true;

        [DefaultValue(false)]
        [Description("Allow placing controls on the title bar"), Category("ExoplanetUI")]
        public bool AllowAddControlOnTitle
        {
            get; set;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (ShowTitle && !AllowAddControlOnTitle && e.Control.Top < TitleHeight)
            {
                e.Control.Top = Padding.Top;
            }
        }

        private int extendSymbol = 0;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Editor("Exoplanet.UI.UIImagePropertyEditor, " + AssemblyRefEx.SystemDesign, typeof(UITypeEditor))]
        [DefaultValue(0)]
        [Description("Extended button font icon"), Category("ExoplanetUI")]
        public int ExtendSymbol
        {
            get => extendSymbol;
            set
            {
                extendSymbol = value;
                Invalidate();
            }
        }

        private int _symbolSize = 24;

        [DefaultValue(24)]
        [Description("Extended button font icon size"), Category("ExoplanetUI")]
        public int ExtendSymbolSize
        {
            get => _symbolSize;
            set
            {
                _symbolSize = Math.Max(value, 16);
                _symbolSize = Math.Min(value, 128);
                Invalidate();
            }
        }

        private Point extendSymbolOffset = new Point(0, 0);

        [DefaultValue(typeof(Point), "0, 0")]
        [Description("Offset of the extended button font icon"), Category("ExoplanetUI")]
        public Point ExtendSymbolOffset
        {
            get => extendSymbolOffset;
            set
            {
                extendSymbolOffset = value;
                Invalidate();
            }
        }

        [DefaultValue(null)]
        [Description("Extended button menu"), Category("ExoplanetUI")]
        public UIContextMenuStrip ExtendMenu
        {
            get; set;
        }

        private bool extendBox;

        [DefaultValue(false)]
        [Description("Show extended button"), Category("ExoplanetUI")]
        public bool ExtendBox
        {
            get => extendBox;
            set
            {
                extendBox = value;
                CalcSystemBoxPos();
                Invalidate();
            }
        }

        internal Rectangle ControlBoxRect;

        internal Rectangle MaximizeBoxRect;

        internal Rectangle MinimizeBoxRect;

        internal Rectangle ExtendBoxRect;

        internal int ControlBoxLeft;

        protected bool IsDesignMode
        {
            get
            {
                if (DesignMode) return true;
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;
                if (Process.GetCurrentProcess().ProcessName == "devenv") return true;
                return false;
            }
        }

        /// <summary>
        /// Title font
        /// </summary>
        protected Font titleFont = UIStyles.Font();

        /// <summary>
        /// Title font
        /// </summary>
        [Description("Title font"), Category("ExoplanetUI")]
        [DefaultValue(typeof(Font), "SimSun, 12pt")]

        public Font TitleFont
        {
            get => titleFont;
            set
            {
                titleFont = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Title bar height
        /// </summary>
        protected int titleHeight = 35;

        /// <summary>
        /// Title bar height
        /// </summary>
        [Description("Title bar height"), Category("ExoplanetUI"), DefaultValue(35)]

        public int TitleHeight
        {
            get => titleHeight;
            set
            {
                titleHeight = Math.Max(value, 29);
                Padding = new Padding(Padding.Left, showTitle ? titleHeight : Padding.Top, Padding.Right, Padding.Bottom);
                Invalidate();
                CalcSystemBoxPos();
            }
        }

        protected virtual void CalcSystemBoxPos() { }

        /// <summary>
        /// Whether to show the form's title bar
        /// </summary>
        protected bool showTitle = true;

        /// <summary>
        /// Whether to show the form's title bar
        /// </summary>
        [Description("Whether to show the form's title bar"), Category("WindowStyle"), DefaultValue(true)]

        public bool ShowTitle
        {
            get => showTitle;
            set
            {
                showTitle = value;
                Padding = new Padding(Padding.Left, value ? titleHeight : 0, Padding.Right, Padding.Bottom);
                Invalidate();
            }
        }

        public readonly Guid Guid = Guid.NewGuid();

        protected FormWindowState lastWindowState = FormWindowState.Normal;
        public event OnWindowStateChanged WindowStateChanged;

        protected void DoWindowStateChanged(FormWindowState thisState)
        {
            lastWindowState = thisState;
            DoWindowStateChanged(thisState, WindowState);
        }

        protected void DoWindowStateChanged(FormWindowState thisState, FormWindowState lastState)
        {
            WindowStateChanged?.Invoke(this, thisState, lastState);

            foreach (var page in UIStyles.Pages.Values)
            {
                page.DoWindowStateChanged(thisState, lastState);
            }
        }

        protected virtual void AfterSetRectColor(Color color) { }

        public event EventHandler RectColorChanged;

        /// <summary>
        /// Title color
        /// </summary>
        protected Color titleForeColor;

        /// <summary>
        /// Title color
        /// </summary>
        [Description("Title foreground color (title color)"), Category("ExoplanetUI"), DefaultValue(typeof(Color), "White")]

        public Color TitleForeColor
        {
            get => titleForeColor;
            set
            {
                if (titleForeColor != value)
                {
                    titleForeColor = value;
                    Invalidate();
                }
            }
        }

        protected Color titleColor;

        /// <summary>
        /// Title bar color
        /// </summary>
        [Description("Title bar color"), Category("ExoplanetUI"), DefaultValue(typeof(Color), "80, 160, 255")]

        public Color TitleColor
        {
            get => titleColor;
            set
            {
                if (titleColor != value)
                {
                    titleColor = value;
                    Invalidate();
                }
            }
        }

        protected Color rectColor;

        /// <summary>
        /// Border color
        /// </summary>
        /// <value>The color of the border style.</value>
        [Description("Border color"), Category("ExoplanetUI")]
        public Color RectColor
        {
            get => rectColor;
            set
            {
                rectColor = value;
                AfterSetRectColor(value);
                RectColorChanged?.Invoke(this, EventArgs.Empty);
                Invalidate();
            }
        }

        protected Color controlBoxCloseFillHoverColor;

        /// <summary>
        /// Title bar close button hover background color
        /// </summary>
        [Description("Title bar close button hover background color"), Category("ExoplanetUI"), DefaultValue(typeof(Color), "Red")]
        public Color ControlBoxCloseFillHoverColor
        {
            get => controlBoxCloseFillHoverColor;
            set
            {
                if (controlBoxCloseFillHoverColor != value)
                {
                    controlBoxCloseFillHoverColor = value;
                    Invalidate();
                }
            }
        }

        protected Color controlBoxForeColor = Color.White;

        /// <summary>
        /// Title bar button color
        /// </summary>
        [Description("Title bar button color"), Category("ExoplanetUI"), DefaultValue(typeof(Color), "White")]
        public Color ControlBoxForeColor
        {
            get => controlBoxForeColor;
            set
            {
                if (controlBoxForeColor != value)
                {
                    controlBoxForeColor = value;
                    Invalidate();
                }
            }
        }

        protected Color controlBoxFillHoverColor;

        /// <summary>
        /// Title bar button hover background color
        /// </summary>
        [Description("Title bar button hover background color"), Category("ExoplanetUI"), DefaultValue(typeof(Color), "115, 179, 255")]
        public Color ControlBoxFillHoverColor
        {
            get => controlBoxFillHoverColor;
            set
            {
                if (ControlBoxFillHoverColor != value)
                {
                    controlBoxFillHoverColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Current control version
        /// </summary>
        [Description("Control version"), Category("ExoplanetUI")]
        public string Version
        {
            get;
        }

        #region IFrameImplementation

        private UITabControl mainTabControl;

        [DefaultValue(null)]
        public UITabControl MainTabControl
        {
            get => mainTabControl;
            set
            {
                mainTabControl = value;
                mainTabControl.Frame = this;

                mainTabControl.PageAdded += DealPageAdded;
                mainTabControl.PageRemoved += DealPageRemoved;
                mainTabControl.Selected += MainTabControl_Selected;
                mainTabControl.Deselected += MainTabControl_Deselected;
                mainTabControl.TabPageAndUIPageChanged += MainTabControl_TabPageAndUIPageChanged;
            }
        }

        private void MainTabControl_TabPageAndUIPageChanged(object sender, TabPageAndUIPageArgs e)
        {
            SelectedPage = e.UIPage;
        }

        private void MainTabControl_Deselected(object sender, TabControlEventArgs e)
        {
            List<UIPage> pages = e.TabPage.GetControls<UIPage>();
            if (pages.Count == 1) pages[0].Final();
        }

        private void MainTabControl_Selected(object sender, TabControlEventArgs e)
        {
            List<UIPage> pages = e.TabPage.GetControls<UIPage>();
            SelectedPage = pages.Count == 1 ? pages[0] : null;
        }

        private UIPage selectedPage = null;
        [Browsable(false)]
        public UIPage SelectedPage
        {
            get => selectedPage;
            private set
            {
                if (selectedPage != value)
                {
                    selectedPage = value;
                    PageSelected?.Invoke(this, new UIPageEventArgs(SelectedPage));
                }
            }
        }

        public event OnUIPageChanged PageSelected;

        public UIPage AddPage(UIPage page, int pageIndex)
        {
            page.PageIndex = pageIndex;
            return AddPage(page);
        }

        public UIPage AddPage(UIPage page, Guid pageGuid)
        {
            page.PageGuid = pageGuid;
            return AddPage(page);
        }

        public UIPage AddPage(UIPage page)
        {
            SetDefaultTabControl();

            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            if (MainTabControl == null)
            {
                throw (new ApplicationException("MainTabControl not specified，Unable to host multiple pages。"));
            }

            page.Frame = this;
            page.OnFrameDealPageParams += Page_OnFrameDealPageParams;
            MainTabControl?.AddPage(page);
            return page;
        }

        private UIBaseForm SetDefaultTabControl()
        {
            List<UITabControl> ctrls = this.GetControls<UITabControl>();
            if (ctrls.Count == 1)
            {
                if (MainTabControl == null)
                {
                    MainTabControl = ctrls[0];
                }

                List<UINavMenu> Menus = this.GetControls<UINavMenu>();
                if (Menus.Count == 1 && Menus[0].TabControl == null)
                {
                    Menus[0].TabControl = ctrls[0];
                }

                List<UINavBar> Bars = this.GetControls<UINavBar>();
                if (Bars.Count == 1 && Bars[0].TabControl == null)
                {
                    Bars[0].TabControl = ctrls[0];
                }
            }

            return this;
        }

        public virtual bool SelectPage(int pageIndex)
        {
            SetDefaultTabControl();
            if (MainTabControl == null) return false;
            return MainTabControl.SelectPage(pageIndex);
        }

        public virtual bool SelectPage(Guid pageGuid)
        {
            SetDefaultTabControl();
            if (MainTabControl == null) return false;
            return MainTabControl.SelectPage(pageGuid);
        }

        public bool RemovePage(int pageIndex) => MainTabControl?.RemovePage(pageIndex) ?? false;

        public bool RemovePage(Guid pageGuid) => MainTabControl?.RemovePage(pageGuid) ?? false;

        public void RemoveAllPages(bool keepMainPage = true) => MainTabControl?.RemoveAllPages(keepMainPage);

        public UIPage GetPage(int pageIndex) => SetDefaultTabControl().MainTabControl?.GetPage(pageIndex);

        public UIPage GetPage(Guid pageGuid) => SetDefaultTabControl().MainTabControl?.GetPage(pageGuid);

        public bool ExistPage(int pageIndex) => GetPage(pageIndex) != null;

        public bool ExistPage(Guid pageGuid) => GetPage(pageGuid) != null;

        public bool SendParamToPage(int pageIndex, object value)
        {
            SetDefaultTabControl();
            UIPage page = GetPage(pageIndex);
            if (page == null)
            {
                throw new NullReferenceException("Failed to find the page with index: " + pageIndex);
            }

            var args = new UIPageParamsArgs(null, page, value);
            page?.DealReceiveParams(args);
            return args.Handled;
        }

        public bool SendParamToPage(Guid pageGuid, object value)
        {
            SetDefaultTabControl();
            UIPage page = GetPage(pageGuid);
            if (page == null)
            {
                throw new NullReferenceException("Could not find the page with index: " + pageGuid);
            }

            var args = new UIPageParamsArgs(null, page, value);
            page?.DealReceiveParams(args);
            return args.Handled;
        }

        private void Page_OnFrameDealPageParams(object sender, UIPageParamsArgs e)
        {
            if (e == null) return;
            if (e.DestPage == null)
            {
                ReceiveParams?.Invoke(this, e);
            }
            else
            {
                e.DestPage?.DealReceiveParams(e);
            }
        }

        public event OnReceiveParams ReceiveParams;

        public T GetPage<T>() where T : UIPage => SetDefaultTabControl().MainTabControl?.GetPage<T>();

        public List<T> GetPages<T>() where T : UIPage => SetDefaultTabControl().MainTabControl?.GetPages<T>();

        public event OnUIPageChanged PageAdded;

        internal void DealPageAdded(object sender, UIPageEventArgs e)
        {
            PageAdded?.Invoke(this, e);
        }

        public event OnUIPageChanged PageRemoved;
        internal void DealPageRemoved(object sender, UIPageEventArgs e)
        {
            PageRemoved?.Invoke(this, e);
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// Finalize
        /// </summary>
        public virtual void Final()
        {
        }


        #endregion IFrameImplementation

        [DefaultValue(true)]
        [Description("The control requires multilingual translation when displayed on the interface"), Category("ExoplanetUI")]
        public bool MultiLanguageSupport { get; set; } = true;

        public virtual void Translate()
        {
            if (IsDesignMode) return;

            var controls = this.GetInterfaceControls<ITranslate>(true).Where(p => p is not UIPage);
            foreach (var control in controls)
            {
                control.Translate();
            }

            SelectedPage?.Translate();
            this.TranslateOther();
        }
    }
}
