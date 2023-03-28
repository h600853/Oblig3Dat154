using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Oblig3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dat154Context dx = new();

        private readonly LocalView<Student> Students;
        private readonly LocalView<Course> Courses;
        private readonly LocalView<Grade> Grades;

        public MainWindow()
        {
            InitializeComponent();
            Students = dx.Students.Local;
            dx.Students.Load();
            studentList.DataContext = Students.OrderBy(s => s.Studentname);

            cbox.SelectionChanged += cbox_SelectionChanged;
            Courses = dx.Courses.Local;
            dx.Courses.Load();

            cbox.DataContext = Courses.Select(c => c.Coursecode).OrderBy(c => c);
            
            Grades = dx.Grades.Local;
            dx.Grades.Load();

            combobox.ItemsSource = Grades.Select(g => g.Grade1).OrderBy(g => g).Distinct();
        }


        private void sokeknapp_Click(object sender, RoutedEventArgs e)
        {
            studentList.DataContext = Students.Where(s => s.Studentname.Contains(tbox.Text))
                .OrderBy(s => s.Studentname);
        }

        private void cbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedvalue = cbox.SelectedItem;
            if (selectedvalue != null)
            {


            var studentsInCourse = from stud in Students
                                   join grade in Grades
                                   on stud.Id equals grade.Studentid
                                   join course in Courses
                                   on grade.Coursecode equals course.Coursecode
                                   where course.Coursecode.Equals(selectedvalue)
                                   select new { stud.Studentname, grade.Grade1 };
            studentList.DataContext = studentsInCourse.OrderBy(g => g.Grade1);
            }

       
        }

        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedvalue = combobox.SelectedItem;
            if (selectedvalue != null)
            {
                string selectedGrade = selectedvalue.ToString();

                var studentsInCourse = from stud in Students
                                       join grade in Grades
                                       on stud.Id equals grade.Studentid
                                       join course in Courses
                                       on grade.Coursecode equals course.Coursecode
                                       where string.Compare(grade.Grade1, selectedGrade) <= 0
                                       select new { stud.Studentname, grade.Grade1 , course.Coursecode };
                studentList.DataContext = studentsInCourse.OrderBy(g => g.Grade1);
            }
        }

        private void GetF_Click(object sender, RoutedEventArgs e)
        {

            var studentsInCourse = from stud in Students
                                   join grade in Grades
                                   on stud.Id equals grade.Studentid
                                   join course in Courses
                                   on grade.Coursecode equals course.Coursecode
                                   where grade.Grade1.Equals("F")
                                   select new { stud.Studentname, grade.Grade1, course.Coursecode };
            studentList.DataContext = studentsInCourse.OrderBy(s => s.Studentname);
        }

        private void Editor_Click(object sender, RoutedEventArgs e)
        {
            Editor ed = new Editor(dx)
            {
                Context = dx
             
            };
            ed.Show();
        }
    }
}
