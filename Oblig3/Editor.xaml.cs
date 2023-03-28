using Microsoft.EntityFrameworkCore.ChangeTracking;
using Oblig3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Oblig3
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {
        public Dat154Context Context { get; set; }
        
        public Editor(Dat154Context context)
        {
            InitializeComponent();
            Context = context;
          
            Cb.ItemsSource = Context.Courses.Select(c => c.Coursecode).OrderBy(c => c).ToList();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            var selecteditem = Cb.SelectedItem;
            if (selecteditem != null)
            {

                Student s = new Student()
                {
                    Studentname = sname.Text,
                    Id = int.Parse(sid.Text),
                    Studentage = int.Parse(sage.Text)
    

            };
                Grade g = new Grade()
                {
                    Studentid = s.Id,
                    Coursecode = selecteditem.ToString(),
                    Grade1 = sgrade.Text   
                

                };

            Context.Students.Add(s);    
            Context.Grades.Add(g);  
            Context.SaveChanges();  
                sid.Text = sname.Text = sage.Text = sgrade.Text = "";
            }
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(sid.Text);   
            Student s = Context.Students.Where(s => s.Id == id).First();    
            Grade g = Context.Grades.Where(g => g.Studentid == id).First();
            if (s != null)
            {
            Context.Grades.Remove(g);
            Context.Students.Remove(s); 
            Context.SaveChanges();
                sid.Text = sname.Text = sage.Text = sgrade.Text = "";
            }
        }
    }
}
