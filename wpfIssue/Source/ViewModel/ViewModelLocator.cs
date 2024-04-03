using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace wpfIssues.ViewModel
{
    public class ViewModelLocator
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelLocator()
        {
            _serviceProvider = App.ServiceProvider;
        }
        public BacklogViewModel BacklogViewModel => _serviceProvider.GetService<BacklogViewModel>() ?? throw new InvalidOperationException("BacklogViewModel service not found.");
        public TodosViewModel TodosViewModel => _serviceProvider.GetService<TodosViewModel>() ?? throw new InvalidOperationException("TodosViewModel service not found.");
        public GanttViewModel GanttViewModel => _serviceProvider.GetService<GanttViewModel>() ?? throw new InvalidOperationException("GanttViewModel service not found.");
    }

}
