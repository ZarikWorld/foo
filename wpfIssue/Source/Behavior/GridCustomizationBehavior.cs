using Microsoft.Xaml.Behaviors;
using Syncfusion.Windows.Controls.Gantt;
using Syncfusion.Windows.Controls.Gantt.Grid;
using Syncfusion.Windows.Controls.Grid;
using System;
using System.Linq;
using System.Windows;

namespace wpfIssues.Behavior
{
    public class GridCustomizationBehavior : Behavior<GanttControl>
    {
        private bool firstTimeLoading = true;
        protected override void OnAttached()
        {
            this.AssociatedObject.Loaded += new System.Windows.RoutedEventHandler(AssociatedObject_Loaded);
        }
        void AssociatedObject_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (firstTimeLoading && AssociatedObject?.GanttGrid != null)
            {
                var ganttGrid = this.AssociatedObject.GanttGrid;
                var gridColumns = ganttGrid.InternalGrid.Columns;

                //2-PrioPunkte
                GridTreeColumn prioPunkte = new GridTreeColumn
                {
                    MappingName = "prioPunkte",
                    HeaderText = "PrioPunkte",
                    StyleInfo = new GridStyleInfo
                    {
                        CellType = "Static",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    }
                };
                ganttGrid.Columns.Insert(2, prioPunkte);
                ganttGrid.InternalGrid.ColumnWidths[3] = 70;

                //3-Schaetzung
                GridTreeColumn schaetzung = new GridTreeColumn
                {
                    MappingName = "DurationDisplay",
                    HeaderText = "Schätzung",
                    StyleInfo = new GridStyleInfo
                    {
                        CellType = "Static",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    }
                };
                ganttGrid.Columns.Insert(3, schaetzung);
                ganttGrid.InternalGrid.ColumnWidths[3] = 70;

                //6-URL
                GridTreeColumn webUrl = new GridTreeColumn
                {
                    MappingName = "web_url",
                    HeaderText = "URL",
                    StyleInfo = new GridStyleInfo
                    {
                        CellType = "Static",
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center
                    }
                };
                ganttGrid.Columns.Insert(6, webUrl);            // collection zero indexing
                ganttGrid.InternalGrid.ColumnWidths[7] = 0;

                ganttGrid.Columns[0].HeaderText = "Titel";
                ganttGrid.Columns[3].HeaderText = "Schätzung";
                ganttGrid.Columns[5].HeaderText = "Ende";

                ganttGrid.InternalGrid.ColumnHeaderStyle.HorizontalAlignment = HorizontalAlignment.Center;
                ganttGrid.ReadOnly = true;
                this.AssociatedObject.ScrollGanttChartTo(DateTime.Now.AddDays(-1));

                ganttGrid.Model.QueryCellInfo += Model_QueryCellInfo;

                var nodes = ganttGrid.Model.Views
                    .OfType<GridTreeControlImpl>()
                    .SelectMany(view => view.Nodes)
                    .Where(node => node.Level == 1 && node.ParentNode != null && node.HasChildNodes)
                    .ToList();

                foreach (var columnX in gridColumns)
                {
                    if (columnX.MappingName == "StartDate" || columnX.MappingName == "FinishDate")
                    {
                        columnX.StyleInfo.HorizontalAlignment = HorizontalAlignment.Center;
                    }
                }

                foreach (var item in nodes)
                {
                    ganttGrid.CollapseNode(item);
                }

                firstTimeLoading = false;
            }
        }
        private void Model_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            if (e.Style.ColumnIndex == 1)
            {
                e.Style.ReadOnly = true;
            }
        }
        public void TriggerCustomizationManually()
        {
            AssociatedObject_Loaded(this.AssociatedObject, null);


            if (AssociatedObject?.GanttGrid != null)
            {
                var ganttGrid = AssociatedObject?.GanttGrid as GanttGrid;
                if (ganttGrid != null)
                {

                    ganttGrid.InternalGrid.ColumnWidths[7] = 0;
                }

            }
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= new System.Windows.RoutedEventHandler(AssociatedObject_Loaded);
        }
    }
}