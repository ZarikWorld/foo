﻿#pragma checksum "..\..\..\..\..\Source\View\BacklogView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "02EA4053B2F4D29A8EE37514C4C1E80D416581B7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Syncfusion;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Controls.DataPager;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.RowFilter;
using Syncfusion.UI.Xaml.TreeGrid;
using Syncfusion.UI.Xaml.TreeGrid.Filtering;
using Syncfusion.Windows;
using Syncfusion.Windows.Collections;
using Syncfusion.Windows.ComponentModel;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Gantt;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Controls.Input;
using Syncfusion.Windows.Controls.Notification;
using Syncfusion.Windows.Controls.Scroll;
using Syncfusion.Windows.Controls.VirtualTreeView;
using Syncfusion.Windows.Data;
using Syncfusion.Windows.Diagnostics;
using Syncfusion.Windows.GridCommon;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Styles;
using Syncfusion.Windows.Tools;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using wpfIssues.Converter;
using wpfIssues.Model;
using wpfIssues.View;


namespace wpfIssues.View {
    
    
    /// <summary>
    /// BacklogView
    /// </summary>
    public partial class BacklogView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 56 "..\..\..\..\..\Source\View\BacklogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbSearchBox;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\..\Source\View\BacklogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbxSelectedMitarbeier;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\..\Source\View\BacklogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Syncfusion.UI.Xaml.Grid.SfDataGrid dgBacklog;
        
        #line default
        #line hidden
        
        
        #line 231 "..\..\..\..\..\Source\View\BacklogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem miStatus;
        
        #line default
        #line hidden
        
        
        #line 232 "..\..\..\..\..\Source\View\BacklogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem miStatusOffen;
        
        #line default
        #line hidden
        
        
        #line 233 "..\..\..\..\..\Source\View\BacklogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem miStatusTest;
        
        #line default
        #line hidden
        
        
        #line 234 "..\..\..\..\..\Source\View\BacklogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem miStatusBearbeitung;
        
        #line default
        #line hidden
        
        
        #line 235 "..\..\..\..\..\Source\View\BacklogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem miStatusOffenKlaerungsbedarf;
        
        #line default
        #line hidden
        
        
        #line 239 "..\..\..\..\..\Source\View\BacklogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem miPrioPunkte;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.14.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/wpfIssues;V1.0.7;component/source/view/backlogview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Source\View\BacklogView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.14.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 13 "..\..\..\..\..\Source\View\BacklogView.xaml"
            ((wpfIssues.View.BacklogView)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.OnUserControl_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.tbSearchBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 56 "..\..\..\..\..\Source\View\BacklogView.xaml"
            this.tbSearchBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.OnSearchBox_TextChanged);
            
            #line default
            #line hidden
            
            #line 56 "..\..\..\..\..\Source\View\BacklogView.xaml"
            this.tbSearchBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.OnSearchBox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.cmbxSelectedMitarbeier = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            
            #line 66 "..\..\..\..\..\Source\View\BacklogView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ClearSearchAndResetDropdown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.dgBacklog = ((Syncfusion.UI.Xaml.Grid.SfDataGrid)(target));
            
            #line 82 "..\..\..\..\..\Source\View\BacklogView.xaml"
            this.dgBacklog.CellDoubleTapped += new System.EventHandler<Syncfusion.UI.Xaml.Grid.GridCellDoubleTappedEventArgs>(this.OnDgBacklogCell_DoubleClick);
            
            #line default
            #line hidden
            
            #line 87 "..\..\..\..\..\Source\View\BacklogView.xaml"
            this.dgBacklog.CurrentCellBeginEdit += new System.EventHandler<Syncfusion.UI.Xaml.Grid.CurrentCellBeginEditEventArgs>(this.OnDgBacklog_CurrentCellBeginEdit);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 230 "..\..\..\..\..\Source\View\BacklogView.xaml"
            ((System.Windows.Controls.ContextMenu)(target)).Loaded += new System.Windows.RoutedEventHandler(this.OnContextMenu_Loaded);
            
            #line default
            #line hidden
            return;
            case 7:
            this.miStatus = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 8:
            this.miStatusOffen = ((System.Windows.Controls.MenuItem)(target));
            
            #line 232 "..\..\..\..\..\Source\View\BacklogView.xaml"
            this.miStatusOffen.Click += new System.Windows.RoutedEventHandler(this.OnContextMenuMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.miStatusTest = ((System.Windows.Controls.MenuItem)(target));
            
            #line 233 "..\..\..\..\..\Source\View\BacklogView.xaml"
            this.miStatusTest.Click += new System.Windows.RoutedEventHandler(this.OnContextMenuMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.miStatusBearbeitung = ((System.Windows.Controls.MenuItem)(target));
            
            #line 234 "..\..\..\..\..\Source\View\BacklogView.xaml"
            this.miStatusBearbeitung.Click += new System.Windows.RoutedEventHandler(this.OnContextMenuMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.miStatusOffenKlaerungsbedarf = ((System.Windows.Controls.MenuItem)(target));
            
            #line 235 "..\..\..\..\..\Source\View\BacklogView.xaml"
            this.miStatusOffenKlaerungsbedarf.Click += new System.Windows.RoutedEventHandler(this.OnContextMenuMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 237 "..\..\..\..\..\Source\View\BacklogView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OnContextMenuMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 238 "..\..\..\..\..\Source\View\BacklogView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OnContextMenuMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.miPrioPunkte = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 15:
            
            #line 240 "..\..\..\..\..\Source\View\BacklogView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OnPrioPunkteList_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 241 "..\..\..\..\..\Source\View\BacklogView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OnPrioPunkteList_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 244 "..\..\..\..\..\Source\View\BacklogView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OnContextMenuMenuItem_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

